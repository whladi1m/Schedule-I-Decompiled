using System;
using FishNet.Serializing;
using ScheduleOne.Clothing;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Product;
using ScheduleOne.Storage;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000937 RID: 2359
	public static class ItemSerializers
	{
		// Token: 0x06003FD1 RID: 16337 RVA: 0x0010C2C8 File Offset: 0x0010A4C8
		private static ItemInstance Read(this Reader reader)
		{
			if (reader.Remaining == 0)
			{
				return null;
			}
			string text = reader.ReadString();
			if (text == typeof(ItemInstance).Name)
			{
				return reader.DirectReadItemInstance();
			}
			if (text == typeof(StorableItemInstance).Name)
			{
				return reader.DirectReadStorableItemInstance();
			}
			if (text == typeof(CashInstance).Name)
			{
				return reader.DirectReadCashInstance();
			}
			if (text == typeof(ClothingInstance).Name)
			{
				return reader.DirectReadClothingInstance();
			}
			if (text == typeof(QualityItemInstance).Name)
			{
				return reader.DirectReadQualityItemInstance();
			}
			if (text == typeof(ProductItemInstance).Name)
			{
				return reader.DirectReadProductItemInstance();
			}
			if (text == typeof(WeedInstance).Name)
			{
				return reader.DirectReadWeedInstance();
			}
			if (text == typeof(MethInstance).Name)
			{
				return reader.DirectReadMethInstance();
			}
			if (text == typeof(CocaineInstance).Name)
			{
				return reader.DirectReadCocaineInstance();
			}
			if (text == typeof(IntegerItemInstance).Name)
			{
				return reader.DirectReadIntegerItemInstance();
			}
			if (text == typeof(WateringCanInstance).Name)
			{
				return reader.DirectReadWateringCanInstance();
			}
			if (text == typeof(TrashGrabberInstance).Name)
			{
				return reader.DirectReadTrashGrabberInstance();
			}
			if (reader.ReadString() == string.Empty)
			{
				return null;
			}
			Console.LogError("ItemSerializers: reader not found for '" + text + "'!", null);
			return null;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0010C47C File Offset: 0x0010A67C
		public static void WriteItemInstance(this Writer writer, ItemInstance value)
		{
			if (value is StorableItemInstance)
			{
				writer.WriteStorableItemInstance((StorableItemInstance)value);
				return;
			}
			if (value == null)
			{
				writer.WriteString(typeof(ItemInstance).Name);
				writer.WriteString(string.Empty);
				return;
			}
			writer.WriteString(typeof(ItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0010C4F0 File Offset: 0x0010A6F0
		public static ItemInstance ReadItemInstance(this Reader reader)
		{
			return reader.Read();
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0010C4F8 File Offset: 0x0010A6F8
		private static ItemInstance DirectReadItemInstance(this Reader reader)
		{
			reader.ReadString() == string.Empty;
			return null;
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0010C50C File Offset: 0x0010A70C
		public static void WriteStorableItemInstance(this Writer writer, StorableItemInstance value)
		{
			if (value is QualityItemInstance)
			{
				writer.WriteQualityItemInstance((QualityItemInstance)value);
				return;
			}
			if (value is CashInstance)
			{
				writer.WriteCashInstance(value as CashInstance);
				return;
			}
			if (value is ClothingInstance)
			{
				writer.WriteClothingInstance(value as ClothingInstance);
				return;
			}
			if (value is IntegerItemInstance)
			{
				writer.WriteIntegerItemInstance(value as IntegerItemInstance);
				return;
			}
			if (value is WateringCanInstance)
			{
				writer.WriteWateringCanInstance(value as WateringCanInstance);
				return;
			}
			if (value is TrashGrabberInstance)
			{
				writer.WriteTrashGrabberInstance(value as TrashGrabberInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(StorableItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0010C5C9 File Offset: 0x0010A7C9
		public static StorableItemInstance ReadStorableItemInstance(this Reader reader)
		{
			return reader.Read() as StorableItemInstance;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0010C5D8 File Offset: 0x0010A7D8
		private static StorableItemInstance DirectReadStorableItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new StorableItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16()
			};
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0010C614 File Offset: 0x0010A814
		public static void WriteCashInstance(this Writer writer, CashInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(CashInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteSingle(value.Balance, AutoPackType.Unpacked);
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0010C660 File Offset: 0x0010A860
		public static CashInstance ReadCashInstance(this Reader reader)
		{
			return reader.Read() as CashInstance;
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0010C670 File Offset: 0x0010A870
		private static CashInstance DirectReadCashInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			CashInstance cashInstance = new CashInstance();
			cashInstance.ID = text;
			cashInstance.Quantity = (int)reader.ReadUInt16();
			cashInstance.SetBalance(reader.ReadSingle(AutoPackType.Unpacked), false);
			return cashInstance;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0010C6BC File Offset: 0x0010A8BC
		public static void WriteQualityItemInstance(this Writer writer, QualityItemInstance value)
		{
			if (value is ProductItemInstance)
			{
				writer.WriteProductItemInstance(value as ProductItemInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(QualityItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0010C71D File Offset: 0x0010A91D
		public static QualityItemInstance ReadQualityItemInstance(this Reader reader)
		{
			return reader.Read() as QualityItemInstance;
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0010C72C File Offset: 0x0010A92C
		private static QualityItemInstance DirectReadQualityItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new QualityItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16()
			};
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x0010C774 File Offset: 0x0010A974
		public static void WriteClothingInstance(this Writer writer, ClothingInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(ClothingInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Color);
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x0010C7C0 File Offset: 0x0010A9C0
		public static ClothingInstance ReadClothingInstance(this Reader reader)
		{
			return reader.Read() as ClothingInstance;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x0010C7D0 File Offset: 0x0010A9D0
		private static ClothingInstance DirectReadClothingInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new ClothingInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Color = (EClothingColor)reader.ReadUInt16()
			};
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0010C818 File Offset: 0x0010AA18
		public static void WriteProductItemInstance(this Writer writer, ProductItemInstance value)
		{
			if (value is WeedInstance)
			{
				writer.WriteWeedInstance(value as WeedInstance);
				return;
			}
			if (value is MethInstance)
			{
				writer.WriteMethInstance(value as MethInstance);
				return;
			}
			if (value is CocaineInstance)
			{
				writer.WriteCocaineInstance(value as CocaineInstance);
				return;
			}
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(ProductItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x0010C8AF File Offset: 0x0010AAAF
		public static ProductItemInstance ReadProductItemInstance(this Reader reader)
		{
			return reader.Read() as ProductItemInstance;
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x0010C8BC File Offset: 0x0010AABC
		private static ProductItemInstance DirectReadProductItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new ProductItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0010C910 File Offset: 0x0010AB10
		public static void WriteWeedInstance(this Writer writer, WeedInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(WeedInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0010C968 File Offset: 0x0010AB68
		public static WeedInstance ReadWeedInstance(this Reader reader)
		{
			return reader.Read() as WeedInstance;
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x0010C978 File Offset: 0x0010AB78
		private static WeedInstance DirectReadWeedInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new WeedInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x0010C9CC File Offset: 0x0010ABCC
		public static void WriteMethInstance(this Writer writer, MethInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(MethInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x0010CA24 File Offset: 0x0010AC24
		public static MethInstance ReadMethInstance(this Reader reader)
		{
			return reader.Read() as MethInstance;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x0010CA34 File Offset: 0x0010AC34
		private static MethInstance DirectReadMethInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new MethInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x0010CA88 File Offset: 0x0010AC88
		public static void WriteCocaineInstance(this Writer writer, CocaineInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(CocaineInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Quality);
			writer.WriteString(value.PackagingID);
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0010CAE0 File Offset: 0x0010ACE0
		public static CocaineInstance ReadCocaineInstance(this Reader reader)
		{
			return reader.Read() as CocaineInstance;
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x0010CAF0 File Offset: 0x0010ACF0
		private static CocaineInstance DirectReadCocaineInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new CocaineInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Quality = (EQuality)reader.ReadUInt16(),
				PackagingID = reader.ReadString()
			};
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0010CB44 File Offset: 0x0010AD44
		public static void WriteIntegerItemInstance(this Writer writer, IntegerItemInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(IntegerItemInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteUInt16((ushort)value.Value);
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0010CB90 File Offset: 0x0010AD90
		public static IntegerItemInstance ReadIntegerItemInstance(this Reader reader)
		{
			return reader.Read() as IntegerItemInstance;
		}

		// Token: 0x06003FEF RID: 16367 RVA: 0x0010CBA0 File Offset: 0x0010ADA0
		private static IntegerItemInstance DirectReadIntegerItemInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new IntegerItemInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				Value = (int)reader.ReadUInt16()
			};
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0010CBE8 File Offset: 0x0010ADE8
		public static void WriteWateringCanInstance(this Writer writer, WateringCanInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(WateringCanInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			writer.WriteSingle(value.CurrentFillAmount, AutoPackType.Unpacked);
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0010CC34 File Offset: 0x0010AE34
		public static WateringCanInstance ReadWateringCanInstance(this Reader reader)
		{
			return reader.Read() as WateringCanInstance;
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0010CC44 File Offset: 0x0010AE44
		private static WateringCanInstance DirectReadWateringCanInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			return new WateringCanInstance
			{
				ID = text,
				Quantity = (int)reader.ReadUInt16(),
				CurrentFillAmount = reader.ReadSingle(AutoPackType.Unpacked)
			};
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0010CC8C File Offset: 0x0010AE8C
		public static void WriteTrashGrabberInstance(this Writer writer, TrashGrabberInstance value)
		{
			if (value == null)
			{
				return;
			}
			writer.WriteString(typeof(TrashGrabberInstance).Name);
			writer.WriteString(value.ID);
			writer.WriteUInt16((ushort)value.Quantity);
			string[] array = value.GetTrashIDs().ToArray();
			writer.WriteArray<string>(array, 0, array.Length);
			ushort[] array2 = value.GetTrashUshortQuantities().ToArray();
			writer.WriteArray<ushort>(array2, 0, array2.Length);
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0010CCF9 File Offset: 0x0010AEF9
		public static TrashGrabberInstance ReadTrashGrabberInstance(this Reader reader)
		{
			return reader.Read() as TrashGrabberInstance;
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0010CD08 File Offset: 0x0010AF08
		private static TrashGrabberInstance DirectReadTrashGrabberInstance(this Reader reader)
		{
			string text = reader.ReadString();
			if (text == string.Empty)
			{
				return null;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance();
			trashGrabberInstance.ID = text;
			trashGrabberInstance.Quantity = (int)reader.ReadUInt16();
			string[] array = new string[20];
			ushort[] array2 = new ushort[20];
			int num = reader.ReadArray<string>(ref array);
			reader.ReadArray<ushort>(ref array2);
			for (int i = 0; i < num; i++)
			{
				trashGrabberInstance.AddTrash(array[i], (int)array2[i]);
			}
			return trashGrabberInstance;
		}

		// Token: 0x04002E03 RID: 11779
		public const bool DEBUG = false;
	}
}
