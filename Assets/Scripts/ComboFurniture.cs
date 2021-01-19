using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
[RequireComponent(typeof(SpriteRenderer))]

public class ComboFurniture : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Fur1;
    public Vector3 Fur1Pos;
    public GameObject Fur2;
    public Vector3 Fur2Pos;
    public GameObject Fur3;
    public Vector3 Fur3Pos;
    public GameObject Fur4;
    public Vector3 Fur4Pos;
    public GameObject Fur5;
    public Vector3 Fur5Pos;
    public GameObject Fur6;
    public Vector3 Fur6Pos;
    void Start()
    {
        if (Fur1 != null)
        {
            GameObject newFur = Instantiate(Fur1);
            newFur.transform.position = this.transform.position + Fur1Pos;
            NetworkServer.Spawn(newFur);
        }
        if (Fur2 != null)
        {
            GameObject newFur = Instantiate(Fur2);
            newFur.transform.position = this.transform.position + Fur2Pos;
            NetworkServer.Spawn(newFur);
        }
        if (Fur3 != null)
        {
            GameObject newFur = Instantiate(Fur3);
            newFur.transform.position = this.transform.position + Fur3Pos;
            NetworkServer.Spawn(newFur);
        }
        if (Fur4 != null)
        {
            GameObject newFur = Instantiate(Fur4);
            newFur.transform.position = this.transform.position + Fur4Pos;
            NetworkServer.Spawn(newFur);
        }
        if (Fur5 != null)
        {
            GameObject newFur = Instantiate(Fur5);
            newFur.transform.position = this.transform.position + Fur5Pos;
            NetworkServer.Spawn(newFur);
        }
        if (Fur6 != null)
        {
            GameObject newFur = Instantiate(Fur6);
            newFur.transform.position = this.transform.position + Fur6Pos;
            NetworkServer.Spawn(newFur);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
