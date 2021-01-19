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
    public int BaseHouseLength;
    public int DayAddHouseLength;
    public int HouseLengthMultiplier;


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

    public (int,int) GenHouseLength()
    {
        float length = param(BaseHouseLength, DayAddHouseLength, this.Day, Multiplier: HouseLengthMultiplier);
        length = length * 2;
        float height = length * Random.Range((float)0.35, (float)0.65);
        float width = length - height;
        return ((int)height, (int)width);
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
        

        int toilet = Mathf.CeilToInt(room * (float)0.2);
        int bedroom = Mathf.CeilToInt(room * (float)0.3);
        int livingroom = room - toilet - bedroom;
        Debug.Log(room);
        Debug.Log(toilet);
        Debug.Log(bedroom);
        Debug.Log(livingroom);

        List<List<int>> bigRoomList = new List<List<int>>();

        if (room % BigRoom == 0)
        {
            roomContain = room / BigRoom;
            for (int i = 0; i < BigRoom; i++)
            {
                List<int> RoomList = new List<int>();
                for (int j = 0; j < roomContain; j++)
                {
                    int roomtype = assignRoom(bedroom, toilet, livingroom);
                    if (roomtype == 1) bedroom = bedroom - 1;
                    if (roomtype == 2) toilet = toilet - 1;
                    if (roomtype == 3) livingroom = livingroom - 1;
                    int roomID = GenRoomWithSize(roomtype);
                    RoomList.Add(roomID);
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
                        int roomtype = assignRoom(bedroom, toilet, livingroom);
                        if (roomtype == 1) bedroom = bedroom - 1;
                        if (roomtype == 2) toilet = toilet - 1;
                        if (roomtype == 3) livingroom = livingroom - 1;
                        int roomID = GenRoomWithSize(roomtype);
                        RoomList.Add(roomID);
                    }
                }
                else
                {
                    for (int e = 0; e < room % roomContain; e++)
                    {
                        int roomtype = assignRoom(bedroom, toilet, livingroom);
                        if (roomtype == 1) bedroom = bedroom - 1;
                        if (roomtype == 2) toilet = toilet - 1;
                        if (roomtype == 3) livingroom = livingroom - 1;
                        int roomID = GenRoomWithSize(roomtype);
                        RoomList.Add(roomID);
                    }
                }
                bigRoomList.Add(RoomList);
            }
        }
        return bigRoomList;


    }

    private int param(int Base, int Mod, int Day, int Multiplier = 1)
    {
        return Base + ((Day/Mod)* Multiplier);
    }

    private int assignRoom(int bedroom, int toilet, int livingroom)
    {
        if (bedroom <= 0)
        {
            if (toilet <= 0)
            {
                return 3;
            }
            if (livingroom <= 0)
            {
                return 2;
            }
            return Random.Range(2, 4);
        }
        if (livingroom <= 0)
        {
            if (toilet <= 0)
            {
                return 1;
            }
            return Random.Range(1, 3);
        }
        if (toilet <= 0)
        {
            int a = Random.Range(1, 3);
            if (a == 2) a = 3;
            return a;
        }
        return Random.Range(1, 4);
    }

    private int GenRoomWithSize(int roomtype)
    {
        int size = 20;
        if (roomtype == 1) size = Random.Range(25, 40);
        if (roomtype == 2) size = Random.Range(20, 30);
        if (roomtype == 3) size = Random.Range(35, 50);
        return (roomtype * 100) + size;
    }
}
