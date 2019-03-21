using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using Harmony;

namespace Ground_Scanner_Redesign
{
	class MapCompTerrainMineralScanned : MapComponent
	{
		private BoolGrid scannedGrid;
		public MapCompTerrainMineralScanned(Map map) : base(map)
		{
			scannedGrid = new BoolGrid(map);
		}

		public override void ExposeData()
		{
			Scribe_Deep.Look(ref scannedGrid, "scannedGrid");
		}

		public bool this[int index]
		{
			get
			{
				return scannedGrid[index];
			}
			set
			{
				if (scannedGrid[index] == value)
				{
					return;
				}
				scannedGrid[index] = value;
				MarkDirty();
			}
		}

		public bool this[IntVec3 c]
		{
			get
			{
				return scannedGrid[c];
			}
			set
			{
				if (scannedGrid[c] == value)
				{
					return;
				}
				scannedGrid[c] = value;
				MarkDirty();
			}
		}

		private void MarkDirty()
		{
			map.deepResourceGrid.Drawer().SetDirty();
		}
	}

	public static class DeepResouceGridEx
	{
		public static FieldInfo drawerInfo = AccessTools.Field(typeof(DeepResourceGrid), "drawer");
		public static CellBoolDrawer Drawer(this DeepResourceGrid deepResourceGrid) =>
			(CellBoolDrawer)drawerInfo.GetValue(deepResourceGrid);
	}
}
