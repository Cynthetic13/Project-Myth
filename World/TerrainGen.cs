using Myth.World.Biomes;
using UnityEngine;
using SimplexNoise;
using Myth.World.Blocks;

namespace Myth.World
{
    public class TerrainGen
    {
        private const float STONE_BASE_HEIGHT = 44f;
        private const float STONE_BASE_NOISE = 0.05f;
        private const float STONE_BASE_NOISE_HEIGHT = 4f;

        private const float STONE_MOUNTAIN_HEIGHT = 8f;
        private const float STONE_MOUNTAIN_FREQUENCY = 0.0008f;
        private const float STONE_MIN_HEIGHT = -12f;

        private const float DIRT_BASE_HEIGHT = 4f;
        private const float DIRT_NOISE = 0.04f;
        private const float DIRT_NOISE_HEIGHT = 3f;
        
        private IBiome _biome = new PeacefulGroveBiome();
        
        public void ChunkGen(Chunk chunk)
        {
            Noise.Seed = World.Instance.seed;
            for (int x = chunk.WorldPosition.x; x < chunk.WorldPosition.x + Chunk.CHUNK_SIZE; x++)
            {
                for (int z = chunk.WorldPosition.z; z < chunk.WorldPosition.z + Chunk.CHUNK_SIZE; z++)
                {
                    ChunkColumnGen(chunk, x, z);
                }
            }
        }

        private void ChunkColumnGen(Chunk chunk, int x, int z)
        {
            int localX = x - chunk.WorldPosition.x;
            int localZ = z - chunk.WorldPosition.z;
            
            float stoneBase = _biome.GetStoneBaseHeight(x, z);
            float stoneMountain = _biome.GetStoneMountainHeight(x, z);
            float stoneMin = _biome.GetStoneMinHeight();
            float dirtBase = _biome.GetDirtBaseHeight(x, z);
            float dirtNoise = _biome.GetDirtNoise(x, z);
            
            int stoneHeight = Mathf.FloorToInt(stoneBase + stoneMountain);
            if (stoneHeight < stoneMin)
                stoneHeight = Mathf.FloorToInt(stoneMin);

            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBase + dirtNoise);
            
            bool topBlockPlaced = false;
            for (int y = Chunk.CHUNK_Y_SIZE; y >= 0; y--)
            {
                if (y <= stoneHeight)
                    chunk.Blocks[localX, y, localZ] = 3; // Stone
                else if (y <= dirtHeight)
                {
                    if (!topBlockPlaced)
                    {
                        chunk.Blocks[localX, y, localZ] = _biome.GetTopBlock(); // Biome Top Block
                        topBlockPlaced = true;
                    }
                    else
                    {
                        chunk.Blocks[localX, y, localZ] = _biome.GetUnderBlock(); // Dirt
                    }
                }
            }
            
            // Set Grass Blocks
            for (int y = 0; y < Chunk.CHUNK_Y_SIZE; y++)
            {
                if (y > dirtHeight) continue;
                
                // Get Block Above This
                var aboveBlock = ChunkManager.Instance.GetBlock(x + chunk.WorldPosition.x, y + 1, z + chunk.WorldPosition.z);
                
                if (!BlockRegistry.IsSolid(aboveBlock))
                {
                    ChunkManager.Instance.SetBlock(x + chunk.WorldPosition.x, y + 1, z + chunk.WorldPosition.z, _biome.GetTopBlock());
                }
            }
        }

        private static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((Noise.CalcPixel2D(x, z, scale) + 1.0f) * (max / 2.0f));
        }
    }
}