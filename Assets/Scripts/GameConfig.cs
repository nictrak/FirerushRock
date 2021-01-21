using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static AudioClip Music;
    public static int Day;
    public static int ContinueDay;
    public static GameObject BangObject;
    public GameObject BangPrefab;
    public static float MusicVolume;
    public static float SfxVolume;
    // Start is called before the first frame update
    void Start()
    {
        BangObject = BangPrefab;
        if (Day == 0)
        {
            Day = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
