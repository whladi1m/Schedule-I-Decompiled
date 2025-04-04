using System;
using System.Collections.Generic;
using ScheduleOne.Property;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A17 RID: 2583
	public class PropertyDropdown : MonoBehaviour
	{
		// Token: 0x060045B3 RID: 17843 RVA: 0x001243A0 File Offset: 0x001225A0
		protected virtual void Awake()
		{
			List<string> list = new List<string>();
			list.Add("None");
			this.TMP_dropdown = base.GetComponent<TMP_Dropdown>();
			if (this.TMP_dropdown != null)
			{
				this.TMP_dropdown.onValueChanged.AddListener(new UnityAction<int>(this.ValueChanged));
				this.TMP_dropdown.AddOptions(list);
			}
			this.dropdown = base.GetComponent<Dropdown>();
			if (this.dropdown != null)
			{
				this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.ValueChanged));
				this.dropdown.AddOptions(list);
			}
			this.intToProperty.Add(0, null);
			Property.onPropertyAcquired = (Property.PropertyChange)Delegate.Combine(Property.onPropertyAcquired, new Property.PropertyChange(this.PropertyAcquired));
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00124470 File Offset: 0x00122670
		private void PropertyAcquired(Property p)
		{
			List<string> list = new List<string>();
			list.Add(p.PropertyName);
			if (this.dropdown != null)
			{
				this.intToProperty.Add(this.dropdown.options.Count, p);
				this.dropdown.AddOptions(list);
			}
			if (this.TMP_dropdown != null)
			{
				this.intToProperty.Add(this.TMP_dropdown.options.Count, p);
				this.TMP_dropdown.AddOptions(list);
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x001244FB File Offset: 0x001226FB
		private void ValueChanged(int newVal)
		{
			this.selectedProperty = this.intToProperty[newVal];
			if (this.onSelectionChanged != null)
			{
				this.onSelectionChanged();
			}
		}

		// Token: 0x0400338A RID: 13194
		public Property selectedProperty;

		// Token: 0x0400338B RID: 13195
		private TMP_Dropdown TMP_dropdown;

		// Token: 0x0400338C RID: 13196
		private Dropdown dropdown;

		// Token: 0x0400338D RID: 13197
		private Dictionary<int, Property> intToProperty = new Dictionary<int, Property>();

		// Token: 0x0400338E RID: 13198
		public Action onSelectionChanged;
	}
}
