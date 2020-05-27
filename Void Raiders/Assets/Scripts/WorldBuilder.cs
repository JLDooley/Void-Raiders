using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace WORLDSPAWN
{
    public class WorldBuilder : MonoBehaviour
    {
        private static WorldBuilder _instance;

        public int worldSize = 10;
        public int worldHeight = 10;
        public int bossLocation;
        public TileSpawner tileSpawner;

        private Vector3 origin;

        private static int _worldSize;
        private static int _worldHeight;

        private bool[,,] worldArray;

        public static WorldBuilder Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("Removing duplicate Worldbuilder");
            }



            //Centre Worldbuilder object in World Array
            if (worldSize % 2 == 0)
            {
                Debug.Log("Even World Size entered, incrementing by one");
                worldSize += 1;
            }

            _worldSize = worldSize;
            _worldHeight = worldHeight;
            worldArray = new bool[_worldSize, _worldSize, _worldHeight];

            Debug.Log(_worldSize);
            bossLocation = Mathf.FloorToInt(_worldSize / 2);
            Debug.Log(bossLocation);
        
            
        }



        void Start()
        {
            SetSlot(bossLocation, 0, bossLocation, true);

            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ResetArray()
        {
            for (int i = 0; i < _worldSize; i++)
                for (int j = 0; j < _worldHeight; j++)
                    for (int k = 0; k < _worldSize; k++)
                        worldArray[i, j, k] = false;
        }


        public bool CheckSlot(int x, int y, int z)
        {
            if (x < 0 || x >= _worldSize || y < 0 || y >= _worldHeight || z < 0 || z >= _worldSize)
            {
                return true;
            }
            else
            {
                return worldArray[x, y, z];
            }

        }

        public void SetSlot(int x, int y, int z, bool occupied)
        {
            if (!(x < 0 || x > 39 || y < 0 || y > 39 || z < 0 || z > 39))
            {
                worldArray[x, y, z] = occupied;
                //if (occupied)
                //{
                //    Instantiate(gridVisPrefab, new Vector3(x - 20, y, z - 20), Quaternion.identity, GameController.Instance.dummyPivot);
                //}
            }

        }
    }
}
