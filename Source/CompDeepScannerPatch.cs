using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using Harmony;

namespace Ground_Scanner_Redesign
{
	[HarmonyPatch(typeof(CompDeepScanner), nameof(CompDeepScanner.PostDrawExtraSelectionOverlays))]
	class CompDeepScannerPatch
	{
		//public override void PostDrawExtraSelectionOverlays()
		public static bool Prefix(CompDeepScanner __instance)
		{
			//no need to check powerComp.PowerOn, just draw what was previously scanned.
			__instance.parent.Map.deepResourceGrid.MarkForDraw();
			return false;
		}
	}
}
