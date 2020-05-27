using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public Transform groundPrefab;
    public Transform undergroundPrefab;

    [Tooltip("Perlin Sample Resolution\nIncrease distance between sample points to increase height variation in terrain.\n1 = Mostly Flat Terrain\n~20 = Hilly Terrain (Best)\n100 = Low, Rolling Hills")]
    [Range(1, 100)] public float resolution = 1f;
    [Tooltip("Perlin Scaling Factor\nAmplify the value range of sample points to increase gradients\n1 = Min 0, Max 1\n10 = Min 0, Max 10\n100 = Min 0, Max 100")]
    public float scale = 10f;

    public enum SlotType { Empty, Ground, Underground, Occupied, Invalid }
    private SlotType[,,] slotTypeArray = new SlotType[40, 40, 40];

    private bool[,,] worldArray = new bool[40, 40, 40];
    private bool resetComplete = false;
    private bool terrainGenComplete = false;
    private bool terrainSpawnComplete = false;

    private List<Vector3Int> groundCoords = new List<Vector3Int>();


    private void Start()
    {
        StartCoroutine(GenerateWorld());
    }

    IEnumerator GenerateWorld()
    {
        resetComplete = false;
        terrainGenComplete = false;
        terrainSpawnComplete = false;

        ResetArray();
        yield return new WaitUntil(() => resetComplete);
        TerrainGen();
        yield return new WaitUntil(() => terrainGenComplete);
        TerrainSpawn();
        yield return new WaitUntil(() => terrainSpawnComplete);
        CombineTerrainMeshes();
    }


    public void ResetArray()
    {
        
        Debug.Log("Resetting");

        for (int x = 0; x < 40; x++)
        {
            for (int y = 0; y < 40; y++)
            {
                for (int z = 0; z < 40; z++)
                {
                    SetSlot(x, y, z, false, SlotType.Empty);
                    
                }
            }
        }

        resetComplete = true;
        Debug.Log("Reset Complete");
    }

    private void TerrainGen()
    {
        
        Debug.Log("Generating Terrain");
        int offset = Random.Range(0, 10);

        for (int x = 0; x < 40; x++)
        {
            for (int z = 0; z < 40; z++)
            {
                //int y = (int)(Mathf.PerlinNoise(x / resolution + offset, z / resolution + offset) * scale);
                int y = (int)Mathf.Min(Mathf.PerlinNoise(x / resolution + offset, z / resolution + offset) * scale, 39f); //Upper limits to 40
                SetSlot(x, y, z, true, SlotType.Ground);
                
                if(y > 0)
                {
                    for (int h = y - 1; h >= 0; h--)
                    {
                        SetSlot(x, h, z, true, SlotType.Underground);
                    }
                }
            }
        }

        terrainGenComplete = true;
        Debug.Log("Terrain Generation Complete");
    }

    private void TerrainSpawn()
    {
        Debug.Log("Spawning Terrain");
        int debugSpawnCount = 0;

        foreach (Vector3Int coord in groundCoords) //Reduce the number of meshes by only creating terrian to a depth of 2 units (or more when exposed to the surface e.g. cliffs)
        {
            int x = coord.x + 20;
            int y = coord.y;
            int z = coord.z + 20;

            if ((CheckSlotType(x, y, z) == SlotType.Ground) && (CheckSlotType(x, y + 1, z) != SlotType.Invalid))
            {
                Instantiate(groundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            else if ((CheckSlotType(x, y + 1, z) == SlotType.Ground) && (CheckSlotType(x, y + 1, z) != SlotType.Invalid))
            {
                Instantiate(undergroundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            else if ((CheckSlotType(x - 1, y, z) != SlotType.Underground) && (CheckSlotType(x - 1, y, z) != SlotType.Invalid))
            {
                Instantiate(undergroundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            else if ((CheckSlotType(x + 1, y, z) != SlotType.Underground) && (CheckSlotType(x + 1, y, z) != SlotType.Invalid))
            {
                Instantiate(undergroundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            else if ((CheckSlotType(x, y, z - 1) != SlotType.Underground) && (CheckSlotType(x, y, z - 1) != SlotType.Invalid))
            {
                Instantiate(undergroundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            else if ((CheckSlotType(x, y, z + 1) != SlotType.Underground) && (CheckSlotType(x, y, z + 1) != SlotType.Invalid))
            {
                Instantiate(undergroundPrefab, new Vector3Int(x - 20, y, z - 20), Quaternion.identity, this.transform);
                debugSpawnCount++;
            }
            
        }

        Debug.Log("Spawned blocks: " + debugSpawnCount);

        terrainSpawnComplete = true;
        Debug.Log("Terrain Spawn Complete");
    }

    private void CombineTerrainMeshes()
    {
        if (this.gameObject.GetComponent<MeshFilter>() == null)
        {
            this.gameObject.AddComponent<MeshFilter>();
        }

        Debug.Log("Combining Meshes");
        
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Destroy(this.gameObject.GetComponent<MeshCollider>());

        int i = 0;
        Debug.Log("Meshes detected: " + meshFilters.Length);
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        transform.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        transform.GetComponent<MeshFilter>().mesh.Optimize();

        //this.gameObject.AddComponent<MeshRenderer>();
        this.gameObject.AddComponent<MeshCollider>();
        transform.gameObject.SetActive(true);
        
        Debug.Log("Mesh Combine Complete");
        //Destroy(block);
    }





    public void SetSlot(int x, int y, int z, bool occupied, SlotType type)
    {
        worldArray[x, y, z] = occupied;
        slotTypeArray[x, y, z] = type;
        //Debug.Log(worldArray[x, y, z]);       //WARNING: Significantly slows program when setting slots enmass (e.g. ResetArray()). Disable if not needed.
        //Debug.Log(slotTypeArray[x, y, z]);    //WARNING: Significantly slows program when setting slots enmass (e.g. ResetArray()). Disable if not needed.

        if ((type == SlotType.Ground)||(type == SlotType.Underground))
        {
            Vector3Int coord = new Vector3Int(x - 20, y, z - 20);
            groundCoords.Add(coord);
        }
    }

    public bool CheckSlotOccupied(int x, int y, int z)
    {
        if ((x < 0) || (x >= worldArray.GetLength(0)) || (y < 0) || (y >= worldArray.GetLength(1)) || (z < 0) || (z >= worldArray.GetLength(2)))
        {
            Debug.Log("Requested WorldArray index out of bounds.");
            return true;
        }
        else
        {
            return worldArray[x, y, z];
        }
    }

    public SlotType CheckSlotType(int x, int y, int z)
    {
        if ((x < 0) || (x >= slotTypeArray.GetLength(0)) || (y < 0) || (y >= slotTypeArray.GetLength(1)) || (z < 0) || (z >= slotTypeArray.GetLength(2)))
        {
            Debug.Log("Requested SlotTypeArray index out of bounds.");
            return SlotType.Invalid;
        }
        else
        {
            return slotTypeArray[x, y, z];
        }
    }
}
