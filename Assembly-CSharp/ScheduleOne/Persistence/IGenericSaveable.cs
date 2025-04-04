using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000362 RID: 866
	public interface IGenericSaveable
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001392 RID: 5010
		Guid GUID { get; }

		// Token: 0x06001393 RID: 5011 RVA: 0x00057472 File Offset: 0x00055672
		void InitializeSaveable()
		{
			if (!Singleton<GenericSaveablesManager>.InstanceExists)
			{
				Console.LogError("GenericSaveablesManager does not exist in scene.", null);
				return;
			}
			Singleton<GenericSaveablesManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06001394 RID: 5012
		void Load(GenericSaveData data);

		// Token: 0x06001395 RID: 5013
		GenericSaveData GetSaveData();
	}
}
