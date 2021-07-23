using System;
using UnityEngine;
using Verse;

namespace Trump_Cancer_Windmill
{
    public class TrumpCancerWindmillSettings : ModSettings
    {
        public int cancerRadius = 20;
        public float cancerChance = 0.00001f;
		public bool disableCancer = false;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref cancerRadius, "cancerRadius", 20, true);
			Scribe_Values.Look(ref cancerChance, "cancerChance", 0.01f, true);
		}

		public void DoSettingsWindowContents(Rect inRect)
		{
			Listing_Standard list = new Listing_Standard() { ColumnWidth = inRect.width };
			list.Begin(inRect);

			list.Label("SettingsRadiusLabel".Translate(cancerRadius));
			cancerRadius = (int)list.Slider(cancerRadius, 1, 100);

			list.Label("SettingsChanceLabel".Translate($"{cancerChance:P3}"));
			cancerChance = list.Slider(cancerChance, 0.0f, 1.0f);

			list.CheckboxLabeled("SettingsDisableCancerLabel".Translate(), ref disableCancer);

			list.End();
		}
	}
}
