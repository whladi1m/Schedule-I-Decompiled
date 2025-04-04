using System;
using System.Collections.Generic;
using ScheduleOne;

// Token: 0x0200000F RID: 15
public static class GUIDManager
{
	// Token: 0x0600004F RID: 79 RVA: 0x000045B4 File Offset: 0x000027B4
	public static void RegisterObject(IGUIDRegisterable obj)
	{
		if (GUIDManager.registeredGUIDs.Contains(obj.GUID))
		{
			ScheduleOne.Console.LogWarning("RegisterObject called and passed obj whose GUID is already registered. Replacing old entries with new", null);
			GUIDManager.registeredGUIDs.Remove(obj.GUID);
			GUIDManager.guidToObject.Remove(obj.GUID);
		}
		GUIDManager.registeredGUIDs.Add(obj.GUID);
		GUIDManager.guidToObject.Add(obj.GUID, obj);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004621 File Offset: 0x00002821
	public static void DeregisterObject(IGUIDRegisterable obj)
	{
		GUIDManager.registeredGUIDs.Remove(obj.GUID);
		GUIDManager.guidToObject.Remove(obj.GUID);
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004648 File Offset: 0x00002848
	public static T GetObject<T>(Guid guid)
	{
		if (!GUIDManager.registeredGUIDs.Contains(guid))
		{
			return default(T);
		}
		object obj = GUIDManager.guidToObject[guid];
		if (!(obj is T))
		{
			ScheduleOne.Console.LogWarning("Object is not of requested type. Returning default(T)", null);
			return default(T);
		}
		return (T)((object)obj);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0000469C File Offset: 0x0000289C
	public static Type GetObjectType(Guid guid)
	{
		IGUIDRegisterable @object = GUIDManager.GetObject<IGUIDRegisterable>(guid);
		if (@object == null)
		{
			return null;
		}
		return @object.GetType();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000046BC File Offset: 0x000028BC
	public static Guid GenerateUniqueGUID()
	{
		Guid guid = default(Guid);
		bool flag = false;
		while (!flag)
		{
			guid = Guid.NewGuid();
			if (!GUIDManager.registeredGUIDs.Contains(guid))
			{
				flag = true;
			}
		}
		return guid;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000046EE File Offset: 0x000028EE
	public static bool IsGUIDAlreadyRegistered(Guid guid)
	{
		return GUIDManager.registeredGUIDs.Contains(guid);
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00004700 File Offset: 0x00002900
	public static bool IsGUIDValid(string guid)
	{
		return guid != null && !(guid == string.Empty) && !(new Guid(guid) == Guid.Empty);
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0000472B File Offset: 0x0000292B
	public static void Clear()
	{
		ScheduleOne.Console.Log("GUIDManager cleared!", null);
		GUIDManager.registeredGUIDs.Clear();
		GUIDManager.guidToObject.Clear();
	}

	// Token: 0x0400006A RID: 106
	private static List<Guid> registeredGUIDs = new List<Guid>();

	// Token: 0x0400006B RID: 107
	private static Dictionary<Guid, object> guidToObject = new Dictionary<Guid, object>();
}
