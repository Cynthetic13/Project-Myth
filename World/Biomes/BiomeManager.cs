using SimplexNoise;

namespace Myth.World.Biomes
{
    public class BiomeManager
    {
        public static IBiome GetBiomeAt(int x, int z)
        {
            float value = (Noise.CalcPixel2D(x, z, 0.001f) + 1f) * 0.5f;

            return new PeacefulGroveBiome();
        }
    }
}