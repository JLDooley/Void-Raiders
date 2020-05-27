using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;


namespace GRIDCITY
{
    public class GameController : MonoBehaviour
    {
        #region Fields
        

        private static GameController _instance;
        public GridCityManager cityManager;
        public Transform dummyPivot;




        //private 

        //Environment
        //public Material[] skyMatArray;
        //public Material[] floorMatArray;
        //public Renderer floorRenderer;
        //public int previousSkyIndex = 0;
        //public int previousFloorIndex = 0;
        public BuildingProfile[] profileLibraryArray;

        #endregion

        #region Properties	
        public static GameController Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Methods
        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            //randomize seed
            Random.InitState(System.Environment.TickCount);
            //Start our game loop coroutine:
            StartCoroutine(GameLoop());
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            else
            {
                Destroy(gameObject);
                Debug.LogError("Multiple GameController instances in Scene. Destroying clone!");
            };
        }

        #endregion

        IEnumerator GameLoop()
        {

            cityManager.SetNavMeshReadyFlag(false);

            RandomizeTowerFlavours();
            yield return new WaitForSeconds(2f);
            //cityManager.BuildRoads();

            yield return new WaitForSeconds(2f);
            cityManager.BuildTowers(Random.Range(25, 45));
            //yield return new WaitForSeconds(3f);
            //cityManager.BakeNavMesh();
            yield return new WaitForSeconds(3f);
            cityManager.SetNavMeshReadyFlag(true);
            yield return new WaitForSeconds(2.0f);


        }

        public void ResetGame()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        //public void RandomizeStage()
        //{
        //    Material newSkyMaterial;
        //    bool skySuccess = false;
        //    while (!skySuccess) //keep 'throwing the die' until sky index is different than the previous one 
        //    {
        //        int skyIndex = Random.Range(0, skyMatArray.Length);
        //        if (skyIndex != previousSkyIndex)
        //        {
        //            newSkyMaterial = skyMatArray[skyIndex];
        //            previousSkyIndex = skyIndex;
        //            RenderSettings.skybox = newSkyMaterial;
        //            skySuccess = true; //we can exit the while loop now
        //        }
        //    }

        //    Material newFloorMaterial;
        //    bool floorSuccess = false;
        //    while (!floorSuccess) //keep 'throwing the die' until floor index is different than the previous one 
        //    {
        //        int floorIndex = Random.Range(0, floorMatArray.Length);
        //        if (floorIndex != previousFloorIndex)
        //        {
        //            newFloorMaterial = floorMatArray[floorIndex];
        //            previousFloorIndex = floorIndex;
        //            floorRenderer.material = newFloorMaterial;
        //            floorSuccess = true; //we can exit the while loop now
        //        }
        //    }
        //}

        //for each game, select a subset from GameController's tower profile library 
        //and assign it to GridCityManager's gameProfileArray
        public void RandomizeTowerFlavours()
        {
            int arraySize = Random.Range(1, profileLibraryArray.Length);
            BuildingProfile[] profileArray = new BuildingProfile[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                int rnd = Random.Range(0, profileLibraryArray.Length);
                profileArray[i] = profileLibraryArray[rnd];
            }
            cityManager.gameProfileArray = profileArray;
        }

        #endregion
    }
}
