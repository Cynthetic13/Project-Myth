using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Mathematics;
using Myth.World.Blocks;

namespace Myth.World
{
    public static class ChunkIO
    {
        private static readonly ConcurrentQueue<(ChunkColumnSaveData data, string path, int seed)> SaveQueue = new();
        private static bool _isSaving;

        public static void QueueChunkSaveData(List<ChunkSaveData> data, string path, int x, int z, int seed)
        {
            var columnData = new ChunkColumnSaveData(new int2(x, z), data);
            SaveQueue.Enqueue((columnData, path, seed));

            if (!_isSaving)
            {
                _isSaving = true;
                _ = ProcessSaveQueueAsync();
            }
        }

        private static async Task ProcessSaveQueueAsync()
        {
            while (SaveQueue.TryDequeue(out var item))
            {
                await SaveChunkAsync(item.data, item.path, item.seed);
            }

            _isSaving = false;
        }

        private static async Task SaveChunkAsync(ChunkColumnSaveData data, string path, int seed)
        {
            string filePath = Path.Combine(path, "chunk_" + data.PositionXZ.x + "_" + data.PositionXZ.y + ".bin");

            Directory.CreateDirectory(path);

            await using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await using BinaryWriter writer = new BinaryWriter(fs);
            
            writer.Write(seed);
            writer.Write(data.PositionXZ.x);
            writer.Write(data.PositionXZ.y);
            
            writer.Write(data.ChunksY.Count);

            foreach (var chunk in data.ChunksY)
            {
                writer.Write(chunk.Position.y);
                writer.Write(chunk.BlockIDs.Length);
                writer.Write(chunk.BlockIDs);
            }

            await fs.FlushAsync();
        }
        
        public static Chunk LoadChunk(string path, WorldPosition worldPosition)
        {
            if (!File.Exists(path)) return null;
            
            Chunk chunk = new Chunk(worldPosition);

            using BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
            
            // Read Seed
            Debug.Log("Seed: " + reader.ReadInt32());
            
            // Read Chunk Location
            chunk.WorldPosition.x = reader.ReadInt32();
            chunk.WorldPosition.y = reader.ReadInt32();
            chunk.WorldPosition.z = reader.ReadInt32();
            
            // Read Block Data
            for (int x = 0; x < 32; x++)
            for (int y = 0; y < 32; y++)
            for (int z = 0; z < 32; z++)
            {
                //chunk.Blocks[x, y, z] = BlockRegistry.GetBlock(reader.ReadByte());
            }

            return chunk;
        }
        
        public struct ChunkSaveData
        {
            public readonly int3 Position;
            public readonly byte[] BlockIDs;

            public ChunkSaveData(int3 position, byte[] blockIDs)
            {
                Position = position;
                BlockIDs = blockIDs;
            }
        }

        private struct ChunkColumnSaveData
        {
            public readonly int2 PositionXZ;
            public readonly List<ChunkSaveData> ChunksY;

            public ChunkColumnSaveData(int2 positionXZ, List<ChunkSaveData> chunksY)
            {
                PositionXZ = positionXZ;
                ChunksY = chunksY;
            }
        }
    }
}