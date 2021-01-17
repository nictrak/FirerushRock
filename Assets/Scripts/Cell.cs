﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System.Runtime;
using Mirror;
[RequireComponent(typeof(SpriteRenderer))]
public class Cell : NetworkBehaviour
{
    private string houseMap;
    [SyncVar]
    private double heat;
    [SyncVar]
    private bool levelOneFire;
    [SyncVar]
    private bool levelTwoFire;
    [SyncVar]
    private bool levelThreeFire;
    private Vector2 gridPosition;
    [SyncVar]
    private double lastHeat;
    private double furniture_survivor;
    //private bool survivor;
    private double door;
    private double wall;
    private bool empty_space;
    public SpriteRenderer spriteRenderer;

    public Color NormalColor;
    public Color WallColor;
    public GameObject HeatSprite;
    public GameObject Wall;
    public GameObject Wall2;
    public GameObject Door;
    public GameObject Survivor;
    public GameObject Furniture;
    public float BaseZ;
    public SpriteRenderer Fire1;
    public SpriteRenderer Fire2;
    public SpriteRenderer Fire3;
    public SpriteRenderer toilet_floor;
    public bool IsShowHeat;
    public string HouseMap { get => houseMap; set => houseMap = value; }
    public double Heat { get => heat; set => heat = value; }
    public bool LevelOneFire { get => levelOneFire; set => levelOneFire = value; }
    public bool LevelTwoFire { get => levelTwoFire; set => levelTwoFire = value; }
    public bool LevelThreeFire { get => levelThreeFire; set => levelThreeFire = value; }
    public Vector2 GridPosition { get => gridPosition; set => gridPosition = value; }

    private FurnitureCatalog FurnitureCatalog;

    // Start is called before the first frame update
    void Start()
    {
        SpawnHouse();
        HeatSprite.transform.localScale = new Vector3(0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        HeatAndFireSync();
        ShowFire();
        ShowHeat();
        UpdateLastHeat();
    }
    [ServerCallback]
    private void SpawnHouse()
    {
        houseMap = "Normal";
        heat = 0;
        levelOneFire = false;
        levelTwoFire = false;
        levelThreeFire = false;
        lastHeat = 0;
        heat = 0;
        
        HeatSprite.transform.localScale = new Vector3(0f, 0f, 1f);
        toilet_floor.enabled = false;

        Debug.Log(FireSystem.wall_array);
        
        furniture_survivor = FireSystem.furniture_array[(int)gridPosition.y, (int)gridPosition.x];
        door = FireSystem.door_array[(int)gridPosition.y, (int)gridPosition.x];
        //survivor = (FireSystem.survivor_array[(int)gridPosition.y, (int)gridPosition.x] == 1);
        wall = FireSystem.wall_array[(int)gridPosition.y, (int)gridPosition.x];
        
        empty_space = false;
        if (wall <= 0 & furniture_survivor == 0 & door == 0) empty_space = true;

        //color
        /*
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (wall>0) spriteRenderer.color = Color.blue;
        if (furniture_survivor>0) spriteRenderer.color = Color.white;
        if (door > 0) spriteRenderer.color = Color.green;
        //if (survivor) spriteRenderer.color = Color.white;
        */

        if (furniture_survivor > 0)
        {
            GameObject selectedFurniture = get_furniture_survivor_gameobject(furniture_survivor);
            GameObject newFurniture = Instantiate(selectedFurniture);
            newFurniture.transform.position = this.transform.position;
            NetworkServer.Spawn(newFurniture);
        }
        /*
        if (survivor)
        {
            GameObject newSurvivor = Instantiate(Survivor);
            newSurvivor.transform.position = this.transform.position;
            NetworkServer.Spawn(newSurvivor);
        }
        */

        if (wall != 0)
        {
            GameObject newWall = null;
            if (wall == 1) { 
                newWall = Instantiate(Wall);
                newWall.transform.position = this.transform.position;
            }
            
            if (wall == 2)
            {
                newWall = Instantiate(Wall2);
                newWall.transform.position = this.transform.position;
            }

            if (wall == -1)
            {
                toilet_floor.enabled = true;
            }
            
            if(newWall != null)
            {
                NetworkServer.Spawn(newWall);
            }
        }

        if (door == 1 | door == 2)
        {
            GameObject newDoor = Instantiate(Door);
            newDoor.transform.position = this.transform.position;
            NetworkServer.Spawn(newDoor);
        }

    }

    private GameObject get_furniture_survivor_gameobject(double furnitureID)
    {
        int ID = (int) furnitureID;
        //Debug.Log(ID);
        return FurnitureCatalog.Furniture(ID);
    }


    [ServerCallback]
    private void HeatAndFireSync()
    {
        heat = FireSystem.heat_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire1 = FireSystem.fire_1_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire2 = FireSystem.fire_2_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire3 = FireSystem.fire_3_array[(int)gridPosition.y, (int)gridPosition.x];
        levelOneFire = fire1 > 0 && door == 0;
        levelTwoFire = fire2 > 0 && door == 0;
        levelThreeFire = fire3 > 0 && door == 0;
    }
    [ServerCallback]
    private void UpdateLastHeat()
    {
        lastHeat = heat;
    }
    private void ShowFire()
    {
        Fire1.enabled = levelOneFire;
        Fire2.enabled = levelTwoFire;
        Fire3.enabled = levelThreeFire;
    }
    private void ShowHeat()
    {
        if (empty_space)
        {
            if (Mathf.Abs((float)(lastHeat - heat)) > 0.001)
            {
                float heatScale;
                if (heat > 0)
                {
                    heatScale = (float)(heat / 100);
                }
                else
                {
                    heatScale = 0;
                }
                if (IsShowHeat)
                {
                    HeatSprite.transform.localScale = new Vector3(heatScale, heatScale, 1f);
                }
                else
                {
                    HeatSprite.transform.localScale = new Vector3(0, 0, 1f);
                }
                lastHeat = heat;
            }
        }
    }

    public void setFurnitureCatalog(FurnitureCatalog furnitureCatalog)
    {
        this.FurnitureCatalog = furnitureCatalog;
    }
}
