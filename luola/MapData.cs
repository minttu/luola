using System.Collections.Generic;

namespace luola
{
    public class MapData
    {
        public List<MapLayerData> Layers { get; set; }
        public List<List<int>> SpawnPoints { get; set; }
    }
}