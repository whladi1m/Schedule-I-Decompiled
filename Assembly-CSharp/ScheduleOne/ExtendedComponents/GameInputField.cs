using System;
using TMPro;
using UnityEngine.Events;

namespace ScheduleOne.ExtendedComponents
{
	// Token: 0x02000619 RID: 1561
	public class GameInputField : TMP_InputField
	{
		// Token: 0x06002915 RID: 10517 RVA: 0x000A96BD File Offset: 0x000A78BD
		protected override void Awake()
		{
			base.Awake();
			base.onSelect.AddListener(new UnityAction<string>(this.EditStart));
			base.onEndEdit.AddListener(new UnityAction<string>(this.EndEdit));
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000A96F3 File Offset: 0x000A78F3
		private void EditStart(string newVal)
		{
			GameInput.IsTyping = true;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000A96FB File Offset: 0x000A78FB
		private void EndEdit(string newVal)
		{
			GameInput.IsTyping = false;
		}
	}
}
