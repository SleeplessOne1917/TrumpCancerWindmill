using HarmonyLib;
using RimWorld;
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
                var harmony = new Harmony("abias1122.TrumpWindmillSound");

                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }

        }

        [HarmonyPatch(typeof(CompPowerPlantWind), "CompTick")]
        class CompPowerPlantWindPatch
        {
            [HarmonyPostfix]
            static void Postfix(CompPowerPlantWind __instance)
            {
                var map = __instance.parent.Map;
                var cellsToCheck = map.AllCells.Where(cell => cell.DistanceTo(__instance.parent.Position) <= CancerConstants.CANCER_RADIUS);
                var pawnsInRadius = cellsToCheck.Select(cell => cell.GetFirstPawn(map)).Where(pawn => pawn != null);
                foreach(var pawn in pawnsInRadius)
                {
                    Log.Message($"Pawn {pawn.Name} in radius of windmill {__instance}");
                }
            }
        }

        [HarmonyPatch(typeof(ThingComp), "PostDrawExtraSelectionOverlays")]
        class ThingCompPatch
        {
            [HarmonyPostfix]
            static void DrawCancerRadius(ThingComp __instance)
            {
                if(__instance is CompPowerPlantWind)
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