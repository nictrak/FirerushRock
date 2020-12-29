using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System.Runtime;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
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

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (wall) spriteRenderer.color = Color.blue;
        if (furniture) spriteRenderer.color = Color.black;
        if (door) spriteRenderer.color = Color.green;
        if (survivor) spriteRenderer.color = Color.yellow;
        if (!wall & !furniture & !door & !survivor) empty_space = true;




    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        heat = FireSystem.heat_array[(int)gridPosition.y, (int)gridPosition.x];
        if (empty_space) { 
            if (Mathf.Abs((float)(lastHeat - heat)) > 0.001)
            {
                float heatScale = (float)(heat / 100);
                HeatSprite.transform.localScale = new Vector3(heatScale, heatScale, 1f);
                lastHeat = heat;
            }
        }
    }
}
