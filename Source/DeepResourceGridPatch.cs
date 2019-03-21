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

			if (!__result &&
				(!___map.GetComponent<MapCompTerrainMineralScanned>()[index] || 
				(___map.edificeGrid[index]?.def.building?.isResourceRock ?? false)))
			{
				__result = true;  //Outside scanned area will be shown, but will be red
			}
		}
	}

	[HarmonyPatch(typeof(DeepResourceGrid), nameof(DeepResourceGrid.GetCellExtraColor))]
	class DeepResourceGridPatchColor
	{
		public static Color unknownColor = Color.red * 0.1f;
		public static Color groundColor = Color.blue * 0.25f;
		public static Color deepColor = Color.green * 0.25f; //based on convoluted DebugMatsSpectrum coloring

		//public Color GetCellExtraColor(int index)
		//This has to be a prefix since base code expects a thingdef at the index
		public static bool Prefix(DeepResourceGrid __instance, int index, ref Color __result, Map ___map)
		{
			if (DebugSettings.godMode) return true;

			if (!___map.GetComponent<MapCompTerrainMineralScanned>()[index])
			{
				__result = unknownColor;
				return false;
			}

			if (___map.edificeGrid[index]?.def.building?.isResourceRock ?? false)
			{
				IntVec3 c = ___map.cellIndices.IndexToCell(index);
				int count = __instance.CountAt(c);
				bool deepMineral = count > 0;
				if (deepMineral)
				{
					ThingDef thingDef = __instance.ThingDefAt(c);
					float ratio = (float)count / (float)thingDef.deepCountPerCell / 2f;

					__result = Color.Lerp(groundColor, deepColor, ratio);
				}
				else
					__result = groundColor;
				return false;
			}
			return true;
		}
	}
}
