using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AEC RID: 2796
	public class ManagementWorldspaceCanvas : Singleton<ManagementWorldspaceCanvas>
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x001399ED File Offset: 0x00137BED
		// (set) Token: 0x06004AC5 RID: 19141 RVA: 0x001399F5 File Offset: 0x00137BF5
		public bool IsOpen { get; protected set; }

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06004AC6 RID: 19142 RVA: 0x001399FE File Offset: 0x00137BFE
		public Property CurrentProperty
		{
			get
			{
				return Player.Local.CurrentProperty;
			}
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x00139A0C File Offset: 0x00137C0C
		public void Open()
		{
			this.IsOpen = true;
			this.Canvas.enabled = true;
			for (int i = 0; i < this.SelectedConfigurables.Count; i++)
			{
				this.SelectedConfigurables[i].Selected();
				this.SelectedConfigurables[i].ShowOutline(this.SelectedOutlineColor);
			}
			this.UpdateInputPrompt();
		}

		// Token: 0x06004AC8 RID: 19144 RVA: 0x00139A70 File Offset: 0x00137C70
		public void Close(bool preserveSelection = false)
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
			{
				this.OutlinedConfigurable.Deselected();
				this.OutlinedConfigurable.HideOutline();
				this.OutlinedConfigurable = null;
			}
			if (this.HoveredConfigurable != null)
			{
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
			}
			if (!preserveSelection)
			{
				this.ClearSelection();
			}
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00139AEC File Offset: 0x00137CEC
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			this.UpdateUIs();
			if (this.IsOpen)
			{
				IConfigurable hoveredConfigurable = this.GetHoveredConfigurable();
				if (hoveredConfigurable != null && !hoveredConfigurable.IsBeingConfiguredByOtherPlayer)
				{
					this.HoveredConfigurable = hoveredConfigurable;
				}
				else
				{
					this.HoveredConfigurable = null;
				}
				this.UpdateSelection();
			}
			else if (this.HoveredConfigurable != null)
			{
				this.HoveredConfigurable.Deselected();
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
			}
			this.UpdateInputPrompt();
			if (Player.Local.CurrentProperty == null)
			{
				this.ClearSelection();
			}
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x00139B84 File Offset: 0x00137D84
		private void UpdateInputPrompt()
		{
			List<IConfigurable> list = new List<IConfigurable>();
			if (this.HoveredConfigurable != null && !this.SelectedConfigurables.Contains(this.HoveredConfigurable))
			{
				list.Add(this.HoveredConfigurable);
			}
			list.AddRange(this.SelectedConfigurables);
			if (list.Count == 0)
			{
				this.HideCrosshairPrompt();
				return;
			}
			bool flag = true;
			if (list.Count > 1)
			{
				for (int i = 0; i < list.Count - 1; i++)
				{
					if (list[i].ConfigurableType != list[i + 1].ConfigurableType)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				this.HideCrosshairPrompt();
				return;
			}
			string typeName = ConfigurableType.GetTypeName(list[0].ConfigurableType);
			if (list.Count > 1)
			{
				this.ShowCrosshairPrompt("Manage " + list.Count.ToString() + "x " + typeName);
				return;
			}
			this.ShowCrosshairPrompt("Manage " + typeName);
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x00139C74 File Offset: 0x00137E74
		private void UpdateUIs()
		{
			foreach (Property property in Property.OwnedProperties)
			{
				float num = Vector3.Distance(property.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
				property.WorldspaceUIContainer.gameObject.SetActive(this.IsOpen && num < 50f);
			}
			List<IConfigurable> configurablesToShow = this.GetConfigurablesToShow();
			this.RemoveNullConfigurables();
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				if (!configurablesToShow.Contains(this.ShownConfigurables[i]) && this.ShownConfigurables[i].WorldspaceUI.IsEnabled)
				{
					IConfigurable config = this.ShownConfigurables[i];
					this.ShownConfigurables[i].WorldspaceUI.Hide(delegate
					{
						this.ShownConfigurables.Remove(config);
					});
				}
			}
			for (int j = 0; j < configurablesToShow.Count; j++)
			{
				if (!this.ShownConfigurables.Contains(configurablesToShow[j]))
				{
					configurablesToShow[j].WorldspaceUI.Show();
					if (!this.ShownConfigurables.Contains(configurablesToShow[j]))
					{
						this.ShownConfigurables.Add(configurablesToShow[j]);
					}
				}
			}
		}

		// Token: 0x06004ACC RID: 19148 RVA: 0x00139DFC File Offset: 0x00137FFC
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.RemoveNullConfigurables();
			this.ShownConfigurables.Sort((IConfigurable a, IConfigurable b) => Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, a.UIPoint.position).CompareTo(Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, b.UIPoint.position)));
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				this.ShownConfigurables[i].WorldspaceUI.SetInternalScale(this.ScaleCurve.Evaluate(Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.ShownConfigurables[i].UIPoint.position) / 5f));
				this.ShownConfigurables[i].WorldspaceUI.UpdatePosition(this.ShownConfigurables[i].UIPoint.position);
				this.ShownConfigurables[i].WorldspaceUI.transform.SetAsFirstSibling();
			}
		}

		// Token: 0x06004ACD RID: 19149 RVA: 0x00139EF8 File Offset: 0x001380F8
		private void UpdateSelection()
		{
			if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.ClearSelection();
			}
			if (this.HoveredConfigurable == null)
			{
				if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
				{
					this.OutlinedConfigurable.Deselected();
					this.OutlinedConfigurable.HideOutline();
					this.OutlinedConfigurable = null;
				}
				return;
			}
			if (this.HoveredConfigurable != null && this.HoveredConfigurable.IsBeingConfiguredByOtherPlayer)
			{
				this.HoveredConfigurable.Deselected();
				this.HoveredConfigurable.HideOutline();
				this.HoveredConfigurable = null;
				return;
			}
			for (int i = 0; i < this.SelectedConfigurables.Count; i++)
			{
				if (this.SelectedConfigurables[i].IsBeingConfiguredByOtherPlayer)
				{
					this.RemoveFromSelection(this.SelectedConfigurables[i]);
					i--;
				}
			}
			if (!this.SelectedConfigurables.Contains(this.HoveredConfigurable) && this.OutlinedConfigurable != this.HoveredConfigurable)
			{
				if (this.OutlinedConfigurable != null && !this.SelectedConfigurables.Contains(this.OutlinedConfigurable))
				{
					this.OutlinedConfigurable.Deselected();
					this.OutlinedConfigurable.HideOutline();
					this.OutlinedConfigurable = null;
				}
				this.HoveredConfigurable.Selected();
				this.HoveredConfigurable.ShowOutline(this.HoveredOutlineColor);
				this.OutlinedConfigurable = this.HoveredConfigurable;
			}
			if (this.HoveredConfigurable == null || !this.HoveredConfigurable.CanBeSelected)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				if (this.SelectedConfigurables.Contains(this.HoveredConfigurable))
				{
					this.RemoveFromSelection(this.HoveredConfigurable);
					return;
				}
				this.AddToSelection(this.HoveredConfigurable);
			}
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0013A091 File Offset: 0x00138291
		private void AddToSelection(IConfigurable config)
		{
			if (this.SelectedConfigurables.Contains(config))
			{
				return;
			}
			config.ShowOutline(this.SelectedOutlineColor);
			config.Selected();
			this.SelectedConfigurables.Add(config);
		}

		// Token: 0x06004ACF RID: 19151 RVA: 0x0013A0C0 File Offset: 0x001382C0
		private void RemoveFromSelection(IConfigurable config)
		{
			if (this.HoveredConfigurable != config)
			{
				config.Deselected();
				config.HideOutline();
			}
			else
			{
				config.ShowOutline(this.HoveredOutlineColor);
			}
			if (this.SelectedConfigurables.Contains(config))
			{
				this.SelectedConfigurables.Remove(config);
			}
		}

		// Token: 0x06004AD0 RID: 19152 RVA: 0x0013A100 File Offset: 0x00138300
		private void ClearSelection()
		{
			IConfigurable[] array = this.SelectedConfigurables.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				this.RemoveFromSelection(array[i]);
			}
		}

		// Token: 0x06004AD1 RID: 19153 RVA: 0x0013A130 File Offset: 0x00138330
		private void RemoveNullConfigurables()
		{
			for (int i = 0; i < this.ShownConfigurables.Count; i++)
			{
				if (this.ShownConfigurables[i].IsDestroyed)
				{
					this.ShownConfigurables.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0013A178 File Offset: 0x00138378
		private IConfigurable GetHoveredConfigurable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.ObjectSelectionLayerMask, true, 0f))
			{
				IConfigurable componentInParent = raycastHit.collider.GetComponentInParent<IConfigurable>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0013A1B8 File Offset: 0x001383B8
		private List<IConfigurable> GetConfigurablesToShow()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return new List<IConfigurable>();
			}
			List<IConfigurable> list = new List<IConfigurable>();
			if (this.CurrentProperty != null && this.CurrentProperty.IsOwned)
			{
				for (int i = 0; i < this.CurrentProperty.Configurables.Count; i++)
				{
					if (this.CurrentProperty.Configurables[i] != null && !this.CurrentProperty.Configurables[i].IsDestroyed && Vector3.Distance(this.CurrentProperty.Configurables[i].Transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) <= 5f)
					{
						list.Add(this.CurrentProperty.Configurables[i]);
					}
				}
			}
			for (int j = 0; j < this.SelectedConfigurables.Count; j++)
			{
				if (!list.Contains(this.SelectedConfigurables[j]))
				{
					list.Add(this.SelectedConfigurables[j]);
				}
			}
			if (!list.Contains(this.HoveredConfigurable) && this.HoveredConfigurable != null)
			{
				list.Add(this.HoveredConfigurable);
			}
			return list;
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0013A2EE File Offset: 0x001384EE
		public void ShowCrosshairPrompt(string message)
		{
			this.CrosshairPrompt.SetLabel(message);
			this.CrosshairPrompt.gameObject.SetActive(true);
			this.CrosshairPrompt.transform.SetAsLastSibling();
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0013A31D File Offset: 0x0013851D
		public void HideCrosshairPrompt()
		{
			this.CrosshairPrompt.gameObject.SetActive(false);
		}

		// Token: 0x04003844 RID: 14404
		public const float VISIBILITY_RANGE = 5f;

		// Token: 0x04003845 RID: 14405
		public const float PROPERTY_CANVAS_RANGE = 50f;

		// Token: 0x04003847 RID: 14407
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003848 RID: 14408
		public AnimationCurve ScaleCurve;

		// Token: 0x04003849 RID: 14409
		public TransitLineVisuals TransitRouteVisualsPrefab;

		// Token: 0x0400384A RID: 14410
		public InputPrompt CrosshairPrompt;

		// Token: 0x0400384B RID: 14411
		[Header("Settings")]
		public LayerMask ObjectSelectionLayerMask;

		// Token: 0x0400384C RID: 14412
		public Color HoveredOutlineColor = Color.white;

		// Token: 0x0400384D RID: 14413
		public Color SelectedOutlineColor = Color.white;

		// Token: 0x0400384E RID: 14414
		private List<IConfigurable> ShownConfigurables = new List<IConfigurable>();

		// Token: 0x0400384F RID: 14415
		public IConfigurable HoveredConfigurable;

		// Token: 0x04003850 RID: 14416
		private IConfigurable OutlinedConfigurable;

		// Token: 0x04003851 RID: 14417
		public List<IConfigurable> SelectedConfigurables = new List<IConfigurable>();
	}
}
