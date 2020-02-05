using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[ExecuteInEditMode]
public class MapGeneration : MonoBehaviour {


    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            if (null != levels[currentLevel].levelValues[UnityEngine.Random.Range(0, levels[currentLevel].levelValues.Length)])
            {
                LevelValues currentValues = levels[currentLevel].levelValues[UnityEngine.Random.Range(0, levels[currentLevel].levelValues.Length)];
                width = currentValues.width;
                height = currentValues.height;
                seed = currentValues.seed;
                GenerateMap();
            }
        }
    }
    private NavMeshSurface surface;

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    [ContextMenuItem("Generate Map", "GenerateMap")]
    [SerializeField]
    private int currentLevel;
    public Levels[] levels;

    private GameObject[] lootObjects;
    private GameObject[] nearWallObjects;
    private GameObject[] interactibleObjects;
    private GameObject[] groundObjects;
    private GameObject portal;
    private GameObject portalHome;
    private GameObject playerSpawn;
    List<GameObject> objectList = new List<GameObject>();
    List<GameObject> InteractionObjectList = new List<GameObject>();

    int[,] map;

    int[,] lootObjectMap;
    int[,] rockObjectMap;
    int[,] groundObjectMap;
    int[,] interactionObjectMap;


    void Awake()
    {

        lootObjects = ConvertToGameObject(Resources.LoadAll("Props/CaveProps/LootObjects", typeof(GameObject)));
        nearWallObjects = ConvertToGameObject(Resources.LoadAll("Props/CaveProps/NearWallObjects", typeof(GameObject)));
        interactibleObjects = ConvertToGameObject(Resources.LoadAll("Props/CaveProps/InteractibleObjects", typeof(GameObject)));
        groundObjects = ConvertToGameObject(Resources.LoadAll("Props/CaveProps/GroundObjects", typeof(GameObject)));
        portal = Resources.Load<GameObject>("Props/CaveProps/QuestItems/Teleport");
        portalHome = Resources.Load<GameObject>("Props/CaveProps/QuestItems/TeleportHome");
        playerSpawn = Resources.Load<GameObject>("Props/CaveProps/QuestItems/PlayerSpawn");

        surface = GameObject.Find("Plane").GetComponent<NavMeshSurface>();

        
    }

    public GameObject[] ConvertToGameObject(UnityEngine.Object[] objectList)
    {
        List<GameObject> arrayToReturn = new List<GameObject>();
        for (int i = 0; i < objectList.Length; i++)
        {
            arrayToReturn.Add((GameObject)objectList[i]);
        }
        return arrayToReturn.ToArray();
    }

    public int borderSize = 1;

    public void GenerateMap()
    {
        GameObject[] enemysArray = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < objectList.Count; i++)
        {
            Destroy(objectList[i]);
        }
        for (int i = 0; i < InteractionObjectList.Count; i++)
        {
            Destroy(InteractionObjectList[i]);
        }
        for (int i = 0; i < enemysArray.Length; i++)
        {
            Destroy(enemysArray[i]);
        }
        InteractionObjectList.Clear();
        objectList.Clear();

        map = new int[width, height];

        lootObjectMap = new int[width, height];
        rockObjectMap = new int[width, height];
        groundObjectMap = new int[width, height];
        interactionObjectMap = new int[width, height];


        RandomFillMap();
        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        
        ProcessMap();
        ObjectPlacer();
        ObjectSpawner();


        /* tässä määritämme kartan reunan paksuuden, sekä varmistamme että reunapalat ovat aina seinään*/

        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedMap[x, y] = 1;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);

        surface = GameObject.Find("Plane").GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    public int roomThresholdSize = 50;

    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);
        int wallThresholdSize = 50;

        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresholdSize)
            {
                foreach (Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);
        
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }

            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {

        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }
    public int passageSize = 2;
    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
       

        List<Coord> line = GetLine(tileA, tileB);
        foreach(Coord c in line)
        {
            DrawCircle(c, passageSize);
        }
    }

    void DrawCircle(Coord c, int r)
    {
        for(int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if(x*x + y*y <= r * r)
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if(IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradienAccumulation = longest / 2;
        for(int i=0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradienAccumulation += shortest;
            if(gradienAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }

                gradienAccumulation -= longest;
            }
        }
        return line;
    }


    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(1,10000000).ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    // muodostaa huoneita 
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 5)
                    map[x, y] = 1;
                    

                else if (neighbourWallTiles <= 4)
                    map[x, y] = 0;

                
            }
        }
    }



    void ObjectSpawner()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (lootObjectMap[x, y] == 1)
                    objectList.Add(Instantiate(lootObjects[UnityEngine.Random.Range(0, lootObjects.Length)], new Vector3((x + 1) - width / 2, -5, (y - 1) - height / 2), Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0)));
                if (rockObjectMap[x, y] == 1)
                    objectList.Add(Instantiate(nearWallObjects[UnityEngine.Random.Range(0, nearWallObjects.Length)], new Vector3((x) - width / 2, -5, (y - 2) - height / 2), Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0)));
                if (groundObjectMap[x, y] == 1)
                    objectList.Add(Instantiate(groundObjects[UnityEngine.Random.Range(0, groundObjects.Length)], new Vector3((x) - width / 2, -5, (y - 1) - height / 2), Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0)));
                if (interactionObjectMap[x, y] == 1)
                    InteractionObjectList.Add(Instantiate(interactibleObjects[UnityEngine.Random.Range(0, interactibleObjects.Length)], new Vector3((x) - width / 2, -5, (y - 1) - height / 2), Quaternion.Euler(-90, UnityEngine.Random.Range(0, 360), 0)));
            }
        }
        SpawnPortalsAndPlayerSpawn();
    }

    private void SpawnPortalsAndPlayerSpawn()
    {
        GameObject firstItemOnList = InteractionObjectList[0];
        GameObject secondItemOnList = InteractionObjectList[1];
        GameObject lastItemOnList = InteractionObjectList[InteractionObjectList.Count - 1];
        GameObject playerSpawnObject = Instantiate(playerSpawn, lastItemOnList.transform.position, lastItemOnList.transform.rotation);
        GameObject portalObject = Instantiate(portal, firstItemOnList.transform.position, firstItemOnList.transform.rotation);
        GameObject portalHomeObject = Instantiate(portalHome, secondItemOnList.transform.position, secondItemOnList.transform.rotation);
        Destroy(firstItemOnList);
        Destroy(lastItemOnList);
        Destroy(secondItemOnList);
        InteractionObjectList[0] = portalObject;
        InteractionObjectList[1] = portalHome;
        InteractionObjectList[InteractionObjectList.Count - 1] = playerSpawnObject;
    }

    public int middleOfRoomModif = 6;
    public int middleOfRoomModif2 = 2;

    public int rareNearWallPlacementModif1 = 1;
    public int rareNearWallPlacementModif2 = 15;
    public int rareNearWallPlacementModif3 = 50;

    public int nearLootPlacementModif1 = 20;
    public int nearLootPlacementModif2 = 10;
    public int nearLootPlacementModif3 = 1;
    void ObjectPlacer()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                int neighbourWallRadius = GetsurroundingWallRadiusCount(x, y);
                int neighbourObject1Radius = GetSurroundingObject1Count(x, y);
                int neighbourObject2Radius = GetSurroundingObject2Radius(x, y);
                // (neighbourWallTiles >1) spawnaa ryppäitä reunoille
                //spawnaa keskelle
                if (neighbourWallRadius <= middleOfRoomModif && neighbourObject1Radius <= middleOfRoomModif2)// isojen huoneiden keskelle
                    lootObjectMap[x, y] = 1;
                else
                    lootObjectMap[x, y] = 0;

                if (neighbourWallTiles == rareNearWallPlacementModif1 && neighbourWallRadius >= rareNearWallPlacementModif2 && neighbourWallRadius <= rareNearWallPlacementModif3)//rareihin paikkoihin reunoille
                    rockObjectMap[x, y] = 1; 
                else
                    rockObjectMap[x, y] = 0;

                if (neighbourWallTiles <=6 ) //kaikkialle(kivet)
                    groundObjectMap[x, y] = 1;
                else
                    groundObjectMap[x, y] = 0;

                if (neighbourWallRadius <= nearLootPlacementModif1 && neighbourWallRadius >= nearLootPlacementModif2 && neighbourObject2Radius < nearLootPlacementModif3)//lootin ympärille
                    interactionObjectMap[x, y] = 1;
                else 
                    interactionObjectMap[x, y] = 0;
            }
        }
    }

    int GetSurroundingObject2Radius(int gridX, int gridY)
    {
        int object4Count = 0;
        for (int neighbourX = gridX - 10; neighbourX <= gridX + 10; neighbourX++)
        {
            for (int neighbourY = gridY - 10; neighbourY <= gridY + 10; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        object4Count += interactionObjectMap[neighbourX, neighbourY];
                    }
                }
                else
                {
                    object4Count++;
                }
            }
        }
        return object4Count;
    }

        int GetSurroundingObject1Count(int gridX, int gridY)
        {
            int object1Count = 0;
            for (int neighbourX = gridX - 3; neighbourX <= gridX + 3; neighbourX++)
            {
                for (int neighbourY = gridY - 3; neighbourY <= gridY + 3; neighbourY++)
                {
                    if (IsInMapRange(neighbourX, neighbourY))
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            object1Count += lootObjectMap[neighbourX, neighbourY];

                        }
                    }
                    else
                    {
                        object1Count++;
                    }
                }
            }
            return object1Count;
        }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                        
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    int GetsurroundingWallRadiusCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 5; neighbourX <= gridX + 5; neighbourX++)
        {
            for (int neighbourY = gridY - 5; neighbourY <= gridY + 5; neighbourY++)
            {
                if (IsInMapRange(neighbourX,neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    [System.Serializable]
    public class Levels
    {
        public LevelValues[] levelValues;
    }

    [System.Serializable]
    public class LevelValues
    {
        public int width;
        public int height;
        public string seed;

    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {
        }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY)
                        {
                            if (map[x, y] == 1)
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }

}
