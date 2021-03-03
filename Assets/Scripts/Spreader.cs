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
    private bool valvePush = false;

    public AudioSource SpraySound;
    public AudioSource RefillSound;

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = 0;
        load = MaxLoad;
        maxLocalScaleX = Bar.transform.localScale.x;
        SpraySound.volume = GameConfig.SfxVolume;
        RefillSound.volume = GameConfig.SfxVolume;
        //sfx = GetComponent<AudioSource>();
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
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                y++;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                x--;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                y--;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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
            if(!SpraySound.isPlaying)
            {
                SpraySound.Play();
            }
            CmdSpawnWater(playerDirection);
        }
        else
        {
            if (SpraySound.isPlaying)
            {
                SpraySound.Stop();
            }
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
        water.transform.position = transform.position + new Vector3(directionalVector.x, directionalVector.y, 0)*0.25f;
        water.MoveVector = directionalVector * WaterVelocity;
        water.LifeTime = LifeTime;
        load -= UseRate;
        NetworkServer.Spawn(water.gameObject);
    }
    [Command(ignoreAuthority = true)]
    public void Fill(float amount)
    {
        /*load += amount;
        if(load > MaxLoad)
        {
            load = MaxLoad;
        }*/
        if (load < MaxLoad)
        {
            load = Mathf.Min(load + amount, MaxLoad);
        }
    }
    [ClientCallback]
    private void FillLoop()
    {
        if ((Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.X)) && !valvePush && isFillable && beingGrab)
        {
            valve.Fill(this);
            valvePush = true;
            RefillSound.Play();
        }
        if (!Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.X))
        {
            valvePush = false;
        }
        /*
        if ((Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X)) && isFillable && beingGrab)
        {
            valve.Fill(this);
        }
        */
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
