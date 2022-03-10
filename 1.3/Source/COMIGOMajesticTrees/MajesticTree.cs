using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace COMIGOMajesticTrees
{
    public class MajesticTree: Plant
    {
        //private static Color GhostColor = new Color(0.0f, 1.0f, 0.0f, 2.0f);
        public MajesticTree() : base()
        {
        }
        public override Graphic Graphic
        {
            get
            {
                if (WorldWorker.SeeThroughTrees)
                {
                    if (WorldWorker.seeThroughCursor)
                    {
                        var a = UI.MouseCell();
                        var b = Position;
                        //Verse.Log.Message(a.ToString() + "  " + b.ToString());
                        if ((Math.Pow(a.x - b.x, 2) + Math.Pow(a.z - 2 - b.z, 2)) <= WorldWorker.cursorSize)
                        {
                            //Verse.Log.Message(base.Graphic.path);
                            return GraphicDatabase.Get(
                                base.Graphic.GetType(),
                                base.Graphic.path + "_Transparent",
                                ShaderDatabase.TransparentPlant,
                                base.Graphic.drawSize,
                                base.Graphic.color,
                                base.Graphic.colorTwo
                            );
                            //return GhostUtility.GhostGraphicFor(base.Graphic, this.def, GhostColor);
                        }
                    }
                    else
                    {
                        return GraphicDatabase.Get(
                            base.Graphic.GetType(),
                            base.Graphic.path + "_Transparent",
                            ShaderDatabase.TransparentPlant,
                            base.Graphic.drawSize,
                            base.Graphic.color,
                            base.Graphic.colorTwo
                        );
                    }
                }
                return base.Graphic;
            }
        }
    }
}
