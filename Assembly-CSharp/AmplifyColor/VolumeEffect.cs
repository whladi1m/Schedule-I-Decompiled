using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C1F RID: 3103
	[Serializable]
	public class VolumeEffect
	{
		// Token: 0x060056C8 RID: 22216 RVA: 0x0016C068 File Offset: 0x0016A268
		public VolumeEffect(AmplifyColorEffect effect)
		{
			this.gameObject = effect;
			this.components = new List<VolumeEffectComponent>();
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0016C084 File Offset: 0x0016A284
		public static VolumeEffect BlendValuesToVolumeEffect(VolumeEffectFlags flags, VolumeEffect volume1, VolumeEffect volume2, float blend)
		{
			VolumeEffect volumeEffect = new VolumeEffect(volume1.gameObject);
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in flags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					VolumeEffectComponent volumeEffectComponent = volume1.FindEffectComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent2 = volume2.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						VolumeEffectComponent volumeEffectComponent3 = new VolumeEffectComponent(volumeEffectComponent.componentName);
						foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (volumeEffectField != null && volumeEffectField2 != null)
								{
									VolumeEffectField volumeEffectField3 = new VolumeEffectField(volumeEffectField.fieldName, volumeEffectField.fieldType);
									string fieldType = volumeEffectField3.fieldType;
									if (!(fieldType == "System.Single"))
									{
										if (!(fieldType == "System.Boolean"))
										{
											if (!(fieldType == "UnityEngine.Vector2"))
											{
												if (!(fieldType == "UnityEngine.Vector3"))
												{
													if (!(fieldType == "UnityEngine.Vector4"))
													{
														if (fieldType == "UnityEngine.Color")
														{
															volumeEffectField3.valueColor = Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blend);
														}
													}
													else
													{
														volumeEffectField3.valueVector4 = Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blend);
													}
												}
												else
												{
													volumeEffectField3.valueVector3 = Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blend);
												}
											}
											else
											{
												volumeEffectField3.valueVector2 = Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blend);
											}
										}
										else
										{
											volumeEffectField3.valueBoolean = volumeEffectField2.valueBoolean;
										}
									}
									else
									{
										volumeEffectField3.valueSingle = Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blend);
									}
									volumeEffectComponent3.fields.Add(volumeEffectField3);
								}
							}
						}
						volumeEffect.components.Add(volumeEffectComponent3);
					}
				}
			}
			return volumeEffect;
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0016C2F4 File Offset: 0x0016A4F4
		public VolumeEffectComponent AddComponent(Component c, VolumeEffectComponentFlags compFlags)
		{
			if (compFlags == null)
			{
				Type type = c.GetType();
				VolumeEffectComponent volumeEffectComponent = new VolumeEffectComponent(((type != null) ? type.ToString() : null) ?? "");
				this.components.Add(volumeEffectComponent);
				return volumeEffectComponent;
			}
			Type type2 = c.GetType();
			VolumeEffectComponent volumeEffectComponent2;
			if ((volumeEffectComponent2 = this.FindEffectComponent(((type2 != null) ? type2.ToString() : null) ?? "")) != null)
			{
				volumeEffectComponent2.UpdateComponent(c, compFlags);
				return volumeEffectComponent2;
			}
			VolumeEffectComponent volumeEffectComponent3 = new VolumeEffectComponent(c, compFlags);
			this.components.Add(volumeEffectComponent3);
			return volumeEffectComponent3;
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x0016C377 File Offset: 0x0016A577
		public void RemoveEffectComponent(VolumeEffectComponent comp)
		{
			this.components.Remove(comp);
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x0016C388 File Offset: 0x0016A588
		public void UpdateVolume()
		{
			if (this.gameObject == null)
			{
				return;
			}
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in this.gameObject.EffectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = this.gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						this.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x0016C41C File Offset: 0x0016A61C
		public void SetValues(AmplifyColorEffect targetColor)
		{
			VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			GameObject gameObject = targetColor.gameObject;
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in effectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null)
					{
						foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (!(field == null) && volumeEffectField != null)
								{
									string fullName = field.FieldType.FullName;
									if (!(fullName == "System.Single"))
									{
										if (!(fullName == "System.Boolean"))
										{
											if (!(fullName == "UnityEngine.Vector2"))
											{
												if (!(fullName == "UnityEngine.Vector3"))
												{
													if (!(fullName == "UnityEngine.Vector4"))
													{
														if (fullName == "UnityEngine.Color")
														{
															field.SetValue(component, volumeEffectField.valueColor);
														}
													}
													else
													{
														field.SetValue(component, volumeEffectField.valueVector4);
													}
												}
												else
												{
													field.SetValue(component, volumeEffectField.valueVector3);
												}
											}
											else
											{
												field.SetValue(component, volumeEffectField.valueVector2);
											}
										}
										else
										{
											field.SetValue(component, volumeEffectField.valueBoolean);
										}
									}
									else
									{
										field.SetValue(component, volumeEffectField.valueSingle);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x0016C63C File Offset: 0x0016A83C
		public void BlendValues(AmplifyColorEffect targetColor, VolumeEffect other, float blendAmount)
		{
			VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			GameObject gameObject = targetColor.gameObject;
			for (int i = 0; i < effectFlags.components.Count; i++)
			{
				VolumeEffectComponentFlags volumeEffectComponentFlags = effectFlags.components[i];
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent2 = other.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						for (int j = 0; j < volumeEffectComponentFlags.componentFields.Count; j++)
						{
							VolumeEffectFieldFlags volumeEffectFieldFlags = volumeEffectComponentFlags.componentFields[j];
							if (volumeEffectFieldFlags.blendFlag)
							{
								FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (!(field == null) && volumeEffectField != null && volumeEffectField2 != null)
								{
									string fullName = field.FieldType.FullName;
									if (!(fullName == "System.Single"))
									{
										if (!(fullName == "System.Boolean"))
										{
											if (!(fullName == "UnityEngine.Vector2"))
											{
												if (!(fullName == "UnityEngine.Vector3"))
												{
													if (!(fullName == "UnityEngine.Vector4"))
													{
														if (fullName == "UnityEngine.Color")
														{
															field.SetValue(component, Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blendAmount));
														}
													}
													else
													{
														field.SetValue(component, Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blendAmount));
													}
												}
												else
												{
													field.SetValue(component, Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blendAmount));
												}
											}
											else
											{
												field.SetValue(component, Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blendAmount));
											}
										}
										else
										{
											field.SetValue(component, volumeEffectField2.valueBoolean);
										}
									}
									else
									{
										field.SetValue(component, Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blendAmount));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x0016C89C File Offset: 0x0016AA9C
		public VolumeEffectComponent FindEffectComponent(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x0016C8E8 File Offset: 0x0016AAE8
		public static Component[] ListAcceptableComponents(AmplifyColorEffect go)
		{
			if (go == null)
			{
				return new Component[0];
			}
			return go.GetComponents(typeof(Component)).Where(delegate(Component comp)
			{
				if (comp != null)
				{
					Type type = comp.GetType();
					return !(((type != null) ? type.ToString() : null) ?? "").StartsWith("UnityEngine.") && !(comp.GetType() == typeof(AmplifyColorEffect));
				}
				return false;
			}).ToArray<Component>();
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x0016C93E File Offset: 0x0016AB3E
		public string[] GetComponentNames()
		{
			return (from r in this.components
			select r.componentName).ToArray<string>();
		}

		// Token: 0x04004082 RID: 16514
		public AmplifyColorEffect gameObject;

		// Token: 0x04004083 RID: 16515
		public List<VolumeEffectComponent> components;
	}
}
