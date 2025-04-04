using System;
using UnityEngine;
using UnityEngine.AI;

namespace ScheduleOne.Tools
{
	// Token: 0x0200085B RID: 2139
	public class SetTerrainObstacles : MonoBehaviour
	{
		// Token: 0x06003A3F RID: 14911 RVA: 0x000F5268 File Offset: 0x000F3468
		private void Start()
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				Console.Log("Skipping SetTerrainObstacles in Editor", null);
				return;
			}
			this.terrain = Terrain.activeTerrain;
			this.Obstacle = this.terrain.terrainData.treeInstances;
			this.lenght = this.terrain.terrainData.size.z;
			this.width = this.terrain.terrainData.size.x;
			this.hight = this.terrain.terrainData.size.y;
			int num = 0;
			GameObject gameObject = new GameObject("Tree_Obstacles");
			gameObject.transform.SetParent(base.transform);
			foreach (TreeInstance treeInstance in this.Obstacle)
			{
				Vector3 vector = Vector3.Scale(treeInstance.position, this.terrain.terrainData.size) + this.terrain.transform.position;
				if (vector.x >= this.Bounds.bounds.min.x && vector.x <= this.Bounds.bounds.max.x && vector.z >= this.Bounds.bounds.min.z && vector.z <= this.Bounds.bounds.max.z)
				{
					Quaternion rotation = Quaternion.AngleAxis(treeInstance.rotation * 57.29578f, Vector3.up);
					GameObject gameObject2 = new GameObject("Obstacle" + num.ToString());
					gameObject2.transform.SetParent(gameObject.transform);
					gameObject2.transform.position = vector;
					gameObject2.transform.rotation = rotation;
					gameObject2.AddComponent<NavMeshObstacle>();
					NavMeshObstacle component = gameObject2.GetComponent<NavMeshObstacle>();
					component.carving = true;
					component.carveOnlyStationary = true;
					if (this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.GetComponent<Collider>() == null)
					{
						this.isError = true;
						Debug.LogError("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.name + "'' please add one of them.");
						break;
					}
					Collider component2 = this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.GetComponent<Collider>();
					if (!(component2.GetType() == typeof(CapsuleCollider)) && !(component2.GetType() == typeof(BoxCollider)))
					{
						this.isError = true;
						Debug.LogError("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.name + "'' please add one of them.");
						break;
					}
					if (component2.GetType() == typeof(CapsuleCollider))
					{
						CapsuleCollider component3 = this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.GetComponent<CapsuleCollider>();
						component.shape = NavMeshObstacleShape.Capsule;
						component.center = component3.center;
						component.radius = component3.radius;
						component.height = component3.height;
					}
					else if (component2.GetType() == typeof(BoxCollider))
					{
						BoxCollider component4 = this.terrain.terrainData.treePrototypes[treeInstance.prototypeIndex].prefab.GetComponent<BoxCollider>();
						component.shape = NavMeshObstacleShape.Box;
						component.center = component4.center;
						component.size = component4.size;
					}
					num++;
				}
			}
			if (!this.isError)
			{
				Debug.Log(this.Obstacle.Length.ToString() + " NavMeshObstacles were succesfully added to scene");
			}
		}

		// Token: 0x04002A0C RID: 10764
		public BoxCollider Bounds;

		// Token: 0x04002A0D RID: 10765
		private TreeInstance[] Obstacle;

		// Token: 0x04002A0E RID: 10766
		private Terrain terrain;

		// Token: 0x04002A0F RID: 10767
		private float width;

		// Token: 0x04002A10 RID: 10768
		private float lenght;

		// Token: 0x04002A11 RID: 10769
		private float hight;

		// Token: 0x04002A12 RID: 10770
		private bool isError;
	}
}
