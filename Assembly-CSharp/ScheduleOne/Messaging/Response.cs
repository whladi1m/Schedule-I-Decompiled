using System;
using FishNet.Serializing.Helping;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000547 RID: 1351
	[Serializable]
	public class Response
	{
		// Token: 0x0600214E RID: 8526 RVA: 0x000894AA File Offset: 0x000876AA
		public Response(string _text, string _label, Action _callback = null, bool _disableDefaultResponseBehaviour = false)
		{
			this.text = _text;
			this.label = _label;
			this.callback = _callback;
			this.disableDefaultResponseBehaviour = _disableDefaultResponseBehaviour;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x0000494F File Offset: 0x00002B4F
		public Response()
		{
		}

		// Token: 0x0400198B RID: 6539
		public string text;

		// Token: 0x0400198C RID: 6540
		public string label;

		// Token: 0x0400198D RID: 6541
		[CodegenExclude]
		public Action callback;

		// Token: 0x0400198E RID: 6542
		public bool disableDefaultResponseBehaviour;
	}
}
