using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Harmony;

namespace Ground_Scanner_Redesign
{
	[HarmonyPatch(typeof(PlaceWorker_ShowDeepResources), nameof(PlaceWorker_ShowDeepResources.DrawGhost))]
	class PlaceWorker_ShowDeepResourcesPatch
	{
		//public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		public static bool Prefix()
		{
			//Simply show what we know. Don't need the scanner on to remember what it scanned.
			Map currentMap = Find.CurrentMap;
			currentMap.deepResourceGrid.MarkForDraw();
			return false;
		}
	}
}
