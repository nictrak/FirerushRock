using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spreader : NetworkBehaviour
{
    public HeatReducer HeatReducerPrefab;
    public Vector3 LeftSpawnVector;
    public Vector3 RightSpawnVector;
    public Vector3 UpSpawnVector;
    public Vector3 DownSpawnVector;
    public int SpreadRate;
    public float WaterVelocity;
    public int LifeTime;
    public float Angle;
    public float Range;
    public float UseRate;
    public float MaxLoad;
    public GameObject Bar;

    private int spawnCounter;
    [SyncVar]
    private float load;
    private PlayerDirection playerDirection;
    private float maxLocalScaleX;
    private bool isFillable;
    private Valve valve;

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = 0;
        load = MaxLoad;
        maxLocalScaleX = Bar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        SyncPlayerDirection();
        Bar.transform.localScale = new Vector3(load / MaxLoad * maxLocalScaleX, Bar.transform.localScale.y, 1);
    }
    private void FixedUpdate()
    {
        SpawnLoop();
        //FillLoop();
    }
    [ClientCallback]
    private void SyncPlayerDirection()
    {
        Grabbable grabbable = GetComponent<Grabbable>();
        bool isFind = false;
        if(grabbable != null)
        {
            if(grabbable.Grabber != null)
            {
                PlayerDirection temp = grabbable.Grabber.GetComponent<PlayerDirection>();
                if (temp != null)
                {
                    playerDirection = temp;
                    isFind = true;
                }
            }
        }
        if (!isFind)
        {
            playerDirection = null;
        }
    }
    [ClientCallback]
    private void SpawnLoop()
    {
        if(playerDirection != null)
        {
            if (Input.GetKey(KeyCode.Space) && load > 0 && playerDirection.isLocalPlayer)
            {

                CmdSpawnWater(playerDirection.Direction);
            }
        }
    }
    [Command(ignoreAuthority = true)]
    private void CmdSpawnWater(string direction)
    {
        Vector2 temp;
        float usedAngle = Angle / 2 * Mathf.Deg2Rad;
        float usedRange = Range / 2;
        usedAngle = Random.Range(-usedAngle, usedAngle);
        usedRange = Random.Range(-usedRange, usedRange);
        HeatReducer water = Instantiate<HeatReducer>(HeatReducerPrefab);
        if (direction == "left")
        {
            water.transform.position = transform.position + LeftSpawnVector + new Vector3(0, usedRange, 0);
            temp = new Vector2(-Mathf.Cos(usedAngle) * WaterVelocity, Mathf.Sin(usedAngle) * WaterVelocity);
            water.MoveVector = temp;
        }
        if (direction == "right")
        {
            water.transform.position = transform.position + RightSpawnVector + new Vector3(0, usedRange, 0); ;
            temp = new Vector2(Mathf.Cos(usedAngle) * WaterVelocity, Mathf.Sin(usedAngle) * WaterVelocity);
            water.MoveVector = temp;
        }
        if (direction == "up")
        {
            water.transform.position = transform.position + UpSpawnVector + new Vector3(usedRange, 0, 0);
            temp = new Vector2(Mathf.Sin(usedAngle) * WaterVelocity, Mathf.Cos(usedAngle) * WaterVelocity);
            water.MoveVector = temp;
        }
        if (direction == "down")
        {
            water.transform.position = transform.position + DownSpawnVector + new Vector3(usedRange, 0, 0);
            temp = new Vector2(Mathf.Sin(usedAngle) * WaterVelocity, -Mathf.Cos(usedAngle) * WaterVelocity);
            water.MoveVector = temp;
        }
        water.LifeTime = LifeTime;
        load -= UseRate;
        NetworkServer.Spawn(water.gameObject);
    }
    public void Fill(float amount)
    {
        load += amount;
        if(load > MaxLoad)
        {
            load = MaxLoad;
        }
    }
    private void FillLoop()
    {
        if (Input.GetKeyDown(KeyCode.K) && isFillable)
        {
            valve.Fill(this);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Valve valve = collision.gameObject.GetComponent<Valve>();
        if(valve != null)
        {
            isFillable = true;
            this.valve = valve;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Valve valve = collision.gameObject.GetComponent<Valve>();
        if (valve != null)
        {
            isFillable = false;
            this.valve = valve;
        }
    }
}
