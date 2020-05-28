using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform chunkPrefab;
    public Transform treePrefab;
    public Transform playerPrefab;
    
    [Range(1,10)] public int worldSize = 8;

    private SlotManager[,] worldArray;

    private int globalOffset;

    private List<Vector3Int> surfaceList = new List<Vector3Int>();

    [Tooltip("Perlin Sample Resolution\nIncrease distance between sample points to increase height variation in terrain.\n1 = Mostly Flat Terrain\n~20 = Hilly Terrain (Best)\n100 = Low, Rolling Hills")]
    [Range(1, 100)] public float resolution = 1f;
    
    [Tooltip("Perlin Scaling Factor\nAmplify the value range of sample points to increase gradients\n1 = Min 0, Max 1\n10 = Min 0, Max 10\n100 = Min 0, Max 100")]
    public float scale = 10f;

    private int treeCount;
    [Range(1, 10)] public int treeDensity = 5;

    private bool chunkSpawnComplete = false;
    private bool surfacesTallyComplete = false;
    

    private void Awake()
    {
        globalOffset = Random.Range(0, 100);    //Without the offset, world is mirrored about the x and z axes
        worldArray = new SlotManager[worldSize, worldSize];
    }

    private void Start()
    {
        StartCoroutine(GenerateWorld());
    }

    IEnumerator GenerateWorld()
    {
        chunkSpawnComplete = false;
        surfacesTallyComplete = false;

        SpawnChunks();
        yield return new WaitUntil(() => chunkSpawnComplete);
        StartCoroutine(SurfacesTally());
        yield return new WaitUntil(() => surfacesTallyComplete);
        SpawnForest();
        yield return new WaitForSeconds(3);
        SpawnPlayer();

    }

    private void SpawnChunks()
    {
        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                Transform newChunk = Instantiate(chunkPrefab, new Vector3Int((i - worldSize / 2) * 16, 0, (j - worldSize / 2) * 16), Quaternion.identity, this.transform);
                worldArray[i, j] = newChunk.GetComponent<SlotManager>();
                
                worldArray[i, j].offset = globalOffset; //Better way of doing this? (get/set? static properties?)
                worldArray[i, j].resolution = resolution;
                worldArray[i, j].scale = scale;
            }
        }
        chunkSpawnComplete = true;
    }
    
    IEnumerator SurfacesTally()
    {
        Debug.Log("Adding Chunk Surfaces");
        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)         
            {
                //for (int x = 0; x < worldSize * 16; x++)              //Not crashing, but very slow. ~1min to process one chunk, ~15 minutes to process 4 chunks
                // {
                //     for (int y = 0; y < 40; y++)
                //     {
                //         for (int z = 0; z < worldSize * 16; z++)
                //         {
                //             yield return new WaitUntil(() => worldArray[i, j].chunkTerrainGenCompleted);


                //             if (worldArray[i, j].CheckSlotType(x, y, z) == SlotManager.SlotType.Ground)
                //             {
                //                 surfaceList.Add(new Vector3Int(x - (worldSize * 16 / 2), y + 1, z - (worldSize * 16 / 2)));
                //                 //Debug.Log("Surface Added");   
                //             }
                //         }
                //     }
                // }
                
                yield return new WaitUntil(() => worldArray[i, j].chunkTerrainGenCompleted);

                foreach (Vector3Int coord in worldArray[i, j].chunkGroundCoords)            //Much faster, ~25 seconds parallel to Terrain Generation for WorldSize = 8 (64 chunks)
                {
                    int chunkX = coord.x + 8;
                    int chunkY = coord.y;
                    int chunkZ = coord.z + 8;
                    int correction = Mathf.FloorToInt(worldSize / 2);

                    if (worldArray[i, j].CheckSlotType(chunkX, chunkY, chunkZ) == SlotManager.SlotType.Ground)
                    {
                        surfaceList.Add(new Vector3Int(coord.x - ((correction - i) * 16), coord.y + 1, coord.z - ((correction - j) * 16)));
                    }       
                }
                //Debug.Log("Chunk Complete");
            }
        }
        surfacesTallyComplete = true;
        Debug.Log("Surface Tally Complete");
    }

    public void SpawnForest()
    {
        treeCount = treeDensity * Random.Range(worldSize - 1, worldSize + 1);
        
        for (int i = 0; i < treeCount; i++)
        {
            int index = Random.Range(0, surfaceList.Count);
            Vector3Int spawnPoint = surfaceList[index];
            surfaceList.RemoveAt(index);
            Instantiate(treePrefab, spawnPoint, Quaternion.identity);
        }
    }

    public void SpawnPlayer()
    {
        int index = Random.Range(0, surfaceList.Count);
        Vector3Int spawnPoint = surfaceList[index];
        Instantiate(playerPrefab, spawnPoint + Vector3Int.up, Quaternion.identity);
    }
        

}
