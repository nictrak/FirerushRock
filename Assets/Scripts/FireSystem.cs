using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;
using System.Timers;

public class FireSystem : MonoBehaviour
{
    public GridSystem grid;
    public int fire_spread_counter;
    private int framerate_counter;
    public static NDArray heat_array;
    private NDArray fire_1_array;
    private NDArray fire_2_array;
    private NDArray fire_3_array;
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
    public int max_heat_point;
    public float heat_decay;
    public float fire2_add_heat;
    public float fire3_add_heat;


    // Start is called before the first frame update




    void Awake()
    {
        framerate_counter = 0;
    //initial array
        var gridsize = ((int)grid.GridSize.x, (int)grid.GridSize.y);
        heat_array = np.zeros(gridsize);
        fire_1_array = np.zeros(gridsize);
        fire_2_array = np.zeros(gridsize);
        fire_3_array = np.zeros(gridsize);
        wall_array = np.zeros(gridsize);
        fire_source_array = np.zeros(gridsize);

        //initial heat
        
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
            {0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
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
    }

    private void FixedUpdate()
    {
        if (framerate_counter >= fire_spread_counter)
        {
            fire_1_array = update_fire_array(heat_array, fire_1_array, fire_1_start_point, fire_1_size);
            fire_2_array = update_fire_array(heat_array, fire_2_array, fire_2_start_point, fire_2_size);
            fire_3_array = update_fire_array(heat_array, fire_3_array, fire_3_start_point, fire_3_size);

            heat_array += heat_decay;
            heat_array = spread(heat_array, fire_2_array, fire2_add_heat);
            heat_array = spread(heat_array, fire_3_array, fire3_add_heat);
            heat_array = heat_array - wall_array * int.MaxValue;
            heat_array = np.clip(heat_array, (NDArray)0, (NDArray)max_heat_point);
            //Debug.Log(GetHeat(new Vector2(0,0)));
            framerate_counter = 0;
        }
        framerate_counter += 1;
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
    
    
    static NDArray update_fire_array(NDArray value_array, NDArray fire_array, int start_point, int size)
    {
        //NDArray new_fire = np.where(value_array > start_point, (NDArray)1, (NDArray)0);
        NDArray new_fire = np.clip(value_array - start_point,0,1);

        return new_fire * size;
    }
    
}
