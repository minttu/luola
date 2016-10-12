using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class MapLayerData
    {
        public string Image { get; set; }
        public string Type { get; set; }
        public float Parallax { get; set; }

        public Texture2D Texture { get; set; }
        public Color[] Colors { get; set; }
        public bool[,] Collisions { get; set; }

        public int Width => Texture.Width;
        public int Height => Texture.Height;

        public void CalculateCollisions()
        {
            if (Type == "detail")
                return;

            Colors = new Color[Width*Height];
            Collisions = new bool[Width, Height];

            Texture.GetData(Colors);

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var solid = Colors[x + Width*y] != Color.Transparent;
                    Collisions[x, y] = solid;
                }
            }
        }
    }
}