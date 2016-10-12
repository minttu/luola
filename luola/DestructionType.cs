using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace luola
{
    public class DestructionType
    {
        public Texture2D Texture;
        public bool[,] EraseData;
        public int Size;

        public DestructionType(GraphicsDevice graphicsDevice, int size)
        {
            Size = size;
            Texture = new Texture2D(graphicsDevice, size, size);
            var data = new Color[size*size];
            EraseData = new bool[size, size];
            float diam = size/2f;
            float diamsq = diam*diam;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var pos = new Vector2(x - diam, y - diam);
                    var solid = pos.LengthSquared() <= diamsq;
                    EraseData[y, x] = solid;
                    data[y*size + x] = solid ? Color.White : Color.Transparent;
                }
            }

            Texture.SetData(data);
        }
    }
}