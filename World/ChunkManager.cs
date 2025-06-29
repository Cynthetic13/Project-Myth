using System.Collections.Generic;
using Myth.Misc;
using Myth.World.Blocks;
using UnityEngine;

namespace Myth.World
{
    // TODO... Remove the Creating and Rendering of Chunks to "ChunkRenderer.cs"
    /// <summary>
    /// ChunkManager.cs
    /// This act as layer between the Terrain and the Chunks
    /// All Positional Data is in World Space
    /// </summary>
    public class ChunkManager : Singleton<ChunkManager>
    {
        private const int MAX_UPDATES_PER_FRAME = 1;
        
        public Material material;
        
        private readonly Dictionary<WorldPosition, Chunk> _chunks = new();
        private readonly Dictionary<Collider, Chunk> _colliders = new();
        private readonly Queue<Chunk> _chunkUpdateQueue = new();
        private readonly TerrainGen _terrainGen = new();

        private void Update()
        {
            int updates = 0;
            while (_chunkUpdateQueue.Count > 0 && updates < MAX_UPDATES_PER_FRAME)
            {
                Chunk chunk = _chunkUpdateQueue.Dequeue();
                UpdateChunk(chunk);
                chunk.ClearDirty();
                updates++;
            }
        }

        public void EnqueueChunkUpdate(Chunk chunk)
        {
            if (!_chunkUpdateQueue.Contains(chunk))
                _chunkUpdateQueue.Enqueue(chunk);
        }
        
        public void CreateChunk(WorldPosition position)
        {
            // Check if the Chunk already exists
            if (_chunks.ContainsKey(position)) return;
            
            // Create a new Chunk and Add it to the List
            Chunk chunk = new Chunk(position);
            _chunks.Add(position, chunk);
            
            // Generate the Chunk Data
            _terrainGen.ChunkGen(chunk);
            
            // Pass Data
            InitializeChunk(chunk);
        }

        private Chunk GetChunk(int x, int y, int z)
        {
            WorldPosition position = new WorldPosition();
            float multiple = Chunk.CHUNK_SIZE;
            float multipleY = Chunk.CHUNK_Y_SIZE;
            position.x = Mathf.FloorToInt(x / multiple) * Chunk.CHUNK_SIZE;
            position.y = Mathf.FloorToInt(y / multipleY) * Chunk.CHUNK_Y_SIZE;
            position.z = Mathf.FloorToInt(z / multiple) * Chunk.CHUNK_SIZE;

            _chunks.TryGetValue(position, out Chunk containerChunk);

            return containerChunk;
        }

        private void InitializeChunk(Chunk chunk)
        {
            // Create a Vector3 for the Chunk Position
            Vector3 chunkPosition = new Vector3(
                chunk.WorldPosition.x,
                chunk.WorldPosition.y,
                chunk.WorldPosition.z
            );
            
            // Create a new GameObject and assign its name, position, and parent
            GameObject newObject = new GameObject($"Chunk_{chunk.WorldPosition.x}_{chunk.WorldPosition.z}");
            newObject.transform.position = chunkPosition;
            newObject.transform.SetParent(transform);

            // Add Mesh Components
            MeshFilter filter = newObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
            MeshCollider meshCollider = newObject.AddComponent<MeshCollider>();
            
            // Cache the Components in the Chunk
            chunk.Filter = filter;
            chunk.Collider = meshCollider;
            
            // Cache Collider
            _colliders[meshCollider] = chunk;
            
            // Set Material
            meshRenderer.sharedMaterial = material;

            // Render the Chunk TODO... Put this logic in a RenderChunk.cs file
            RenderChunk(chunk);
        }
        
        private void UpdateChunk(Chunk chunk)
        {
            if (chunk.IsDirty)
            {
                RenderChunk(chunk);
                chunk.ClearDirty();
            }
        }

        private void RenderChunk(Chunk chunk)
        {
            // Generate and set MeshData
            MeshData meshData = new MeshData();
            
            for(int x = 0; x < Chunk.CHUNK_SIZE; x++)
            for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
            {
                for (int y = 0; y < Chunk.CHUNK_Y_SIZE; y++)
                {
                    meshData = Block.AddBlockData(chunk, x, y, z, meshData);
                }
            }

            chunk.MeshData = meshData;
            
            // Get Cached Components
            MeshFilter filter = chunk.Filter;
            MeshCollider chunkCollider = chunk.Collider;
            
            // Clear meshes
            filter.mesh.Clear();
            chunkCollider.sharedMesh = null;

            // Create a new Mesh and assign it to the Chunk
            filter.mesh.vertices = chunk.MeshData.Vertices.ToArray();
            filter.mesh.triangles = chunk.MeshData.Triangles.ToArray();
            filter.mesh.uv = chunk.MeshData.Uvs.ToArray();
            
            filter.mesh.RecalculateNormals();

            // Create a new Collision Mesh and assign it to the Chunk
            Mesh collisionMesh = new Mesh()
            {
                vertices = chunk.MeshData.CollisionVertices.ToArray(),
                triangles = chunk.MeshData.CollisionTriangles.ToArray()
            };
            chunkCollider.sharedMesh = collisionMesh;
            chunkCollider.sharedMesh.RecalculateNormals();
        }

        /// <summary>
        /// Get the BlockID in World Space
        /// </summary>
        /// <param name="x">Block Position X</param>
        /// <param name="y">Block Position Y</param>
        /// <param name="z">Block Position Z</param>
        /// <returns>Returns the BlockID</returns>
        public byte GetBlock(int x, int y, int z)
        {
            Chunk containerChunk = GetChunk(x, y, z);

            if (containerChunk != null)
            {
                byte blockID = containerChunk.GetBlock(
                    x - containerChunk.WorldPosition.x,
                    y - containerChunk.WorldPosition.y,
                    z - containerChunk.WorldPosition.z);

                return blockID;
            }

            return 0;
        }

        public byte GetGlobalBlock(int x, int y, int z)
        {
            Chunk neighborChunk = GetChunk(x, y, z);

            if (neighborChunk != null)
            {
                byte blockID = neighborChunk.GetBlock(
                    x - neighborChunk.WorldPosition.x,
                    y - neighborChunk.WorldPosition.y,
                    z - neighborChunk.WorldPosition.z);

                return blockID;
            }

            return 0;
        }

        public void SetBlock(int x, int y, int z, byte blockID, bool markDirty = false)
        {
            Chunk chunk = GetChunk(x, y, z);
            
            if(chunk == null) return;
            
            // Get Chunk Space Coords
            int localX = x - chunk.WorldPosition.x;
            int localY = y - chunk.WorldPosition.y;
            int localZ = z - chunk.WorldPosition.z;
            
            // Set the Block
            chunk.SetBlock(localX, localY, localZ, blockID);
            
            // Set this Chunk to Dirty
            if (markDirty)
            {
                chunk.MarkDirty();
                MarkNeighborChunksDirty(x, y, z);
            }
            
            // Set new block tickable if it's tickable
            //if(chunk.Blocks[localX, localY, localZ].isTickable)
                //TickManager.Instance.RegisterTickable(chunk.Blocks[localX, localY, localZ]);
        }

        public Chunk GetChunkFromCollider(Collider meshCollider)
        {
            _colliders.TryGetValue(meshCollider, out Chunk chunk);
            return chunk;
        }

        private void MarkNeighborChunksDirty(int x, int y, int z)
        {
            foreach (var offset in new[]
                     {
                         new Vector3Int(-1, 0, 0), new Vector3Int(1, 0, 0),
                         new Vector3Int(0, -1, 0), new Vector3Int(0, 1, 0),
                         new Vector3Int(0, 0, -1), new Vector3Int(0, 0, 1)
                     })
            {
                var neighborChunk = GetChunk(x + offset.x, y + offset.y, z + offset.z);
                if(neighborChunk != null)
                    neighborChunk.MarkDirty();
            }
        }
    }
}