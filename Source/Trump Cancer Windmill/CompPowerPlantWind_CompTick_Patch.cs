using HarmonyLib;
using RimWorld;
using System.Reflection;
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
    }
}