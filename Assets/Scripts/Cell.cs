using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    private string houseMap;
    private float heat;
    private bool levelOneFire;
    private bool levelTwoFire;
    private bool levelThreeFire;
    private GameObject fireSimulation;

    public Color NormalColor;
    public Color WallColor;
    public string HouseMap { get => houseMap; set => houseMap = value; }
    public float Heat { get => heat; set => heat = value; }
    public bool LevelOneFire { get => levelOneFire; set => levelOneFire = value; }
    public bool LevelTwoFire { get => levelTwoFire; set => levelTwoFire = value; }
    public bool LevelThreeFire { get => levelThreeFire; set => levelThreeFire = value; }
    public GameObject FireSystem { get => fireSimulation; set => fireSimulation = value; }

    // Start is called before the first frame update
    void Start()
    {
        houseMap = "Normal";
        heat = 0;
        levelOneFire = false;
        levelTwoFire = false;
        levelThreeFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
