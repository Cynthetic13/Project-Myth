using SimplexNoise;

namespace Myth.World.Biomes
{
    public class PeacefulGroveBiome : IBiome
    {
        public float GetStoneBaseHeight(int x, int z) => 44f + GetNoise(x, z, 0.05f, 4f);
        public float GetStoneMountainHeight(int x, int z) => GetNoise(x, z, 0.0008f, 8f);
        public float GetStoneMinHeight() => -12f;
        public float GetDirtBaseHeight(int x, int z) => 4f;
        public float GetDirtNoise(int x, int z) => GetNoise(x, z, 0.04f, 3f);
        public byte GetTopBlock() => 2;
        public byte GetUnderBlock() => 1;

        private float GetNoise(int x, int z, float scale, float max)
        {
            return (Noise.CalcPixel2D(x, z, scale) + 1.0f) * (max / 2.0f);
        }
    }
}