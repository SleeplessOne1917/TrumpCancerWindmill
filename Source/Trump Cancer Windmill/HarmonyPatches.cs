using HarmonyLib;
using RimWorld;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trump_Cancer_Windmill;
using Verse;

namespace TrumpCancerWindmill
{
    namespace HarmonyPatches
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

        [HarmonyPatch(typeof(CompPowerPlantWind), "CompTick")]
        class CompPowerPlantWindPatch
        {
            [HarmonyPostfix]
            static void Postfix(CompPowerPlantWind __instance)
            {
                foreach(var pawn in GetPawnsInRadius(__instance))
                {
                    if (Rand.Value < CancerConstants.CANCER_CHANCE)
                    {
                        var cancerOnPawn = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Carcinoma);
                        var severity = Rand.Range(0.15f, 0.30f);

                        if(cancerOnPawn != null)
                        {
                            cancerOnPawn.Severity += severity;
                            Messages.Message($"{pawn.Name}'s cancer worsened!", MessageTypeDefOf.NegativeHealthEvent);
                        }
                        else
                        {
                            var cancer = HediffMaker.MakeHediff(HediffDefOf.Carcinoma, pawn);
                            cancer.Severity = severity;
                            pawn.health.AddHediff(cancer);

                            Messages.Message($"{pawn.Name} has cancer!", MessageTypeDefOf.NegativeHealthEvent);
                        }
                    }
                }
            }

            private static IEnumerable<Pawn> GetPawnsInRadius(CompPowerPlantWind __instance)
            {
                var map = __instance.parent.Map;
                var cellsToCheck = map.AllCells.Where(cell => cell.DistanceTo(__instance.parent.Position) <= CancerConstants.CANCER_RADIUS);
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
                    GenDraw.DrawRadiusRing(__instance.parent.Position, CancerConstants.CANCER_RADIUS);
                }
            }
        }

        [HarmonyPatch(typeof(PlaceWorker_WindTurbine), "DrawGhost")]
        class PlaceWorker_WindTurbinePatch
        {
            [HarmonyPostfix]
            static void DrawCancerRadius(IntVec3 center)
            {
                GenDraw.DrawRadiusRing(center, CancerConstants.CANCER_RADIUS);
            }
        }
    }
}