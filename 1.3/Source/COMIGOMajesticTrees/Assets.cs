using UnityEngine;
using Verse;
using RimWorld;

namespace COMIGOMajesticTrees
{
    [StaticConstructorOnStartup]
    static class Assets
    {
        public static readonly Texture2D SeeThroughIcon = ContentFinder<Texture2D>.Get("UI/ToggleMajesticSeeThroughIcon", false);
    }
}
