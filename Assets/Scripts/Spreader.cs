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
    private bool beingGrab = false;
    private float playerDirection = 0;
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
        FillLoop();
    }
    [ClientCallback]
    private void SyncPlayerDirection()
    {
        //TODO Add LocalPlayer checking
        Grabbable grabbable = GetComponent<Grabbable>();
        if (grabbable != null && grabbable.Grabber != null && grabbable.Grabber.isLocalPlayer)
        {
            beingGrab = true;
            int x = 0;
            int y = 0;
            if (Input.GetKey(KeyCode.W))
                y++;
            if (Input.GetKey(KeyCode.A))
                x--;
            if (Input.GetKey(KeyCode.S))
                y--;
            if (Input.GetKey(KeyCode.D))
                x++;
            if (x != 0 || y != 0)
                playerDirection = Mathf.Atan2(y, x);
        }
        else
            beingGrab = false;
    }
    [ClientCallback]
    private void SpawnLoop()
    {
        if (Input.GetKey(KeyCode.Space) && load > 0 && beingGrab)
        {
            CmdSpawnWater(playerDirection);
        }
    }
    [Command(ignoreAuthority = true)]
    private void CmdSpawnWater(float playerDirection)
    {
        float usedAngle = Angle * Mathf.Deg2Rad / 2;
        float usedRange = Range / 2;
        usedAngle = Random.Range(playerDirection - usedAngle, playerDirection + usedAngle);
        usedRange = Random.Range(-usedRange, usedRange);
        HeatReducer water = Instantiate<HeatReducer>(HeatReducerPrefab);
        Vector2 directionalVector = new Vector2(Mathf.Cos(usedAngle), Mathf.Sin(usedAngle));
        water.transform.position = transform.position + new Vector3(directionalVector.x, directionalVector.y, 0);
        water.MoveVector = directionalVector * WaterVelocity;
        water.LifeTime = LifeTime;
        load -= UseRate;
        NetworkServer.Spawn(water.gameObject);
    }
    [Command(ignoreAuthority = true)]
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
        if (Input.GetKeyDown(KeyCode.K) && isFillable && beingGrab)
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
