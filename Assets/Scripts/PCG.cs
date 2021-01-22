using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class PCG : MonoBehaviour
{

    public bool DebugStart = false;
    public bool DebugFurnitureBoundary = false;
    public int DebugRoomCount = 11;
    public int DebugRoomGroup = 3;

    private static int PLACE_FLOOR = 1;
    private static int PLACE_WALL  = 2;
    private static int PLACE_ANY   = 3;

    private static int WALL_LEFT  = 1;
    private static int WALL_DOWN  = 2;
    private static int WALL_RIGHT = 4;
    private static int WALL_UP    = 8;
    private static int WALL_ANY   = 15;

    private static int ORIENT_FLOOR_LEFT  = 1;
    private static int ORIENT_FLOOR_DOWN  = 2;
    private static int ORIENT_FLOOR_RIGHT = 4;
    private static int ORIENT_FLOOR_UP    = 8;
    private static int ORIENT_WALL_0      = 16;
    private static int ORIENT_WALL_1      = 32;
    private static int ORIENT_WALL_2      = 64;
    private static int ORIENT_WALL_3      = 128;
    private static int ORIENT_ANY         = 255;

    private static int ENTRANCE_TYPE = -5;
    private static int FIRE_TYPE     = -6;
    private static int CAT_TYPE      = -7;
    private static int PAINTING_TYPE = -8;
    private static int VALVE_TYPE    = -9;

    private static Point ORIGIN = new Point(0, 0);
    private static Rectangle ZERO_RECT = new Rectangle(0, 0, 0, 0);

    private static FurniturePreset DOOR_0 = new FurniturePreset(-1, 0, 0, 0, new Point(3, 1)/*, new Point(3, 3), new Point(0, 0)*/);
    private static FurniturePreset DOOR_1 = new FurniturePreset(-2, 0, 0, 0, new Point(1, 3)/*, new Point(3, 3), new Point(-3, 0)*/);
    private static FurniturePreset DOOR_2 = new FurniturePreset(-3, 0, 0, 0, new Point(3, 1)/*, new Point(3, 3), new Point(0, -3)*/);
    private static FurniturePreset DOOR_3 = new FurniturePreset(-4, 0, 0, 0, new Point(1, 3)/*, new Point(5, 3), new Point(0, 0)*/);
    private static FurniturePreset ENTRANCE = new FurniturePreset(-5, 0, 0, 0, ORIGIN/*, ORIGIN, ORIGIN*/);
    private static FurniturePreset FIRE = new FurniturePreset(-6, 0, 0, 0, new Point(3, 3)/*, ORIGIN, ORIGIN*/);
    private static FurniturePreset CAT  = new FurniturePreset(-7, 0, 0, 0, new Point(1, 1)/*, ORIGIN, ORIGIN*/);
    private static FurniturePreset PAINTING = new FurniturePreset(-8, 0, 0, 0, new Point(1, 1)/*, ORIGIN, ORIGIN*/);
    private static FurniturePreset VALVE = new FurniturePreset(-9, 0, 0, 0, new Point(1, 1));

    private static Dictionary<int, List<RoomPreset>> ROOM_PRESET_DICTIONARY;

    private static List<int> ORIENTATION_MAPPING;

    private static bool INIT_FLAG = false;


    private System.Random random = new System.Random();

    private Vector2 entranceDoor;
    public Vector2 EntranceDoor { get => entranceDoor; }


    private T RandomChoice<T>(List<T> list)
    {
        int choice = random.Next(list.Count);
        return list[choice];
    }
    private T RandomChoicePop<T>(List<T> list)
    {
        int choice = random.Next(list.Count);
        T item = list[choice];
        list.RemoveAt(choice);
        return item;
    }
    private int RoundPCG(double d)
    {
        return Convert.ToInt32(Math.Round(d, MidpointRounding.AwayFromZero));
    }

    private class Point
    {

        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            Point objPoint = obj as Point;
            return x == objPoint.x && y == objPoint.y;
        }
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

    }
    private class Rectangle
    {

        public double x1;
        public double y1;
        public double x2;
        public double y2;

        private double collideError = 0.001;

        public Rectangle(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public override bool Equals(object obj)
        {
            Rectangle objRectangle = obj as Rectangle;
            return x1 == objRectangle.x1 && y1 == objRectangle.y1 && x2 == objRectangle.x2 && y2 == objRectangle.y2;
        }
        public override int GetHashCode()
        {
            return x1.GetHashCode() ^ y1.GetHashCode() ^ x2.GetHashCode() ^ y2.GetHashCode();
        }

        public Point GetPoint(int index)
        {
            if (index % 4 == 0)
                return new Point(x1, y1);
            if (index % 4 == 1)
                return new Point(x2, y1);
            if (index % 4 == 2)
                return new Point(x2, y2);
            return new Point(x1, y2);
        }
        public double GetWidth()
        {
            return x2 - x1;
        }
        public double GetHeight()
        {
            return y2 - y1;
        }

        public bool Collide(Rectangle rectangle)
        {
            return x2 + collideError >= rectangle.x1 && x1 - collideError <= rectangle.x2 && y2 + collideError >= rectangle.y1 && y1 - collideError <= rectangle.y2;
        }

    }
    private class Graph<NodeType, EdgeType>
    {

        public Dictionary<NodeType, Dictionary<NodeType, List<EdgeType>>> graph = new Dictionary<NodeType, Dictionary<NodeType, List<EdgeType>>>();

        public void Connect(NodeType firstNode, NodeType secondNode, EdgeType edge)
        {
            if (graph.ContainsKey(firstNode) == false)
                graph[firstNode] = new Dictionary<NodeType, List<EdgeType>>();
            if (graph[firstNode].ContainsKey(secondNode) == false)
                graph[firstNode][secondNode] = new List<EdgeType>();
            graph[firstNode][secondNode].Add(edge);
        }

    }

    private class Area
    {

        public double proportion;
        public List<Area> children;
        public Rectangle rectangle;

        public Area()
        {
            children = new List<Area>();
        }

        public Area(double proportion)
        {
            this.proportion = proportion;
            children = new List<Area>();
        }

    }
    private class Room
    {

        public int index;
        public Area area;
        public RoomPreset preset;
        public List<Point> geometry;
        public List<Furniture> furniture;

        public Room(int index, Area area, RoomPreset preset)
        {
            this.index = index;
            this.area = area;
            this.preset = preset;
            geometry = new List<Point>();
            furniture = new List<Furniture>();
        }

        public override bool Equals(object obj)
        {
            Room objRoom = obj as Room;
            return index == objRoom.index;
        }
        public override int GetHashCode()
        {
            return index.GetHashCode();
        }

    }
    private class Furniture
    {

        public FurniturePreset preset;
        public Point position;
        public int orientation;
        public Rectangle boundary;

        public Furniture(FurniturePreset preset, Point position, int orientation, Rectangle boundary)
        {
            this.preset = preset;
            this.position = position;
            this.orientation = orientation;
            this.boundary = boundary;
        }

        public int GetID(int roomType)
        {
            if (preset.type < 0)
                return 0;
            return roomType * 1000 + preset.type * 10 + /*orientation*/ORIENTATION_MAPPING[orientation];
        }

    }

    private class RoomPreset
    {

        public int type;
        public int tile;
        public int connectionCount;
        public int furnitureCount;
        public List<(FurniturePreset preset, int min, int max)> viableFurniture;

        public RoomPreset(int type, int tile, List<(FurniturePreset, int, int)> viableFurniture)

        {
            this.type = type;
            this.tile = tile;
            this.viableFurniture = viableFurniture;
        }

    }
    private class FurniturePreset
    {

        public int type;
        public int placementFlag;
        public int orientationFlag;
        public int wallsideFlag;
        public Point size;

        public FurniturePreset(int type, int placement, int wallside, int orientation, (double width, double height) actual)
            : this(type, placement, wallside, orientation, new Point(actual.width, actual.height))
        {

        }
        public FurniturePreset(int type, int placement, int wallside, int orientation, Point size)
        {
            this.type = type;
            this.placementFlag = placement;
            this.wallsideFlag = wallside;
            this.orientationFlag = orientation;
            this.size = size;
        }

    }

    private void Partition(int recursion, List<Area> areaHierarchy, Rectangle boundary, Dictionary<double, SortedSet<double>> interceptX, Dictionary<double, SortedSet<double>> interceptY)
    {
        if (interceptX.ContainsKey(boundary.x1) == false)
            interceptX[boundary.x1] = new SortedSet<double>();
        interceptX[boundary.x1].Add(boundary.y1);
        interceptX[boundary.x1].Add(boundary.y2);
        if (interceptY.ContainsKey(boundary.y1) == false)
            interceptY[boundary.y1] = new SortedSet<double>();
        interceptY[boundary.y1].Add(boundary.x1);
        interceptY[boundary.y1].Add(boundary.x2);
        if (interceptX.ContainsKey(boundary.x2) == false)
            interceptX[boundary.x2] = new SortedSet<double>();
        interceptX[boundary.x2].Add(boundary.y1);
        interceptX[boundary.x2].Add(boundary.y2);
        if (interceptY.ContainsKey(boundary.y2) == false)
            interceptY[boundary.y2] = new SortedSet<double>();
        interceptY[boundary.y2].Add(boundary.x1);
        interceptY[boundary.y2].Add(boundary.x2);
        if (areaHierarchy.Count == 0)
            return;
        if (areaHierarchy.Count == 1)
        {
            areaHierarchy[0].rectangle = boundary;
            Partition(recursion + 1, areaHierarchy[0].children, boundary, interceptX, interceptY);
            return;
        }
        int partitionDirection = boundary.GetWidth() >= boundary.GetHeight() ? 0 : 1;
        double boundaryWidthStart, boundaryWidthEnd, boundaryWidthLength;
        double boundaryHeightStart, boundaryHeightEnd, boundaryHeightLength;
        if (partitionDirection == 0)
        {
            boundaryWidthStart = boundary.x1;
            boundaryWidthEnd = boundary.x2;
            boundaryWidthLength = boundary.GetWidth();
            boundaryHeightStart = boundary.y1;
            boundaryHeightEnd = boundary.y2;
            boundaryHeightLength = boundary.GetHeight();
        }
        else
        {
            boundaryWidthStart = boundary.y1;
            boundaryWidthEnd = boundary.y2;
            boundaryWidthLength = boundary.GetHeight();
            boundaryHeightStart = boundary.x1;
            boundaryHeightEnd = boundary.x2;
            boundaryHeightLength = boundary.GetWidth();
        }
        double totalProportion = 0;
        foreach (Area area in areaHierarchy)
            totalProportion += area.proportion;
        double cumulativeProportion = areaHierarchy[0].proportion;
        // calculate height here for more accuarte currentRatioError
        int partitionCount = 1;
        double partitionProportion = cumulativeProportion;
        double bestPartitionWidthLength = (cumulativeProportion / totalProportion) * boundaryWidthLength;
        List<double> bestPartitionHeightList = new List<double>();
        double bestRatioError = Math.Pow(Math.Log(boundaryHeightLength / bestPartitionWidthLength), 2);
        for (int i = 1; i < areaHierarchy.Count; i++)
        {
            cumulativeProportion += areaHierarchy[i].proportion;
            double currentPartitionWidthLength = (cumulativeProportion / totalProportion) * boundaryWidthLength;

            List<double> currentPartitionHeightList = new List<double>();
            double currentRatioError = 0;
            double subCumulativeProportion = 0;
            double previousHeight = boundaryHeightStart;
            for (int j = 0; j < partitionCount + 1; j++)
            {
                subCumulativeProportion += areaHierarchy[j].proportion;
                currentPartitionHeightList.Add(boundaryHeightStart + (subCumulativeProportion / cumulativeProportion) * boundaryHeightLength);
                currentRatioError += Math.Pow(Math.Log((currentPartitionHeightList[j] - previousHeight) / currentPartitionWidthLength), 2);
            }
            currentRatioError /= (partitionCount + 1);
            //double currentRatioError = Math.Abs(Math.Log(boundaryHeightLength / (currentPartitionWidthLength * (i + 1))));

            if (currentRatioError <= bestRatioError)
            {
                partitionCount++;
                partitionProportion = cumulativeProportion;
                bestPartitionWidthLength = currentPartitionWidthLength;

                bestPartitionHeightList = currentPartitionHeightList;

                bestRatioError = currentRatioError;
            }
            else
                break;
        }
        double partitionWidth;
        if (partitionCount < areaHierarchy.Count)
            partitionWidth = boundaryWidthStart + bestPartitionWidthLength;
        else
            partitionWidth = boundaryWidthEnd;
        List<double> partitionHeightList = new List<double>();
        partitionHeightList.Add(boundaryHeightStart);

        partitionHeightList.AddRange(bestPartitionHeightList);
        partitionHeightList.Add(boundaryHeightEnd);
        for (int i = 0; i < partitionCount; i++)
        {
            Rectangle subpartitionRectangle;
            if (partitionDirection == 0)
                subpartitionRectangle = new Rectangle(boundaryWidthStart, partitionHeightList[i], partitionWidth, partitionHeightList[i + 1]);
            else
                subpartitionRectangle = new Rectangle(partitionHeightList[i], boundaryWidthStart, partitionHeightList[i + 1], partitionWidth);
            areaHierarchy[i].rectangle = subpartitionRectangle;
            Partition(recursion + 1, areaHierarchy[i].children, subpartitionRectangle, interceptX, interceptY);
        }
        if (partitionCount < areaHierarchy.Count)
        {
            Rectangle remainingPartitionRectangle;
            if (partitionDirection == 0)
                remainingPartitionRectangle = new Rectangle(partitionWidth, boundaryHeightStart, boundaryWidthEnd, boundaryHeightEnd);
            else
                remainingPartitionRectangle = new Rectangle(boundaryHeightStart, partitionWidth, boundaryHeightEnd, boundaryWidthEnd);
            // doubt
            Partition(recursion, areaHierarchy.GetRange(partitionCount, areaHierarchy.Count - partitionCount), remainingPartitionRectangle, interceptX, interceptY);
        }
    }
    private bool VerifyPartition(List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            if (room.area.rectangle.GetWidth() < 5 || room.area.rectangle.GetHeight() < 3)
                return false;
        }
        return true;
    }
    private Dictionary<Point, HashSet<Room>> Geometrize(List<Room> roomList, Dictionary<double, SortedSet<double>> interceptX, Dictionary<double, SortedSet<double>> interceptY)
    {
        Dictionary<Point, HashSet<Room>> roomPointDictionary = new Dictionary<Point, HashSet<Room>>();
        foreach (Room room in roomList)
        {
            double x1 = room.area.rectangle.x1;
            double y1 = room.area.rectangle.y1;
            double x2 = room.area.rectangle.x2;
            double y2 = room.area.rectangle.y2;
            foreach (double x in interceptY[y1])
            {
                if (x < x1)
                    continue;
                if (x == x2)
                    break;
                else if (x > x2)
                    Debug.Log("PCG IMPOSSIBLE: FAULTY INTERCEPT");
                room.geometry.Add(new Point(x, y1));
            }
            foreach (double y in interceptX[x2])
            {
                if (y < y1)
                    continue;
                if (y == y2)
                    break;
                else if (y > y2)
                    Debug.Log("PCG IMPOSSIBLE: FAULTY INTERCEPT");
                room.geometry.Add(new Point(x2, y));
            }
            foreach (double x in interceptY[y2].Reverse())
            {
                if (x > x2)
                    continue;
                if (x == x1)
                    break;
                else if (x < x1)
                    Debug.Log("PCG IMPOSSIBLE: FAULTY INTERCEPT");
                room.geometry.Add(new Point(x, y2));
            }
            foreach (double y in interceptX[x1].Reverse())
            {
                if (y > y2)
                    continue;
                if (y == y1)
                    break;
                else if (y < y1)
                    Debug.Log("PCG IMPOSSIBLE: FAULTY INTERCEPT");
                room.geometry.Add(new Point(x1, y));
            }
            foreach (Point point in room.geometry)
            {
                if (roomPointDictionary.ContainsKey(point) == false)
                    roomPointDictionary[point] = new HashSet<Room>();
                roomPointDictionary[point].Add(room);
            }
        }
        return roomPointDictionary;
    }

    private Graph<Room, (Point, Point)> GenerateTreeConnection(Room rootRoom, List<Room> roomList, Dictionary<Point, HashSet<Room>> roomPointDictionary)
    {
        Graph<Room, (Point, Point)> roomGraph = new Graph<Room, (Point, Point)>();
        HashSet<Room> connected = new HashSet<Room>();
        connected.Add(rootRoom);
        Dictionary<Room, List<(Room, Point, Point)>> availableConnection = new Dictionary<Room, List<(Room, Point, Point)>>();
        for (int i = 0; i < rootRoom.geometry.Count; i++)
        {
            Point firstPoint = rootRoom.geometry[i];
            Point secondPoint = rootRoom.geometry[(i + 1) % rootRoom.geometry.Count];
            if (firstPoint.x == secondPoint.x && Math.Abs(firstPoint.y - secondPoint.y) <= 4)
                continue;
            if (firstPoint.y == secondPoint.y && Math.Abs(firstPoint.x - secondPoint.x) <= 6)
                continue;
            foreach (Room nextRoom in roomPointDictionary[firstPoint])
            {
                if (connected.Contains(nextRoom))
                    continue;
                /*if (roomGraph.graph[rootRoom].ContainsKey(nextRoom))
                    continue;*/
                if (roomPointDictionary[secondPoint].Contains(nextRoom) == false)
                    continue;
                if (availableConnection.ContainsKey(nextRoom) == false)
                    availableConnection[nextRoom] = new List<(Room, Point, Point)>();
                availableConnection[nextRoom].Add((rootRoom, firstPoint, secondPoint));
            }
        }
        while (connected.Count < roomList.Count)
        {
            int choice;
            Dictionary<Room, List<(Room, Point, Point)>>.KeyCollection.Enumerator keyEnumerator = availableConnection.Keys.GetEnumerator();
            keyEnumerator.MoveNext();
            choice = random.Next(availableConnection.Count);
            for (int i = 0; i < choice; i++)
                keyEnumerator.MoveNext();
            Room secondRoom = keyEnumerator.Current;
            choice = random.Next(availableConnection[secondRoom].Count);

            (Room firstRoom, Point firstPoint, Point  secondPoint) = availableConnection[secondRoom][choice];

            roomGraph.Connect(firstRoom, secondRoom, (firstPoint, secondPoint));
            roomGraph.Connect(secondRoom, firstRoom, (secondPoint, firstPoint));
            connected.Add(secondRoom);
            availableConnection.Remove(secondRoom);
            for (int i = 0; i < secondRoom.geometry.Count; i++)
            {
                Point thirdPoint = secondRoom.geometry[i];
                Point fourthPoint = secondRoom.geometry[(i + 1) % secondRoom.geometry.Count];
                if (thirdPoint.x == fourthPoint.x && Math.Abs(thirdPoint.y - fourthPoint.y) <= 4)
                    continue;
                if (thirdPoint.y == fourthPoint.y && Math.Abs(thirdPoint.x - fourthPoint.x) <= 6)
                    continue;
                foreach (Room thirdRoom in roomPointDictionary[thirdPoint])
                {
                    if (connected.Contains(thirdRoom))
                        continue;
                    /*if (roomGraph.graph[secondRoom].ContainsKey(thirdRoom))
                        continue;*/
                    if (roomPointDictionary[fourthPoint].Contains(thirdRoom) == false)
                        continue;
                    if (availableConnection.ContainsKey(thirdRoom) == false)
                        availableConnection[thirdRoom] = new List<(Room, Point, Point)>();
                    availableConnection[thirdRoom].Add((secondRoom, thirdPoint, fourthPoint));
                }
            }
        }
        return roomGraph;
    }
    private void GenerateAdditionalConnection(Room rootRoom, List<Room> roomList, Dictionary<Point, HashSet<Room>> roomPointDictionary, Graph<Room, (Point, Point)> roomGraph, int connectingPathLength)
    {
        Dictionary<Room, int> depthRoomDictionary = new Dictionary<Room, int>();
        depthRoomDictionary[rootRoom] = 0;
        Stack<Room> depthRoomStack = new Stack<Room>();
        depthRoomStack.Push(rootRoom);
        while (depthRoomStack.Count > 0)
        {
            Room firstRoom = depthRoomStack.Pop();
            foreach (Room secondRoom in roomGraph.graph[firstRoom].Keys)
            {
                if (depthRoomDictionary.ContainsKey(secondRoom))
                    continue;
                depthRoomDictionary[secondRoom] = depthRoomDictionary[firstRoom] + 1;
                depthRoomStack.Push(secondRoom);
            }
        }
        Dictionary<(Room, Room), (Point, Point)> additionalConnection = new Dictionary<(Room, Room), (Point, Point)>();
        foreach (Room firstRoom in roomList)
        {
            for (int i = 0; i < firstRoom.geometry.Count; i++)
            {
                Point firstPoint = firstRoom.geometry[i];
                Point secondPoint = firstRoom.geometry[(i + 1) % firstRoom.geometry.Count];
                if (firstPoint.x == secondPoint.x && Math.Abs(firstPoint.y - secondPoint.y) <= 4)
                    continue;
                if (firstPoint.y == secondPoint.y && Math.Abs(firstPoint.x - secondPoint.x) <= 6)
                    continue;
                foreach (Room secondRoom in roomPointDictionary[firstPoint])
                {
                    if (firstRoom.Equals(secondRoom))
                        continue;
                    if (roomPointDictionary[secondPoint].Contains(secondRoom) == false)
                        continue;
                    if (roomGraph.graph[firstRoom].ContainsKey(secondRoom))
                        continue;
                    if (additionalConnection.ContainsKey((secondRoom, firstRoom)))
                        continue;
                    Room firstReferenceRoom = firstRoom;
                    Room secondReferenceRoom = secondRoom;
                    int pathLength = 0;
                    while (firstReferenceRoom.Equals(secondReferenceRoom) == false)
                    {
                        // CLIMB THE TREE!
                        if (depthRoomDictionary[firstReferenceRoom] < depthRoomDictionary[secondReferenceRoom])
                        {
                            foreach (Room nextReferenceRoom in roomGraph.graph[secondReferenceRoom].Keys)
                            {
                                if (depthRoomDictionary[nextReferenceRoom] != depthRoomDictionary[secondReferenceRoom] - 1)
                                    continue;
                                secondReferenceRoom = nextReferenceRoom;
                                pathLength++;
                                break;
                            }
                        }
                        else
                        {
                            foreach (Room nextReferenceRoom in roomGraph.graph[firstReferenceRoom].Keys)
                            {
                                if (depthRoomDictionary[nextReferenceRoom] != depthRoomDictionary[firstReferenceRoom] - 1)
                                    continue;
                                firstReferenceRoom = nextReferenceRoom;
                                pathLength++;
                                break;
                            }
                        }
                    }
                    if (pathLength < connectingPathLength)
                        continue;
                    additionalConnection[(firstRoom, secondRoom)] = (firstPoint, secondPoint);
                }
            }
        }
        foreach ((Room firstRoom, Room secondRoom) in additionalConnection.Keys)
        {
            (Point firstPoint, Point secondPoint) = additionalConnection[(firstRoom, secondRoom)];
            roomGraph.Connect(firstRoom, secondRoom, (firstPoint, secondPoint));
            roomGraph.Connect(secondRoom, firstRoom, (secondPoint, firstPoint));
        }
    }

    private void PlaceDoor(Dictionary<double, SortedSet<double>> interceptY, Dictionary<Point, HashSet<Room>> roomPointDictionary, Graph<Room, (Point, Point)> roomGraph, double width, double height)
    {
        Point firstMiddle = new Point(width, height);
        Point secondMiddle = new Point(0, height);
        foreach (double x in interceptY[height])
        {
            if (x < width / 2)
            {
                secondMiddle.x = x;
                continue;
            }
            else
            {
                firstMiddle.x = x;
                break;
            }
        }
        HashSet<(Room, Room)> doorPlaced = new HashSet<(Room, Room)>();
        foreach (Room firstRoom in roomGraph.graph.Keys)
        {
            if (roomPointDictionary[firstMiddle].Contains(firstRoom) && roomPointDictionary[secondMiddle].Contains(firstRoom))
            {
                double entranceX = (secondMiddle.x + 1) + (firstMiddle.x - secondMiddle.x - 5) * random.NextDouble();
                double entranceY = firstMiddle.y;
                Rectangle entranceBoundary = new Rectangle(entranceX, entranceY - 4, entranceX + 4, entranceY);
                firstRoom.furniture.Add(new Furniture(ENTRANCE, new Point(entranceX, entranceY), 2, entranceBoundary));
            }
            foreach (Room secondRoom in roomGraph.graph[firstRoom].Keys)
            {
                if (doorPlaced.Contains((secondRoom, firstRoom)))
                    continue;
                (Point firstPoint, Point secondPoint) = roomGraph.graph[firstRoom][secondRoom][0]; // one door per connection
                if (firstPoint.x < secondPoint.x && firstPoint.y == secondPoint.y)
                {
                    double doorX = (firstPoint.x + 1) + (secondPoint.x - firstPoint.x - 4) * random.NextDouble();
                    double doorY = firstPoint.y;
                    Point doorPosition = new Point(doorX, doorY);
                    Rectangle firstRoomDoorBoundary = new Rectangle(doorX, doorY, doorX + 3, doorY + 3);
                    Rectangle secondRoomDoorBoundary = new Rectangle(doorX, doorY - 3, doorX + 3, doorY);
                    firstRoom.furniture.Add(new Furniture(DOOR_0, doorPosition, 0, firstRoomDoorBoundary));
                    secondRoom.furniture.Add(new Furniture(DOOR_2, doorPosition, 2, secondRoomDoorBoundary));
                }
                if (firstPoint.x == secondPoint.x && firstPoint.y < secondPoint.y)
                {
                    double doorX = firstPoint.x;
                    double doorY = (firstPoint.y + 1) + (secondPoint.y - firstPoint.y - 4) * random.NextDouble();
                    Point doorPosition = new Point(doorX, doorY);
                    Rectangle firstRoomDoorBoundary = new Rectangle(doorX - 3, doorY, doorX, doorY + 3);
                    Rectangle secondRoomDoorBoundary = new Rectangle(doorX, doorY, doorX + 5, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(DOOR_1, doorPosition, 1, firstRoomDoorBoundary));
                    secondRoom.furniture.Add(new Furniture(DOOR_3, doorPosition, 3, secondRoomDoorBoundary));
                }
                if (firstPoint.x > secondPoint.x && firstPoint.y == secondPoint.y)
                {
                    double doorX = (secondPoint.x + 1) + (firstPoint.x - secondPoint.x - 4) * random.NextDouble();
                    double doorY = firstPoint.y;
                    Point doorPosition = new Point(doorX, doorY);
                    Rectangle firstRoomDoorBoundary = new Rectangle(doorX, doorY - 3, doorX + 3, doorY);
                    Rectangle secondRoomDoorBoundary = new Rectangle(doorX, doorY, doorX + 3, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(DOOR_2, doorPosition, 2, firstRoomDoorBoundary));
                    secondRoom.furniture.Add(new Furniture(DOOR_0, doorPosition, 0, secondRoomDoorBoundary));
                }
                if (firstPoint.x == secondPoint.x && firstPoint.y > secondPoint.y)
                {
                    double doorX = firstPoint.x;
                    double doorY = (secondPoint.y + 1) + (firstPoint.y - secondPoint.y - 4) * random.NextDouble();
                    Point doorPosition = new Point(doorX, doorY);
                    Rectangle firstRoomDoorBoundary = new Rectangle(doorX, doorY, doorX + 5, doorY + 3);
                    Rectangle secondRoomDoorBoundary = new Rectangle(doorX - 3, doorY, doorX, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(DOOR_3, doorPosition, 3, firstRoomDoorBoundary));
                    secondRoom.furniture.Add(new Furniture(DOOR_1, doorPosition, 1, secondRoomDoorBoundary));
                }
                doorPlaced.Add((firstRoom, secondRoom));
            }
        }
    }
    private void PlaceValve(List<Room> roomList, int valveCount)
    {
        List<Room> roomListClone = new List<Room>(roomList);
        for (int i = 0; i < valveCount && roomListClone.Count > 0; i++)
        {
            Room valveRoom = RandomChoicePop(roomListClone);
            double valveX = (valveRoom.area.rectangle.x1 + 3) + (valveRoom.area.rectangle.GetWidth() - 4) * random.NextDouble();
            double valveY = (valveRoom.area.rectangle.y1 + 1) + (valveRoom.area.rectangle.GetHeight() - 2) * random.NextDouble();
            valveRoom.furniture.Add(new Furniture(VALVE, new Point(valveX, valveY), 0, new Rectangle(valveX, valveY, valveX + 1, valveY + 1)));
        }
    }
    private bool PlaceFurniture2(List<Room> roomList, int attemptPerRoom)
    {
        int choice;
        foreach (Room room in roomList)
        {
            int attempt = 0;
            int successFurniture = 0;
            int preferedFurnitureCount = RoundPCG(room.area.rectangle.GetWidth() * room.area.rectangle.GetHeight() / 10);
            List<(FurniturePreset preset, int min, int max)> furnitureChoice = new List<(FurniturePreset, int, int)>(room.preset.viableFurniture); // the only good constructor is a copy constructor
            List<int> furnitureTally = new List<int>(furnitureChoice.Count); // capacity >= count
            Stack<FurniturePreset> requiredFurniture = new Stack<FurniturePreset>();
            for (int i = 0; i < furnitureChoice.Count; i++)
            {
                for (int j = 0; j < furnitureChoice[i].min; j++)
                    requiredFurniture.Push(furnitureChoice[i].preset);
                furnitureTally.Add(furnitureChoice[i].max - furnitureChoice[i].min);
            }
            while (attempt < attemptPerRoom * Math.Max(1, preferedFurnitureCount - successFurniture))
            {
                attempt++;
                FurniturePreset preset;
                int furnitureChoiceIndex = -1;
                if (requiredFurniture.Count > 0)
                    preset = requiredFurniture.Peek();
                else
                {
                    if (furnitureChoice.Count == 0)
                        break;
                    choice = random.Next(furnitureChoice.Count);
                    if (furnitureTally[choice] == 0) // == is intended
                    {
                        furnitureTally.RemoveAt(choice);
                        furnitureChoice.RemoveAt(choice);
                        continue;
                    }
                    furnitureChoiceIndex = choice;
                    preset = furnitureChoice[choice].preset;
                }
                double furnitureX, furnitureY;
                double furnitureSizeX, furnitureSizeY;
                int furnitureOrientation;
                int placement = (preset.placementFlag == 3) ? 1 + random.Next(2) : preset.placementFlag;
                if (placement == PLACE_FLOOR)
                {
                    List<int> allowedOrientation = new List<int>();
                    for (int j = 0; j < 4; j++)
                    {
                        if (((preset.orientationFlag >> j) & 1) > 0)
                        {
                            allowedOrientation.Add(j);
                        }
                    }
                    choice = random.Next(allowedOrientation.Count);
                    furnitureOrientation = allowedOrientation[choice];
                    if (furnitureOrientation % 2 == 0)
                    {
                        furnitureSizeX = preset.size.x;
                        furnitureSizeY = preset.size.y;
                    }
                    else
                    {
                        furnitureSizeX = preset.size.y;
                        furnitureSizeY = preset.size.x;
                    }
                    furnitureX = (room.area.rectangle.x1 + 4) + (room.area.rectangle.GetWidth() - furnitureSizeX - 5) * random.NextDouble();
                    furnitureY = (room.area.rectangle.y1 + 2) + (room.area.rectangle.GetHeight() - furnitureSizeY - 3) * random.NextDouble();
                }
                else // PLACE_WALL
                {
                    List<int> allowedWallside = new List<int>();
                    for (int j = 0; j < 4; j++)
                    {
                        if (((preset.wallsideFlag >> j) & 1) > 0)
                            allowedWallside.Add(j);
                    }
                    choice = random.Next(allowedWallside.Count);
                    int wallside = allowedWallside[choice];
                    List<int> allowedOrientation = new List<int>();
                    for (int j = 0; j < 8; j++)
                    {
                        if (((preset.orientationFlag >> j) & 1) > 0)
                        {
                            if (j < 4)
                                allowedOrientation.Add(j);
                            else
                                allowedOrientation.Add((wallside + j) % 4);
                        }
                    }
                    choice = random.Next(allowedOrientation.Count);
                    furnitureOrientation = allowedOrientation[choice];
                    if (furnitureOrientation % 2 == 0)
                    {
                        furnitureSizeX = preset.size.x;
                        furnitureSizeY = preset.size.y;
                    }
                    else
                    {
                        furnitureSizeX = preset.size.y;
                        furnitureSizeY = preset.size.x;
                    }
                    if (wallside == 0)
                    {
                        furnitureX = (room.area.rectangle.x1 + 3) + (room.area.rectangle.GetWidth() - furnitureSizeX - 3) * random.NextDouble();
                        furnitureY = room.area.rectangle.y1 + 1;
                    }
                    else if (wallside == 1)
                    {
                        furnitureX = room.area.rectangle.x2 - furnitureSizeX;
                        furnitureY = (room.area.rectangle.y1 + 1) + (room.area.rectangle.GetHeight() - furnitureSizeY - 1) * random.NextDouble();
                    }
                    else if (wallside == 2)
                    {
                        furnitureX = (room.area.rectangle.x1 + 3) + (room.area.rectangle.GetWidth() - furnitureSizeX - 3) * random.NextDouble();
                        furnitureY = room.area.rectangle.y2 - furnitureSizeY;
                    }
                    else /*(allowedWallside[side] == 3)*/
                    {
                        furnitureX = room.area.rectangle.x1 + 3;
                        furnitureY = (room.area.rectangle.y1 + 1) + (room.area.rectangle.GetHeight() - furnitureSizeY - 1) * random.NextDouble();
                    }
                }
                Point furniturePosition = new Point(furnitureX, furnitureY);
                Rectangle furnitureBoundary;
                if (room.area.rectangle.GetWidth() - 4 < furnitureSizeX || room.area.rectangle.GetHeight() - 2 < furnitureSizeY)
                {
                    if (requiredFurniture.Count > 0)
                        // required furniture larger than (roomsize - 1)
                        return false;
                    continue;
                }
                furnitureBoundary = new Rectangle(furnitureX - 1, furnitureY - 1, furnitureX + furnitureSizeX + 1, furnitureY + furnitureSizeY + 1);
                bool collide = false;
                foreach (Furniture placedFurniture in room.furniture)
                {
                    if (placedFurniture.boundary.Collide(furnitureBoundary))
                    {
                        collide = true;
                        break;
                    }
                }
                if (collide)
                {
                    //Debug.Log("Collide");
                    continue;
                }
                room.furniture.Add(new Furniture(preset, furniturePosition, furnitureOrientation, furnitureBoundary));
                if (requiredFurniture.Count > 0)
                    requiredFurniture.Pop();
                else /*if (furnitureChoiceIndex != -1)*/
                    --furnitureTally[furnitureChoiceIndex];
                successFurniture++;
            }
            if (requiredFurniture.Count > 0)
                // required furniture unfit
                return false;
        }
        return true;
    }
    private void PlaceFireAndCat(List<Room> roomList, int fireCount, int catCount)
    {
        List<Room> roomListClone = new List<Room>(roomList);
        int choice;
        for (int i = 0; i < catCount; i++)
        {
            if (roomListClone.Count == 0)
                return;
            choice = random.Next(roomListClone.Count);
            Room catRoom = roomListClone[choice];
            double catX = (catRoom.area.rectangle.x1 + 3) + (catRoom.area.rectangle.GetWidth() - 4) * random.NextDouble();
            double catY = (catRoom.area.rectangle.y1 + 1) + (catRoom.area.rectangle.GetHeight() - 2) * random.NextDouble();
            catRoom.furniture.Add(new Furniture(CAT, new Point(catX, catY), 0, new Rectangle(catX, catY, catX + 1, catY + 1)));
            roomListClone.RemoveAt(choice);
        }
        for (int i = 0; i < fireCount; i++)
        {
            if (roomListClone.Count == 0)
                return;
            choice = random.Next(roomListClone.Count);
            Room fireRoom = roomListClone[choice];
            double fireX = (fireRoom.area.rectangle.x1 + 3) + (fireRoom.area.rectangle.GetWidth() - 6) * random.NextDouble();
            double fireY = (fireRoom.area.rectangle.y1 + 1) + (fireRoom.area.rectangle.GetHeight() - 4) * random.NextDouble();
            //fireRoom.furniture.Add(new Furniture(FIRE, new Point(fireX, fireY), 0, new Rectangle(fireX, fireY, fireX + 3, fireY + 3)));
            fireRoom.furniture.Add(new Furniture(FIRE, new Point(fireX, fireY), 0, ZERO_RECT));
            roomListClone.RemoveAt(choice);
        }
    }
    private void PlacePainting(List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            double paintingX = room.area.rectangle.x1 + 1;
            List<double> doorY = new List<double>();
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.preset.type == DOOR_3.type)
                    doorY.Add(furniture.position.y);
            }
            double currentY = room.area.rectangle.y1 + 1;
            while (currentY + 6 < room.area.rectangle.y2)
            {
                double paintingY = currentY + 5 * random.NextDouble();
                bool rerollFlag = true;
                while (rerollFlag)
                {
                    rerollFlag = false;
                    foreach (double y in doorY)
                    {
                        if (paintingY >= y - 1 && paintingY < y + 2)
                        {
                            paintingY = currentY + 5 * random.NextDouble(); // bogo search
                            rerollFlag = true;
                            break;
                        }
                    }
                }
                room.furniture.Add(new Furniture(PAINTING, new Point(paintingX, paintingY), 3, ZERO_RECT));
                currentY += 6;
            }
        }
    }


    private void DrawRoom(NDArray roomArray, List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            int x1 = RoundPCG(room.area.rectangle.x1);
            int y1 = RoundPCG(room.area.rectangle.y1);
            int x2 = RoundPCG(room.area.rectangle.x2);
            int y2 = RoundPCG(room.area.rectangle.y2);
            roomArray[x1.ToString() + ":" + (x2 + 1).ToString(), y1.ToString() + ":" + (y2 + 1).ToString()] = 1;
            roomArray[(x1 + 3).ToString() + ":" + x2.ToString(), (y1 + 1).ToString() + ":" + y2.ToString()] = room.preset.tile;
            roomArray[(x1 + 1).ToString() + ":" + (x1 + 3).ToString(), (y1 + 1).ToString() + ":" + y2.ToString()] = 2;
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.preset.type == ENTRANCE_TYPE)
                {
                    int entranceX = RoundPCG(furniture.position.x);
                    int entranceY = RoundPCG(furniture.position.y);
                    //int entranceY = roomArray.shape[1] - 1;
                    roomArray[entranceX.ToString() + ":" + (entranceX + 2).ToString() + ", " + entranceY.ToString()] = 2;
                    roomArray[(entranceX + 2).ToString() + ":" + (entranceX + 4).ToString() + ", " + entranceY.ToString()] = 0;
                }
                if (furniture.preset.type == -1)
                {
                    int doorX = RoundPCG(furniture.position.x);
                    int doorY = RoundPCG(furniture.position.y);
                    roomArray[doorX.ToString() + ":" + (doorX + 2).ToString() + ", " + doorY.ToString()] = 2;
                    roomArray[(doorX + 2).ToString() + ":" + (doorX + 3).ToString() + ", " + doorY.ToString()] = 0;
                }
                if (furniture.preset.type == -2)
                {
                    int doorX = RoundPCG(furniture.position.x);
                    int doorY = RoundPCG(furniture.position.y) + 1;
                    roomArray[doorX.ToString() + ", " + doorY.ToString()] = 0;
                }
                if (furniture.preset.type == -3)
                {
                    int doorX = RoundPCG(furniture.position.x);
                    int doorY = RoundPCG(furniture.position.y);
                    roomArray[doorX.ToString() + ":" + (doorX + 2).ToString() + ", " + doorY.ToString()] = 2;
                    roomArray[(doorX + 2).ToString() + ":" + (doorX + 3).ToString() + ", " + doorY.ToString()] = 0;
                }
                if (furniture.preset.type == -4)
                {
                    int doorX = RoundPCG(furniture.position.x);
                    int doorY = RoundPCG(furniture.position.y) + 1;
                    roomArray[doorX.ToString() + ":" + (doorX + 3).ToString() + ", " + doorY.ToString()] = 0;
                }
            }
        }
    }
    private void DrawDoor(NDArray doorArray, List<Room> roomList, int doorCount)
    {
        List<Point> doorPixel = new List<Point>();
        foreach (Room room in roomList)
        {
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.preset.type == ENTRANCE_TYPE)
                {
                    int x = RoundPCG(furniture.position.x) + 2;
                    int y = RoundPCG(furniture.position.y);
                    //int y = doorArray.shape[1] - 1;
                    entranceDoor = new Vector2(y, -x);
                    doorArray[x.ToString() + ", " + y.ToString()] = 8;
                }
                else if (furniture.preset.type == -1)
                {
                    int x = RoundPCG(furniture.position.x) + 2;
                    int y = RoundPCG(furniture.position.y);
                    doorArray[x.ToString() + ", " + y.ToString()] = 1;
                    doorPixel.Add(new Point(x, y));
                }
                else if (furniture.preset.type == -4)
                {
                    int x = RoundPCG(furniture.position.x) + 2;
                    int y = RoundPCG(furniture.position.y) + 1;
                    doorArray[x.ToString() + ", " + y.ToString()] = 2;
                    doorPixel.Add(new Point(x, y));
                }
            }
        }
        while (doorPixel.Count > doorCount)
        {
            int index = random.Next(doorPixel.Count);
            doorArray[doorPixel[index].x.ToString() + ", " + doorPixel[index].y.ToString()] = 0;
            doorPixel.RemoveAt(index); // O(n) intensify
        }
    }
    private void DrawFurniture(NDArray furnitureArray, List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            foreach (Furniture furniture in room.furniture)
            {
                int x = RoundPCG(furniture.position.x);
                int y = RoundPCG(furniture.position.y);
                if (furniture.preset.type == CAT_TYPE)
                {
                    furnitureArray[x.ToString() + ", " + y.ToString()] = 51 + random.Next(4);
                }
                else if (furniture.preset.type == PAINTING_TYPE)
                {
                    furnitureArray[x.ToString() + ", " + y.ToString()] = 41 + random.Next(6);
                }
                else if (furniture.preset.type == VALVE_TYPE)
                {
                    furnitureArray[x.ToString() + ", " + y.ToString()] = 6;
                }
                else if (furniture.preset.type >= 0)
                {
                    int width = RoundPCG(furniture.boundary.GetWidth()) - 2;
                    int height = RoundPCG(furniture.boundary.GetHeight()) - 2;
                    if (!DebugFurnitureBoundary)
                        furnitureArray[x.ToString() + ", " + y.ToString()] = furniture.GetID(room.preset.type);
                    else
                        furnitureArray[x.ToString() + ":" + (x + width).ToString() + ", " + y.ToString() + ":" + (y + height).ToString()] = furniture.GetID(room.preset.type);
                }
            }
        }
    }
    private void DrawFire(NDArray fireArray, List<Room> roomList, int fireCount)
    {
        foreach (Room room in roomList)
        {
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.preset.type != FIRE_TYPE)
                    continue;
                int fireX = RoundPCG(furniture.position.x);
                int fireY = RoundPCG(furniture.position.y);
                fireArray[fireX.ToString() + ", " + fireY.ToString()] = 1;
            }
        }
    }
    // private void DrawEverything(); // nah

    public List<List<int>> DebugHouseHierarchy(int roomCount, int firstBreadth)
    {
        List<List<int>> houseHierarchy = new List<List<int>>();
        List<double> tempList1 = new List<double>();
        double sum = 0;
        for (int i = 0; i < firstBreadth; i++)
        {
            tempList1.Add(random.NextDouble());
            sum += tempList1[tempList1.Count - 1];
        }
        double unit = sum / (roomCount - firstBreadth);
        double run = 0;
        for (int i = 0; i < firstBreadth; i++)
        {
            List<int> tempList2 = new List<int>();
            tempList2.Add(1 + random.Next(3));
            while (run < tempList1[i] - 0.000001)
            {
                tempList2.Add(1 + random.Next(3));
                run += unit;
            }
            houseHierarchy.Add(tempList2);
            run -= tempList1[i];
        }
        return houseHierarchy;
    }

    private static void InitializePCG()
    {
        ROOM_PRESET_DICTIONARY = new Dictionary<int, List<RoomPreset>>();
        ROOM_PRESET_DICTIONARY[1] = new List<RoomPreset>();
        ROOM_PRESET_DICTIONARY[2] = new List<RoomPreset>();
        ROOM_PRESET_DICTIONARY[3] = new List<RoomPreset>();
        {
            // Bedroom
            FurniturePreset doubleBed          = new FurniturePreset(1, PLACE_WALL, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_WALL_0, (2, 2));
            FurniturePreset singleBed          = new FurniturePreset(2, PLACE_WALL, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_WALL_0, (1, 2));
            FurniturePreset closet             = new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (2, 1));
            FurniturePreset bookshelf          = new FurniturePreset(4, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1));
            FurniturePreset smallChairAndTable = new FurniturePreset(5, PLACE_FLOOR, 0, ORIENT_ANY, (1, 2));

            RoomPreset bedroomPreset1 = new RoomPreset(1, 0, new List<(FurniturePreset, int, int)>());
            bedroomPreset1.viableFurniture.Add((doubleBed, 1, 1));
            bedroomPreset1.viableFurniture.Add((singleBed, 0, 0));
            bedroomPreset1.viableFurniture.Add((closet, 1, 3));
            bedroomPreset1.viableFurniture.Add((bookshelf, 0, 2));
            bedroomPreset1.viableFurniture.Add((smallChairAndTable, 0, 2));

            RoomPreset bedroomPreset2 = new RoomPreset(1, 0, new List<(FurniturePreset, int, int)>());
            bedroomPreset2.viableFurniture.Add((doubleBed, 1, 1));
            bedroomPreset2.viableFurniture.Add((singleBed, 0, 1));
            bedroomPreset2.viableFurniture.Add((closet, 1, 3));
            bedroomPreset2.viableFurniture.Add((bookshelf, 0, 1));
            bedroomPreset2.viableFurniture.Add((smallChairAndTable, 1, 3));

            RoomPreset bedroomPreset3 = new RoomPreset(1, 0, new List<(FurniturePreset, int, int)>());
            bedroomPreset3.viableFurniture.Add((doubleBed, 0, 0));
            bedroomPreset3.viableFurniture.Add((singleBed, 1, 3));
            bedroomPreset3.viableFurniture.Add((closet, 1, 3));
            bedroomPreset3.viableFurniture.Add((bookshelf, 0, 3));
            bedroomPreset3.viableFurniture.Add((smallChairAndTable, 0, 2));

            ROOM_PRESET_DICTIONARY[1].Add(bedroomPreset1);
            ROOM_PRESET_DICTIONARY[1].Add(bedroomPreset2);
            ROOM_PRESET_DICTIONARY[1].Add(bedroomPreset3);

            /*List<(FurniturePreset, int, int)> viableFurniture = new List<(FurniturePreset, int, int)>();
            //viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 3)), 1, -1));
            viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_WALL_0, (2, 2)), 1, 1));
            viableFurniture.Add((new FurniturePreset(2, PLACE_WALL, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_WALL_0, (1, 2)), 0, -1));
            viableFurniture.Add((new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 2)), 0, -1));
            viableFurniture.Add((new FurniturePreset(4, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 0, -1));
            viableFurniture.Add((new FurniturePreset(5, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 2)), 0, -1));
            ROOM_PRESET_DICTIONARY[1].Add(new RoomPreset(1, 0, viableFurniture));*/

        }
        {
            // Bathroom
            FurniturePreset toilet  = new FurniturePreset(1, PLACE_WALL, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_WALL_0, (1, 1));
            FurniturePreset bathtub = new FurniturePreset(2, PLACE_WALL, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 2));
            FurniturePreset shower  = new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1));
            FurniturePreset basin   = new FurniturePreset(4, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1));

            RoomPreset bathroomPreset1 = new RoomPreset(2, -1, new List<(FurniturePreset, int, int)>());
            bathroomPreset1.viableFurniture.Add((toilet, 1, 1));
            bathroomPreset1.viableFurniture.Add((bathtub, 0, 0));
            bathroomPreset1.viableFurniture.Add((shower, 0, 1));
            bathroomPreset1.viableFurniture.Add((basin, 1, 2));

            RoomPreset bathroomPreset2 = new RoomPreset(2, -1, new List<(FurniturePreset, int, int)>());
            bathroomPreset2.viableFurniture.Add((toilet, 0, 1));
            bathroomPreset2.viableFurniture.Add((bathtub, 0, 1));
            bathroomPreset2.viableFurniture.Add((shower, 1, 1));
            bathroomPreset2.viableFurniture.Add((basin, 1, 2));

            ROOM_PRESET_DICTIONARY[2].Add(bathroomPreset1);
            ROOM_PRESET_DICTIONARY[2].Add(bathroomPreset2);

            /*List<(FurniturePreset, int, int)> viableFurniture = new List<(FurniturePreset, int, int)>();
            //viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 3)), 1, -1));
            viableFurniture.Add((new FurniturePreset(1, PLACE_ANY, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_RIGHT | ORIENT_FLOOR_UP | ORIENT_WALL_0, (1, 1)), 1, 1));
            viableFurniture.Add((new FurniturePreset(2, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 2)), 1, -1));
            viableFurniture.Add((new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 1, -1));
            viableFurniture.Add((new FurniturePreset(4, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 1, -1));
            ROOM_PRESET_DICTIONARY[2].Add(new RoomPreset(2, -1, viableFurniture));

            viableFurniture = new List<(FurniturePreset, int, int)>();
            //viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 3)), 1, -1));
            viableFurniture.Add((new FurniturePreset(1, PLACE_ANY, WALL_LEFT | WALL_RIGHT | WALL_UP, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_RIGHT | ORIENT_FLOOR_UP | ORIENT_WALL_0, (1, 1)), 1, 1));
            viableFurniture.Add((new FurniturePreset(2, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 2)), 0, 0));
            viableFurniture.Add((new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 0, 0));
            viableFurniture.Add((new FurniturePreset(4, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 1, -1));
            ROOM_PRESET_DICTIONARY[2].Add(new RoomPreset(2, -1, viableFurniture));*/
        }
        {
            // Livingroom
            FurniturePreset quadChairAndBigTable         = new FurniturePreset(1, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (2, 4));
            FurniturePreset bookshelf                    = new FurniturePreset(2, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1));
            FurniturePreset plantPot                     = new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1));
            FurniturePreset smallSofa                    = new FurniturePreset(4, PLACE_ANY, WALL_ANY, ORIENT_ANY, (1, 1));
            FurniturePreset bigSofa                      = new FurniturePreset(5, PLACE_ANY, WALL_ANY, ORIENT_ANY, (1, 2));
            FurniturePreset mixedSofaAndTable            = new FurniturePreset(6, PLACE_ANY, WALL_ANY, ORIENT_ANY, (4, 2));
            FurniturePreset doubleChairAndSmallTable     = new FurniturePreset(7, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3));
            FurniturePreset doubleSmallSofaAndRoundChair = new FurniturePreset(8, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3));
            FurniturePreset drawer                       = new FurniturePreset(9, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 1));

            RoomPreset livingRoomPreset1 = new RoomPreset(3, 0, new List<(FurniturePreset, int, int)>());
            livingRoomPreset1.viableFurniture.Add((quadChairAndBigTable, 1, 1));
            livingRoomPreset1.viableFurniture.Add((bookshelf, 0, 1));
            livingRoomPreset1.viableFurniture.Add((plantPot, 2, 6));
            livingRoomPreset1.viableFurniture.Add((smallSofa, 0, 1));
            livingRoomPreset1.viableFurniture.Add((bigSofa, 0, 0));
            livingRoomPreset1.viableFurniture.Add((mixedSofaAndTable, 0, 0));
            livingRoomPreset1.viableFurniture.Add((doubleChairAndSmallTable, 1, 2));
            livingRoomPreset1.viableFurniture.Add((doubleSmallSofaAndRoundChair, 0, 0));
            livingRoomPreset1.viableFurniture.Add((drawer, 0, 2));

            RoomPreset livingRoomPreset2 = new RoomPreset(3, 0, new List<(FurniturePreset, int, int)>());
            livingRoomPreset2.viableFurniture.Add((quadChairAndBigTable, 1, 1));
            livingRoomPreset2.viableFurniture.Add((bookshelf, 2, 6));
            livingRoomPreset2.viableFurniture.Add((plantPot, 0, 1));
            livingRoomPreset2.viableFurniture.Add((smallSofa, 0, 2));
            livingRoomPreset2.viableFurniture.Add((bigSofa, 0, 2));
            livingRoomPreset2.viableFurniture.Add((mixedSofaAndTable, 0, 2));
            livingRoomPreset2.viableFurniture.Add((doubleChairAndSmallTable, 0, 0));
            livingRoomPreset2.viableFurniture.Add((doubleSmallSofaAndRoundChair, 0, 2));
            livingRoomPreset2.viableFurniture.Add((drawer, 0, 1));

            RoomPreset livingRoomPreset3 = new RoomPreset(3, 0, new List<(FurniturePreset, int, int)>());
            livingRoomPreset3.viableFurniture.Add((quadChairAndBigTable, 0, 0));
            livingRoomPreset3.viableFurniture.Add((bookshelf, 0, 2));
            livingRoomPreset3.viableFurniture.Add((plantPot, 1, 4));
            livingRoomPreset3.viableFurniture.Add((smallSofa, 0, 2));
            livingRoomPreset3.viableFurniture.Add((bigSofa, 0, 0));
            livingRoomPreset3.viableFurniture.Add((mixedSofaAndTable, 0, 0));
            livingRoomPreset3.viableFurniture.Add((doubleChairAndSmallTable, 1, 3));
            livingRoomPreset3.viableFurniture.Add((doubleSmallSofaAndRoundChair, 0, 0));
            livingRoomPreset3.viableFurniture.Add((drawer, 1, 1));

            RoomPreset livingRoomPreset4 = new RoomPreset(3, 0, new List<(FurniturePreset, int, int)>());
            livingRoomPreset4.viableFurniture.Add((quadChairAndBigTable, 0, 0));
            livingRoomPreset4.viableFurniture.Add((bookshelf, 0, 2));
            livingRoomPreset4.viableFurniture.Add((plantPot, 0, 4));
            livingRoomPreset4.viableFurniture.Add((smallSofa, 0, 1));
            livingRoomPreset4.viableFurniture.Add((bigSofa, 0, 0));
            livingRoomPreset4.viableFurniture.Add((mixedSofaAndTable, 0, 1));
            livingRoomPreset4.viableFurniture.Add((doubleChairAndSmallTable, 0, 1));
            livingRoomPreset4.viableFurniture.Add((doubleSmallSofaAndRoundChair, 0, 3));
            livingRoomPreset4.viableFurniture.Add((drawer, 3, 5));

            ROOM_PRESET_DICTIONARY[3].Add(livingRoomPreset1);
            ROOM_PRESET_DICTIONARY[3].Add(livingRoomPreset2);
            ROOM_PRESET_DICTIONARY[3].Add(livingRoomPreset3);
            ROOM_PRESET_DICTIONARY[3].Add(livingRoomPreset4);

            /*List<(FurniturePreset, int, int)> viableFurniture = new List<(FurniturePreset, int, int)>();
            //viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 3)), 1, -1));
            viableFurniture.Add((new FurniturePreset(1, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (2, 4)), 1, 1));
            viableFurniture.Add((new FurniturePreset(2, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 0, -1));
            viableFurniture.Add((new FurniturePreset(3, PLACE_WALL, WALL_UP, ORIENT_WALL_0, (1, 1)), 0, -1));
            viableFurniture.Add((new FurniturePreset(4, PLACE_ANY, WALL_ANY, ORIENT_ANY, (1, 1)), 0, -1));
            viableFurniture.Add((new FurniturePreset(5, PLACE_ANY, WALL_ANY, ORIENT_ANY, (1, 2)), 0, -1));
            viableFurniture.Add((new FurniturePreset(6, PLACE_ANY, WALL_ANY, ORIENT_ANY, (2, 4)), 0, -1));
            viableFurniture.Add((new FurniturePreset(7, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3)), 0, -1));
            viableFurniture.Add((new FurniturePreset(8, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3)), 0, -1));
            viableFurniture.Add((new FurniturePreset(9, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 1)), 0, -1));
            ROOM_PRESET_DICTIONARY[3].Add(new RoomPreset(3, 0, viableFurniture));

            viableFurniture = new List<(FurniturePreset, int, int)>();
            //viableFurniture.Add((new FurniturePreset(1, PLACE_WALL, WALL_ANY, ORIENT_WALL_0, (1, 3)), 1, -1));
            
            viableFurniture.Add((new FurniturePreset(7, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3)), 0, -1));
            viableFurniture.Add((new FurniturePreset(8, PLACE_ANY, WALL_ANY, ORIENT_FLOOR_LEFT | ORIENT_FLOOR_UP, (1, 3)), 0, -1));
            ROOM_PRESET_DICTIONARY[3].Add(new RoomPreset(3, 0, viableFurniture));*/
        }
        {
            ORIENTATION_MAPPING = new List<int>();
            ORIENTATION_MAPPING.Add(2);
            ORIENTATION_MAPPING.Add(4);
            ORIENTATION_MAPPING.Add(3);
            ORIENTATION_MAPPING.Add(1);
        }
        INIT_FLAG = true;
    }

    public (NDArray, NDArray, NDArray, NDArray) GenerateHouse3(List<List<int>> houseHierarchy, double width, double height, int doorCount, int fireCount, int catCount, int valveCount)
    {

        if (INIT_FLAG == false)
            InitializePCG();

        string houseHierarchyString = "[";
        for (int i = 0; i < houseHierarchy.Count; i++)
        {
            houseHierarchyString += "[";
            for (int j = 0; j < houseHierarchy[i].Count; j++)
            {
                houseHierarchyString += houseHierarchy[i][j];
                if (j != houseHierarchy[i].Count - 1)
                    houseHierarchyString += ", ";
            }
            houseHierarchyString += "]";
            if (i != houseHierarchy.Count - 1)
                houseHierarchyString += ", [";
        }
        houseHierarchyString += "]";
        Debug.Log("PCG: " + houseHierarchyString);

        double widthIncrement = width / 256;
        double heightIncrement = height / 256;

        bool LogDebug = true;
        int resizeAttempt = 0;
        while (true)
        {
            if (++resizeAttempt == 0)
            {
                if (LogDebug)
                    Debug.Log("PCG: too many attempts, return null");
                return (null, null, null, null);
            }

            Debug.Log("PCG: attempt " + resizeAttempt + ", house size = " + width + " * " + height);

            Area rootArea = new Area(1);
            List<Room> roomList = new List<Room>();
            int roomIndex = 0;
            for (int i = 0; i < houseHierarchy.Count; i++)
            {
                Area firstDepthArea = new Area();
                int firstDepthProportion = 0;
                for (int j = 0; j < houseHierarchy[i].Count; j++)
                {
                    Area secondDepthArea = new Area(houseHierarchy[i][j] % 100);
                    firstDepthProportion += houseHierarchy[i][j] % 100;
                    roomList.Add(new Room(roomIndex++, secondDepthArea, RandomChoice(ROOM_PRESET_DICTIONARY[houseHierarchy[i][j] / 100])));
                    firstDepthArea.children.Add(secondDepthArea);
                }
                firstDepthArea.proportion = firstDepthProportion;
                firstDepthArea.children.Sort((firstArea, secondArea) => firstArea.proportion.CompareTo(secondArea.proportion));
                rootArea.children.Add(firstDepthArea);
            }
            rootArea.children.Sort((firstArea, secondArea) => firstArea.proportion.CompareTo(secondArea.proportion));

            Rectangle boundary = new Rectangle(0, 0, width, height);
            Dictionary<double, SortedSet<double>> interceptX = new Dictionary<double, SortedSet<double>>();
            Dictionary<double, SortedSet<double>> interceptY = new Dictionary<double, SortedSet<double>>();
            Partition(0, rootArea.children, boundary, interceptX, interceptY);

            if (VerifyPartition(roomList) == false)
            {
                if (LogDebug)
                    Debug.Log("PCG: room too small");
                width += widthIncrement;
                height += heightIncrement;
                continue;
            }

            Dictionary<Point, HashSet<Room>> roomPointDictionary = Geometrize(roomList, interceptX, interceptY);

            Room rootRoom = roomList[0];
            Graph<Room, (Point, Point)> roomGraph = GenerateTreeConnection(rootRoom, roomList, roomPointDictionary);

            int connectingPathLength = 4;
            GenerateAdditionalConnection(rootRoom, roomList, roomPointDictionary, roomGraph, connectingPathLength);

            PlaceValve(roomList, valveCount);

            PlaceDoor(interceptY, roomPointDictionary, roomGraph, width, height);

            PlaceFireAndCat(roomList, fireCount, catCount);

            int attemptPerRoom = 256;
            if (PlaceFurniture2(roomList, attemptPerRoom) == false)
            {
                if (LogDebug)
                    Debug.Log("PCG: cannot fit required furniture");
                width += widthIncrement;
                height += heightIncrement;
                continue;
            }

            PlacePainting(roomList);

            NDArray roomArray = np.zeros((RoundPCG(width) + 1, RoundPCG(height) + 1));
            DrawRoom(roomArray, roomList);

            NDArray doorArray = np.zeros((RoundPCG(width) + 1, RoundPCG(height) + 1));
            DrawDoor(doorArray, roomList, doorCount);

            NDArray furnitureArray = np.zeros((RoundPCG(width) + 1, RoundPCG(height) + 1));
            DrawFurniture(furnitureArray, roomList);

            NDArray fireArray = np.zeros((RoundPCG(width) + 1, RoundPCG(height) + 1));
            DrawFire(fireArray, roomList, fireCount);

            return (roomArray, doorArray, furnitureArray, fireArray);

        }

    }

    // Start is called before the first frame update
    void Start()
    {

        if (!DebugStart)
            return;

        List<List<int>> houseHierarchy = DebugHouseHierarchy(DebugRoomCount, DebugRoomGroup);
        foreach (List<int> list in houseHierarchy)
        {
            for (int i = 0; i < list.Count; i++)
                list[i] = list[i] * 100 + 25 + RoundPCG(50 * random.NextDouble());
        }

        double width = 50, height = 75;
        (NDArray roomArray, NDArray doorArray, NDArray furnitureArray, NDArray fireArray) = (null, null, null, null);
        while (roomArray == null)
        {
            (roomArray, doorArray, furnitureArray, fireArray) = GenerateHouse3(houseHierarchy, width, height, 5, 5, 5, 5);
            width *= 1.5;
            height *= 1.5;
        }

        if (roomArray == null)
            return;

        string arrayString = "";
        List<(char, string)> furnitureList = new List<(char, string)>();
        char furniture = ' ';
        for (int i = 0; i < roomArray.Shape[0]; i++)
        {
            for (int j = 0; j < roomArray.Shape[1]; j++)
            {

                string roomString = roomArray[i.ToString() + ", " + j.ToString()].ToString();
                string doorString = doorArray[i.ToString() + ", " + j.ToString()].ToString();
                string furnitureString = furnitureArray[i.ToString() + ", " + j.ToString()].ToString();
                string fireString = fireArray[i.ToString() + ", " + j.ToString()].ToString();

                if (fireString == "1")
                    arrayString += "/\\"; // /\ is fire
                else if (furnitureString != "0")
                {
                    if (!DebugFurnitureBoundary)
                    {
                        arrayString += ++furniture + " "; // so funny haha
                        furnitureList.Add((furniture, furnitureString));
                        if (furniture == '~')
                            furniture = ' ';
                    }
                    else
                        arrayString += "O ";
                }
                else if (doorString != "0")
                    arrayString += doorString + " ";
                else
                {
                    if (roomString == "1")
                        arrayString += "@ ";
                    else if (roomString == "2")
                        arrayString += "# ";
                    else
                        //arrayString += " " + roomString;
                        arrayString += "  ";
                }

            }
            arrayString += "\n";
        }
        if (!DebugFurnitureBoundary)
        {
            foreach ((char c, string s) in furnitureList)
            {
                arrayString += c + " " + s + "\n";
            }
        }
        if (arrayString.Length > 16300)
        {
            arrayString = arrayString.Substring(arrayString.Length - 16300);
        }
        Debug.Log(arrayString);

    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

}
