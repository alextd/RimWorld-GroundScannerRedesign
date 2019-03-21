using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace Ground_Scanner_Redesign
{
	class CompTerrainMineralScanner : CompTerrainPump
	{
		protected override void AffectCell(IntVec3 c)
		{
			Log.Message($"Adding {c} to MapCompTerrainMineralScanned");
			parent.Map.GetComponent<MapCompTerrainMineralScanned>()[c] = true;
		}
	}

	public class CompProperties_TerrainMineralScanner : CompProperties_TerrainPump
	{
		public CompProperties_TerrainMineralScanner()
		{
			this.compClass = typeof(CompTerrainMineralScanner);
		}
	}
}
