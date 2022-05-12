using HarmonyLib;
using Verse;
using RimWorld;
using System;
using System.Reflection;
using System.Linq;

namespace COMIGOMajesticTrees
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
            foreach (var def in DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.plant != null))
            {
                var extension = def.GetModExtension<PlantExtension>();
                if (extension != null)
                {
                    if (extension.makeLeafless)
                    {
                        LongEventHandler.ExecuteWhenFinished(delegate
                        {
                            var basePath = def.graphicData.texPath;
                            var lastIdx = basePath.LastIndexOf('/');
                            var leaflessPath = basePath.Substring(0, lastIdx + 1) + "Leafless_" 
                                + basePath.Substring(lastIdx + 1, basePath.Length - lastIdx - 1);
                            def.plant.leaflessGraphic = GraphicDatabase.Get(def.graphicData.graphicClass, leaflessPath, 
                                def.graphic.Shader, def.graphicData.drawSize, def.graphicData.color, def.graphicData.colorTwo);
                        });
                    }
                }
            }
        }

        public static bool IsTransparent(this Plant plant)
        {
            var extension = plant.def.GetModExtension<PlantExtension>();
            if (extension != null && extension.makeTransparent)
            {
                return true;
            }
            return false;
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
        public bool makeLeafless;
    }

    [HarmonyPatch(typeof(Plant), nameof(Plant.Graphic), MethodType.Getter)]
    public static class Plant_GraphicPatch
    {
        public static void Postfix(Plant __instance, ref Graphic __result)
        {
            if (__instance.IsTransparent())
            {
                __result = GetTransparentGraphic(__instance, __result);
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
                            GetTranparentPath(initialGraphic.path),
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
                        GetTranparentPath(initialGraphic.path),
                        ShaderDatabase.TransparentPlant,
                        initialGraphic.drawSize,
                        initialGraphic.color,
                        initialGraphic.colorTwo
                    );
                }
            }
            return initialGraphic;
        }

        public static string GetTranparentPath(string path)
        {
            var lastIdx = path.LastIndexOf('/');
            return path.Substring(0, lastIdx + 1) + "Transparent_" + path.Substring(lastIdx + 1, path.Length - lastIdx - 1);
        }
    }
}