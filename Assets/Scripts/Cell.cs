using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System.Runtime;
using Mirror;
[RequireComponent(typeof(SpriteRenderer))]
public class Cell : NetworkBehaviour
{
    private string houseMap;
    private double heat;
    private bool levelOneFire;
    private bool levelTwoFire;
    private bool levelThreeFire;
    private Vector2 gridPosition;
    private double lastHeat;
    private bool furniture;
    private bool survivor;
    private bool door;
    private bool wall;
    private bool empty_space;
    public SpriteRenderer spriteRenderer;

    public Color NormalColor;
    public Color WallColor;
    public GameObject HeatSprite;
    public GameObject Wall;
    public GameObject Door;
    public GameObject Survivor;
    public GameObject Furniture;
    public float BaseZ;
    public SpriteRenderer Fire1;
    public SpriteRenderer Fire2;
    public SpriteRenderer Fire3;
    public bool IsShowHeat;
    public string HouseMap { get => houseMap; set => houseMap = value; }
    public double Heat { get => heat; set => heat = value; }
    public bool LevelOneFire { get => levelOneFire; set => levelOneFire = value; }
    public bool LevelTwoFire { get => levelTwoFire; set => levelTwoFire = value; }
    public bool LevelThreeFire { get => levelThreeFire; set => levelThreeFire = value; }
    public Vector2 GridPosition { get => gridPosition; set => gridPosition = value; }

    // Start is called before the first frame update
    void Start()
    {
        houseMap = "Normal";
        heat = 0;
        levelOneFire = false;
        levelTwoFire = false;
        levelThreeFire = false;
        lastHeat = 0;
        heat = 0;
        HeatSprite.transform.localScale = new Vector3(0f, 0f, 1f);

        furniture = (FireSystem.furniture_array[(int)gridPosition.y, (int)gridPosition.x] == 1);
        door = (FireSystem.door_array[(int)gridPosition.y, (int)gridPosition.x] == 1);
        survivor = (FireSystem.survivor_array[(int)gridPosition.y, (int)gridPosition.x] == 1);
        wall = (FireSystem.wall_array[(int)gridPosition.y, (int)gridPosition.x] == 1);
        empty_space = false;
        if (!wall & !furniture & !door & !survivor) empty_space = true;


        //color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (wall) spriteRenderer.color = Color.blue;
        if (furniture) spriteRenderer.color = Color.white;
        if (door) spriteRenderer.color = Color.green;
        if (survivor) spriteRenderer.color = Color.white;
        if (furniture)
        {
            GameObject newFurniture = Instantiate(Furniture);
            newFurniture.transform.position = this.transform.position;
            NetworkServer.Spawn(newFurniture);
        }

        if (survivor)
        {
            GameObject newSurvivor = Instantiate(Survivor);
            newSurvivor.transform.position = this.transform.position;
            NetworkServer.Spawn(newSurvivor);
        }

        if (wall)
        {
            GameObject newWall = Instantiate(Wall);
            newWall.transform.position = this.transform.position;
            NetworkServer.Spawn(newWall);
        }

        if (door)
        {
            GameObject newDoor = Instantiate(Door);
            newDoor.transform.position = this.transform.position;
            NetworkServer.Spawn(newDoor);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        heat = FireSystem.heat_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire1 = FireSystem.fire_1_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire2 = FireSystem.fire_2_array[(int)gridPosition.y, (int)gridPosition.x];
        double fire3 = FireSystem.fire_3_array[(int)gridPosition.y, (int)gridPosition.x];
        bool isFire1 = fire1 > 0 && !door;
        bool isFire2 = fire2 > 0 && !door;
        bool isFire3 = fire3 > 0 && !door;
        if (empty_space) { 
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
        Fire1.enabled = isFire1;
        Fire2.enabled = isFire2;
        Fire3.enabled = isFire3;
    }
}
