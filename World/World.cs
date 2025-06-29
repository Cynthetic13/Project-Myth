using System;
using Myth.Misc;
using UnityEngine;

namespace Myth.World
{
    public class World : Singleton<World>
    {
        [SerializeField] private bool generateChunks = true;
        public string worldName = "world";
        public int seed;
        public int renderDistance;
        
        private void Start()
        {
            if (seed == 0)
                seed = UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue);

            if (!generateChunks) return;
            
            for (int x = -renderDistance; x < renderDistance; x++)
            {
                for (int z = -renderDistance; z < renderDistance; z++)
                {
                    WorldPosition position = new(x * Chunk.CHUNK_SIZE, 0, z * Chunk.CHUNK_SIZE);
                    ChunkManager.Instance.CreateChunk(position);
                }
            }
        }
    }
}