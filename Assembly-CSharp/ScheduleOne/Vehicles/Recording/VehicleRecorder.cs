using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x020007CB RID: 1995
	public class VehicleRecorder : MonoBehaviour
	{
		// Token: 0x06003699 RID: 13977 RVA: 0x000E5D28 File Offset: 0x000E3F28
		protected virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				this.IS_RECORDING = !this.IS_RECORDING;
				if (this.IS_RECORDING)
				{
					this.keyFrames.Clear();
					this.vehicleToRecord = PlayerSingleton<PlayerMovement>.Instance.currentVehicle;
				}
			}
			if (this.vehicleToRecord && this.IS_RECORDING)
			{
				if (this.timeSinceKeyFrame >= 1f / (float)VehicleRecorder.frameRate)
				{
					this.timeSinceKeyFrame = 0f;
					VehicleKeyFrame item = this.Capture();
					this.keyFrames.Add(item);
				}
				Console.Log(this.vehicleToRecord.speed_Kmh, null);
				this.timeSinceKeyFrame += Time.deltaTime;
			}
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000E5DE0 File Offset: 0x000E3FE0
		private VehicleKeyFrame Capture()
		{
			VehicleKeyFrame vehicleKeyFrame = new VehicleKeyFrame();
			vehicleKeyFrame.position = this.vehicleToRecord.transform.position;
			vehicleKeyFrame.rotation = this.vehicleToRecord.transform.rotation;
			vehicleKeyFrame.brakesApplied = this.vehicleToRecord.brakesApplied;
			vehicleKeyFrame.reversing = this.vehicleToRecord.isReversing;
			if (this.vehicleToRecord.GetComponent<VehicleLights>())
			{
				vehicleKeyFrame.headlightsOn = this.vehicleToRecord.GetComponent<VehicleLights>().headLightsOn;
			}
			foreach (Wheel wheel in this.vehicleToRecord.wheels)
			{
				vehicleKeyFrame.wheels.Add(this.CaptureWheel(wheel));
			}
			return vehicleKeyFrame;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000E5EC0 File Offset: 0x000E40C0
		private VehicleKeyFrame.WheelTransform CaptureWheel(Wheel wheel)
		{
			return new VehicleKeyFrame.WheelTransform
			{
				yPos = wheel.transform.Find("Model").transform.localPosition.y,
				rotation = wheel.transform.Find("Model").transform.localRotation
			};
		}

		// Token: 0x04002778 RID: 10104
		public static int frameRate = 24;

		// Token: 0x04002779 RID: 10105
		public bool IS_RECORDING;

		// Token: 0x0400277A RID: 10106
		public List<VehicleKeyFrame> keyFrames = new List<VehicleKeyFrame>();

		// Token: 0x0400277B RID: 10107
		private LandVehicle vehicleToRecord;

		// Token: 0x0400277C RID: 10108
		private float timeSinceKeyFrame;
	}
}
