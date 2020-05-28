using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    
    public Transform leafPrefab;
    private int maxDepth = 2;
    private int depth = 0;
    

    private int inc_x, inc_y, inc_z;

    void Start()
    {
        if (depth < maxDepth)
        {
            
            //CreateLeaf();     //Keeps crashing when trying to spawn new blocks
        }
    }


    void CreateLeaf()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnPosition(i);
            Vector3 leafSpawnPos = new Vector3(transform.localPosition.x + inc_x, transform.localPosition.y + inc_y, transform.localPosition.z + inc_z);
            Instantiate(leafPrefab, leafSpawnPos, Quaternion.identity, transform).GetComponent<LeafSpawner>().Initialize(this);
        }
    }

    private void Initialize(LeafSpawner parent)
    {
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;

    }

    public void SpawnPosition(int position)
    {
        switch (position)
        {
            case 0:
                inc_x = 0;
                inc_y = -1;
                inc_z = 0;
                break;

            case 1:
                inc_x = 0;
                inc_y = -1;
                inc_z = 1;
                break;

            case 2:
                inc_x = -1;
                inc_y = -1;
                inc_z = 0;
                break;

            case 3:
                inc_x = 0;
                inc_y = -1;
                inc_z = -1;
                break;

            case 4:
                inc_x = -1;
                inc_y = -1;
                inc_z = 0;
                break;

            default:
                break;

        }
    }

}
