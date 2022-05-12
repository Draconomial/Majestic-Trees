using HarmonyLib;
using Verse;
using RimWorld;
using System;

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

        public class PlantExtension : DefModExtension
        {
            public bool makeTransparent;
        }

        [HarmonyPatch(typeof(Plant), nameof(Plant.Graphic))]
        public static class Plant_GraphicPatch
        {
            public static void Postfix(Plant __instance, ref Graphic __result)
            {
                var extension = __instance.def.GetModExtension<PlantExtension>();
                if (extension != null)
                {
                    //__result = GetTransparentGraphic(__instance, __result);
                }
            }

            public static Graphic GetTransparentGraphic(Plant __instance, Graphic initialGraphic)
            {
                if (WorldWorker.SeeThroughTrees)
                {
                    if (WorldWorker.seeThroughCursor)
                    {
                        var a = UI.MouseCell();
                        var b = __instance.Position;
                        //Verse.Log.Message(a.ToString() + "  " + b.ToString());
                        if ((Math.Pow(a.x - b.x, 2) + Math.Pow(a.z - 2 - b.z, 2)) <= WorldWorker.cursorSize)
                        {
                            //Verse.Log.Message(base.Graphic.path);
                            return GraphicDatabase.Get(
                                initialGraphic.GetType(),
                                initialGraphic.path + "_Transparent",
                                ShaderDatabase.TransparentPlant,
                                initialGraphic.drawSize,
                                initialGraphic.color,
                                initialGraphic.colorTwo
                            );
                            //return GhostUtility.GhostGraphicFor(base.Graphic, this.def, GhostColor);
                        }
                    }
                    else
                    {
                        return GraphicDatabase.Get(
                            initialGraphic.GetType(),
                            initialGraphic.path + "_Transparent",
                            ShaderDatabase.TransparentPlant,
                            initialGraphic.drawSize,
                            initialGraphic.color,
                            initialGraphic.colorTwo
                        );
                    }
                }
                return initialGraphic;
            }
        }
    }
}
