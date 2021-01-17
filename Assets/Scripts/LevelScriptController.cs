using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class LevelScriptController : MonoBehaviour
{
    // Start is called before the first frame update
    public ParameterGenerator ParameterGenerator;
    public PCG PCG;
    public FireSystem FireSystem;
    public GridSystem GridSystem;

    void Start()
    {

        GameConfig.Day = 1;
        startScript();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void startScript()
    {
        ParameterGenerator.SetDay(GameConfig.Day);
        (int h, int w) = ParameterGenerator.GenHouseLength();
        Debug.Log("h =" + h + " w =" + w);
        Debug.Log(ParameterGenerator.GenRoom().Count);
        (NDArray wallArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = PCG.GenerateHouse2(ParameterGenerator.GenRoom(), ParameterGenerator.GenDoor(), ParameterGenerator.GenFire(), ParameterGenerator.GenSurvivor());

        int height = wallArray.shape[0];
        int width = wallArray.shape[1];
        Debug.Log(height);
        Debug.Log(width);
        /*
        Debug.Log(height);
        Debug.Log(width);
        
        Debug.Log(wallArray);
        Debug.Log(doorArray);
        Debug.Log(furnitureArray);
        Debug.Log(fireArray);
        */
        //Debug.Log(furnitureArray);
        FireSystem.startF(wallArray, doorArray, furnitureArray, fireArray,width,height);
        /*
        Debug.Log(FireSystem.fire_source_array);
        Debug.Log(FireSystem.heat_array);
        Debug.Log(FireSystem.wall_array);
        Debug.Log(FireSystem.door_array);
        */
        GridSystem.startF(height, width);




    }
}
