using System.Collections;
using System.Collections.Generic;
using Unity.Connect.Share.Editor;
using UnityEngine;

public class TestColourZone : MonoBehaviour
{
    public Transform boss;
    
    private Transform currentCell;
    private float distance;
    private float transition;

    public float cellSize = 10;

    

    private void Awake()
    {
        if (cellSize <= 0) cellSize = 1;
    }

    void Start()
    {
        currentCell = GetComponent<Transform>();

        //Debug.Log(currentCell.position);

        distance = new Vector3(currentCell.position.x - boss.position.x, 0f, currentCell.position.z - boss.position.z).magnitude;
        //Debug.Log(distance);



        transition = distance / cellSize;
        //Debug.Log(transition);

        if (transition <= 3)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (transition <= 5)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
