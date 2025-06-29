namespace Myth.World.Biomes
{
    public interface IBiomeProvider
    {
        BiomeWeight[] GetBiomeWeights(int x, int z);
    }
}