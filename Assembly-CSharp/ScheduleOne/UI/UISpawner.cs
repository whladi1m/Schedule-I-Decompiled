using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2F RID: 2607
	public class UISpawner : MonoBehaviour
	{
		// Token: 0x06004647 RID: 17991 RVA: 0x00126284 File Offset: 0x00124484
		private void Start()
		{
			this.nextSpawnTime = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x001262A4 File Offset: 0x001244A4
		private void Update()
		{
			if (this.SpawnRateMultiplier == 0f)
			{
				return;
			}
			if (Time.time > this.nextSpawnTime)
			{
				this.nextSpawnTime = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval) / this.SpawnRateMultiplier;
				if (this.Prefabs.Length != 0)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[UnityEngine.Random.Range(0, this.Prefabs.Length)], base.transform);
					if (this.UniformScale)
					{
						float num = UnityEngine.Random.Range(this.MinScale.x, this.MaxScale.x);
						gameObject.transform.localScale = new Vector3(num, num, 1f);
					}
					else
					{
						gameObject.transform.localScale = new Vector3(UnityEngine.Random.Range(this.MinScale.x, this.MaxScale.x), UnityEngine.Random.Range(this.MinScale.y, this.MaxScale.y), 1f);
					}
					gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-this.SpawnArea.rect.width / 2f, this.SpawnArea.rect.width / 2f), UnityEngine.Random.Range(-this.SpawnArea.rect.height / 2f, this.SpawnArea.rect.height / 2f), 0f);
					if (this.OnSpawn != null)
					{
						this.OnSpawn.Invoke(gameObject);
					}
				}
			}
		}

		// Token: 0x04003403 RID: 13315
		public RectTransform SpawnArea;

		// Token: 0x04003404 RID: 13316
		public GameObject[] Prefabs;

		// Token: 0x04003405 RID: 13317
		public float MinInterval = 1f;

		// Token: 0x04003406 RID: 13318
		public float MaxInterval = 5f;

		// Token: 0x04003407 RID: 13319
		public float SpawnRateMultiplier = 1f;

		// Token: 0x04003408 RID: 13320
		public Vector2 MinScale = Vector2.one;

		// Token: 0x04003409 RID: 13321
		public Vector2 MaxScale = Vector2.one;

		// Token: 0x0400340A RID: 13322
		public bool UniformScale = true;

		// Token: 0x0400340B RID: 13323
		private float nextSpawnTime;

		// Token: 0x0400340C RID: 13324
		public UnityEvent<GameObject> OnSpawn;
	}
}
