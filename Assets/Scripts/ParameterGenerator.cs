using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterGenerator : MonoBehaviour
{
    public int BaseBigRoom;
    public int DayAddBigRoom;
    public int BaseRoom;
    public int DayAddRoom;
    public int BaseDoor;
    public int DayAddDoor;
    public int BaseFire;
    public int DayAddFire;
    public int BaseSurvivor;
    public int DayAddSurvivor;

    private int Day;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDay(int day)
    {
        Day = day;
    }

    public int GenDoor()
    {
        return param(BaseDoor, DayAddDoor, this.Day);
    }

    public int GenFire()
    {
        return param(BaseFire, DayAddFire, this.Day);
    }

    public int GenSurvivor()
    {
        return param(BaseSurvivor, DayAddSurvivor, this.Day);
    }


    public List<List<int>> GenRoom()
    {
        int room = param(BaseRoom, DayAddRoom, this.Day);
        int BigRoom = param(BaseBigRoom, DayAddBigRoom, this.Day);
        int roomContain = 0;

        List<List<int>> bigRoomList = new List<List<int>>();

        if (room % BigRoom == 0)
        {
            roomContain = room / BigRoom;
            for (int i = 0; i < BigRoom; i++)
            {
                List<int> RoomList = new List<int>();
                for (int j = 0; j < roomContain; j++)
                {
                    RoomList.Add(Random.Range(1, 4));
                }
                bigRoomList.Add(RoomList);
            }
        }
        else
        {
            roomContain = (room / BigRoom) + 1;
            for (int i = 0; i < BigRoom; i++)
            {
                List<int> RoomList = new List<int>();
                if (i < BigRoom - 1)
                {
                    for (int j = 0; j < roomContain; j++)
                    {
                        RoomList.Add(Random.Range(1, 4));
                    }
                }
                else
                {
                    for (int e = 0; e < room % roomContain; e++)
                    {
                        RoomList.Add(Random.Range(1, 4));
                    }
                }
                bigRoomList.Add(RoomList);
            }
        }
        return bigRoomList;


    }

    private int param(int Base, int Mod, int Day)
    {
        return Base + (Day/Mod);
    }
}
