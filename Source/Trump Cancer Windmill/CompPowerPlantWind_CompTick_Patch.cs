using HarmonyLib;
using RimWorld;
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
            static void Postfix()
            {
                Log.Message("Logging wind turbine tick");
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