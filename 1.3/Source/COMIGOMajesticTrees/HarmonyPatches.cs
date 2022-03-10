using HarmonyLib;
using Verse;
using RimWorld;

namespace COMIGOMajesticTrees
{
    class HarmonyPatches
    {
        [StaticConstructorOnStartup]
        static class MajesticPatches
        {
            // make sure DoPatching() is called at start either by
            // the mod loader or by your injector

            static MajesticPatches()
            {
                var harmony = new Harmony("MajesticTrees.Patch.Comigo");
                Harmony.DEBUG = false;
                harmony.PatchAll();
            }
        }

        [HarmonyPatch(typeof(PlaySettings))]
        [HarmonyPatch("DoPlaySettingsGlobalControls")]
        public static class PlaySettingsPatch
        {
            public static void Postfix(WidgetRow row, bool worldView)
            {
                if (!worldView)
                {
                    row.ToggleableIcon(ref WorldWorker.SeeThroughTrees, Assets.SeeThroughIcon, "Enable or disable tree transparency", SoundDefOf.Mouseover_ButtonToggle);
                }
            }
        }
    }
}
