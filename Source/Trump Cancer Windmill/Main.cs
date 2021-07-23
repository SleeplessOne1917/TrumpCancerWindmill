using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trump_Cancer_Windmill;
using Verse;
using UnityEngine;

namespace TrumpCancerWindmill
{

    [StaticConstructorOnStartup]
    internal static class Main
    {
        static Main()
        {
            var harmony = new Harmony("abias1122.TrumpCancerWindmill");

            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }

    public class TrumpCancerWindmill : Mod
    {
        public static TrumpCancerWindmillSettings Settings;

        public TrumpCancerWindmill(ModContentPack content) : base(content)
        {
            Settings = GetSettings<TrumpCancerWindmillSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "ModLabel".Translate();
        }
    }

    [HarmonyPatch(typeof(CompPowerPlantWind), "CompTick")]
    class CompPowerPlantWindPatch
    {
        [HarmonyPostfix]
        static void Postfix(CompPowerPlantWind __instance)
        {
            if (!TrumpCancerWindmill.Settings.disableCancer)
            {
                foreach (var pawn in GetPawnsInRadius(__instance))
                {
                    if (Rand.Value < TrumpCancerWindmill.Settings.cancerChance)
                    {
                        var cancerOnPawn = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Carcinoma);
                        var severity = Rand.Range(0.15f, 0.30f);

                        if (cancerOnPawn != null)
                        {
                            cancerOnPawn.Severity += severity;
                            Messages.Message("CancerWorsenedMessage".Translate(pawn.Name), MessageTypeDefOf.NegativeHealthEvent);
                        }
                        else
                        {
                            var cancer = HediffMaker.MakeHediff(HediffDefOf.Carcinoma, pawn);
                            cancer.Severity = severity;
                            pawn.health.AddHediff(cancer);

                            Messages.Message("GotCancerMessage".Translate(pawn.Name), MessageTypeDefOf.NegativeHealthEvent);
                        }
                    }
                }
            }
        }

        private static IEnumerable<Pawn> GetPawnsInRadius(CompPowerPlantWind __instance)
        {
            var map = __instance.parent.Map;
            var cellsToCheck = map.AllCells.Where(cell => cell.DistanceTo(__instance.parent.Position) <= TrumpCancerWindmill.Settings.cancerRadius);
            return cellsToCheck.Select(cell => cell.GetFirstPawn(map)).Where(pawn => pawn != null);
        }
    }

    [HarmonyPatch(typeof(ThingComp), "PostDrawExtraSelectionOverlays")]
    class ThingCompPatch
    {
        [HarmonyPostfix]
        static void DrawCancerRadius(ThingComp __instance)
        {
            if (__instance is CompPowerPlantWind)
            {
                GenDraw.DrawRadiusRing(__instance.parent.Position, TrumpCancerWindmill.Settings.cancerRadius);
            }
        }
    }

    [HarmonyPatch(typeof(PlaceWorker_WindTurbine), "DrawGhost")]
    class PlaceWorker_WindTurbinePatch
    {
        [HarmonyPostfix]
        static void DrawCancerRadius(IntVec3 center)
        {
            GenDraw.DrawRadiusRing(center, TrumpCancerWindmill.Settings.cancerRadius);
        }
    }
}