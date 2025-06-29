using System;
using System.Linq;

namespace Myth.World.Biomes
{
    public static class BiomeBlender
    {
        public static float BlendFloat(Func<IBiome, float> selector, BiomeWeight[] biomes)
        {
            float result = 0f;
            foreach (var b in biomes)
            {
                result += selector(b.Biome) * b.Weight;
            }

            return result;
        }

        public static float BlendFloat(Func<IBiome, int, int, float> selector, int x, int z, BiomeWeight[] biomes)
        {
            float result = 0f;
            foreach (var b in biomes)
            {
                result += selector(b.Biome, x, z) * b.Weight;
            }

            return result;
        }

        public static byte BlendBlock(Func<IBiome, byte> selector, BiomeWeight[] biomes)
        {
            return selector(biomes.OrderByDescending(b => b.Weight).First().Biome);
        }
    }
}