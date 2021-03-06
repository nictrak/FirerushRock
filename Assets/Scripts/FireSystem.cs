﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System.Timers;
using UnityEngine.SceneManagement;
using Mirror;
public class FireSystem : MonoBehaviour
{
    public GridSystem grid;
    public int fire_spread_counter_4p;
    public int fire_spread_counter_3p;
    public int fire_spread_counter_2p;
    public int fire_spread_counter_1p;
    private int fire_spread_counter;
    private int framerate_counter;
    public static NDArray heat_array;
    //public static NDArray fire_1_array;
    public static NDArray fire_2_array;
    //public static NDArray fire_3_array;
    public static NDArray wall_array;
    public static NDArray fire_source_array;
    public static NDArray door_array;
    public static NDArray furniture_array;
    public static NDArray survivor_array;
    public int fire_1_size;
    public int fire_2_size;
    public int fire_3_size;
    public int fire_1_start_point;
    public int fire_2_start_point;
    public int fire_3_start_point;
    public static int fire1StartPoint;
    public static int fire2StartPoint;
    public static int fire3StartPoint;
    public int max_heat_point;
    public float heat_decay;
    public float fire2_add_heat;
    public float fire3_add_heat;
    private (int, int) gridsize;
    private NDArray zero_array;
    public static bool isRun;
    private NDArray wall_array_clip;
    private bool isMusicHigh;
    public PlaySceneAudio PlaySceneAudio;
    private bool isPassDay;
    public LevelScriptController levelScriptController;
    // Start is called before the first frame update


    private GameObject NetworkObj;
    private NetworkManager2 NetworkMananger;


    void Awake()
    {
        fire_spread_counter = 20;
        framerate_counter = 0;
        isRun = false;
        isMusicHigh = false;
        isPassDay = false;
        fire1StartPoint = fire_1_start_point;
        fire2StartPoint = fire_2_start_point;
        fire3StartPoint = fire_3_start_point;

        NetworkObj = GameObject.FindGameObjectWithTag("Network");
        NetworkMananger = NetworkObj.GetComponent<NetworkManager2>();
        int playerCount = NetworkMananger.CountPlayer();
        Debug.Log("PLayer NUMMMMMMMMMMMMMMMMMMMMMMMMMM : " + playerCount);
        if (playerCount == 1) fire_spread_counter = fire_spread_counter_1p;
        else
        {
            if (playerCount == 2) fire_spread_counter = fire_spread_counter_2p;
            else
            {
                if (playerCount == 3) fire_spread_counter = fire_spread_counter_3p;
                else
                {
                    if (playerCount == 4) fire_spread_counter = fire_spread_counter_4p;
                }
            }
        }
        Debug.Log("fire_spread_counter : " + fire_spread_counter);
    }

    public void startF(NDArray wallArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray, int height, int width)
    {
        isRun = true;
        framerate_counter = 0;
        //initial array

        gridsize = (width, height);
        zero_array = np.zeros(gridsize);
        heat_array = np.zeros(gridsize);
        //fire_1_array = np.zeros(gridsize);
        fire_2_array = np.zeros(gridsize);
        //fire_3_array = np.zeros(gridsize);
        wall_array = np.zeros(gridsize);
        fire_source_array = np.zeros(gridsize);

        //initial heat
        fire_source_array = fireArray + shift_right(fireArray) + shift_down(fireArray) + shift_down(shift_right(fireArray));
        heat_array = fire_source_array * max_heat_point;

        wall_array = wallArray;
        wall_array_clip = np.clip(wall_array, 0, 1);
        door_array = doorArray;
        furniture_array = furnitureArray;
    }

    private void FixedUpdate()
    {
        if(!isRun) zero_array = np.zeros(gridsize);
        if (isRun)
        {
            if (framerate_counter >= fire_spread_counter)
            {
                fire_2_array = update_fire_array(heat_array, fire_2_array, fire_2_start_point, fire_2_size);
                //fire_3_array = update_fire_array(heat_array, fire_3_array, fire_3_start_point, fire_3_size);

                heat_array += heat_decay;
                //heat_array = spread(heat_array, fire_2_array, fire2_add_heat);
                //heat_array = spread(heat_array, fire_3_array, fire3_add_heat);
                heat_array = newSpread(heat_array, 25);
                heat_array = heat_array - wall_array_clip * int.MaxValue;
                heat_array = np.clip(heat_array, (NDArray)0, (NDArray)max_heat_point);
                //Debug.Log(GetHeat(new Vector2(0,0)));
                framerate_counter = 0;
                /*
                if (zero_array.Equals(np.zeros(fire_2_array.shape)))
                {
                    Debug.Log("yesssss");
                }
                else
                {
                    Debug.Log("noooooo");
                }
                */
                if (fire_2_array.Equals(zero_array) || Input.GetKey(KeyCode.F1))
                {
                    //TODO Fix this to multiplayer
                    PassDay();
                    isRun = false;
                }

                //Debug.Log((float)(double)fire_3_array.sum()/ fire_3_size);
                if (isMusicHigh == false)
                {
                    if((float)(double)fire_2_array.sum() / fire_2_size >=  100)
                    {
                        Debug.Log("ChangeMusic");
                        PlaySceneAudio.ChangeMusicToHigh();
                        isMusicHigh = true;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                levelScriptController.MissionFailed();
                isRun = false;
            }
            framerate_counter += 1;
        }
    }
    private void PassDay()
    {
        if (!isPassDay)
        {
            //GameConfig.Day++;
            //GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>().ServerChangeScene("Lobby");
            levelScriptController.MissionComplete();
            isPassDay = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    static NDArray shift_left(NDArray array)
    {
        NDArray a = array[":,1:"];
        NDArray z = np.zeros((array.shape));
        z[":,:-1"] = a;
        return z;
    }
    
    static NDArray shift_right(NDArray array)
    {
        NDArray a = array[":,:-1"];
        NDArray z = np.zeros((array.shape));
        z[":,1:"] = a;
        return z;
    }

    static NDArray shift_up(NDArray array)
    {
        NDArray a = array["1:,:"];
        NDArray z = np.zeros((array.shape));
        z[":-1,:"] = a;
        return z;
    }

    static NDArray shift_down(NDArray array)
    {
        NDArray a = array[":-1,:"];
        NDArray z = np.zeros((array.shape));
        z["1:,:"] = a;
        return z;
    }

    static NDArray spread(NDArray value_array, NDArray fire_array, float add_heat)
    {
        NDArray spreader_array = np.clip(fire_array, (NDArray)0, (NDArray)1);
        NDArray total = shift_up(shift_left(spreader_array)) + shift_up(spreader_array) + shift_up(shift_right(spreader_array)) + shift_left(spreader_array) + spreader_array + shift_right(spreader_array) + shift_down(shift_left(spreader_array)) + shift_down(spreader_array) + shift_down(shift_right(spreader_array));
        total = total * add_heat;
        return value_array + total;
    }

    static NDArray newSpread(NDArray heat_array, int divider)
    {
        NDArray spreader_array = np.clip(heat_array / divider,(NDArray)0, (NDArray)3);
        NDArray total = shift_up(shift_left(spreader_array)) + shift_up(spreader_array) + shift_up(shift_right(spreader_array)) + shift_left(spreader_array) + spreader_array + shift_right(spreader_array) + shift_down(shift_left(spreader_array)) + shift_down(spreader_array) + shift_down(shift_right(spreader_array));
        return heat_array + total;
    }


    static NDArray update_fire_array(NDArray value_array, NDArray fire_array, int start_point, int size)
    {
        //NDArray new_fire = np.where(value_array > start_point, (NDArray)1, (NDArray)0);
        NDArray new_fire = np.clip(value_array - start_point,0,1);

        return new_fire * size;
    }

    public static void setRun(bool run)
    {
        isRun = run;
    }
    
}

/*
        fire_source_array = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };
        heat_array = fire_source_array * 100;


        //initial wall
       
        wall_array = new int[,] {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,1,1,1,0,1,1,1,1,1,1},
            {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
            {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

        door_array = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        furniture_array = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        survivor_array = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };
        */
