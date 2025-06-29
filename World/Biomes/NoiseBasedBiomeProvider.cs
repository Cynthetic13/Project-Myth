namespace Myth.World.Biomes
{
    public class NoiseBasedBiomeProvider : IBiomeProvider
    {
        private IBiome[] biomes = new IBiome[]
        {
            new PeacefulGroveBiome(),
            //new ThicketWoodsBiome()
        };

        public BiomeWeight[] GetBiomeWeights(int x, int z)
        {
            float[] rawWeights = new float[biomes.Length];
            float total = 0f;

            for (int i = 0; i < biomes.Length; i++)
            {
                float noise = (SimplexNoise.Noise.CalcPixel2D(x + i * 100, z + i * 100, 0.001f) + 1f) / 2f;
                rawWeights[i] = noise;
                total += noise;
            }

            BiomeWeight[] weighted = new BiomeWeight[biomes.Length];
            for (int i = 0; i < biomes.Length; i++)
            {
                weighted[i] = new BiomeWeight
                {
                    Biome = biomes[i],
                    Weight = rawWeights[i] / total
                };
            }

            return weighted;
        }
    }
}