using RimWorld;
using Verse;

namespace COMIGOMajesticTrees
{
    public class MapWorker : MapComponent
    {
        private IntVec3 prevPos;
        private bool prevEnabled;
        private bool prevCursorMode;
        private WorldWorker cachedWorldWorker;
        private int tick = 0;
        public MapWorker(Map map) : base(map)
        {
            cachedWorldWorker = Find.World.GetComponent<WorldWorker>();
            prevPos.x = prevPos.y = prevPos.z = 0;
        }
        public override void MapComponentUpdate()
        {
            base.MapComponentTick();
            if (!WorldWorker.SeeThroughTrees && WorldWorker.SeeThroughTrees == prevEnabled)
            {
                return;
            }
            if (WorldWorker.seeThroughCursor != prevCursorMode)
            {
                map.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
            }
            if (WorldWorker.seeThroughCursor)
            {
                tick++;
                if (tick % WorldWorker.cursorResolution != 0)
                {
                    return;
                }
                var a = UI.MouseCell();
                if (!a.InBounds(map))
                {
                    return;
                }
                if (a.Equals(prevPos))
                {
                    return;
                }
                a.z += 2;
                map.mapDrawer.MapMeshDirty(prevPos, MapMeshFlag.Things, true, true);
                map.mapDrawer.MapMeshDirty(a, MapMeshFlag.Things, true, true);
                prevPos = a;
            }
            else if (WorldWorker.SeeThroughTrees != prevEnabled)
            {
                map.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
            }
            prevEnabled = WorldWorker.SeeThroughTrees;
            prevCursorMode = WorldWorker.seeThroughCursor;
        }
    }
}