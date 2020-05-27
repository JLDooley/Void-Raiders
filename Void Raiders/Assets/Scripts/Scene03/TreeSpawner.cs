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

    private void Awake()
    {
        trunkHeight = Random.Range(3, 4);
    }

    void Start()
    {
        
    }

}
