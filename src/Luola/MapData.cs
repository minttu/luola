using System.Collections.Generic;

namespace Luola
{
    public class MapData
    {
        public List<MapLayerData> Layers { get; set; }
        public List<List<int>> SpawnPoints { get; set; }
        public List<List<int>> PickupPoints { get; set; }
    }
}