using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameIntro : NetworkBehaviour
{
    // Start is called before the first frame update
    public int time;
    private int count;
    private GameObject[] Firefighters;
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        FireSystemSetRun(false);
        if (count >= time)
        {
            FireSystemSetRun(true);
            //FirefighterToSpawnPoint();
            Destroy(this.gameObject);
        }
    }
    [ServerCallback]
    private void FireSystemSetRun(bool isRun)
    {
        FireSystem.setRun(isRun);
    }
    private void GetAllFirefighter()
    {
        Firefighters = GameObject.FindGameObjectsWithTag("Player");
    }
    public void FirefighterToSpawnPoint()
    {
        GetAllFirefighter();
        for (int i = 0; i < Firefighters.Length; i++)
        {
            Firefighter ff;
            ff = Firefighters[i].GetComponent<Firefighter>();
            ff.ToSpawnPoint();
        }
    }
}
