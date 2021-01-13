using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class LevelControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int Day = 1;

    public ParameterGenerator gen;
    private int HouseHeight = 10;
    private int HouseWidth = 10;
    public NDArray fireArray;
    public PCG pcg;
    void Start()
    {
        gen.SetDay(Day);
        HouseHeight = gen.GenHouseHeight();
        HouseWidth = gen.GenHouseWeight();
        Day = 1;
        GenMap();
        //(NDArray roomArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = pcg.GenerateHouse(Gen.GenRoom(), HouseWidth, HouseHeight, 4, Gen.GenDoor(), Gen.GenFire(), Gen.GenSurvivor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double getHouseHeight()
    {
        HouseHeight = gen.GenHouseHeight();
        return HouseHeight;
    }

    public double getHouseWidth()
    {
        HouseWidth = gen.GenHouseWeight();
        return HouseWidth;
    }

    public void GenMap()
    {
        gen.SetDay(Day);
        HouseHeight = gen.GenHouseHeight();
        HouseWidth = gen.GenHouseWeight();
        Debug.Log(1);
        (NDArray roomArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = pcg.GenerateHouse(gen.GenRoom(), 40, 40, 4, gen.GenDoor(), gen.GenFire(), gen.GenSurvivor());
        //(NDArray a, NDArray b, NDArray c, NDArray d) = pcg.GenerateHouse(Gen.GenRoom(), HouseWidth, HouseHeight, 4, Gen.GenDoor(), Gen.GenFire(), Gen.GenSurvivor());
        //(NDArray, NDArray, NDArray, NDArray) GenerateHouse(List<List<int>> houseHierarchy, double width, double height, int connectingPathLength, int doorCount, int fireCount, int catCount)
        Debug.Log(2);
    }

}
