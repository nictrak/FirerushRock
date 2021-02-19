using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static AudioClip Music;
    public static int ContinueDay;
    public static GameObject BangObject;
    public GameObject BangPrefab;
    public static float MusicVolume;
    public static float SfxVolume;
    private static bool isStart;
    // Start is called before the first frame update
    void Start()
    {
        if (!isStart)
        {
            BangObject = BangPrefab;

            if (MusicVolume == 0)
            {
                MusicVolume = (float)0.5;
            }
            if (SfxVolume == 0)
            {
                SfxVolume = (float)0.5;
            }
        }
        isStart = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(MusicVolume);
    }
}
