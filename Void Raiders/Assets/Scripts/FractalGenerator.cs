using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalGenerator : MonoBehaviour
{

		#region Fields
		public Mesh[] meshes;
		//public TrailRenderer trails;
		public Material material;
		public int maxDepth;
		public float childScale;
		public float spawnProbability;
		public float maxRotationSpeed;
		public float maxTwist;


		#endregion

		#region Properties	
		private int depth;
		private static Vector3[] childDirections = { Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
		private static Quaternion[] childOrientations = { Quaternion.identity, Quaternion.Euler(0, 0, -90f), Quaternion.Euler(0, 0, 90f), Quaternion.Euler(90f, 0, 0), Quaternion.Euler(-90f, 0, 0) };
		private Material[,] materials;
		private float rotationSpeed;

		#endregion

		#region Methods
		#region Unity Methods



		void Start()
		{
			if (materials == null)
			{
				InitializeMaterials();
			}
			rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
			transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

			gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
			gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
			if (depth < maxDepth)
			{
				StartCoroutine(CreateChildren());
			}


		}



		private IEnumerator CreateChildren()
		{
			for (int i = 0; i < childDirections.Length; i++)
			{
				if (Random.value < spawnProbability)
				{
					yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
					new GameObject("Fractal Child").AddComponent<FractalGenerator>().Initialize(this, i);
				}
			}
		}

		private void Initialize(FractalGenerator parent, int childIndex)
		{
			spawnProbability = parent.spawnProbability;
			maxRotationSpeed = parent.maxRotationSpeed;
			maxTwist = parent.maxTwist;

			meshes = parent.meshes;
			materials = parent.materials;
			maxDepth = parent.maxDepth;
			childScale = parent.childScale;
			depth = parent.depth + 1;
			transform.parent = parent.transform;    //Nest child in its parent

			transform.localScale = Vector3.one * childScale;    //	Compounded due to being scaled relative to the parent's scale
			transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);   //how does this work, would the offset not be constant: (0,0.75,0) for childScale = 0.5? Is the localScale declaration affecting this?
			transform.localRotation = childOrientations[childIndex];
		}

		private void InitializeMaterials()
		{
			materials = new Material[maxDepth + 1, 2];
			for (int i = 0; i <= maxDepth; i++)
			{
				float t = i / (maxDepth - 1f);
				t *= t;
				materials[i, 0] = new Material(material);
				materials[i, 0].color = Color.Lerp(Color.blue, Color.red, t);
				materials[i, 1] = new Material(material);
				materials[i, 1].color = Color.Lerp(Color.cyan, Color.magenta, t);
			}
			materials[maxDepth, 0].color = Color.yellow;
			materials[maxDepth, 1].color = Color.white;
		}

		void Update()
		{
			transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

		}

		#endregion
		#endregion

}
