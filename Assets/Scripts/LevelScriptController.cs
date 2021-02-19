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
    public SpawnGrass SpawnGrass;
    public FireSystem FireSystem;
    public GridSystem GridSystem;
    public SetupSystem SetupSystem;
    public static int Cat;
    private int Day;
    private GameObject[] players;

    private Score Score;
    public WinScoreText WinScoreText;
    public WinCanvas WinCanvas;
    public Timer Timer;
    public PlaySceneAudio playSceneAudio;
    public LoseCanvas LoseCanvas;
    public ScoreText LoseScoreText;
    public ScoreText UIScoreText;

    void Start()
    {
        startScript();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ServerCallback]
    public void startScript()
    {
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        Score.ResetCat();
        this.Day = Score.getDay();
        ParameterGenerator.SetDay(Day);
        (int h, int w) = ParameterGenerator.GenHouseLength();
        Debug.Log("h =" + h + " w =" + w);
        //int valveCount = 5; // NEED PARAMETER
        Cat = ParameterGenerator.GenSurvivor();
        //Debug.Log(Cat);
        //(NDArray wallArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = PCG.GenerateHouse3(ParameterGenerator.GenRoom(), w, h, ParameterGenerator.GenDoor(), ParameterGenerator.GenFire(), ParameterGenerator.GenSurvivor(), ParameterGenerator.GenValve());
        (NDArray wallArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = PCG.Dummy();
        Debug.Log("PCG complete");
        int height = wallArray.shape[0];
        int width = wallArray.shape[1];
        //Debug.Log(wallArray);
        Debug.Log(height);
        Debug.Log(width);
        

        SpawnGrass.PlaceGrass(wallArray.shape[1], wallArray.shape[0]);
        Vector2 entrancePosition = PCG.EntranceDoor;
        SpawnGrass.InstantiateSpawn(entrancePosition.x, entrancePosition.y);

        /*
        Debug.Log(height);
        Debug.Log(width);
        
        Debug.Log(wallArray);
        Debug.Log(doorArray);
        Debug.Log(furnitureArray);
        Debug.Log(fireArray);
        
        Debug.Log(furnitureArray);
        */
        FireSystem.startF(wallArray, doorArray, furnitureArray, fireArray,width,height);
        /*
        Debug.Log("Start Fire System");
        
        Debug.Log(FireSystem.fire_source_array);
        Debug.Log(FireSystem.heat_array);
        Debug.Log(FireSystem.wall_array);
        Debug.Log(FireSystem.door_array);
        */
        GridSystem.startF(height, width);
        /*
        Debug.Log("Start Grid System");
        */
        //UIScoreText.updateScore(Score.getScore());
        Timer.timeLimit = ParameterGenerator.GenTime();
        Timer.startf();
        Timer.setWorking(true);
        
    }
    public void ToLobby()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerGrab>().ForceRelease();
        }
        GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>().ServerChangeScene("Lobby");
    }

    public void MissionComplete()
    {
        Timer.setWorking(false);
        FireSystem.setRun(false);
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        Score.CalculateScore();
        WinCanvas.Win();
        Score.addDay();
    }

    public void MissionFailed()
    {
        Timer.setWorking(false);
        FireSystem.setRun(false);
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        Score.setOldScoreToScore();
        playSceneAudio.StopMusic();
        LoseCanvas.Lose();
        Score.ResetScore();
        Score.setDay(1);
    }
}
