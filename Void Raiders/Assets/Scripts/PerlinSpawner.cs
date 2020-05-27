using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PerlinSpawner : MonoBehaviour
{
    public GameObject spawnObjects;
    public int width = 40;
    public int length = 40;
    public int height = 40;
    [Range(1, 100)] public float resolution = 1f;
    public float scale = 10f;
    [Range(0,10)] public int spawnProbability = 5;

    public float scanRadius = 2f;

    void Start()
    {
        for (int w = 0; w < width; w++)
        {    
            for (int l = 0; l < length; l++)
            {
                int result = (int) (Mathf.PerlinNoise(w / resolution, l / resolution) * scale);

                Vector3 position = new Vector3(w - (width / 2), height * result, l - (length / 2));
                
                if (result < spawnProbability)
                {
                    Debug.Log("Checking");
                    Collider[] volumeScan = Physics.OverlapSphere(position, scanRadius);

                    if (volumeScan.Length == 0)
                    {
                        Debug.Log("Spawning");
                        Instantiate(spawnObjects, position, Quaternion.identity);
                    }

                    
                }
            }
            
        }
    }

    
    void Update()
    {
        
    }
}
