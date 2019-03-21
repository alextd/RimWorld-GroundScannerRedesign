using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using RimWorld;
using Harmony;

namespace Ground_Scanner_Redesign
{
	public static class CompTerrainPumpEx
	{
		public static PropertyInfo WorkingInfo = AccessTools.Property(typeof(CompTerrainPump), "Working");
		public static bool Working(this CompTerrainPump comp) => (bool)WorkingInfo.GetValue(comp, null);

		public static FieldInfo progressTicksInfo = AccessTools.Field(typeof(CompTerrainPump), "progressTicks");
		public static int progressTicks(this CompTerrainPump comp) => (int)progressTicksInfo.GetValue(comp);
		public static void setProgressTicks(this CompTerrainPump comp, int v) => progressTicksInfo.SetValue(comp, v);

		public static PropertyInfo CurrentRadiusInfo = AccessTools.Property(typeof(CompTerrainPump), "CurrentRadius");
		public static float CurrentRadius(this CompTerrainPump comp) => (float)CurrentRadiusInfo.GetValue(comp, null);
	}
	class CompTerrainMineralScanner : CompTerrainPump
	{
		MapCompTerrainMineralScanned comp;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			comp = parent.Map.GetComponent<MapCompTerrainMineralScanned>();
		}

		protected override void AffectCell(IntVec3 c)
		{
			//Log.Message($"Adding {c} to MapCompTerrainMineralScanned");
			comp[c] = true;
		}

		//Radius can't handle > 60
		public IEnumerable<IntVec3> AffectedCells()
		{
			return new CellRect(
				parent.Position.x,
				parent.Position.z,
				parent.RotatedSize.x,
				parent.RotatedSize.z)
				.ExpandedBy((int)this.CurrentRadius())
				.ClipInsideMap(parent.Map).Cells;
		}

		public override void CompTickRare()
		{
			if (this.Working())
			{
				this.setProgressTicks(this.progressTicks() + 250);

				//GenRadial has a max radius of 60
				/*
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius());
				for (int i = 0; i < num; i++)
				{
					AffectCell(parent.Position + GenRadial.RadialPattern[i]);
				}
				*/

				//A dumb square instead
				foreach (IntVec3 cell in AffectedCells())
					AffectCell(cell);
			}
		}

		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius() < parent.Map.Size.x)
			{
				GenDraw.DrawFieldEdges(AffectedCells().ToList());

				//GenDraw.DrawRadiusRing(parent.Position, CurrentRadius);
			}
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
