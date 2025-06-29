using System;
using Myth.World.Blocks;
using UnityEngine;
using Unity.Mathematics;

namespace Myth.World
{
    /// <summary>
    /// Chunk.cs
    /// This is the Chunk Data
    /// All Positional data is in Chunk Space
    /// One per Chunk
    /// </summary>
    public class Chunk
    {
        public const int CHUNK_SIZE = 16;
        public const int CHUNK_Y_SIZE = 128;
        public bool IsDirty { get; private set; }
        public MeshFilter Filter;
        public MeshCollider Collider;
        public WorldPosition WorldPosition;
        public readonly byte[,,] Blocks = new byte[CHUNK_SIZE, CHUNK_Y_SIZE, CHUNK_SIZE];
        public MeshData MeshData;

        public Chunk(WorldPosition worldPosition)
        {
            // Clear Array and set it to all Air Blocks (id = 0)
            Array.Clear(Blocks, 0, Blocks.Length);
            WorldPosition = worldPosition;
        }

        /// <summary>
        /// Gets and returns a BlockID at a given position
        /// !!! ONLY call this from ChunkManager.cs !!!
        /// </summary>
        /// <param name="x">Block Position X</param>
        /// <param name="y">Block Position Y</param>
        /// <param name="z">Block Position Z</param>
        /// <returns>Returns the BlockID</returns>
        public byte GetBlock(int x, int y, int z)
        {
            // Check if the Block is out of range
            if (!InRange(x) || !InYRange(y) || !InRange(z))
                return ChunkManager.Instance.GetGlobalBlock(x, y, z);
            
            return Blocks[x, y, z];
        }

        /// <summary>
        /// Sets the BlockID at this location
        /// !!! ONLY call this from ChunkManager.cs !!!
        /// </summary>
        /// <param name="x">Block Position X</param>
        /// <param name="y">Block Position Y</param>
        /// <param name="z">Block Position Z</param>
        /// <param name="blockID">BlockID to Set</param>
        public void SetBlock(int x, int y, int z, byte blockID)
        {
            // Check if the Block is out of range
            if (!InRange(x) || !InYRange(y) ||  !InRange(z))
                throw new Exception("Block was out of Range. Make sure this is only called in the ChunkManager.");
            
            Blocks[x, y, z] = blockID;
        }

        /// <summary>
        /// Checks if the Dirty Flag is active
        /// If so, we Enqueue the Chunk Rebuild
        /// </summary>
        public void MarkDirty()
        {
            if (IsDirty) return;
            
            IsDirty = true;
            ChunkManager.Instance.EnqueueChunkUpdate(this);
        }

        /// <summary>
        /// Clears the Dirty Flag
        /// </summary>
        public void ClearDirty()
        {
            IsDirty = false;
        }

        /// <summary>
        /// Helper function to check if the Block is in Chunk Space
        /// If the Block is within Chunk Bounds, return true
        /// Otherwise return false
        /// </summary>
        /// <param name="index">The Block Positional Data (x or z)</param>
        /// <returns></returns>
        private static bool InRange(int index)
        {
            return index is >= 0 and < CHUNK_SIZE;
        }

        /// <summary>
        /// Helper function to check if the Block is in Chunk Space along the Y axis
        /// If the Block is within Chunk Bounds, return true
        /// Otherwise return false
        /// </summary>
        /// <param name="index">The Block Y Positional Data</param>
        /// <returns></returns>
        private static bool InYRange(int index)
        {
            return index is >= 0 and < CHUNK_Y_SIZE;
        }
    }
}