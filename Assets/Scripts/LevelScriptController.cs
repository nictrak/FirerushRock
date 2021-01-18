using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using Mirror;

public class LevelScriptController : NetworkBehaviour
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
    [ServerCallback]
    public void startScript()
    {
        ParameterGenerator.SetDay(GameConfig.Day);
        (int h, int w) = ParameterGenerator.GenHouseLength();
        Debug.Log("h =" + h + " w =" + w);
        (NDArray wallArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = PCG.GenerateHouse3(ParameterGenerator.GenRoom(), w, h, ParameterGenerator.GenDoor(), ParameterGenerator.GenFire(), ParameterGenerator.GenSurvivor());
        Debug.Log("PCG complete");
        int height = wallArray.shape[0];
        int width = wallArray.shape[1];
        Debug.Log(wallArray);
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
        Debug.Log("Start Fire System");
        /*
        Debug.Log(FireSystem.fire_source_array);
        Debug.Log(FireSystem.heat_array);
        Debug.Log(FireSystem.wall_array);
        Debug.Log(FireSystem.door_array);
        */
        GridSystem.startF(height, width);
        Debug.Log("Start Grid System");




    }
}
