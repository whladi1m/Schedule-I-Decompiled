using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009BE RID: 2494
	public class DialogueCanvas : Singleton<DialogueCanvas>
	{
		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x0011A847 File Offset: 0x00118A47
		public bool isActive
		{
			get
			{
				return this.currentHandler != null;
			}
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0011A855 File Offset: 0x00118A55
		protected override void Awake()
		{
			base.Awake();
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x0011A88C File Offset: 0x00118A8C
		public void DisplayDialogueNode(DialogueHandler diag, DialogueNodeData node, string dialogueText, List<string> choices)
		{
			if (diag != this.currentHandler)
			{
				this.StartDialogue(diag);
			}
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			this.currentNode = node;
			this.dialogueRollout = base.StartCoroutine(this.RolloutDialogue(dialogueText, choices));
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x0011A8E0 File Offset: 0x00118AE0
		public void OverrideText(string text)
		{
			this.overrideText = text;
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			this.dialogueText.text = this.overrideText;
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0011A936 File Offset: 0x00118B36
		public void StopTextOverride()
		{
			this.overrideText = string.Empty;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0011A943 File Offset: 0x00118B43
		private void Update()
		{
			if (this.isActive)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					this.spaceDownThisFrame = true;
				}
				else
				{
					this.spaceDownThisFrame = false;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					this.leftClickThisFrame = true;
					return;
				}
				this.leftClickThisFrame = false;
			}
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0011A97D File Offset: 0x00118B7D
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isActive)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (!DialogueHandler.activeDialogue.AllowExit)
			{
				return;
			}
			action.used = true;
			this.currentHandler.EndDialogue();
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0011A9BA File Offset: 0x00118BBA
		protected IEnumerator RolloutDialogue(string text, List<string> choices)
		{
			List<int> activeDialogueChoices = new List<int>();
			this.dialogueText.maxVisibleCharacters = 0;
			this.dialogueText.text = text;
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			float rolloutTime = (float)text.Length * 0.015f;
			if (this.SkipNextRollout)
			{
				this.SkipNextRollout = false;
				rolloutTime = 0f;
			}
			float i = 0f;
			while (i < rolloutTime && !this.spaceDownThisFrame && !this.leftClickThisFrame)
			{
				int maxVisibleCharacters = (int)(i / 0.015f);
				this.dialogueText.maxVisibleCharacters = maxVisibleCharacters;
				yield return new WaitForEndOfFrame();
				i += Time.deltaTime;
			}
			this.dialogueText.maxVisibleCharacters = text.Length;
			this.spaceDownThisFrame = false;
			this.leftClickThisFrame = false;
			this.hasChoiceBeenSelected = false;
			if (this.choiceSelectionResidualCoroutine != null)
			{
				base.StopCoroutine(this.choiceSelectionResidualCoroutine);
			}
			this.continuePopup.gameObject.SetActive(false);
			for (int j = 0; j < this.dialogueChoices.Count; j++)
			{
				this.dialogueChoices[j].gameObject.SetActive(false);
				this.dialogueChoices[j].canvasGroup.alpha = 1f;
				if (choices.Count > j)
				{
					this.dialogueChoices[j].text.text = choices[j];
					this.dialogueChoices[j].button.interactable = true;
					string empty = string.Empty;
					if (this.IsChoiceValid(j, out empty))
					{
						this.dialogueChoices[j].notPossibleGameObject.SetActive(false);
						this.dialogueChoices[j].button.interactable = true;
						ColorBlock colors = this.dialogueChoices[j].button.colors;
						colors.disabledColor = colors.pressedColor;
						this.dialogueChoices[j].button.colors = colors;
						this.dialogueChoices[j].text.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
					}
					else
					{
						this.dialogueChoices[j].notPossibleText.text = empty.ToUpper();
						this.dialogueChoices[j].notPossibleGameObject.SetActive(true);
						ColorBlock colors2 = this.dialogueChoices[j].button.colors;
						colors2.disabledColor = colors2.normalColor;
						this.dialogueChoices[j].button.colors = colors2;
						this.dialogueChoices[j].button.interactable = false;
						this.dialogueChoices[j].notPossibleText.ForceMeshUpdate(false, false);
						this.dialogueChoices[j].text.GetComponent<RectTransform>().offsetMax = new Vector2(-(this.dialogueChoices[j].notPossibleText.preferredWidth + 20f), 0f);
					}
					activeDialogueChoices.Add(j);
				}
			}
			if (activeDialogueChoices.Count == 0 || (activeDialogueChoices.Count == 1 && choices[0] == ""))
			{
				this.continuePopup.gameObject.SetActive(true);
				yield return new WaitUntil(() => this.spaceDownThisFrame || this.leftClickThisFrame);
				this.continuePopup.gameObject.SetActive(false);
				this.spaceDownThisFrame = false;
				this.leftClickThisFrame = false;
				this.currentHandler.ContinueSubmitted();
			}
			else
			{
				for (int k = 0; k < activeDialogueChoices.Count; k++)
				{
					this.dialogueChoices[activeDialogueChoices[k]].gameObject.SetActive(true);
				}
				while (!this.hasChoiceBeenSelected)
				{
					string empty2 = string.Empty;
					if (Input.GetKey(KeyCode.Alpha1) && this.IsChoiceValid(0, out empty2))
					{
						this.ChoiceSelected(0);
					}
					else if (Input.GetKey(KeyCode.Alpha2) && this.IsChoiceValid(1, out empty2))
					{
						this.ChoiceSelected(1);
					}
					else if (Input.GetKey(KeyCode.Alpha3) && this.IsChoiceValid(2, out empty2))
					{
						this.ChoiceSelected(2);
					}
					else if (Input.GetKey(KeyCode.Alpha4) && this.IsChoiceValid(3, out empty2))
					{
						this.ChoiceSelected(3);
					}
					else if (Input.GetKey(KeyCode.Alpha5) && this.IsChoiceValid(4, out empty2))
					{
						this.ChoiceSelected(4);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(5, out empty2))
					{
						this.ChoiceSelected(5);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(6, out empty2))
					{
						this.ChoiceSelected(6);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(7, out empty2))
					{
						this.ChoiceSelected(7);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(8, out empty2))
					{
						this.ChoiceSelected(8);
					}
					yield return new WaitForEndOfFrame();
				}
			}
			yield break;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0011A9D7 File Offset: 0x00118BD7
		private IEnumerator ChoiceSelectionResidual(DialogueChoiceEntry choice, float fadeTime)
		{
			yield return new WaitForSeconds(0.25f);
			float realFadeTime = fadeTime - 0.25f;
			for (float i = 0f; i < realFadeTime; i += Time.deltaTime)
			{
				choice.canvasGroup.alpha = Mathf.Sqrt(Mathf.Lerp(1f, 0f, i / realFadeTime));
				yield return new WaitForEndOfFrame();
			}
			choice.gameObject.SetActive(false);
			this.choiceSelectionResidualCoroutine = null;
			yield break;
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0011A9F4 File Offset: 0x00118BF4
		private void StartDialogue(DialogueHandler handler)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.currentHandler = handler;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			Vector3 normalized = (this.currentHandler.LookPosition.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized;
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z), Vector3.up);
			PlayerSingleton<PlayerMovement>.Instance.LerpPlayerRotation(quaternion, 0.3f);
			Vector3 vector = new Vector3(Mathf.Sqrt(Mathf.Pow(normalized.x, 2f) + Mathf.Pow(normalized.z, 2f)), normalized.y, 0f);
			float x = -Mathf.Atan2(vector.y, vector.x) * 57.295776f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, quaternion * Quaternion.Euler(x, 0f, 0f), 0.3f, true);
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0011AB30 File Offset: 0x00118D30
		public void EndDialogue()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.continuePopup.gameObject.SetActive(false);
			for (int i = 0; i < this.dialogueChoices.Count; i++)
			{
				this.dialogueChoices[i].gameObject.SetActive(false);
			}
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			if (this.choiceSelectionResidualCoroutine != null)
			{
				base.StopCoroutine(this.choiceSelectionResidualCoroutine);
			}
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.currentHandler = null;
			this.currentNode = null;
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0011AC38 File Offset: 0x00118E38
		public void ChoiceSelected(int choiceIndex)
		{
			string empty = string.Empty;
			if (!this.IsChoiceValid(choiceIndex, out empty))
			{
				return;
			}
			this.hasChoiceBeenSelected = true;
			for (int i = 0; i < this.dialogueChoices.Count; i++)
			{
				if (i == choiceIndex)
				{
					this.dialogueChoices[i].button.interactable = false;
					if (this.choiceSelectionResidualCoroutine != null)
					{
						base.StopCoroutine(this.choiceSelectionResidualCoroutine);
					}
					this.choiceSelectionResidualCoroutine = base.StartCoroutine(this.ChoiceSelectionResidual(this.dialogueChoices[i], 0.75f));
				}
				else
				{
					this.dialogueChoices[i].gameObject.SetActive(false);
				}
			}
			this.currentHandler.ChoiceSelected(choiceIndex);
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0011ACEC File Offset: 0x00118EEC
		private bool IsChoiceValid(int choiceIndex, out string reason)
		{
			if (this.currentNode != null && this.currentHandler.CurrentChoices.Count > choiceIndex)
			{
				return this.currentHandler.CheckChoice(this.currentHandler.CurrentChoices[choiceIndex].ChoiceLabel, out reason);
			}
			reason = string.Empty;
			return false;
		}

		// Token: 0x04003145 RID: 12613
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003146 RID: 12614
		public bool SkipNextRollout;

		// Token: 0x04003147 RID: 12615
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003148 RID: 12616
		public RectTransform Container;

		// Token: 0x04003149 RID: 12617
		[SerializeField]
		protected TextMeshProUGUI dialogueText;

		// Token: 0x0400314A RID: 12618
		[SerializeField]
		protected GameObject continuePopup;

		// Token: 0x0400314B RID: 12619
		[SerializeField]
		protected List<DialogueChoiceEntry> dialogueChoices = new List<DialogueChoiceEntry>();

		// Token: 0x0400314C RID: 12620
		private DialogueHandler currentHandler;

		// Token: 0x0400314D RID: 12621
		private DialogueNodeData currentNode;

		// Token: 0x0400314E RID: 12622
		private bool spaceDownThisFrame;

		// Token: 0x0400314F RID: 12623
		private bool leftClickThisFrame;

		// Token: 0x04003150 RID: 12624
		private string overrideText = string.Empty;

		// Token: 0x04003151 RID: 12625
		private Coroutine dialogueRollout;

		// Token: 0x04003152 RID: 12626
		private Coroutine choiceSelectionResidualCoroutine;

		// Token: 0x04003153 RID: 12627
		private bool hasChoiceBeenSelected;
	}
}
