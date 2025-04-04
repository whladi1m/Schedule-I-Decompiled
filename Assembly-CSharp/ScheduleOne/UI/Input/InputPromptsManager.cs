using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.Input
{
	// Token: 0x02000B1C RID: 2844
	public class InputPromptsManager : Singleton<InputPromptsManager>
	{
		// Token: 0x06004BC6 RID: 19398 RVA: 0x0013DD04 File Offset: 0x0013BF04
		public PromptImage GetPromptImage(string controlPath, RectTransform parent)
		{
			if (this.GetDisplayNameForControlPath(controlPath) == string.Empty)
			{
				Console.LogError("GetPromptImage: controlPath " + controlPath + " not found", null);
				return null;
			}
			if (this.IsControlPathMouseRelated(controlPath))
			{
				if (controlPath == "leftButton")
				{
					return UnityEngine.Object.Instantiate<GameObject>(this.LeftClickPromptPrefab, parent).GetComponent<PromptImage>();
				}
				if (controlPath == "middleButton")
				{
					return UnityEngine.Object.Instantiate<GameObject>(this.MiddleClickPromptPrefab, parent).GetComponent<PromptImage>();
				}
				if (controlPath == "rightButton")
				{
					return UnityEngine.Object.Instantiate<GameObject>(this.RightClickPromptPrefab, parent).GetComponent<PromptImage>();
				}
				return null;
			}
			else
			{
				if (this.IsControlPathExtraWideKey(controlPath))
				{
					PromptImageWithText component = UnityEngine.Object.Instantiate<GameObject>(this.ExtraWideKeyPromptPrefab, parent).GetComponent<PromptImageWithText>();
					component.Label.text = this.GetDisplayNameForControlPath(controlPath);
					return component;
				}
				if (this.IsControlPathWideKey(controlPath))
				{
					PromptImageWithText component2 = UnityEngine.Object.Instantiate<GameObject>(this.WideKeyPromptPrefab, parent).GetComponent<PromptImageWithText>();
					component2.Label.text = this.GetDisplayNameForControlPath(controlPath);
					return component2;
				}
				PromptImageWithText component3 = UnityEngine.Object.Instantiate<GameObject>(this.KeyPromptPrefab, parent).GetComponent<PromptImageWithText>();
				component3.Label.text = this.GetDisplayNameForControlPath(controlPath);
				return component3;
			}
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0013DE21 File Offset: 0x0013C021
		private bool IsControlPathMouseRelated(string controlPath)
		{
			return controlPath == "leftButton" || controlPath == "middleButton" || controlPath == "rightButton";
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x0013DE54 File Offset: 0x0013C054
		private bool IsControlPathWideKey(string controlPath)
		{
			return controlPath == "escape" || controlPath == "tab" || controlPath == "capsLock" || controlPath == "enter" || controlPath == "backspace" || controlPath == "leftShift" || controlPath == "rightShift" || controlPath == "shift" || controlPath == "ctrl" || controlPath == "leftCtrl" || controlPath == "rightCtrl" || controlPath == "leftAlt" || controlPath == "rightAlt";
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x0013DF25 File Offset: 0x0013C125
		private bool IsControlPathExtraWideKey(string controlPath)
		{
			return controlPath == "space";
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x0013DF38 File Offset: 0x0013C138
		public string GetDisplayNameForControlPath(string controlPath)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(controlPath);
			if (num <= 2580104964U)
			{
				if (num <= 873244444U)
				{
					if (num <= 404911044U)
					{
						if (num <= 305218375U)
						{
							if (num <= 102175844U)
							{
								if (num != 68538398U)
								{
									if (num == 102175844U)
									{
										if (controlPath == "ctrl")
										{
											return "Ctrl";
										}
									}
								}
								else if (controlPath == "semicolon")
								{
									return ";";
								}
							}
							else if (num != 203579616U)
							{
								if (num != 220357235U)
								{
									if (num == 305218375U)
									{
										if (controlPath == "rightShift")
										{
											return "Shift";
										}
									}
								}
								else if (controlPath == "f8")
								{
									return "F8";
								}
							}
							else if (controlPath == "f9")
							{
								return "F9";
							}
						}
						else if (num <= 371355806U)
						{
							if (num != 337800568U)
							{
								if (num == 371355806U)
								{
									if (controlPath == "f3")
									{
										return "F3";
									}
								}
							}
							else if (controlPath == "f1")
							{
								return "F1";
							}
						}
						else if (num != 388133425U)
						{
							if (num != 397845604U)
							{
								if (num == 404911044U)
								{
									if (controlPath == "f5")
									{
										return "F5";
									}
								}
							}
							else if (controlPath == "rightCtrl")
							{
								return "Ctrl";
							}
						}
						else if (controlPath == "f2")
						{
							return "F2";
						}
					}
					else if (num <= 629943691U)
					{
						if (num <= 438466282U)
						{
							if (num != 421688663U)
							{
								if (num == 438466282U)
								{
									if (controlPath == "f7")
									{
										return "F7";
									}
								}
							}
							else if (controlPath == "f4")
							{
								return "F4";
							}
						}
						else if (num != 455243901U)
						{
							if (num != 584777654U)
							{
								if (num == 629943691U)
								{
									if (controlPath == "leftAlt")
									{
										return "Alt";
									}
								}
							}
							else if (controlPath == "downArrow")
							{
								return "Down";
							}
						}
						else if (controlPath == "f6")
						{
							return "F6";
						}
					}
					else if (num <= 822911587U)
					{
						if (num != 806133968U)
						{
							if (num == 822911587U)
							{
								if (controlPath == "4")
								{
									return "4";
								}
							}
						}
						else if (controlPath == "5")
						{
							return "5";
						}
					}
					else if (num != 839689206U)
					{
						if (num != 856466825U)
						{
							if (num == 873244444U)
							{
								if (controlPath == "1")
								{
									return "1";
								}
							}
						}
						else if (controlPath == "6")
						{
							return "6";
						}
					}
					else if (controlPath == "7")
					{
						return "7";
					}
				}
				else if (num <= 1409389383U)
				{
					if (num <= 1007465396U)
					{
						if (num <= 894689925U)
						{
							if (num != 890022063U)
							{
								if (num == 894689925U)
								{
									if (controlPath == "space")
									{
										return "Space";
									}
								}
							}
							else if (controlPath == "0")
							{
								return "0";
							}
						}
						else if (num != 906799682U)
						{
							if (num != 923577301U)
							{
								if (num == 1007465396U)
								{
									if (controlPath == "9")
									{
										return "9";
									}
								}
							}
							else if (controlPath == "2")
							{
								return "2";
							}
						}
						else if (controlPath == "3")
						{
							return "3";
						}
					}
					else if (num <= 1115936390U)
					{
						if (num != 1024243015U)
						{
							if (num == 1115936390U)
							{
								if (controlPath == "rightArrow")
								{
									return "Right";
								}
							}
						}
						else if (controlPath == "8")
						{
							return "8";
						}
					}
					else if (num != 1141236441U)
					{
						if (num != 1362922900U)
						{
							if (num == 1409389383U)
							{
								if (controlPath == "shift")
								{
									return "Shift";
								}
							}
						}
						else if (controlPath == "equals")
						{
							return "=";
						}
					}
					else if (controlPath == "upArrow")
					{
						return "Up";
					}
				}
				else if (num <= 2026475230U)
				{
					if (num <= 1421919596U)
					{
						if (num != 1420235188U)
						{
							if (num == 1421919596U)
							{
								if (controlPath == "leftBracket")
								{
									return "[";
								}
							}
						}
						else if (controlPath == "leftShift")
						{
							return "Shift";
						}
					}
					else if (num != 1673971305U)
					{
						if (num != 1863842679U)
						{
							if (num == 2026475230U)
							{
								if (controlPath == "backquote")
								{
									return "`";
								}
							}
						}
						else if (controlPath == "backslash")
						{
							return "\\";
						}
					}
					else if (controlPath == "leftArrow")
					{
						return "Left";
					}
				}
				else if (num <= 2266937356U)
				{
					if (num != 2235328556U)
					{
						if (num == 2266937356U)
						{
							if (controlPath == "middleButton")
							{
								return "MMB";
							}
						}
					}
					else if (controlPath == "backspace")
					{
						return "Back";
					}
				}
				else if (num != 2330187635U)
				{
					if (num != 2566336076U)
					{
						if (num == 2580104964U)
						{
							if (controlPath == "period")
							{
								return ".";
							}
						}
					}
					else if (controlPath == "tab")
					{
						return "Tab";
					}
				}
				else if (controlPath == "rightBracket")
				{
					return "]";
				}
			}
			else if (num <= 3943445553U)
			{
				if (num <= 3775669363U)
				{
					if (num <= 2995289047U)
					{
						if (num <= 2787020153U)
						{
							if (num != 2652972038U)
							{
								if (num == 2787020153U)
								{
									if (controlPath == "leftCtrl")
									{
										return "Ctrl";
									}
								}
							}
							else if (controlPath == "escape")
							{
								return "Esc";
							}
						}
						else if (num != 2831451249U)
						{
							if (num != 2878618703U)
							{
								if (num == 2995289047U)
								{
									if (controlPath == "quote")
									{
										return "'";
									}
								}
							}
							else if (controlPath == "capsLock")
							{
								return "Caps";
							}
						}
						else if (controlPath == "rightButton")
						{
							return "RMB";
						}
					}
					else if (num <= 3413371114U)
					{
						if (num != 3220866424U)
						{
							if (num == 3413371114U)
							{
								if (controlPath == "slash")
								{
									return "/";
								}
							}
						}
						else if (controlPath == "comma")
						{
							return ",";
						}
					}
					else if (num != 3724402957U)
					{
						if (num != 3758891744U)
						{
							if (num == 3775669363U)
							{
								if (controlPath == "d")
								{
									return "D";
								}
							}
						}
						else if (controlPath == "e")
						{
							return "E";
						}
					}
					else if (controlPath == "enter")
					{
						return "Enter";
					}
				}
				else if (num <= 3876335077U)
				{
					if (num <= 3809224601U)
					{
						if (num != 3792446982U)
						{
							if (num == 3809224601U)
							{
								if (controlPath == "f")
								{
									return "F";
								}
							}
						}
						else if (controlPath == "g")
						{
							return "G";
						}
					}
					else if (num != 3826002220U)
					{
						if (num != 3859557458U)
						{
							if (num == 3876335077U)
							{
								if (controlPath == "b")
								{
									return "B";
								}
							}
						}
						else if (controlPath == "c")
						{
							return "C";
						}
					}
					else if (controlPath == "a")
					{
						return "A";
					}
				}
				else if (num <= 3909191772U)
				{
					if (num != 3893112696U)
					{
						if (num == 3909191772U)
						{
							if (controlPath == "rightAlt")
							{
								return "Alt";
							}
						}
					}
					else if (controlPath == "m")
					{
						return "M";
					}
				}
				else if (num != 3909890315U)
				{
					if (num != 3926667934U)
					{
						if (num == 3943445553U)
						{
							if (controlPath == "n")
							{
								return "N";
							}
						}
					}
					else if (controlPath == "o")
					{
						return "O";
					}
				}
				else if (controlPath == "l")
				{
					return "L";
				}
			}
			else if (num <= 4109376740U)
			{
				if (num <= 4027333648U)
				{
					if (num <= 3977000791U)
					{
						if (num != 3960223172U)
						{
							if (num == 3977000791U)
							{
								if (controlPath == "h")
								{
									return "H";
								}
							}
						}
						else if (controlPath == "i")
						{
							return "I";
						}
					}
					else if (num != 3993778410U)
					{
						if (num != 4010556029U)
						{
							if (num == 4027333648U)
							{
								if (controlPath == "u")
								{
									return "U";
								}
							}
						}
						else if (controlPath == "j")
						{
							return "J";
						}
					}
					else if (controlPath == "k")
					{
						return "K";
					}
				}
				else if (num <= 4060888886U)
				{
					if (num != 4044111267U)
					{
						if (num == 4060888886U)
						{
							if (controlPath == "w")
							{
								return "W";
							}
						}
					}
					else if (controlPath == "t")
					{
						return "T";
					}
				}
				else if (num != 4077666505U)
				{
					if (num != 4094444124U)
					{
						if (num == 4109376740U)
						{
							if (controlPath == "leftButton")
							{
								return "LMB";
							}
						}
					}
					else if (controlPath == "q")
					{
						return "Q";
					}
				}
				else if (controlPath == "v")
				{
					return "V";
				}
			}
			else if (num <= 4197582936U)
			{
				if (num <= 4127999362U)
				{
					if (num != 4111221743U)
					{
						if (num == 4127999362U)
						{
							if (controlPath == "s")
							{
								return "S";
							}
						}
					}
					else if (controlPath == "p")
					{
						return "P";
					}
				}
				else if (num != 4144776981U)
				{
					if (num != 4160372143U)
					{
						if (num == 4197582936U)
						{
							if (controlPath == "f10")
							{
								return "F10";
							}
						}
					}
					else if (controlPath == "minus")
					{
						return "-";
					}
				}
				else if (controlPath == "r")
				{
					return "R";
				}
			}
			else if (num <= 4228665076U)
			{
				if (num != 4214360555U)
				{
					if (num == 4228665076U)
					{
						if (controlPath == "y")
						{
							return "Y";
						}
					}
				}
				else if (controlPath == "f11")
				{
					return "F11";
				}
			}
			else if (num != 4231138174U)
			{
				if (num != 4245442695U)
				{
					if (num == 4278997933U)
					{
						if (controlPath == "z")
						{
							return "Z";
						}
					}
				}
				else if (controlPath == "x")
				{
					return "X";
				}
			}
			else if (controlPath == "f12")
			{
				return "F12";
			}
			return string.Empty;
		}

		// Token: 0x0400390A RID: 14602
		[Header("Input Prompt Prefabs")]
		public GameObject KeyPromptPrefab;

		// Token: 0x0400390B RID: 14603
		public GameObject WideKeyPromptPrefab;

		// Token: 0x0400390C RID: 14604
		public GameObject ExtraWideKeyPromptPrefab;

		// Token: 0x0400390D RID: 14605
		public GameObject LeftClickPromptPrefab;

		// Token: 0x0400390E RID: 14606
		public GameObject MiddleClickPromptPrefab;

		// Token: 0x0400390F RID: 14607
		public GameObject RightClickPromptPrefab;
	}
}
