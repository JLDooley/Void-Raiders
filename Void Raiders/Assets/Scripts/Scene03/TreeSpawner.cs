using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{

    public Transform trunkPrefab;
    public Transform leafPrefab;

    private int trunkHeight;
    private int leafWidth;
    private int leafHeight;

    private int inc_x, inc_y, inc_z;


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
                inc_x = 1;
                inc_y = -1;
                inc_z = 0;
                break;

            default:
                break;

        }
    }


    private void Awake()
    {
        trunkHeight = Random.Range(3, 5);
    }

    void Start()
    {
        for (int i = 0; i < trunkHeight; i++)
        {
            Vector3 spawnPos = new Vector3(transform.localPosition.x, transform.localPosition.y + i, transform.localPosition.z);
            Instantiate(trunkPrefab, spawnPos, Quaternion.identity, transform);
        }

        Vector3 crownLeafSpawnPos = new Vector3(transform.localPosition.x, transform.localPosition.y + trunkHeight, transform.localPosition.z);
        Transform crownLeaf = Instantiate(leafPrefab, crownLeafSpawnPos, Quaternion.identity, transform);

        for (int i = 0; i < 5; i++)     //Fractal script (LeafSpawner.cs) kept crashing the system
        {
            SpawnPosition(i);
            Vector3 leafSpawnPos = new Vector3(crownLeaf.position.x + inc_x, crownLeaf.position.y + inc_y, crownLeaf.position.z + inc_z);
            Collider[] occupySlot = Physics.OverlapBox(leafSpawnPos, new Vector3(0.45f, 0.45f, 0.45f));
            if (occupySlot.Length < 1)
            {
                Instantiate(leafPrefab, leafSpawnPos, Quaternion.identity, crownLeaf.transform);
            }
            
        }

    }

}
