using System;
using UnityEngine;
using UnityEngine.UI;

namespace ItemIconCreator
{
	// Token: 0x02000222 RID: 546
	public class IconCreatorCanvas : MonoBehaviour
	{
		// Token: 0x06000BA6 RID: 2982 RVA: 0x00036357 File Offset: 0x00034557
		private void Awake()
		{
			IconCreatorCanvas.instance = this;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00036360 File Offset: 0x00034560
		public void SetInfo(int totalItens, int currentItem, string itemName, bool isRecording, KeyCode key)
		{
			this.borders.gameObject.SetActive(isRecording);
			if (!isRecording)
			{
				this.textLabel.text = "Go to your icon builder in hierarchy and press 'Build icons'";
				return;
			}
			this.textLabel.text = string.Concat(new string[]
			{
				currentItem.ToString(),
				" / ",
				totalItens.ToString(),
				" - ",
				itemName,
				"   |   Press <b>",
				key.ToString(),
				"</b> to continue"
			});
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000363F2 File Offset: 0x000345F2
		public void SetTakingPicture()
		{
			this.textLabel.text = "Generating icon...";
		}

		// Token: 0x04000D10 RID: 3344
		public Text textLabel;

		// Token: 0x04000D11 RID: 3345
		public GameObject borders;

		// Token: 0x04000D12 RID: 3346
		public static IconCreatorCanvas instance;
	}
}
