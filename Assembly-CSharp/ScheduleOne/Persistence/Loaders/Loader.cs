using System;
using System.Collections.Generic;
using System.IO;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000388 RID: 904
	public class Loader
	{
		// Token: 0x06001474 RID: 5236 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Load(string mainPath)
		{
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005B4D4 File Offset: 0x000596D4
		public bool TryLoadFile(string parentPath, string fileName, out string contents)
		{
			return this.TryLoadFile(Path.Combine(parentPath, fileName), out contents, true);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0005B4E8 File Offset: 0x000596E8
		public bool TryLoadFile(string path, out string contents, bool autoAddExtension = true)
		{
			contents = string.Empty;
			string text = path;
			if (autoAddExtension)
			{
				text += ".json";
			}
			if (!File.Exists(text))
			{
				return false;
			}
			try
			{
				contents = File.ReadAllText(text);
			}
			catch (Exception ex)
			{
				string str = "Error reading file: ";
				string str2 = text;
				string str3 = "\n";
				Exception ex2 = ex;
				Console.LogError(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), null);
				return false;
			}
			return true;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0005B560 File Offset: 0x00059760
		protected List<DirectoryInfo> GetDirectories(string parentPath)
		{
			if (!Directory.Exists(parentPath))
			{
				return new List<DirectoryInfo>();
			}
			List<DirectoryInfo> list = new List<DirectoryInfo>();
			string[] directories = Directory.GetDirectories(parentPath);
			for (int i = 0; i < directories.Length; i++)
			{
				list.Add(new DirectoryInfo(directories[i]));
			}
			return list;
		}
	}
}
