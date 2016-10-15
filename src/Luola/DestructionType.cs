using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Luola
{
    public class DestructionType
    {
        public readonly bool[,] EraseData;
        public readonly Texture2D ExplosionTexture;
        public readonly int Size;

        public DestructionType(Game game, int size)
        {
            Size = size;
            EraseData = new bool[size, size];
            var diam = size/2f;
            var diamsq = diam*diam;
            for (var y = 0; y < size; y++)
                for (var x = 0; x < size; x++)
                {
                    var pos = new Vector2(x - diam, y - diam);
                    var solid = pos.LengthSquared() <= diamsq;
                    EraseData[y, x] = solid;
                }

            try
            {
                ExplosionTexture = game.Content.Load<Texture2D>("explosions/" + size);
            }
            catch (ContentLoadException)
            {
                ExplosionTexture = null;
            }
        }
    }
}