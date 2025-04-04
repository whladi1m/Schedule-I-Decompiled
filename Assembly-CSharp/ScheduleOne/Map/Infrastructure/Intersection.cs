using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C0A RID: 3082
	public class Intersection : MonoBehaviour
	{
		// Token: 0x06005620 RID: 22048 RVA: 0x001698C2 File Offset: 0x00167AC2
		protected virtual void Start()
		{
			Singleton<CoroutineService>.Instance.StartCoroutine(this.Run());
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x001698D5 File Offset: 0x00167AD5
		protected IEnumerator Run()
		{
			for (;;)
			{
				this.SetPath1Lights(TrafficLight.State.Green);
				this.SetPath2Lights(TrafficLight.State.Red);
				if (this.timeOffset != 0f)
				{
					yield return new WaitForSecondsRealtime(Mathf.Abs(this.timeOffset));
					this.timeOffset = 0f;
				}
				yield return new WaitForSecondsRealtime(this.path1Time);
				this.SetPath1Lights(TrafficLight.State.Orange);
				yield return new WaitForSecondsRealtime(TrafficLight.amberTime);
				this.SetPath1Lights(TrafficLight.State.Red);
				yield return new WaitForSecondsRealtime(1f);
				this.SetPath2Lights(TrafficLight.State.Green);
				yield return new WaitForSecondsRealtime(this.path2Time);
				this.SetPath2Lights(TrafficLight.State.Orange);
				yield return new WaitForSecondsRealtime(TrafficLight.amberTime);
				this.SetPath2Lights(TrafficLight.State.Red);
				yield return new WaitForSecondsRealtime(1f);
			}
			yield break;
		}

		// Token: 0x06005622 RID: 22050 RVA: 0x001698E4 File Offset: 0x00167AE4
		protected void SetPath1Lights(TrafficLight.State state)
		{
			foreach (TrafficLight trafficLight in this.path1Lights)
			{
				trafficLight.state = state;
			}
			if (state == TrafficLight.State.Green)
			{
				using (List<GameObject>.Enumerator enumerator2 = this.path1Obstacles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject gameObject = enumerator2.Current;
						gameObject.gameObject.SetActive(false);
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.path1Obstacles)
			{
				gameObject2.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x001699C4 File Offset: 0x00167BC4
		protected void SetPath2Lights(TrafficLight.State state)
		{
			foreach (TrafficLight trafficLight in this.path2Lights)
			{
				trafficLight.state = state;
			}
			if (state == TrafficLight.State.Green)
			{
				using (List<GameObject>.Enumerator enumerator2 = this.path2Obstacles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject gameObject = enumerator2.Current;
						gameObject.gameObject.SetActive(false);
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.path2Obstacles)
			{
				gameObject2.gameObject.SetActive(true);
			}
		}

		// Token: 0x04004007 RID: 16391
		[Header("References")]
		[SerializeField]
		protected List<TrafficLight> path1Lights = new List<TrafficLight>();

		// Token: 0x04004008 RID: 16392
		[SerializeField]
		protected List<TrafficLight> path2Lights = new List<TrafficLight>();

		// Token: 0x04004009 RID: 16393
		[SerializeField]
		protected List<GameObject> path1Obstacles = new List<GameObject>();

		// Token: 0x0400400A RID: 16394
		[SerializeField]
		protected List<GameObject> path2Obstacles = new List<GameObject>();

		// Token: 0x0400400B RID: 16395
		[Header("Settings")]
		[SerializeField]
		protected float path1Time = 10f;

		// Token: 0x0400400C RID: 16396
		[SerializeField]
		protected float path2Time = 10f;

		// Token: 0x0400400D RID: 16397
		[SerializeField]
		protected float timeOffset;
	}
}
