using Myth.World.Blocks;

namespace Myth.World.Biomes
{
    public interface IBiome
    {
        float GetStoneBaseHeight(int x, int z);
        float GetStoneMountainHeight(int x, int z);
        float GetStoneMinHeight();
        float GetDirtBaseHeight(int x, int z);
        float GetDirtNoise(int x, int z);
        byte GetTopBlock();
        byte GetUnderBlock();
    }
}