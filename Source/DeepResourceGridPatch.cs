using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Harmony;

namespace Ground_Scanner_Redesign
{
	[HarmonyPatch(typeof(DeepResourceGrid), nameof(DeepResourceGrid.GetCellBool))]
	class DeepResourceGridPatch
	{
		//public bool GetCellBool(int index)
		public static void Postfix(int index, ref bool __result, Map ___map)
		{
			if (DebugSettings.godMode) return;
			if (__result)
				if (!___map.GetComponent<MapCompTerrainMineralScanned>()[index])
				{
					Log.Message($"Couldn't see {CellIndicesUtility.IndexToCell(index, ___map.Size.x)} Due to MapCompTerrainMineralScanned");
					__result = false;
				}
		}
	}
}
