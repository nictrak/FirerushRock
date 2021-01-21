using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DamageDoor : DamageFromFire
{
    private Cell doorCell;

    public Cell DoorCell { get => doorCell; set => doorCell = value; }

    // Start is called before the first frame update
    void Start()
    {
        DamageSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if(doorCell != null)
        Damage(doorCell);
    }

}
