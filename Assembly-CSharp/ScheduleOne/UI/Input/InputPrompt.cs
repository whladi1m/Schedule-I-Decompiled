using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.UI.Input
{
	// Token: 0x02000B1A RID: 2842
	[ExecuteInEditMode]
	public class InputPrompt : MonoBehaviour
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x0013D8CB File Offset: 0x0013BACB
		private InputPromptsManager manager
		{
			get
			{
				if (!Singleton<InputPromptsManager>.InstanceExists)
				{
					return GameObject.Find("@InputPromptsManager").GetComponent<InputPromptsManager>();
				}
				return Singleton<InputPromptsManager>.Instance;
			}
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x0013D8E9 File Offset: 0x0013BAE9
		private void OnEnable()
		{
			this.RefreshPromptImages();
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0013D902 File Offset: 0x0013BB02
		private void OnDisable()
		{
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x0013D918 File Offset: 0x0013BB18
		private void RefreshPromptImages()
		{
			this.AppliedAlignment = this.Alignment;
			this.displayedActions.Clear();
			this.displayedActions.AddRange(this.Actions);
			int childCount = this.ImagesContainer.childCount;
			Transform[] array = new Transform[childCount];
			for (int i = 0; i < childCount; i++)
			{
				array[i] = this.ImagesContainer.GetChild(i);
			}
			for (int j = 0; j < childCount; j++)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(array[j].gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(array[j].gameObject);
				}
			}
			this.promptImages.Clear();
			float num = 0f;
			for (int k = 0; k < this.Actions.Count; k++)
			{
				string text;
				string controlPath;
				this.Actions[k].action.GetBindingDisplayString(0, out text, out controlPath, (InputBinding.DisplayStringOptions)0);
				PromptImage promptImage = this.manager.GetPromptImage(controlPath, this.ImagesContainer);
				if (!(promptImage == null))
				{
					num += promptImage.Width;
					foreach (Image image in promptImage.transform.GetComponentsInChildren<Image>())
					{
						if (this.OverridePromptImageColor)
						{
							image.color = this.PromptImageColor;
						}
					}
					this.promptImages.Add(promptImage);
				}
			}
			num += InputPrompt.Spacing * (float)this.Actions.Count;
			this.LabelComponent.text = this.Label;
			this.LabelComponent.ForceMeshUpdate(false, false);
			num += this.LabelComponent.preferredWidth;
			float num2 = 0f;
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				num2 = -InputPrompt.Spacing;
			}
			else if (this.Alignment == InputPrompt.EInputPromptAlignment.Middle)
			{
				num2 = -num / 2f;
			}
			else if (this.Alignment == InputPrompt.EInputPromptAlignment.Right)
			{
				num2 = InputPrompt.Spacing;
			}
			float num3 = 1f;
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				this.LabelComponent.alignment = TextAlignmentOptions.CaplineRight;
				num3 = -1f;
			}
			else
			{
				this.LabelComponent.alignment = TextAlignmentOptions.CaplineLeft;
			}
			float num4 = 0f;
			for (int m = 0; m < this.promptImages.Count; m++)
			{
				this.promptImages[m].GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + num4 * num3 + this.promptImages[m].Width * 0.5f * num3, 0f);
				num4 += this.promptImages[m].Width + InputPrompt.Spacing;
			}
			this.LabelComponent.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + num4 * num3 + this.LabelComponent.GetComponent<RectTransform>().sizeDelta.x * 0.5f * num3, 0f);
			this.UpdateShade();
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x0013DBF3 File Offset: 0x0013BDF3
		public void SetLabel(string label)
		{
			this.Label = label;
			this.LabelComponent.text = this.Label;
			this.UpdateShade();
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0013DC14 File Offset: 0x0013BE14
		private void UpdateShade()
		{
			float num = this.LabelComponent.preferredWidth + 90f;
			this.Shade.sizeDelta = new Vector2(num, this.Shade.sizeDelta.y);
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				this.Shade.anchoredPosition = new Vector2(-num / 2f, 0f);
				return;
			}
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Middle)
			{
				this.Shade.anchoredPosition = new Vector2(0f, 0f);
				return;
			}
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Right)
			{
				this.Shade.anchoredPosition = new Vector2(num / 2f, 0f);
			}
		}

		// Token: 0x040038F9 RID: 14585
		public static float Spacing = 10f;

		// Token: 0x040038FA RID: 14586
		[Header("Settings")]
		public List<InputActionReference> Actions = new List<InputActionReference>();

		// Token: 0x040038FB RID: 14587
		public string Label;

		// Token: 0x040038FC RID: 14588
		public InputPrompt.EInputPromptAlignment Alignment;

		// Token: 0x040038FD RID: 14589
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040038FE RID: 14590
		public RectTransform ImagesContainer;

		// Token: 0x040038FF RID: 14591
		public TextMeshProUGUI LabelComponent;

		// Token: 0x04003900 RID: 14592
		public RectTransform Shade;

		// Token: 0x04003901 RID: 14593
		[Header("Settings")]
		public bool OverridePromptImageColor;

		// Token: 0x04003902 RID: 14594
		public Color PromptImageColor = Color.white;

		// Token: 0x04003903 RID: 14595
		[SerializeField]
		private List<PromptImage> promptImages = new List<PromptImage>();

		// Token: 0x04003904 RID: 14596
		private List<InputActionReference> displayedActions = new List<InputActionReference>();

		// Token: 0x04003905 RID: 14597
		private InputPrompt.EInputPromptAlignment AppliedAlignment;

		// Token: 0x02000B1B RID: 2843
		public enum EInputPromptAlignment
		{
			// Token: 0x04003907 RID: 14599
			Left,
			// Token: 0x04003908 RID: 14600
			Middle,
			// Token: 0x04003909 RID: 14601
			Right
		}
	}
}
