using System;
using System.Collections.Generic;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.GameTime;
using ScheduleOne.Growing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne
{
	// Token: 0x02000231 RID: 561
	public class Console : Singleton<Console>
	{
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00036F30 File Offset: 0x00035130
		private static Player player
		{
			get
			{
				return Player.Local;
			}
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00036F37 File Offset: 0x00035137
		private static void LogCommandError(string error)
		{
			Console.LogWarning(error, null);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00036F40 File Offset: 0x00035140
		private static void LogUnrecognizedFormat(string[] correctExamples)
		{
			string text = string.Empty;
			for (int i = 0; i < correctExamples.Length; i++)
			{
				if (i > 0)
				{
					text += ",";
				}
				text = text + "'" + correctExamples[i] + "'";
			}
			Console.LogWarning("Unrecognized command format. Correct format example(s): " + text, null);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00036F98 File Offset: 0x00035198
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<Console>.Instance != this)
			{
				return;
			}
			if (Console.commands.Count == 0)
			{
				Console.commands.Add("freecam", new Console.FreeCamCommand());
				Console.commands.Add("save", new Console.Save());
				Console.commands.Add("settime", new Console.SetTimeCommand());
				Console.commands.Add("give", new Console.AddItemToInventoryCommand());
				Console.commands.Add("clearinventory", new Console.ClearInventoryCommand());
				Console.commands.Add("changecash", new Console.ChangeCashCommand());
				Console.commands.Add("changebalance", new Console.ChangeOnlineBalanceCommand());
				Console.commands.Add("addxp", new Console.GiveXP());
				Console.commands.Add("spawnvehicle", new Console.SpawnVehicleCommand());
				Console.commands.Add("setmovespeed", new Console.SetMoveSpeedCommand());
				Console.commands.Add("setjumpforce", new Console.SetJumpMultiplier());
				Console.commands.Add("teleport", new Console.Teleport());
				Console.commands.Add("setowned", new Console.SetPropertyOwned());
				Console.commands.Add("packageproduct", new Console.PackageProduct());
				Console.commands.Add("setstaminareserve", new Console.SetStaminaReserve());
				Console.commands.Add("raisewanted", new Console.RaisedWanted());
				Console.commands.Add("lowerwanted", new Console.LowerWanted());
				Console.commands.Add("clearwanted", new Console.ClearWanted());
				Console.commands.Add("sethealth", new Console.SetHealth());
				Console.commands.Add("settimescale", new Console.SetTimeScale());
				Console.commands.Add("setvar", new Console.SetVariableValue());
				Console.commands.Add("setqueststate", new Console.SetQuestState());
				Console.commands.Add("setquestentrystate", new Console.SetQuestEntryState());
				Console.commands.Add("setemotion", new Console.SetEmotion());
				Console.commands.Add("setunlocked", new Console.SetUnlocked());
				Console.commands.Add("setrelationship", new Console.SetRelationship());
				Console.commands.Add("addemployee", new Console.AddEmployeeCommand());
				Console.commands.Add("setdiscovered", new Console.SetDiscovered());
				Console.commands.Add("growplants", new Console.GrowPlants());
				Console.commands.Add("setlawintensity", new Console.SetLawIntensity());
				Console.commands.Add("setquality", new Console.SetQuality());
				Console.commands.Add("bind", new Console.Bind());
				Console.commands.Add("unbind", new Console.Unbind());
				Console.commands.Add("clearbinds", new Console.ClearBinds());
				Console.commands.Add("hideui", new Console.HideUI());
				Console.commands.Add("disable", new Console.Disable());
				Console.commands.Add("enable", new Console.Enable());
				Console.commands.Add("endtutorial", new Console.EndTutorial());
				Console.commands.Add("disablenpcasset", new Console.DisableNPCAsset());
				Console.commands.Add("showfps", new Console.ShowFPS());
				Console.commands.Add("hidefps", new Console.HideFPS());
				Console.commands.Add("cleartrash", new Console.ClearTrash());
			}
			foreach (KeyValuePair<string, Console.ConsoleCommand> keyValuePair in Console.commands)
			{
				Console.Commands.Add(keyValuePair.Value);
			}
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.RunStartupCommands));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.RunStartupCommands));
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000373A0 File Offset: 0x000355A0
		private void RunStartupCommands()
		{
			if (Application.isEditor || Debug.isDebugBuild)
			{
				foreach (string args in this.startupCommands)
				{
					Console.SubmitCommand(args);
				}
			}
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00037400 File Offset: 0x00035600
		[HideInCallstack]
		public static void Log(object message, UnityEngine.Object context = null)
		{
			Debug.Log(message, context);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00037409 File Offset: 0x00035609
		[HideInCallstack]
		public static void LogWarning(object message, UnityEngine.Object context = null)
		{
			Debug.LogWarning(message, context);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00037412 File Offset: 0x00035612
		[HideInCallstack]
		public static void LogError(object message, UnityEngine.Object context = null)
		{
			Debug.LogError(message, context);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0003741C File Offset: 0x0003561C
		public static void SubmitCommand(List<string> args)
		{
			if (args.Count == 0)
			{
				return;
			}
			if (!InstanceFinder.IsHost && !Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			for (int i = 0; i < args.Count; i++)
			{
				args[i] = args[i].ToLower();
			}
			string text = args[0];
			Console.ConsoleCommand consoleCommand;
			if (Console.commands.TryGetValue(text, out consoleCommand))
			{
				args.RemoveAt(0);
				consoleCommand.Execute(args);
				return;
			}
			Console.LogWarning("Command '" + text + "' not found.", null);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000374A8 File Offset: 0x000356A8
		public static void SubmitCommand(string args)
		{
			Console.SubmitCommand(new List<string>(args.Split(new char[]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries)));
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000374C8 File Offset: 0x000356C8
		public void AddBinding(KeyCode key, string command)
		{
			Console.Log("Binding " + key.ToString() + " to " + command, null);
			if (this.keyBindings.ContainsKey(key))
			{
				this.keyBindings[key] = command;
				return;
			}
			this.keyBindings.Add(key, command);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00037521 File Offset: 0x00035721
		public void RemoveBinding(KeyCode key)
		{
			Console.Log("Unbinding " + key.ToString(), null);
			this.keyBindings.Remove(key);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0003754D File Offset: 0x0003574D
		public void ClearBindings()
		{
			Console.Log("Clearing all key bindings", null);
			this.keyBindings.Clear();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00037568 File Offset: 0x00035768
		private void Update()
		{
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				foreach (KeyValuePair<KeyCode, string> keyValuePair in this.keyBindings)
				{
					if (Input.GetKeyDown(keyValuePair.Key))
					{
						Console.SubmitCommand(keyValuePair.Value);
					}
				}
			}
		}

		// Token: 0x04000D45 RID: 3397
		public Transform TeleportPointsContainer;

		// Token: 0x04000D46 RID: 3398
		public List<Console.LabelledGameObject> LabelledGameObjectList;

		// Token: 0x04000D47 RID: 3399
		[Tooltip("Commands that run on startup (Editor only)")]
		public List<string> startupCommands = new List<string>();

		// Token: 0x04000D48 RID: 3400
		public static List<Console.ConsoleCommand> Commands = new List<Console.ConsoleCommand>();

		// Token: 0x04000D49 RID: 3401
		private static Dictionary<string, Console.ConsoleCommand> commands = new Dictionary<string, Console.ConsoleCommand>();

		// Token: 0x04000D4A RID: 3402
		private Dictionary<KeyCode, string> keyBindings = new Dictionary<KeyCode, string>();

		// Token: 0x02000232 RID: 562
		public abstract class ConsoleCommand
		{
			// Token: 0x1700025F RID: 607
			// (get) Token: 0x06000BE0 RID: 3040
			public abstract string CommandWord { get; }

			// Token: 0x17000260 RID: 608
			// (get) Token: 0x06000BE1 RID: 3041
			public abstract string CommandDescription { get; }

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x06000BE2 RID: 3042
			public abstract string ExampleUsage { get; }

			// Token: 0x06000BE3 RID: 3043
			public abstract void Execute(List<string> args);
		}

		// Token: 0x02000233 RID: 563
		public class SetTimeCommand : Console.ConsoleCommand
		{
			// Token: 0x17000262 RID: 610
			// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00037618 File Offset: 0x00035818
			public override string CommandWord
			{
				get
				{
					return "settime";
				}
			}

			// Token: 0x17000263 RID: 611
			// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0003761F File Offset: 0x0003581F
			public override string CommandDescription
			{
				get
				{
					return "Sets the time of day to the specified 24-hour time";
				}
			}

			// Token: 0x17000264 RID: 612
			// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00037626 File Offset: 0x00035826
			public override string ExampleUsage
			{
				get
				{
					return "settime 1530";
				}
			}

			// Token: 0x06000BE8 RID: 3048 RVA: 0x00037630 File Offset: 0x00035830
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0 || !TimeManager.IsValid24HourTime(args[0]))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'settime 1530'", null);
					return;
				}
				if (Player.Local.IsSleeping)
				{
					Console.LogWarning("Can't set time whilst sleeping", null);
					return;
				}
				Console.Log("Time set to " + args[0], null);
				NetworkSingleton<TimeManager>.Instance.SetTime(int.Parse(args[0]), false);
			}
		}

		// Token: 0x02000234 RID: 564
		public class SpawnVehicleCommand : Console.ConsoleCommand
		{
			// Token: 0x17000265 RID: 613
			// (get) Token: 0x06000BEA RID: 3050 RVA: 0x000376AE File Offset: 0x000358AE
			public override string CommandWord
			{
				get
				{
					return "spawnvehicle";
				}
			}

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x06000BEB RID: 3051 RVA: 0x000376B5 File Offset: 0x000358B5
			public override string CommandDescription
			{
				get
				{
					return "Spawns a vehicle at the player's location";
				}
			}

			// Token: 0x17000267 RID: 615
			// (get) Token: 0x06000BEC RID: 3052 RVA: 0x000376BC File Offset: 0x000358BC
			public override string ExampleUsage
			{
				get
				{
					return "spawnvehicle shitbox";
				}
			}

			// Token: 0x06000BED RID: 3053 RVA: 0x000376C4 File Offset: 0x000358C4
			public override void Execute(List<string> args)
			{
				bool flag = false;
				if (args.Count > 0 && NetworkSingleton<VehicleManager>.Instance.GetVehiclePrefab(args[0]) != null)
				{
					flag = true;
					Console.Log("Spawning '" + args[0] + "'...", null);
					Vector3 position = Console.player.transform.position + Console.player.transform.forward * 4f + Console.player.transform.up * 1f;
					Quaternion rotation = Console.player.transform.rotation;
					NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle(args[0], position, rotation, true);
				}
				if (!flag)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'spawnvehicle shitbox'", null);
				}
			}
		}

		// Token: 0x02000235 RID: 565
		public class AddItemToInventoryCommand : Console.ConsoleCommand
		{
			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00037798 File Offset: 0x00035998
			public override string CommandWord
			{
				get
				{
					return "give";
				}
			}

			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0003779F File Offset: 0x0003599F
			public override string CommandDescription
			{
				get
				{
					return "Gives the player the specified item. Optionally specify a quantity.";
				}
			}

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x000377A6 File Offset: 0x000359A6
			public override string ExampleUsage
			{
				get
				{
					return "give ogkush 5";
				}
			}

			// Token: 0x06000BF2 RID: 3058 RVA: 0x000377B0 File Offset: 0x000359B0
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'give watering_can', 'give watering_can 5'", null);
					return;
				}
				ItemDefinition item = Registry.GetItem(args[0]);
				if (!(item != null))
				{
					Console.LogWarning("Unrecognized item code '" + args[0] + "'", null);
					return;
				}
				ItemInstance defaultInstance = item.GetDefaultInstance(1);
				if (args[0] == "cash")
				{
					Console.LogWarning("Unrecognized item code '" + args[0] + "'", null);
					return;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(defaultInstance, 1))
				{
					int num = 1;
					if (args.Count > 1)
					{
						bool flag = false;
						if (int.TryParse(args[1], out num) && num > 0)
						{
							flag = true;
						}
						if (!flag)
						{
							Console.LogWarning("Unrecognized quantity '" + args[1] + "'. Please provide a positive integer", null);
						}
					}
					int num2 = 0;
					while (num > 0 && PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(defaultInstance, 1))
					{
						PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(defaultInstance);
						num--;
						num2++;
					}
					Console.Log(string.Concat(new string[]
					{
						"Added ",
						num2.ToString(),
						" ",
						item.Name,
						" to inventory"
					}), null);
					return;
				}
				Console.LogWarning("Insufficient inventory space", null);
			}
		}

		// Token: 0x02000236 RID: 566
		public class ClearInventoryCommand : Console.ConsoleCommand
		{
			// Token: 0x1700026B RID: 619
			// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0003790A File Offset: 0x00035B0A
			public override string CommandWord
			{
				get
				{
					return "clearinventory";
				}
			}

			// Token: 0x1700026C RID: 620
			// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00037911 File Offset: 0x00035B11
			public override string CommandDescription
			{
				get
				{
					return "Clears the player's inventory";
				}
			}

			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0003790A File Offset: 0x00035B0A
			public override string ExampleUsage
			{
				get
				{
					return "clearinventory";
				}
			}

			// Token: 0x06000BF7 RID: 3063 RVA: 0x00037918 File Offset: 0x00035B18
			public override void Execute(List<string> args)
			{
				Console.Log("Clearing player inventory...", null);
				PlayerSingleton<PlayerInventory>.Instance.ClearInventory();
			}
		}

		// Token: 0x02000237 RID: 567
		public class ChangeCashCommand : Console.ConsoleCommand
		{
			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x0003792F File Offset: 0x00035B2F
			public override string CommandWord
			{
				get
				{
					return "changecash";
				}
			}

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00037936 File Offset: 0x00035B36
			public override string CommandDescription
			{
				get
				{
					return "Changes the player's cash balance by the specified amount";
				}
			}

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0003793D File Offset: 0x00035B3D
			public override string ExampleUsage
			{
				get
				{
					return "changecash 5000";
				}
			}

			// Token: 0x06000BFC RID: 3068 RVA: 0x00037944 File Offset: 0x00035B44
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'changecash 5000', 'changecash -5000'", null);
					return;
				}
				if (num > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(num, true, false);
					Console.Log("Gave player " + MoneyManager.FormatAmount(num, false, false) + " cash", null);
					return;
				}
				if (num < 0f)
				{
					num = Mathf.Clamp(num, -NetworkSingleton<MoneyManager>.Instance.cashBalance, 0f);
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(num, true, false);
					Console.Log("Removed " + MoneyManager.FormatAmount(num, false, false) + " cash from player", null);
				}
			}
		}

		// Token: 0x02000238 RID: 568
		public class ChangeOnlineBalanceCommand : Console.ConsoleCommand
		{
			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000BFE RID: 3070 RVA: 0x000379F7 File Offset: 0x00035BF7
			public override string CommandWord
			{
				get
				{
					return "changebalance";
				}
			}

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000BFF RID: 3071 RVA: 0x000379FE File Offset: 0x00035BFE
			public override string CommandDescription
			{
				get
				{
					return "Changes the player's online balance by the specified amount";
				}
			}

			// Token: 0x17000273 RID: 627
			// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00037A05 File Offset: 0x00035C05
			public override string ExampleUsage
			{
				get
				{
					return "changebalance 5000";
				}
			}

			// Token: 0x06000C01 RID: 3073 RVA: 0x00037A0C File Offset: 0x00035C0C
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num))
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'changebalance 5000', 'changebalance -5000'", null);
					return;
				}
				if (num > 0f)
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Added online balance", num, 1f, "Added by developer console");
					Console.Log("Increased online balance by " + MoneyManager.FormatAmount(num, false, false), null);
					return;
				}
				if (num < 0f)
				{
					num = Mathf.Clamp(num, -NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance, 0f);
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Removed online balance", num, 1f, "Removed by developer console");
					Console.Log("Decreased online balance by " + MoneyManager.FormatAmount(num, false, false), null);
				}
			}
		}

		// Token: 0x02000239 RID: 569
		public class SetMoveSpeedCommand : Console.ConsoleCommand
		{
			// Token: 0x17000274 RID: 628
			// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00037ACF File Offset: 0x00035CCF
			public override string CommandWord
			{
				get
				{
					return "setmovespeed";
				}
			}

			// Token: 0x17000275 RID: 629
			// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00037AD6 File Offset: 0x00035CD6
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's move speed multiplier";
				}
			}

			// Token: 0x17000276 RID: 630
			// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00037ADD File Offset: 0x00035CDD
			public override string ExampleUsage
			{
				get
				{
					return "setmovespeed 1";
				}
			}

			// Token: 0x06000C06 RID: 3078 RVA: 0x00037AE4 File Offset: 0x00035CE4
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setmovespeed 1'", null);
					return;
				}
				Console.Log("Setting player move speed multiplier to " + num.ToString(), null);
				PlayerMovement.StaticMoveSpeedMultiplier = num;
			}
		}

		// Token: 0x0200023A RID: 570
		public class SetJumpMultiplier : Console.ConsoleCommand
		{
			// Token: 0x17000277 RID: 631
			// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00037B40 File Offset: 0x00035D40
			public override string CommandWord
			{
				get
				{
					return "setjumpforce";
				}
			}

			// Token: 0x17000278 RID: 632
			// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00037B47 File Offset: 0x00035D47
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's jump force multiplier";
				}
			}

			// Token: 0x17000279 RID: 633
			// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00037B4E File Offset: 0x00035D4E
			public override string ExampleUsage
			{
				get
				{
					return "setjumpforce 1";
				}
			}

			// Token: 0x06000C0B RID: 3083 RVA: 0x00037B58 File Offset: 0x00035D58
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setjumpforce 1'", null);
					return;
				}
				Console.Log("Setting player jump force multiplier to " + num.ToString(), null);
				PlayerMovement.JumpMultiplier = num;
			}
		}

		// Token: 0x0200023B RID: 571
		public class SetPropertyOwned : Console.ConsoleCommand
		{
			// Token: 0x1700027A RID: 634
			// (get) Token: 0x06000C0D RID: 3085 RVA: 0x00037BB4 File Offset: 0x00035DB4
			public override string CommandWord
			{
				get
				{
					return "setowned";
				}
			}

			// Token: 0x1700027B RID: 635
			// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00037BBB File Offset: 0x00035DBB
			public override string CommandDescription
			{
				get
				{
					return "Sets the specified property or business as owned";
				}
			}

			// Token: 0x1700027C RID: 636
			// (get) Token: 0x06000C0F RID: 3087 RVA: 0x00037BC2 File Offset: 0x00035DC2
			public override string ExampleUsage
			{
				get
				{
					return "setowned barn, setowned laundromat";
				}
			}

			// Token: 0x06000C10 RID: 3088 RVA: 0x00037BCC File Offset: 0x00035DCC
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"setowned barn",
						"setowned manor"
					});
					return;
				}
				string code = args[0].ToLower();
				Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == code);
				Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == code);
				if (property == null && business == null)
				{
					Console.LogCommandError("Could not find unowned property with code '" + code + "'");
					return;
				}
				if (property != null)
				{
					property.SetOwned();
				}
				if (business != null)
				{
					business.SetOwned();
				}
				Console.Log("Property with code '" + code + "' is now owned", null);
			}
		}

		// Token: 0x0200023D RID: 573
		public class Teleport : Console.ConsoleCommand
		{
			// Token: 0x1700027D RID: 637
			// (get) Token: 0x06000C15 RID: 3093 RVA: 0x00037CC5 File Offset: 0x00035EC5
			public override string CommandWord
			{
				get
				{
					return "teleport";
				}
			}

			// Token: 0x1700027E RID: 638
			// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00037CCC File Offset: 0x00035ECC
			public override string CommandDescription
			{
				get
				{
					return "Teleports the player to the specified location";
				}
			}

			// Token: 0x1700027F RID: 639
			// (get) Token: 0x06000C17 RID: 3095 RVA: 0x00037CD3 File Offset: 0x00035ED3
			public override string ExampleUsage
			{
				get
				{
					return "teleport townhall, teleport barn";
				}
			}

			// Token: 0x06000C18 RID: 3096 RVA: 0x00037CDC File Offset: 0x00035EDC
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"teleport docks",
						"teleport barn"
					});
					return;
				}
				string text = args[0].ToLower();
				Transform transform = null;
				Vector3 b = Vector3.zero;
				for (int i = 0; i < Singleton<Console>.Instance.TeleportPointsContainer.childCount; i++)
				{
					if (Singleton<Console>.Instance.TeleportPointsContainer.GetChild(i).name.ToLower() == text)
					{
						transform = Singleton<Console>.Instance.TeleportPointsContainer.GetChild(i);
						break;
					}
				}
				if (transform == null)
				{
					for (int j = 0; j < Property.Properties.Count; j++)
					{
						if (Property.Properties[j].PropertyCode.ToLower() == text)
						{
							transform = Property.Properties[j].SpawnPoint;
							b = Vector3.up * 1f;
							break;
						}
					}
				}
				if (transform == null)
				{
					for (int k = 0; k < Business.Businesses.Count; k++)
					{
						if (Business.Businesses[k].PropertyCode.ToLower() == text)
						{
							transform = Business.Businesses[k].SpawnPoint;
							b = Vector3.up * 1f;
							break;
						}
					}
				}
				if (transform == null)
				{
					Console.LogCommandError("Unrecognized destination");
					return;
				}
				PlayerSingleton<PlayerMovement>.Instance.Teleport(transform.transform.position + b);
				Player.Local.transform.forward = transform.transform.forward;
				Console.Log("Teleported to '" + text + "'", null);
			}
		}

		// Token: 0x0200023E RID: 574
		public class PackageProduct : Console.ConsoleCommand
		{
			// Token: 0x17000280 RID: 640
			// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00037E9C File Offset: 0x0003609C
			public override string CommandWord
			{
				get
				{
					return "packageprodcut";
				}
			}

			// Token: 0x17000281 RID: 641
			// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00037EA3 File Offset: 0x000360A3
			public override string CommandDescription
			{
				get
				{
					return "Packages the equipped product with the specified packaging";
				}
			}

			// Token: 0x17000282 RID: 642
			// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00037EAA File Offset: 0x000360AA
			public override string ExampleUsage
			{
				get
				{
					return "packageproduct jar, packageproduct baggie";
				}
			}

			// Token: 0x06000C1D RID: 3101 RVA: 0x00037EB4 File Offset: 0x000360B4
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"packageproduct jar",
						"packageproduct baggie"
					});
					return;
				}
				PackagingDefinition packagingDefinition = Registry.GetItem(args[0].ToLower()) as PackagingDefinition;
				if (packagingDefinition == null)
				{
					Console.LogCommandError("Unrecognized packaging ID");
					return;
				}
				if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance is ProductItemInstance)
				{
					(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance as ProductItemInstance).SetPackaging(packagingDefinition);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
					Console.Log("Applied packaging '" + packagingDefinition.Name + "' to equipped product", null);
					return;
				}
				Console.LogCommandError("No product equipped");
			}
		}

		// Token: 0x0200023F RID: 575
		public class SetStaminaReserve : Console.ConsoleCommand
		{
			// Token: 0x17000283 RID: 643
			// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00037F8C File Offset: 0x0003618C
			public override string CommandWord
			{
				get
				{
					return "setstaminareserve";
				}
			}

			// Token: 0x17000284 RID: 644
			// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00037F93 File Offset: 0x00036193
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's stamina reserve (default 100) to the specified amount.";
				}
			}

			// Token: 0x17000285 RID: 645
			// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00037F9A File Offset: 0x0003619A
			public override string ExampleUsage
			{
				get
				{
					return "setstaminareserve 200";
				}
			}

			// Token: 0x06000C22 RID: 3106 RVA: 0x00037FA4 File Offset: 0x000361A4
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setstaminareserve 200'", null);
					return;
				}
				Console.Log("Setting player stamina reserve to " + num.ToString(), null);
				PlayerMovement.StaminaReserveMax = num;
				PlayerSingleton<PlayerMovement>.Instance.SetStamina(num, true);
			}
		}

		// Token: 0x02000240 RID: 576
		public class RaisedWanted : Console.ConsoleCommand
		{
			// Token: 0x17000286 RID: 646
			// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0003800C File Offset: 0x0003620C
			public override string CommandWord
			{
				get
				{
					return "raisewanted";
				}
			}

			// Token: 0x17000287 RID: 647
			// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00038013 File Offset: 0x00036213
			public override string CommandDescription
			{
				get
				{
					return "Raises the player's wanted level";
				}
			}

			// Token: 0x17000288 RID: 648
			// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0003800C File Offset: 0x0003620C
			public override string ExampleUsage
			{
				get
				{
					return "raisewanted";
				}
			}

			// Token: 0x06000C27 RID: 3111 RVA: 0x0003801C File Offset: 0x0003621C
			public override void Execute(List<string> args)
			{
				Console.Log("Raising wanted level...", null);
				if (Console.player.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None)
				{
					Singleton<LawManager>.Instance.PoliceCalled(Console.player, new Crime());
				}
				Console.player.CrimeData.Escalate();
			}
		}

		// Token: 0x02000241 RID: 577
		public class LowerWanted : Console.ConsoleCommand
		{
			// Token: 0x17000289 RID: 649
			// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00038068 File Offset: 0x00036268
			public override string CommandWord
			{
				get
				{
					return "lowerwanted";
				}
			}

			// Token: 0x1700028A RID: 650
			// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0003806F File Offset: 0x0003626F
			public override string CommandDescription
			{
				get
				{
					return "Lowers the player's wanted level";
				}
			}

			// Token: 0x1700028B RID: 651
			// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00038068 File Offset: 0x00036268
			public override string ExampleUsage
			{
				get
				{
					return "lowerwanted";
				}
			}

			// Token: 0x06000C2C RID: 3116 RVA: 0x00038076 File Offset: 0x00036276
			public override void Execute(List<string> args)
			{
				Console.Log("Lowering wanted level...", null);
				Console.player.CrimeData.Deescalate();
			}
		}

		// Token: 0x02000242 RID: 578
		public class ClearWanted : Console.ConsoleCommand
		{
			// Token: 0x1700028C RID: 652
			// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00038092 File Offset: 0x00036292
			public override string CommandWord
			{
				get
				{
					return "clearwanted";
				}
			}

			// Token: 0x1700028D RID: 653
			// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00038099 File Offset: 0x00036299
			public override string CommandDescription
			{
				get
				{
					return "Clears the player's wanted level";
				}
			}

			// Token: 0x1700028E RID: 654
			// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00038092 File Offset: 0x00036292
			public override string ExampleUsage
			{
				get
				{
					return "clearwanted";
				}
			}

			// Token: 0x06000C31 RID: 3121 RVA: 0x000380A0 File Offset: 0x000362A0
			public override void Execute(List<string> args)
			{
				Console.Log("Clearing wanted level...", null);
				Console.player.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.None);
				Console.player.CrimeData.ClearCrimes();
			}
		}

		// Token: 0x02000243 RID: 579
		public class SetHealth : Console.ConsoleCommand
		{
			// Token: 0x1700028F RID: 655
			// (get) Token: 0x06000C33 RID: 3123 RVA: 0x000380CC File Offset: 0x000362CC
			public override string CommandWord
			{
				get
				{
					return "sethealth";
				}
			}

			// Token: 0x17000290 RID: 656
			// (get) Token: 0x06000C34 RID: 3124 RVA: 0x000380D3 File Offset: 0x000362D3
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's health to the specified amount";
				}
			}

			// Token: 0x17000291 RID: 657
			// (get) Token: 0x06000C35 RID: 3125 RVA: 0x000380DA File Offset: 0x000362DA
			public override string ExampleUsage
			{
				get
				{
					return "sethealth 100";
				}
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x000380E4 File Offset: 0x000362E4
			public override void Execute(List<string> args)
			{
				if (!Console.player.Health.IsAlive)
				{
					Console.LogWarning("Can't set health whilst dead", null);
					return;
				}
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'sethealth 100'", null);
					return;
				}
				Console.Log("Setting player health to " + num.ToString(), null);
				Console.player.Health.SetHealth(num);
				if (num < 0f)
				{
					PlayerSingleton<PlayerCamera>.Instance.JoltCamera();
				}
			}
		}

		// Token: 0x02000244 RID: 580
		public class SetEnergy : Console.ConsoleCommand
		{
			// Token: 0x17000292 RID: 658
			// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00038179 File Offset: 0x00036379
			public override string CommandWord
			{
				get
				{
					return "setenergy";
				}
			}

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00038180 File Offset: 0x00036380
			public override string CommandDescription
			{
				get
				{
					return "Sets the player's energy to the specified amount";
				}
			}

			// Token: 0x17000294 RID: 660
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00038187 File Offset: 0x00036387
			public override string ExampleUsage
			{
				get
				{
					return "setenergy 100";
				}
			}

			// Token: 0x06000C3B RID: 3131 RVA: 0x00038190 File Offset: 0x00036390
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'setenergy 100'", null);
					return;
				}
				num = Mathf.Clamp(num, 0f, 100f);
				Console.Log("Setting player energy to " + num.ToString(), null);
				Player.Local.Energy.SetEnergy(num);
			}
		}

		// Token: 0x02000245 RID: 581
		public class FreeCamCommand : Console.ConsoleCommand
		{
			// Token: 0x17000295 RID: 661
			// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00038207 File Offset: 0x00036407
			public override string CommandWord
			{
				get
				{
					return "freecam";
				}
			}

			// Token: 0x17000296 RID: 662
			// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003820E File Offset: 0x0003640E
			public override string CommandDescription
			{
				get
				{
					return "Toggles free cam mode";
				}
			}

			// Token: 0x17000297 RID: 663
			// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00038207 File Offset: 0x00036407
			public override string ExampleUsage
			{
				get
				{
					return "freecam";
				}
			}

			// Token: 0x06000C40 RID: 3136 RVA: 0x00038215 File Offset: 0x00036415
			public override void Execute(List<string> args)
			{
				if (PlayerSingleton<PlayerCamera>.Instance.FreeCamEnabled)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetFreeCam(false, true);
					return;
				}
				PlayerSingleton<PlayerCamera>.Instance.SetFreeCam(true, true);
			}
		}

		// Token: 0x02000246 RID: 582
		public class Save : Console.ConsoleCommand
		{
			// Token: 0x17000298 RID: 664
			// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003823C File Offset: 0x0003643C
			public override string CommandWord
			{
				get
				{
					return "save";
				}
			}

			// Token: 0x17000299 RID: 665
			// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00038243 File Offset: 0x00036443
			public override string CommandDescription
			{
				get
				{
					return "Forces a save";
				}
			}

			// Token: 0x1700029A RID: 666
			// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0003823C File Offset: 0x0003643C
			public override string ExampleUsage
			{
				get
				{
					return "save";
				}
			}

			// Token: 0x06000C45 RID: 3141 RVA: 0x0003824A File Offset: 0x0003644A
			public override void Execute(List<string> args)
			{
				Console.Log("Forcing save...", null);
				Singleton<SaveManager>.Instance.Save();
			}
		}

		// Token: 0x02000247 RID: 583
		public class SetTimeScale : Console.ConsoleCommand
		{
			// Token: 0x1700029B RID: 667
			// (get) Token: 0x06000C47 RID: 3143 RVA: 0x00038261 File Offset: 0x00036461
			public override string CommandWord
			{
				get
				{
					return "settimescale";
				}
			}

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00038268 File Offset: 0x00036468
			public override string CommandDescription
			{
				get
				{
					return "Sets the time scale. Default 1";
				}
			}

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0003826F File Offset: 0x0003646F
			public override string ExampleUsage
			{
				get
				{
					return "settimescale 1";
				}
			}

			// Token: 0x06000C4A RID: 3146 RVA: 0x00038278 File Offset: 0x00036478
			public override void Execute(List<string> args)
			{
				if (!Singleton<Settings>.Instance.PausingFreezesTime)
				{
					Console.LogWarning("Can't set time scale right now.", null);
					return;
				}
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): 'settimescale 1'", null);
					return;
				}
				num = Mathf.Clamp(num, 0f, 20f);
				Console.Log("Setting time scale to " + num.ToString(), null);
				Time.timeScale = num;
			}
		}

		// Token: 0x02000248 RID: 584
		public class SetVariableValue : Console.ConsoleCommand
		{
			// Token: 0x1700029E RID: 670
			// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000382FD File Offset: 0x000364FD
			public override string CommandWord
			{
				get
				{
					return "setvar";
				}
			}

			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00038304 File Offset: 0x00036504
			public override string CommandDescription
			{
				get
				{
					return "Sets the value of the specified variable";
				}
			}

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0003830B File Offset: 0x0003650B
			public override string ExampleUsage
			{
				get
				{
					return "setvar <variable> <value>";
				}
			}

			// Token: 0x06000C4F RID: 3151 RVA: 0x00038314 File Offset: 0x00036514
			public override void Execute(List<string> args)
			{
				if (args.Count >= 2)
				{
					string variableName = args[0].ToLower();
					string value = args[1];
					NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(variableName, value, true);
					return;
				}
				Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
			}
		}

		// Token: 0x02000249 RID: 585
		public class SetQuestState : Console.ConsoleCommand
		{
			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00038363 File Offset: 0x00036563
			public override string CommandWord
			{
				get
				{
					return "setqueststate";
				}
			}

			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0003836A File Offset: 0x0003656A
			public override string CommandDescription
			{
				get
				{
					return "Sets the state of the specified quest";
				}
			}

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00038371 File Offset: 0x00036571
			public override string ExampleUsage
			{
				get
				{
					return "setqueststate <quest name> <state>";
				}
			}

			// Token: 0x06000C54 RID: 3156 RVA: 0x00038378 File Offset: 0x00036578
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				string text2 = args[1];
				text = text.Replace("_", " ");
				Quest quest = Quest.GetQuest(text);
				if (quest == null)
				{
					Console.LogWarning("Failed to find quest with name '" + text + "'", null);
					return;
				}
				EQuestState state = EQuestState.Inactive;
				if (!Enum.TryParse<EQuestState>(text2, true, out state))
				{
					Console.LogWarning("Failed to parse quest state '" + text2 + "'", null);
					return;
				}
				quest.SetQuestState(state, true);
			}
		}

		// Token: 0x0200024A RID: 586
		public class SetQuestEntryState : Console.ConsoleCommand
		{
			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0003841E File Offset: 0x0003661E
			public override string CommandWord
			{
				get
				{
					return "setquestentrystate";
				}
			}

			// Token: 0x170002A5 RID: 677
			// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00038425 File Offset: 0x00036625
			public override string CommandDescription
			{
				get
				{
					return "Sets the state of the specified quest entry";
				}
			}

			// Token: 0x170002A6 RID: 678
			// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0003842C File Offset: 0x0003662C
			public override string ExampleUsage
			{
				get
				{
					return "setquestentrystate <quest name> <entry index> <state>";
				}
			}

			// Token: 0x06000C59 RID: 3161 RVA: 0x00038434 File Offset: 0x00036634
			public override void Execute(List<string> args)
			{
				if (args.Count < 3)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				int num = int.TryParse(args[1], out num) ? num : -1;
				string text2 = args[2];
				text = text.Replace("_", " ");
				Quest quest = Quest.GetQuest(text);
				if (quest == null)
				{
					Console.LogWarning("Failed to find quest with name '" + text + "'", null);
					return;
				}
				if (num < 0 || num >= quest.Entries.Count)
				{
					Console.LogWarning("Invalid entry index", null);
					return;
				}
				EQuestState state = EQuestState.Inactive;
				if (!Enum.TryParse<EQuestState>(text2, true, out state))
				{
					Console.LogWarning("Failed to parse quest state '" + text2 + "'", null);
					return;
				}
				quest.SetQuestEntryState(num, state, true);
			}
		}

		// Token: 0x0200024B RID: 587
		public class SetEmotion : Console.ConsoleCommand
		{
			// Token: 0x170002A7 RID: 679
			// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00038513 File Offset: 0x00036713
			public override string CommandWord
			{
				get
				{
					return "setemotion";
				}
			}

			// Token: 0x170002A8 RID: 680
			// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0003851A File Offset: 0x0003671A
			public override string CommandDescription
			{
				get
				{
					return "Sets the facial expression of the player's avatar.";
				}
			}

			// Token: 0x170002A9 RID: 681
			// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00038521 File Offset: 0x00036721
			public override string ExampleUsage
			{
				get
				{
					return "setemotion cheery";
				}
			}

			// Token: 0x06000C5E RID: 3166 RVA: 0x00038528 File Offset: 0x00036728
			public override void Execute(List<string> args)
			{
				if (!Singleton<Settings>.Instance.PausingFreezesTime)
				{
					Console.LogWarning("Can't set time scale right now.", null);
					return;
				}
				if (args.Count == 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				if (!Player.Local.Avatar.EmotionManager.HasEmotion(text))
				{
					Console.LogWarning("Unrecognized emotion '" + text + "'", null);
					return;
				}
				Console.Log("Setting emotion to " + text, null);
				Player.Local.Avatar.EmotionManager.AddEmotionOverride(text, "console", 0f, 0);
			}
		}

		// Token: 0x0200024C RID: 588
		public class SetUnlocked : Console.ConsoleCommand
		{
			// Token: 0x170002AA RID: 682
			// (get) Token: 0x06000C60 RID: 3168 RVA: 0x000385D8 File Offset: 0x000367D8
			public override string CommandWord
			{
				get
				{
					return "setunlocked";
				}
			}

			// Token: 0x170002AB RID: 683
			// (get) Token: 0x06000C61 RID: 3169 RVA: 0x000385DF File Offset: 0x000367DF
			public override string CommandDescription
			{
				get
				{
					return "Unlocks the given NPC";
				}
			}

			// Token: 0x170002AC RID: 684
			// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000385E6 File Offset: 0x000367E6
			public override string ExampleUsage
			{
				get
				{
					return "setunlocked <npc_id>";
				}
			}

			// Token: 0x06000C63 RID: 3171 RVA: 0x000385F0 File Offset: 0x000367F0
			public override void Execute(List<string> args)
			{
				if (args.Count < 1)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				NPC npc = NPCManager.GetNPC(text);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID '" + text + "'", null);
					return;
				}
				npc.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, true);
			}
		}

		// Token: 0x0200024D RID: 589
		public class SetRelationship : Console.ConsoleCommand
		{
			// Token: 0x170002AD RID: 685
			// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0003865E File Offset: 0x0003685E
			public override string CommandWord
			{
				get
				{
					return "setrelationship";
				}
			}

			// Token: 0x170002AE RID: 686
			// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00038665 File Offset: 0x00036865
			public override string CommandDescription
			{
				get
				{
					return "Sets the relationship scalar of the given NPC. Range is 0-5.";
				}
			}

			// Token: 0x170002AF RID: 687
			// (get) Token: 0x06000C67 RID: 3175 RVA: 0x0003866C File Offset: 0x0003686C
			public override string ExampleUsage
			{
				get
				{
					return "setrelationship <npc_id> 5";
				}
			}

			// Token: 0x06000C68 RID: 3176 RVA: 0x00038674 File Offset: 0x00036874
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogWarning("Unrecognized command format. Example usage: " + this.ExampleUsage, null);
					return;
				}
				string text = args[0].ToLower();
				NPC npc = NPCManager.GetNPC(text);
				if (npc == null)
				{
					Console.LogWarning("Failed to find NPC with ID '" + text + "'", null);
					return;
				}
				float num = 0f;
				if (!float.TryParse(args[1], out num) || num < 0f || num > 5f)
				{
					Console.LogWarning("Invalid scalar value. Must be between 0 and 5.", null);
					return;
				}
				npc.RelationData.SetRelationship(num);
			}
		}

		// Token: 0x0200024E RID: 590
		public class AddEmployeeCommand : Console.ConsoleCommand
		{
			// Token: 0x170002B0 RID: 688
			// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00038713 File Offset: 0x00036913
			public override string CommandWord
			{
				get
				{
					return "addemployee";
				}
			}

			// Token: 0x170002B1 RID: 689
			// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0003871A File Offset: 0x0003691A
			public override string CommandDescription
			{
				get
				{
					return "Adds an employee of the specified type to the given property.";
				}
			}

			// Token: 0x170002B2 RID: 690
			// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00038721 File Offset: 0x00036921
			public override string ExampleUsage
			{
				get
				{
					return "addemployee botanist barn";
				}
			}

			// Token: 0x06000C6D RID: 3181 RVA: 0x00038728 File Offset: 0x00036928
			public override void Execute(List<string> args)
			{
				if (args.Count < 2)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						"setowned barn",
						"setowned manor"
					});
					return;
				}
				args[0].ToLower();
				EEmployeeType type = EEmployeeType.Botanist;
				if (!Enum.TryParse<EEmployeeType>(args[0], true, out type))
				{
					Console.LogCommandError("Unrecognized employee type '" + args[0] + "'");
					return;
				}
				string code = args[1].ToLower();
				Property property = Property.OwnedProperties.Find((Property x) => x.PropertyCode.ToLower() == code);
				if (property == null)
				{
					Console.LogCommandError("Could not find property with code '" + code + "'");
					return;
				}
				NetworkSingleton<EmployeeManager>.Instance.CreateNewEmployee(property, type);
				Console.Log(string.Concat(new string[]
				{
					"Adding employee of type '",
					type.ToString(),
					"' to property '",
					property.PropertyCode,
					"'"
				}), null);
			}
		}

		// Token: 0x02000250 RID: 592
		public class SetDiscovered : Console.ConsoleCommand
		{
			// Token: 0x170002B3 RID: 691
			// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00038852 File Offset: 0x00036A52
			public override string CommandWord
			{
				get
				{
					return "setdiscovered";
				}
			}

			// Token: 0x170002B4 RID: 692
			// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00038859 File Offset: 0x00036A59
			public override string CommandDescription
			{
				get
				{
					return "Sets the specified product as discovered";
				}
			}

			// Token: 0x170002B5 RID: 693
			// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00038860 File Offset: 0x00036A60
			public override string ExampleUsage
			{
				get
				{
					return "setdiscovered ogkush";
				}
			}

			// Token: 0x06000C74 RID: 3188 RVA: 0x00038868 File Offset: 0x00036A68
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string text = args[0].ToLower();
				ProductDefinition productDefinition = Registry.GetItem(text) as ProductDefinition;
				if (productDefinition == null)
				{
					Console.LogCommandError("Unrecognized product code '" + text + "'");
					return;
				}
				NetworkSingleton<ProductManager>.Instance.DiscoverProduct(productDefinition.ID);
				Console.Log(productDefinition.Name + " now discovered", null);
			}
		}

		// Token: 0x02000251 RID: 593
		public class GrowPlants : Console.ConsoleCommand
		{
			// Token: 0x170002B6 RID: 694
			// (get) Token: 0x06000C76 RID: 3190 RVA: 0x000388F1 File Offset: 0x00036AF1
			public override string CommandWord
			{
				get
				{
					return "growplants";
				}
			}

			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000388F8 File Offset: 0x00036AF8
			public override string CommandDescription
			{
				get
				{
					return "Sets ALL plants in the world fully grown";
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06000C78 RID: 3192 RVA: 0x000388F1 File Offset: 0x00036AF1
			public override string ExampleUsage
			{
				get
				{
					return "growplants";
				}
			}

			// Token: 0x06000C79 RID: 3193 RVA: 0x00038900 File Offset: 0x00036B00
			public override void Execute(List<string> args)
			{
				Plant[] array = UnityEngine.Object.FindObjectsOfType<Plant>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Pot.FullyGrowPlant();
				}
			}
		}

		// Token: 0x02000252 RID: 594
		public class SetLawIntensity : Console.ConsoleCommand
		{
			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0003892E File Offset: 0x00036B2E
			public override string CommandWord
			{
				get
				{
					return "setlawintensity";
				}
			}

			// Token: 0x170002BA RID: 698
			// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00038935 File Offset: 0x00036B35
			public override string CommandDescription
			{
				get
				{
					return "Sets the intensity of law enforcement activity on a scale of 0-10.";
				}
			}

			// Token: 0x170002BB RID: 699
			// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0003893C File Offset: 0x00036B3C
			public override string ExampleUsage
			{
				get
				{
					return "setlawintensity 6";
				}
			}

			// Token: 0x06000C7E RID: 3198 RVA: 0x00038944 File Offset: 0x00036B44
			public override void Execute(List<string> args)
			{
				float num = 0f;
				if (args.Count == 0 || !float.TryParse(args[0], out num) || num < 0f)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				float num2 = Mathf.Clamp(num, 0f, 10f);
				Console.Log("Setting law enforcement intensity to " + num2.ToString(), null);
				Singleton<LawController>.Instance.SetInternalIntensity(num2 / 10f);
			}
		}

		// Token: 0x02000253 RID: 595
		public class SetQuality : Console.ConsoleCommand
		{
			// Token: 0x170002BC RID: 700
			// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000389C7 File Offset: 0x00036BC7
			public override string CommandWord
			{
				get
				{
					return "setquality";
				}
			}

			// Token: 0x170002BD RID: 701
			// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000389CE File Offset: 0x00036BCE
			public override string CommandDescription
			{
				get
				{
					return "Sets the quality of the currently equipped item.";
				}
			}

			// Token: 0x170002BE RID: 702
			// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000389D5 File Offset: 0x00036BD5
			public override string ExampleUsage
			{
				get
				{
					return "setquality standard, setquality heavenly";
				}
			}

			// Token: 0x06000C83 RID: 3203 RVA: 0x000389DC File Offset: 0x00036BDC
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string text = args[0].ToLower();
				EQuality quality;
				if (!Enum.TryParse<EQuality>(text, true, out quality))
				{
					Console.LogCommandError("Unrecognized quality '" + text + "'");
				}
				if (PlayerSingleton<PlayerInventory>.Instance.isAnythingEquipped && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance is QualityItemInstance)
				{
					(PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ItemInstance as QualityItemInstance).SetQuality(quality);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
					Console.Log("Set quality to " + quality.ToString(), null);
					return;
				}
				Console.LogCommandError("No quality item equipped");
			}
		}

		// Token: 0x02000254 RID: 596
		public class Bind : Console.ConsoleCommand
		{
			// Token: 0x170002BF RID: 703
			// (get) Token: 0x06000C85 RID: 3205 RVA: 0x00038AB1 File Offset: 0x00036CB1
			public override string CommandWord
			{
				get
				{
					return "bind";
				}
			}

			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00038AB8 File Offset: 0x00036CB8
			public override string CommandDescription
			{
				get
				{
					return "Binds the given key to the given command.";
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00038ABF File Offset: 0x00036CBF
			public override string ExampleUsage
			{
				get
				{
					return "bind t 'settime 1200'";
				}
			}

			// Token: 0x06000C88 RID: 3208 RVA: 0x00038AC8 File Offset: 0x00036CC8
			public override void Execute(List<string> args)
			{
				if (args.Count > 1)
				{
					string text = args[0].ToLower();
					KeyCode key;
					if (!Enum.TryParse<KeyCode>(text, true, out key))
					{
						Console.LogCommandError("Unrecognized keycode '" + text + "'");
					}
					string command = string.Join(" ", args.ToArray()).Substring(text.Length + 1);
					Singleton<Console>.Instance.AddBinding(key, command);
					return;
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x02000255 RID: 597
		public class Unbind : Console.ConsoleCommand
		{
			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00038B4A File Offset: 0x00036D4A
			public override string CommandWord
			{
				get
				{
					return "unbind";
				}
			}

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00038B51 File Offset: 0x00036D51
			public override string CommandDescription
			{
				get
				{
					return "Removes the given bind.";
				}
			}

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00038B58 File Offset: 0x00036D58
			public override string ExampleUsage
			{
				get
				{
					return "unbind t";
				}
			}

			// Token: 0x06000C8D RID: 3213 RVA: 0x00038B60 File Offset: 0x00036D60
			public override void Execute(List<string> args)
			{
				if (args.Count > 0)
				{
					string text = args[0].ToLower();
					KeyCode key;
					if (!Enum.TryParse<KeyCode>(text, true, out key))
					{
						Console.LogCommandError("Unrecognized keycode '" + text + "'");
					}
					Singleton<Console>.Instance.RemoveBinding(key);
					return;
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x02000256 RID: 598
		public class ClearBinds : Console.ConsoleCommand
		{
			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000C8F RID: 3215 RVA: 0x00038BC3 File Offset: 0x00036DC3
			public override string CommandWord
			{
				get
				{
					return "clearbinds";
				}
			}

			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000C90 RID: 3216 RVA: 0x00038BCA File Offset: 0x00036DCA
			public override string CommandDescription
			{
				get
				{
					return "Clears ALL binds.";
				}
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000C91 RID: 3217 RVA: 0x00038BC3 File Offset: 0x00036DC3
			public override string ExampleUsage
			{
				get
				{
					return "clearbinds";
				}
			}

			// Token: 0x06000C92 RID: 3218 RVA: 0x00038BD1 File Offset: 0x00036DD1
			public override void Execute(List<string> args)
			{
				Singleton<Console>.Instance.ClearBindings();
			}
		}

		// Token: 0x02000257 RID: 599
		public class HideUI : Console.ConsoleCommand
		{
			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00038BDD File Offset: 0x00036DDD
			public override string CommandWord
			{
				get
				{
					return "hideui";
				}
			}

			// Token: 0x170002C9 RID: 713
			// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00038BE4 File Offset: 0x00036DE4
			public override string CommandDescription
			{
				get
				{
					return "Hides all on-screen UI.";
				}
			}

			// Token: 0x170002CA RID: 714
			// (get) Token: 0x06000C96 RID: 3222 RVA: 0x00038BDD File Offset: 0x00036DDD
			public override string ExampleUsage
			{
				get
				{
					return "hideui";
				}
			}

			// Token: 0x06000C97 RID: 3223 RVA: 0x00038BEB File Offset: 0x00036DEB
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.canvas.enabled = false;
			}
		}

		// Token: 0x02000258 RID: 600
		public class GiveXP : Console.ConsoleCommand
		{
			// Token: 0x170002CB RID: 715
			// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00038BFD File Offset: 0x00036DFD
			public override string CommandWord
			{
				get
				{
					return "addxp";
				}
			}

			// Token: 0x170002CC RID: 716
			// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00038C04 File Offset: 0x00036E04
			public override string CommandDescription
			{
				get
				{
					return "Adds the specified amount of experience points.";
				}
			}

			// Token: 0x170002CD RID: 717
			// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00038C0B File Offset: 0x00036E0B
			public override string ExampleUsage
			{
				get
				{
					return "addxp 100";
				}
			}

			// Token: 0x06000C9C RID: 3228 RVA: 0x00038C14 File Offset: 0x00036E14
			public override void Execute(List<string> args)
			{
				int num = 0;
				if (args.Count == 0 || !int.TryParse(args[0], out num) || num < 0)
				{
					Console.LogWarning("Unrecognized command format. Correct format example(s): " + this.ExampleUsage, null);
					return;
				}
				Console.Log("Giving " + num.ToString() + " experience points", null);
				NetworkSingleton<LevelManager>.Instance.AddXP(num);
			}
		}

		// Token: 0x02000259 RID: 601
		public class Disable : Console.ConsoleCommand
		{
			// Token: 0x170002CE RID: 718
			// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00038C7D File Offset: 0x00036E7D
			public override string CommandWord
			{
				get
				{
					return "disable";
				}
			}

			// Token: 0x170002CF RID: 719
			// (get) Token: 0x06000C9F RID: 3231 RVA: 0x00038C84 File Offset: 0x00036E84
			public override string CommandDescription
			{
				get
				{
					return "Disables the specified GameObject";
				}
			}

			// Token: 0x170002D0 RID: 720
			// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00038C8B File Offset: 0x00036E8B
			public override string ExampleUsage
			{
				get
				{
					return "disable pp";
				}
			}

			// Token: 0x06000CA1 RID: 3233 RVA: 0x00038C94 File Offset: 0x00036E94
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string code = args[0].ToLower();
				Console.LabelledGameObject labelledGameObject = Singleton<Console>.Instance.LabelledGameObjectList.Find((Console.LabelledGameObject x) => x.Label.ToLower() == code);
				if (labelledGameObject == null)
				{
					Console.LogCommandError("Could not find GameObject with label '" + code + "'");
					return;
				}
				labelledGameObject.GameObject.SetActive(false);
			}
		}

		// Token: 0x0200025B RID: 603
		public class Enable : Console.ConsoleCommand
		{
			// Token: 0x170002D1 RID: 721
			// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00038D35 File Offset: 0x00036F35
			public override string CommandWord
			{
				get
				{
					return "enable";
				}
			}

			// Token: 0x170002D2 RID: 722
			// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00038D3C File Offset: 0x00036F3C
			public override string CommandDescription
			{
				get
				{
					return "Enables the specified GameObject";
				}
			}

			// Token: 0x170002D3 RID: 723
			// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x00038D43 File Offset: 0x00036F43
			public override string ExampleUsage
			{
				get
				{
					return "enable pp";
				}
			}

			// Token: 0x06000CA8 RID: 3240 RVA: 0x00038D4C File Offset: 0x00036F4C
			public override void Execute(List<string> args)
			{
				if (args.Count <= 0)
				{
					Console.LogUnrecognizedFormat(new string[]
					{
						this.ExampleUsage
					});
					return;
				}
				string code = args[0].ToLower();
				Console.LabelledGameObject labelledGameObject = Singleton<Console>.Instance.LabelledGameObjectList.Find((Console.LabelledGameObject x) => x.Label.ToLower() == code);
				if (labelledGameObject == null)
				{
					Console.LogCommandError("Could not find GameObject with label '" + code + "'");
					return;
				}
				labelledGameObject.GameObject.SetActive(true);
			}
		}

		// Token: 0x0200025D RID: 605
		public class EndTutorial : Console.ConsoleCommand
		{
			// Token: 0x170002D4 RID: 724
			// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00038DED File Offset: 0x00036FED
			public override string CommandWord
			{
				get
				{
					return "endtutorial";
				}
			}

			// Token: 0x170002D5 RID: 725
			// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00038DF4 File Offset: 0x00036FF4
			public override string CommandDescription
			{
				get
				{
					return "Forces the tutorial to end immediately (only if the player is actually in the tutorial).";
				}
			}

			// Token: 0x170002D6 RID: 726
			// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00038DED File Offset: 0x00036FED
			public override string ExampleUsage
			{
				get
				{
					return "endtutorial";
				}
			}

			// Token: 0x06000CAF RID: 3247 RVA: 0x00038DFB File Offset: 0x00036FFB
			public override void Execute(List<string> args)
			{
				NetworkSingleton<GameManager>.Instance.EndTutorial(false);
			}
		}

		// Token: 0x0200025E RID: 606
		public class DisableNPCAsset : Console.ConsoleCommand
		{
			// Token: 0x170002D7 RID: 727
			// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00038E08 File Offset: 0x00037008
			public override string CommandWord
			{
				get
				{
					return "disablenpcasset";
				}
			}

			// Token: 0x170002D8 RID: 728
			// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00038E0F File Offset: 0x0003700F
			public override string CommandDescription
			{
				get
				{
					return "Disabled the given asset under all NPCs";
				}
			}

			// Token: 0x170002D9 RID: 729
			// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00038E16 File Offset: 0x00037016
			public override string ExampleUsage
			{
				get
				{
					return "disablenpcasset avatar";
				}
			}

			// Token: 0x06000CB4 RID: 3252 RVA: 0x00038E20 File Offset: 0x00037020
			public override void Execute(List<string> args)
			{
				if (args.Count > 0)
				{
					string text = args[0];
					using (List<NPC>.Enumerator enumerator = NPCManager.NPCRegistry.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NPC npc = enumerator.Current;
							for (int i = 0; i < npc.transform.childCount; i++)
							{
								Transform child = npc.transform.GetChild(i);
								if (text == "all" || child.name.ToLower() == text.ToLower())
								{
									child.gameObject.SetActive(false);
								}
							}
						}
						return;
					}
				}
				Console.LogUnrecognizedFormat(new string[]
				{
					this.ExampleUsage
				});
			}
		}

		// Token: 0x0200025F RID: 607
		public class ShowFPS : Console.ConsoleCommand
		{
			// Token: 0x170002DA RID: 730
			// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00038EEC File Offset: 0x000370EC
			public override string CommandWord
			{
				get
				{
					return "showfps";
				}
			}

			// Token: 0x170002DB RID: 731
			// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00038EF3 File Offset: 0x000370F3
			public override string CommandDescription
			{
				get
				{
					return "Shows FPS label.";
				}
			}

			// Token: 0x170002DC RID: 732
			// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00038EEC File Offset: 0x000370EC
			public override string ExampleUsage
			{
				get
				{
					return "showfps";
				}
			}

			// Token: 0x06000CB9 RID: 3257 RVA: 0x00038EFA File Offset: 0x000370FA
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.fpsLabel.gameObject.SetActive(true);
			}
		}

		// Token: 0x02000260 RID: 608
		public class HideFPS : Console.ConsoleCommand
		{
			// Token: 0x170002DD RID: 733
			// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00038F11 File Offset: 0x00037111
			public override string CommandWord
			{
				get
				{
					return "hidefps";
				}
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00038F18 File Offset: 0x00037118
			public override string CommandDescription
			{
				get
				{
					return "Hides FPS label.";
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00038F11 File Offset: 0x00037111
			public override string ExampleUsage
			{
				get
				{
					return "hidefps";
				}
			}

			// Token: 0x06000CBE RID: 3262 RVA: 0x00038F1F File Offset: 0x0003711F
			public override void Execute(List<string> args)
			{
				Singleton<HUD>.Instance.fpsLabel.gameObject.SetActive(false);
			}
		}

		// Token: 0x02000261 RID: 609
		public class ClearTrash : Console.ConsoleCommand
		{
			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00038F36 File Offset: 0x00037136
			public override string CommandWord
			{
				get
				{
					return "cleartrash";
				}
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00038F3D File Offset: 0x0003713D
			public override string CommandDescription
			{
				get
				{
					return "Instantly removes all trash from the world.";
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00038F36 File Offset: 0x00037136
			public override string ExampleUsage
			{
				get
				{
					return "cleartrash";
				}
			}

			// Token: 0x06000CC3 RID: 3267 RVA: 0x00038F44 File Offset: 0x00037144
			public override void Execute(List<string> args)
			{
				NetworkSingleton<TrashManager>.Instance.DestroyAllTrash();
			}
		}

		// Token: 0x02000262 RID: 610
		[Serializable]
		public class LabelledGameObject
		{
			// Token: 0x04000D4F RID: 3407
			public string Label;

			// Token: 0x04000D50 RID: 3408
			public GameObject GameObject;
		}
	}
}
