using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI.Management;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000584 RID: 1412
	public class TransitRoute
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x00090145 File Offset: 0x0008E345
		// (set) Token: 0x0600234B RID: 9035 RVA: 0x0009014D File Offset: 0x0008E34D
		public ITransitEntity Source { get; protected set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x00090156 File Offset: 0x0008E356
		// (set) Token: 0x0600234D RID: 9037 RVA: 0x0009015E File Offset: 0x0008E35E
		public ITransitEntity Destination { get; protected set; }

		// Token: 0x0600234E RID: 9038 RVA: 0x00090167 File Offset: 0x0008E367
		public TransitRoute(ITransitEntity source, ITransitEntity destination)
		{
			this.Source = source;
			this.Destination = destination;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onFixedUpdate = (Action)Delegate.Combine(instance.onFixedUpdate, new Action(this.Update));
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000901A4 File Offset: 0x0008E3A4
		public void Destroy()
		{
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onFixedUpdate = (Action)Delegate.Remove(instance.onFixedUpdate, new Action(this.Update));
			if (this.visuals != null)
			{
				UnityEngine.Object.Destroy(this.visuals.gameObject);
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000901F8 File Offset: 0x0008E3F8
		public void SetVisualsActive(bool active)
		{
			if (this.visuals == null)
			{
				this.visuals = UnityEngine.Object.Instantiate<GameObject>(Singleton<ManagementWorldspaceCanvas>.Instance.TransitRouteVisualsPrefab.gameObject, GameObject.Find("_Temp").transform).GetComponent<TransitLineVisuals>();
			}
			this.visuals.gameObject.SetActive(active);
			if (active)
			{
				this.Update();
			}
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0009025C File Offset: 0x0008E45C
		private void Update()
		{
			this.ValidateEntities();
			if (this.visuals == null || !this.visuals.gameObject.activeSelf)
			{
				return;
			}
			if (this.Source == null || this.Destination == null)
			{
				this.visuals.gameObject.SetActive(false);
				return;
			}
			Vector3.Distance(this.Source.LinkOrigin.position, this.Destination.LinkOrigin.position);
			this.visuals.SetSourcePosition(this.Source.LinkOrigin.position);
			this.visuals.SetDestinationPosition(this.Destination.LinkOrigin.position);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0009030E File Offset: 0x0008E50E
		public virtual void SetSource(ITransitEntity source)
		{
			this.Source = source;
			if (this.onSourceChange != null)
			{
				this.onSourceChange(this.Source);
			}
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x00090330 File Offset: 0x0008E530
		public bool AreEntitiesNonNull()
		{
			this.ValidateEntities();
			return this.Source != null && this.Destination != null;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0009034B File Offset: 0x0008E54B
		public virtual void SetDestination(ITransitEntity destination)
		{
			this.Destination = destination;
			if (this.onDestinationChange != null)
			{
				this.onDestinationChange(this.Destination);
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0009036D File Offset: 0x0008E56D
		private void ValidateEntities()
		{
			if (this.Source != null && this.Source.IsDestroyed)
			{
				this.SetSource(null);
			}
			if (this.Destination != null && this.Destination.IsDestroyed)
			{
				this.SetDestination(null);
			}
		}

		// Token: 0x04001A6D RID: 6765
		protected TransitLineVisuals visuals;

		// Token: 0x04001A6E RID: 6766
		public Action<ITransitEntity> onSourceChange;

		// Token: 0x04001A6F RID: 6767
		public Action<ITransitEntity> onDestinationChange;
	}
}
