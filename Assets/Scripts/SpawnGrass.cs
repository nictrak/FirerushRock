using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnGrass : MonoBehaviour
{

    public Vector3 OriginPosition;
    public float GrassSize = 5;
    public GameObject GrassTemplate;
    public GameObject SpawnPointTemplate;

    public void InstantiateSpawn(float entranceX, float entranceY)
    {
        GameObject spawn = Instantiate(SpawnPointTemplate);
        spawn.transform.position = new Vector3(OriginPosition.x + entranceX, OriginPosition.y + entranceY, OriginPosition.z - 1);
        NetworkServer.Spawn(spawn);
    }

    public void PlaceGrass(int houseWidth, int houseHeight)
    {
        int grassWidth = Mathf.RoundToInt(houseWidth / GrassSize) + 9;
        int grassHeight = Mathf.RoundToInt(houseHeight / GrassSize) + 8;
        for (int i = 0; i < grassWidth; i++)
        {
            for (int j = 0; j < grassHeight; j++)
            {
                float x = OriginPosition.x - (GrassSize * 2) + i * GrassSize;
                float y = OriginPosition.y + (GrassSize * 3) - j * GrassSize;
                GameObject grass = Instantiate(GrassTemplate);
                grass.transform.position = new Vector3(x, y, OriginPosition.z);
                NetworkServer.Spawn(grass);
            }
        }
    }

    // Start is called before the first frame update
    /*void Start()
    {
        PlaceGrass(0, 0);
        InstantiateSpawn(0, 0);
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
