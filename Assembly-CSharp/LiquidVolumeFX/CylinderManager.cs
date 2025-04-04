using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200016E RID: 366
	public class CylinderManager : MonoBehaviour
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x0001FC34 File Offset: 0x0001DE34
		private void Update()
		{
			if (Time.time < this.startingDelay)
			{
				return;
			}
			for (int i = 0; i < this.numCylinders; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/CylinderFlask"));
				gameObject.hideFlags = HideFlags.DontSave;
				gameObject.transform.SetParent(base.transform, false);
				gameObject.transform.localScale = new Vector3(this.scale, this.scale * this.heightMultiplier, this.scale);
				float x = Mathf.Cos((float)i / (float)this.numCylinders * 3.1415927f * 2f) * this.circleRadius;
				float z = Mathf.Sin((float)i / (float)this.numCylinders * 3.1415927f * 2f) * this.circleRadius;
				gameObject.transform.position = new Vector3(x, -2f, z);
				FlaskAnimator flaskAnimator = gameObject.AddComponent<FlaskAnimator>();
				flaskAnimator.initialPosition = gameObject.transform.position;
				flaskAnimator.finalPosition = gameObject.transform.position + Vector3.up;
				flaskAnimator.duration = 5f + (float)i * 0.5f;
				flaskAnimator.acceleration = 0.001f;
				flaskAnimator.delay = 4f;
				LiquidVolume component = gameObject.GetComponent<LiquidVolume>();
				component.liquidColor1 = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				component.liquidColor2 = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				component.turbulence2 = 0f;
				component.refractionBlur = false;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x040007DD RID: 2013
		public float startingDelay = 1f;

		// Token: 0x040007DE RID: 2014
		public int numCylinders = 16;

		// Token: 0x040007DF RID: 2015
		public float scale = 0.2f;

		// Token: 0x040007E0 RID: 2016
		public float heightMultiplier = 2f;

		// Token: 0x040007E1 RID: 2017
		public float circleRadius = 1.75f;
	}
}
