using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class PCG : MonoBehaviour
{

    private System.Random random;


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
            return x2 >= rectangle.x1 && x1 <= rectangle.x2 && y2 >= rectangle.y1 && y1 <= rectangle.y2;
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
        public int type;
        public List<Point> geometry;
        public List<Furniture> furniture;

        public Room(int index, Area area, int type)
        {
            this.index = index;
            this.area = area;
            this.type = type;
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

        public int type;
        public Rectangle padding;
        public Rectangle margin;

        public Furniture(int type, Rectangle padding, Rectangle margin)
        {
            this.type = type;
            this.padding = padding;
            this.margin = margin;
        }

    }

    private List<Room> GenerateAreaHierarchy(Area rootArea, List<List<int>> houseHierarchy)
    {
        // list of list : always 2-depth house hierarchy
        List<Room> roomList = new List<Room>();
        int roomIndex = 0;
        for (int i = 0; i < houseHierarchy.Count; i++)
        {
            Area firstDepthArea = new Area(0.75 + random.NextDouble() / 2);
            for (int j = 0; j < houseHierarchy[i].Count; j++)
            {
                Area secondDepthArea = new Area(0.75 + random.NextDouble() / 2);
                roomList.Add(new Room(roomIndex++, secondDepthArea, houseHierarchy[i][j]));
                firstDepthArea.children.Add(secondDepthArea);
            }
            firstDepthArea.children.Sort((firstArea, secondArea) => firstArea.proportion.CompareTo(secondArea.proportion));
            rootArea.children.Add(firstDepthArea);
        }
        rootArea.children.Sort((firstArea, secondArea) => -firstArea.proportion.CompareTo(secondArea.proportion));
        return roomList;
    }

    private void Partition(int recursion, List<Area> areaHierarchy, Rectangle boundary, Graph<Point, bool> pointGraph)
    {
        for (int i = 0; i < 4; i++)
        {
            pointGraph.Connect(boundary.GetPoint(i), boundary.GetPoint((i - 1) % 4), true);
            pointGraph.Connect(boundary.GetPoint(i), boundary.GetPoint((i + 1) % 4), true);
        }
        if (areaHierarchy.Count == 0)
            return;
        if (areaHierarchy.Count == 1)
        {
            areaHierarchy[0].rectangle = boundary;
            Partition(recursion + 1, areaHierarchy[0].children, boundary, pointGraph);
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
        double bestRatioError = Math.Abs(Math.Log(boundaryHeightLength / bestPartitionWidthLength));
        for (int i = 1; i < areaHierarchy.Count; i++)
        {
            cumulativeProportion += areaHierarchy[i].proportion;
            double currentPartitionWidthLength = (cumulativeProportion / totalProportion) * boundaryWidthLength;
            double currentRatioError = Math.Abs(Math.Log(boundaryHeightLength / (currentPartitionWidthLength * (i + 1))));
            if (currentRatioError <= bestRatioError)
            {
                partitionCount++;
                partitionProportion = cumulativeProportion;
                bestPartitionWidthLength = currentPartitionWidthLength;
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
        // height here
        List<double> partitionHeightList = new List<double>();
        partitionHeightList.Add(boundaryHeightStart);
        cumulativeProportion = 0;
        for (int i = 0; i < partitionCount - 1; i++)
        {
            cumulativeProportion += areaHierarchy[i].proportion;
            partitionHeightList.Add(boundaryHeightStart + (cumulativeProportion / partitionProportion) * boundaryHeightLength);
        }
        partitionHeightList.Add(boundaryHeightEnd);
        for (int i = 0; i < partitionCount; i++)
        {
            Rectangle subpartitionRectangle;
            if (partitionDirection == 0)
                subpartitionRectangle = new Rectangle(boundaryWidthStart, partitionHeightList[i], partitionWidth, partitionHeightList[i + 1]);
            else
                subpartitionRectangle = new Rectangle(partitionHeightList[i], boundaryWidthStart, partitionHeightList[i + 1], partitionWidth);
            areaHierarchy[i].rectangle = subpartitionRectangle;
            Partition(recursion + 1, areaHierarchy[i].children, subpartitionRectangle, pointGraph);
        }
        if (partitionCount < areaHierarchy.Count)
        {
            Rectangle remainingPartitionRectangle;
            if (partitionDirection == 0)
                remainingPartitionRectangle = new Rectangle(partitionWidth, boundaryHeightStart, boundaryWidthEnd, boundaryHeightEnd);
            else
                remainingPartitionRectangle = new Rectangle(boundaryHeightStart, partitionWidth, boundaryHeightEnd, boundaryWidthEnd);
            // doubt
            Partition(recursion, areaHierarchy.GetRange(partitionCount, areaHierarchy.Count - partitionCount), remainingPartitionRectangle, pointGraph);
        }
    }

    private bool VerifyPartition(List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            if (room.area.rectangle.GetWidth() <= 7 || room.area.rectangle.GetHeight() <= 5)
                return false;
        }
        return true;
    }

    private Dictionary<Point, HashSet<Room>> Geometrize(List<Room> roomList, Graph<Point, bool> pointGraph)
    {
        Dictionary<Point, HashSet<Room>> roomPointDictionary = new Dictionary<Point, HashSet<Room>>();
        Dictionary<double, SortedSet<double>> interceptX = new Dictionary<double, SortedSet<double>>();
        Dictionary<double, SortedSet<double>> interceptY = new Dictionary<double, SortedSet<double>>();
        foreach (Point firstPoint in pointGraph.graph.Keys)
        {
            if (interceptX.ContainsKey(firstPoint.x) == false)
                interceptX[firstPoint.x] = new SortedSet<double>();
            interceptX[firstPoint.x].Add(firstPoint.y);
            if (interceptY.ContainsKey(firstPoint.y) == false)
                interceptY[firstPoint.y] = new SortedSet<double>();
            interceptY[firstPoint.y].Add(firstPoint.x);
            foreach (Point secondPoint in pointGraph.graph[firstPoint].Keys)
            {
                // either same x or same y
                if (firstPoint.x == secondPoint.x)
                    interceptX[firstPoint.x].Add(secondPoint.y);
                else
                    interceptY[firstPoint.y].Add(secondPoint.x);
            }
        }
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
                if (x >= x2)
                    break;
                room.geometry.Add(new Point(x, y1));
            }
            foreach (double y in interceptX[x2])
            {
                if (y < y1)
                    continue;
                if (y >= y2)
                    break;
                room.geometry.Add(new Point(x2, y));
            }
            foreach (double x in interceptY[y2].Reverse())
            {
                if (x > x2)
                    continue;
                if (x <= x1)
                    break;
                room.geometry.Add(new Point(x, y2));
            }
            foreach (double y in interceptX[x1].Reverse())
            {
                if (y > y2)
                    continue;
                if (y <= y1)
                    break;
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

    private void PlaceDoor(Graph<Room, (Point, Point)> roomGraph)
    {
        HashSet<(Room, Room)> doorPlaced = new HashSet<(Room, Room)>();
        foreach (Room firstRoom in roomGraph.graph.Keys)
        {
            foreach (Room secondRoom in roomGraph.graph[firstRoom].Keys)
            {
                if (doorPlaced.Contains((secondRoom, firstRoom)))
                    continue;
                (Point firstPoint, Point secondPoint) = roomGraph.graph[firstRoom][secondRoom][0]; // one door per connection
                if (firstPoint.x < secondPoint.x && firstPoint.y == secondPoint.y)
                {
                    double doorX = (firstPoint.x + 3) + (secondPoint.x - firstPoint.x - 6) * random.NextDouble();
                    double doorY = firstPoint.y;
                    Rectangle doorPadding = new Rectangle(doorX, doorY, doorX + 3, doorY + 1);
                    Rectangle firstRoomDoorMargin = new Rectangle(doorX, doorY, doorX + 3, doorY + 3);
                    Rectangle secondRoomDoorMargin = new Rectangle(doorX, doorY - 3, doorX + 3, doorY);
                    firstRoom.furniture.Add(new Furniture(0, doorPadding, firstRoomDoorMargin));
                    secondRoom.furniture.Add(new Furniture(2, doorPadding, secondRoomDoorMargin));
                }
                if (firstPoint.x == secondPoint.x && firstPoint.y < secondPoint.y)
                {
                    double doorX = firstPoint.x;
                    double doorY = (firstPoint.y + 1) + (secondPoint.y - firstPoint.y - 4) * random.NextDouble();
                    Rectangle doorPadding = new Rectangle(doorX, doorY, doorX + 1, doorY + 3);
                    Rectangle firstRoomDoorMargin = new Rectangle(doorX - 3, doorY, doorX, doorY + 3);
                    Rectangle secondRoomDoorMargin = new Rectangle(doorX, doorY, doorX + 5, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(1, doorPadding, firstRoomDoorMargin));
                    secondRoom.furniture.Add(new Furniture(3, doorPadding, secondRoomDoorMargin));
                }
                if (firstPoint.x > secondPoint.x && firstPoint.y == secondPoint.y)
                {
                    double doorX = (secondPoint.x + 3) + (firstPoint.x - secondPoint.x - 6) * random.NextDouble();
                    double doorY = firstPoint.y;
                    Rectangle doorPadding = new Rectangle(doorX, doorY, doorX + 3, doorY + 1);
                    Rectangle firstRoomDoorMargin = new Rectangle(doorX, doorY - 3, doorX + 3, doorY);
                    Rectangle secondRoomDoorMargin = new Rectangle(doorX, doorY, doorX + 3, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(2, doorPadding, firstRoomDoorMargin));
                    secondRoom.furniture.Add(new Furniture(0, doorPadding, secondRoomDoorMargin));
                }
                if (firstPoint.x == secondPoint.x && firstPoint.y > secondPoint.y)
                {
                    double doorX = firstPoint.x;
                    double doorY = (secondPoint.y + 1) + (firstPoint.y - secondPoint.y - 4) * random.NextDouble();
                    Rectangle doorPadding = new Rectangle(doorX, doorY, doorX + 1, doorY + 3);
                    Rectangle firstRoomDoorMargin = new Rectangle(doorX, doorY, doorX + 5, doorY + 3);
                    Rectangle secondRoomDoorMargin = new Rectangle(doorX - 3, doorY, doorX, doorY + 3);
                    firstRoom.furniture.Add(new Furniture(3, doorPadding, firstRoomDoorMargin));
                    secondRoom.furniture.Add(new Furniture(1, doorPadding, secondRoomDoorMargin));
                }
                doorPlaced.Add((firstRoom, secondRoom));
            }
        }
    }

    private void Furnish(List<Room> roomList, Dictionary<int, List<(double, double)>> viableFurniture, int furnitureCount, int attemptCount)
    {
        foreach (Room room in roomList)
        {
            int successFurnish = 0;
            for (int i = 0; i < furnitureCount * attemptCount; i++)
            {
                if (successFurnish >= furnitureCount)
                    break;
                int choice = random.Next(viableFurniture[room.type].Count);
                (double furnitureWidth, double furnitureHeight) = viableFurniture[room.type][choice];
                if (room.area.rectangle.GetWidth() - 3 <= furnitureWidth || room.area.rectangle.GetHeight() - 5 <= furnitureHeight)
                    continue;
                double furnitureX = 0; // bs
                double furnitureY = 0; // also bs
                int furnitureType = 8;
                int wall = random.Next(2);
                if (wall == 0)
                {
                    int side = random.Next(4);
                    furnitureType = side + 4;
                    if (side == 0)
                    {
                        furnitureX = (room.area.rectangle.x1 + 3) + (room.area.rectangle.GetWidth() - furnitureWidth - 3) * random.NextDouble();
                        furnitureY = room.area.rectangle.y1 + 1;
                    }
                    if (side == 1)
                    {
                        furnitureX = room.area.rectangle.x2 - furnitureWidth;
                        furnitureY = (room.area.rectangle.y1 + 1) + (room.area.rectangle.GetHeight() - furnitureHeight - 1) * random.NextDouble();
                    }
                    if (side == 2)
                    {
                        furnitureX = (room.area.rectangle.x1 + 3) + (room.area.rectangle.GetWidth() - furnitureWidth - 3) * random.NextDouble();
                        furnitureY = room.area.rectangle.y2 - furnitureHeight;
                    }
                    if (side == 3)
                    {
                        furnitureX = room.area.rectangle.x1 + 3;
                        furnitureY = (room.area.rectangle.y1 + 1) + (room.area.rectangle.GetHeight() - furnitureHeight - 1) * random.NextDouble();
                    }
                }
                else
                {
                    furnitureX = (room.area.rectangle.x1 + 4) + (room.area.rectangle.GetWidth() - furnitureWidth - 5) * random.NextDouble();
                    furnitureY = (room.area.rectangle.y1 + 2) + (room.area.rectangle.GetHeight() - furnitureHeight - 3) * random.NextDouble();
                }
                Rectangle furniturePadding = new Rectangle(furnitureX, furnitureY, furnitureX + furnitureWidth, furnitureY + furnitureHeight);
                Rectangle furnitureMargin = new Rectangle(furnitureX - 1, furnitureY - 1, furnitureX + furnitureWidth + 1, furnitureY + furnitureHeight + 1);
                bool collide = false;
                foreach (Furniture placedFurniture in room.furniture)
                {
                    if (placedFurniture.margin.Collide(furnitureMargin))
                    {
                        collide = true;
                        break;
                    }
                }
                if (collide)
                    continue;
                room.furniture.Add(new Furniture(furnitureType, furniturePadding, furnitureMargin));
                successFurnish++;
            }
        }
    }


    private void DrawRoom(NDArray roomArray, List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            int x1 = Convert.ToInt32(room.area.rectangle.x1);
            int y1 = Convert.ToInt32(room.area.rectangle.y1);
            int x2 = Convert.ToInt32(room.area.rectangle.x2);
            int y2 = Convert.ToInt32(room.area.rectangle.y2);
            roomArray[(x1 + 3).ToString() + ":" + x2.ToString(), (y1 + 1).ToString() + ":" + y2.ToString()] = 0;
            roomArray[(x1 + 1).ToString() + ":" + (x1 + 3).ToString(), (y1 + 1).ToString() + ":" + y2.ToString()] = 2;
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.type == 0)
                {
                    int doorX = Convert.ToInt32(furniture.padding.x1);
                    int doorY = Convert.ToInt32(furniture.padding.y1);
                    roomArray[doorX.ToString() + ":" + (doorX + 2).ToString() + ", " + doorY.ToString() + ":" + (doorY + 1).ToString()] = 0/*2*/;
                    roomArray[(doorX + 2).ToString() + ":" + (doorX + 3).ToString() + ", " + doorY.ToString() + ":" + (doorY + 1).ToString()] = 0;
                }
                if (furniture.type == 3)
                {
                    int doorX = Convert.ToInt32(furniture.padding.x1);
                    int doorY = Convert.ToInt32(furniture.padding.y1);
                    roomArray[doorX.ToString() + ":" + (doorX + 3).ToString() + ", " + doorY.ToString() + ":" + (doorY + 3).ToString()] = 0;
                }
            }
        }
    }

    private void DrawDoor(NDArray doorArray, List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.type == 0)
                {
                    int x = Convert.ToInt32(furniture.padding.x1) + 1;
                    int y = Convert.ToInt32(furniture.padding.y1);
                    doorArray[x.ToString() + ", " + y.ToString()] = 1;
                }
                if (furniture.type == 3)
                {
                    int x = Convert.ToInt32(furniture.padding.x1);
                    int y = Convert.ToInt32(furniture.padding.y1) + 1;
                    doorArray[x.ToString() + ", " + y.ToString()] = 2;
                }
            }
        }
    }

    private void DrawFurniture(NDArray furnitureArray, List<Room> roomList)
    {
        foreach (Room room in roomList)
        {
            foreach (Furniture furniture in room.furniture)
            {
                if (furniture.type < 4)
                    continue;
                int x = Convert.ToInt32(furniture.padding.x1);
                int y = Convert.ToInt32(furniture.padding.y1);
                furnitureArray[x.ToString() + ", " + y.ToString()] = furniture.type;
            }
        }
    }

    // private void DrawEverything();

    private List<List<int>> DebugHouseHierarchy(int roomCount, int firstBreadth)
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
            tempList2.Add(random.Next(12));
            while (run < tempList1[i] - 0.000001)
            {
                tempList2.Add(random.Next(12));
                run += unit;
            }
            houseHierarchy.Add(tempList2);
            run -= tempList1[i];
        }
        return houseHierarchy;
    }

    public (NDArray, NDArray, NDArray) GenerateHouse(List<List<int>> houseHierarchy, double width, double height)
    {

        Area rootArea = new Area(1);
        List<Room> roomList = GenerateAreaHierarchy(rootArea, houseHierarchy) ;

        Rectangle boundary = new Rectangle(0, 0, width, height);
        Graph<Point, bool> pointGraph = new Graph<Point, bool>();
        Partition(0, rootArea.children, boundary, pointGraph);

        if (VerifyPartition(roomList) == false)
            return (null, null, null);

        Dictionary<Point, HashSet<Room>> roomPointDictionary = Geometrize(roomList, pointGraph);

        Room rootRoom = roomList[0];
        Graph<Room, (Point, Point)> roomGraph = GenerateTreeConnection(rootRoom, roomList, roomPointDictionary);
        
        GenerateAdditionalConnection(rootRoom, roomList, roomPointDictionary, roomGraph, 5);

        PlaceDoor(roomGraph);

        Dictionary<int, List<(double, double)>> viableFurniture = new Dictionary<int, List<(double, double)>>();
        for (int i = 0; i < 12; i++)
        {
            viableFurniture[i] = new List<(double, double)>();
            viableFurniture[i].Add((1, 1));
            viableFurniture[i].Add((2, 1));
            viableFurniture[i].Add((1, 2));
            viableFurniture[i].Add((3, 2));
            viableFurniture[i].Add((2, 3));
        }
        Furnish(roomList, viableFurniture, 10, 5);

        NDArray roomArray = np.ones((Convert.ToInt32(width + 1), Convert.ToInt32(height + 1)));
        DrawRoom(roomArray, roomList);

        NDArray doorArray = np.zeros((Convert.ToInt32(width + 1), Convert.ToInt32(height + 1)));
        DrawDoor(doorArray, roomList);

        NDArray furnitureArray = np.zeros((Convert.ToInt32(width + 1), Convert.ToInt32(height + 1)));
        DrawFurniture(furnitureArray, roomList);

        return (roomArray, doorArray, furnitureArray);

    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Hello World!");

        //int seed = 3;
        random = new System.Random(/*seed*/);

        List<List<int>> houseHierarchy = DebugHouseHierarchy(11, 3);
        string houseHierarchyString = "";
        for (int i = 0; i < houseHierarchy.Count; i++)
        {
            houseHierarchyString += "[";
            for (int j = 0; j < houseHierarchy[i].Count; j++)
            {
                houseHierarchyString += houseHierarchy[i][j].ToString();
                if (j < houseHierarchy[i].Count - 1)
                    houseHierarchyString += ", ";
                else
                    houseHierarchyString += "]";
            }
            if (i < houseHierarchy.Count - 1)
                houseHierarchyString += ", ";
        }
        Debug.Log(houseHierarchyString);

        int halfLength = 25;
        double width = halfLength + halfLength * random.NextDouble();
        double height = halfLength + halfLength * random.NextDouble();
        (NDArray roomArray, NDArray doorArray, NDArray furnitureArray) = GenerateHouse(houseHierarchy, width, height);
        while (roomArray == null)
        {
            halfLength = Math.Max(halfLength + 1, Convert.ToInt32(halfLength * 1.25));
            //Debug.Log(halfLength);
            width = halfLength + halfLength * random.NextDouble();
            height = halfLength + halfLength * random.NextDouble();
            (roomArray, doorArray, furnitureArray) = GenerateHouse(houseHierarchy, width, height);
        }

        string arrayString = "";
        for (int i = 0; i < roomArray.Shape[0]; i++)
        {
            for (int j = 0; j < roomArray.Shape[1]; j++)
            {

                string roomString = roomArray[i.ToString() + ", " + j.ToString()].ToString();
                string doorString = doorArray[i.ToString() + ", " + j.ToString()].ToString();
                string furnitureString = furnitureArray[i.ToString() + ", " + j.ToString()].ToString();

                string value = furnitureString;
                if (value == "0")
                    value = doorString;
                if (value == "0")
                {
                    value = roomString;
                    if (value == "0")
                        arrayString += "  ";
                    if (value == "1")
                        arrayString += "@ ";
                    if (value == "2")
                        arrayString += "# ";
                }
                else
                    arrayString += value + " ";

            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
