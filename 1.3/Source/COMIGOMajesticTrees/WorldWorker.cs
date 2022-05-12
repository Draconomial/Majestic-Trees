using RimWorld;
using RimWorld.Planet;
using Verse;
using System;

namespace COMIGOMajesticTrees
{
    public class WorldWorker : WorldComponent
    {
        public static bool SeeThroughTrees = true; // Scribed

        public static bool seeThroughCursor = true;
        public static int cursorResolution = 1;
        public static double cursorSize = 4.5;
        public WorldWorker(World world) : base(world)
        {
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref SeeThroughTrees, "SeeThroughMajesticTrees", true);
        }

        public override void WorldComponentUpdate()
        {
            seeThroughCursor = bool.Parse(XmlExtensions.XmlMod.getSetting("ComigoGames.Textures.Trees", "seeThroughCursor"));
            cursorResolution = ((int)Math.Floor(float.Parse(XmlExtensions.XmlMod.getSetting("ComigoGames.Textures.Trees", "cursorResolution"))));
            cursorSize = Math.Pow(float.Parse(XmlExtensions.XmlMod.getSetting("ComigoGames.Textures.Trees", "cursorSize")), 2);
        }
    }
}
