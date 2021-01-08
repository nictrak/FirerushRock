using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreader : MonoBehaviour
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
    private float load;
    private PlayerDirection playerDirection;

    // Start is called before the first frame update
    void Start()
    {
        spawnCounter = 0;
        load = MaxLoad;
    }

    // Update is called once per frame
    void Update()
    {
        SyncPlayerDirection();
        Bar.transform.localScale = new Vector3(load / MaxLoad, Bar.transform.localScale.y, 1);
    }
    private void FixedUpdate()
    {
        if (spawnCounter < SpreadRate)
        {
            spawnCounter += 1;
        }
        else
        {
            SpawnLoop();
            spawnCounter = 0;
        }
    }
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
    private void SpawnLoop()
    {
        if (Input.GetKey(KeyCode.Space) && load > 0)
        {
            if (playerDirection != null)
            {
                Vector2 temp;
                float usedAngle = Angle / 2 * Mathf.Deg2Rad;
                float usedRange = Range / 2;
                usedAngle = Random.Range(-usedAngle, usedAngle);
                usedRange = Random.Range(-usedRange, usedRange);
                HeatReducer water = Instantiate<HeatReducer>(HeatReducerPrefab);
                if (playerDirection.Direction == "left")
                {
                    water.transform.position = transform.position + LeftSpawnVector + new Vector3(0, usedRange, 0);
                    temp = new Vector2(-Mathf.Cos(usedAngle) * WaterVelocity, Mathf.Sin(usedAngle) * WaterVelocity);
                    water.MoveVector = temp;
                }
                if (playerDirection.Direction == "right")
                {
                    water.transform.position = transform.position + RightSpawnVector + new Vector3(0, usedRange, 0); ;
                    temp = new Vector2(Mathf.Cos(usedAngle) * WaterVelocity, Mathf.Sin(usedAngle) * WaterVelocity);
                    water.MoveVector = temp;
                }
                if (playerDirection.Direction == "up")
                {
                    water.transform.position = transform.position + UpSpawnVector + new Vector3(usedRange, 0, 0);
                    temp = new Vector2(Mathf.Sin(usedAngle) * WaterVelocity, Mathf.Cos(usedAngle) * WaterVelocity);
                    water.MoveVector = temp;
                }
                if (playerDirection.Direction == "down")
                {
                    water.transform.position = transform.position + DownSpawnVector + new Vector3(usedRange, 0, 0);
                    temp = new Vector2(Mathf.Sin(usedAngle) * WaterVelocity, -Mathf.Cos(usedAngle) * WaterVelocity);
                    water.MoveVector = temp;
                }
                water.LifeTime = LifeTime;
                load -= UseRate;
            }
        }
    }
}
