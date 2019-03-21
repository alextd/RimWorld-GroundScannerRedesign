using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Harmony;
using UnityEngine;

namespace Ground_Scanner_Redesign
{
	[HarmonyPatch(typeof(DeepResourceGrid), nameof(DeepResourceGrid.GetCellBool))]
	class DeepResourceGridPatch
	{
		//public bool GetCellBool(int index)
		public static void Postfix(int index, ref bool __result, Map ___map)
		{
			if (DebugSettings.godMode) return;

			if (!__result && !___map.GetComponent<MapCompTerrainMineralScanned>()[index])
			{
				__result = true;  //Outside scanned area will be shown, but will be red
			}
		}
	}

	[HarmonyPatch(typeof(DeepResourceGrid), nameof(DeepResourceGrid.GetCellExtraColor))]
	class DeepResourceGridPatchColor
	{
		public static Color unknownColor = Color.red * 0.2f;
		//public Color GetCellExtraColor(int index)
		public static bool Prefix(int index, ref Color __result, Map ___map)
		{
			if (DebugSettings.godMode) return true;

			if (!___map.GetComponent<MapCompTerrainMineralScanned>()[index])
			{
				__result = unknownColor;
				return false;
			}
			return true;
		}
	}
}
