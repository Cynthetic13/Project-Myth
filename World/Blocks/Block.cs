using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Myth.World.Blocks
{
    public static class Block
    {
        private const float TILE_SIZE = 0.25f;
        private static byte _blockID;

        private static readonly Dictionary<FaceDirection, Vector3[]> blockVertices = new()
        {
            {
                FaceDirection.Up, new Vector3[]
                {
                    new(-0.5f, 0.5f, 0.5f),
                    new(0.5f, 0.5f, 0.5f),
                    new(0.5f, 0.5f, -0.5f),
                    new(-0.5f, 0.5f, -0.5f)
                }
            },
            {
                FaceDirection.Down, new Vector3[]
                {
                    new(-0.5f, -0.5f, -0.5f),
                    new(0.5f, -0.5f, -0.5f),
                    new(0.5f, -0.5f, 0.5f),
                    new(-0.5f, -0.5f, 0.5f)
                }
            },
            {
                FaceDirection.North, new Vector3[]
                {
                    new(0.5f, -0.5f, 0.5f),
                    new(0.5f, 0.5f, 0.5f),
                    new(-0.5f, 0.5f, 0.5f),
                    new(-0.5f, -0.5f, 0.5f)
                }
            },
            {
                FaceDirection.East, new Vector3[]
                {
                    new(0.5f, -0.5f, -0.5f),
                    new(0.5f, 0.5f, -0.5f),
                    new(0.5f, 0.5f, 0.5f),
                    new(0.5f, -0.5f, 0.5f)
                }
            },
            {
                FaceDirection.South, new Vector3[]
                {
                    new(-0.5f, -0.5f, -0.5f),
                    new(-0.5f, 0.5f, -0.5f),
                    new(0.5f, 0.5f, -0.5f),
                    new(0.5f, -0.5f, -0.5f)
                }
            },
            {
                FaceDirection.West, new Vector3[]
                {
                    new(-0.5f, -0.5f, 0.5f),
                    new(-0.5f, 0.5f, 0.5f),
                    new(-0.5f, 0.5f, -0.5f),
                    new(-0.5f, -0.5f, -0.5f)
                }
            }
        };

        public static MeshData AddCustomBlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            _blockID = chunk.GetBlock(x, y, z);

            meshData.UseRenderDataForCollision = BlockRegistry.IsSolid(_blockID);

            MeshData customMeshData = BlockRegistry.GetCustomBlockMeshData(_blockID);

            Vector3 position = new(x + chunk.WorldPosition.x, y + chunk.WorldPosition.y, z + chunk.WorldPosition.z);
            
            // VFX Graph
            if (GameObject.Find("VFX " + _blockID + " at " + position) == null)
            {
                var effect = BlockRegistry.GetCustomEffects(_blockID);
                var vfxGameObject = new GameObject("VFX  " + _blockID + " at " + position);
                vfxGameObject.transform.localPosition = new Vector3(position.x, position.y, position.z);

                var vfx = vfxGameObject.AddComponent<VisualEffect>();
                vfx.visualEffectAsset = effect;
                vfx.Play();
            }
            
            // Point Light
            if (GameObject.Find("Light " + _blockID + " at " + position) == null)
            {
                PointLightData lightData = BlockRegistry.GetPointLightData(_blockID);
                var lightObject = new GameObject("Light " + _blockID + " at " + position);
                lightObject.transform.localPosition = new Vector3(position.x, position.y + 0.5f, position.z);
            
                var light = lightObject.AddComponent<Light>();
                light.type = LightType.Point;
                light.color = lightData.Color;
                light.range = lightData.Range;
                light.intensity = lightData.Intensity;
            }

            int vertexOffset = meshData.Vertices.Count;

            foreach (var vertex in customMeshData.Vertices)
                meshData.Vertices.Add(new Vector3(vertex.x + x, vertex.y + y, vertex.z + z));

            foreach (var triangle in customMeshData.Triangles)
                meshData.Triangles.Add(triangle + vertexOffset);

            meshData.Uvs.AddRange(customMeshData.Uvs);
            
            return meshData;
        }
        
        public static MeshData AddBlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            _blockID = chunk.GetBlock(x, y, z);
            
            if (BlockRegistry.IsAir(_blockID))
                return meshData;

            if (BlockRegistry.IsCustomBlock(_blockID))
                return AddCustomBlockData(chunk, x, y, z, meshData);
            
            meshData.UseRenderDataForCollision = BlockRegistry.IsSolid(_blockID);
            
            if (ShouldRenderFace(chunk, x, y + 1, z)) // Up
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.Up);

            if (ShouldRenderFace(chunk, x, y - 1, z)) // Down
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.Down);
            
            if(ShouldRenderFace(chunk, x, y, z + 1)) // North
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.North);

            if (ShouldRenderFace(chunk, x, y, z - 1)) // South
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.South);

            if (ShouldRenderFace(chunk, x + 1, y, z)) // East
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.East);
            
            if (ShouldRenderFace(chunk, x - 1, y, z)) // West
                meshData = AddFaceData(x, y, z, meshData, FaceDirection.West);
            
            return meshData;
        }

        private static bool ShouldRenderFace(Chunk chunk, int x, int y, int z)
        {
            byte neighborID = GetBlockSafe(chunk, x, y, z);
            return !BlockRegistry.IsSolid(neighborID);
        }

        private static MeshData AddFaceData(int x, int y, int z, MeshData meshData,
            FaceDirection direction)
        {
            foreach(var offset in blockVertices[direction])
                meshData.AddVertex(new Vector3(x, y, z) + offset);
            
            meshData.AddQuadTriangles();
            meshData.Uvs.AddRange(FaceUVs(direction));

            return meshData;
        }

        private static byte GetBlockSafe(Chunk chunk, int x, int y, int z)
        {
            if (x < 0 || x >= Chunk.CHUNK_SIZE ||
                y < 0 || y >= Chunk.CHUNK_Y_SIZE ||
                z < 0 || z >= Chunk.CHUNK_SIZE)
            {
                return 0; // Air
            }

            return chunk.GetBlock(x, y, z);
        }
        
        private static Tile TexturePosition(FaceDirection direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case FaceDirection.Up:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[0].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[0].y;
                    return tile;
                case FaceDirection.Down:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[1].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[1].y;
                    return tile;
                case FaceDirection.North:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[2].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[2].y;
                    return tile;
                case FaceDirection.East:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[3].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[3].y;
                    return tile;
                case FaceDirection.South:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[4].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[4].y;
                    return tile;
                case FaceDirection.West:
                    tile.X = BlockRegistry.GetTextureOffset(_blockID)[5].x;
                    tile.Y = BlockRegistry.GetTextureOffset(_blockID)[5].y;
                    return tile;
            }

            return tile;
        }
        
        private static Vector2[] FaceUVs(FaceDirection direction)
        {
            Vector2[] uvs = new Vector2[4];
            Tile tilePos = TexturePosition(direction);

            uvs[0] = new Vector2(TILE_SIZE * tilePos.X + TILE_SIZE,
                TILE_SIZE * tilePos.Y);
            uvs[1] = new Vector2(TILE_SIZE * tilePos.X + TILE_SIZE,
                TILE_SIZE * tilePos.Y + TILE_SIZE);
            uvs[2] = new Vector2(TILE_SIZE * tilePos.X,
                TILE_SIZE * tilePos.Y + TILE_SIZE);
            uvs[3] = new Vector2(TILE_SIZE * tilePos.X,
                TILE_SIZE * tilePos.Y);

            return uvs;
        }
        
        private struct Tile
        {
            public int X;
            public int Y;
        }
    }
}