using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WORLDSPAWN
{
    public class TileSpawner : MonoBehaviour
    {
        private int x, y, z;
        private int inc_x, inc_z;
        public Transform tilePrefab;

        public void SetIncrements(int degrees)
        {
            switch (degrees)
            {
                case 0:
                    inc_x = 0;
                    inc_z = -1;
                    break;
                case 90:
                    inc_x = -1;
                    inc_z = 0;
                    break;
                case 180:
                    inc_x = 0;
                    inc_z = 1;
                    break;
                case 270:
                    inc_x = 1;
                    inc_z = 0;
                    break;
                default:
                    break;
            }
        }

        void SetInterval(int x, int y, int z)
        {
        
        }

        //void Start()
        //{
        //    for (int nextRotation = 0; nextRotation < 360; nextRotation += 90)
        //    {
        //        SetIncrements(nextRotation);

        //        Vector3 incVector = Vector3.right * inc_x + Vector3.forward * inc_z;
        //        Quaternion nextQuat = Quaternion.Euler(0f, nextRotation, 0f);

        //        if (!WorldBuilder.CheckSlot(x + inc_x, y, z + inc_z))
        //        {
        //            WorldBuilder.SetSlot(x + inc_x, y + inc_y, z + inc_z, true);
        //            Instantiate(tilePrefab, transform.position + incVector, nextQuat);
        //            //child.parent = this.transform;
        //            //child.GetComponent<RouteTile>().Initialize(recursionCount + 1, 0, nextRotation);
        //        }
        //        else
        //        {
        //            GameObject.Destroy(gameObject);
        //        }
        //    }
        //}

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
