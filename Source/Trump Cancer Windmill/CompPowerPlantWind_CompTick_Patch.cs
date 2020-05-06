using HarmonyLib;
using RimWorld;
using Verse;

namespace Trump_Cancer_Windmill
{
    [StaticConstructorOnStartup]
    public static class CompPowerPlantWind_CompTick_Patch
    {
        static CompPowerPlantWind_CompTick_Patch()
        {
            var harmony = new Harmony("abias1122.TrumpWindmillSound");

            harmony.Patch(
                AccessTools.Method(typeof(CompPowerPlantWind), nameof(CompPowerPlantWind.CompTick)),
                null,
                new HarmonyMethod(typeof(CompPowerPlantWind_CompTick_Patch), nameof(PostfixCompTick)));
        }

        static void PostfixCompTick()
        {
            Log.Message("Logging wind turbine tick");
        }
    }
}
