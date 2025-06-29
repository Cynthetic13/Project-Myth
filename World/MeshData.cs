using UnityEngine;
using System.Collections.Generic;

namespace Myth.World
{
    /// <summary>
    /// MeshData for the Chunk
    /// One per Chunk
    /// </summary>
    public class MeshData
    {
        // Mesh Data
        public readonly List<Vector3> Vertices = new();
        public readonly List<int> Triangles = new();
        public readonly List<Vector2> Uvs = new();
        
        // Collision Data (for Solid Blocks)
        public readonly List<Vector3> CollisionVertices = new();
        public readonly List<int> CollisionTriangles = new();

        public bool UseRenderDataForCollision;

        public void AddVertex(Vector3 vertex)
        {
            Vertices.Add(vertex);

            if (!UseRenderDataForCollision) return;
            
            CollisionVertices.Add(vertex);
        }

        public void AddQuadTriangles()
        {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);
            
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);

            if (!UseRenderDataForCollision) return;
            
            CollisionTriangles.Add(CollisionVertices.Count - 4);
            CollisionTriangles.Add(CollisionVertices.Count - 3);
            CollisionTriangles.Add(CollisionVertices.Count - 2);
                
            CollisionTriangles.Add(CollisionVertices.Count - 4);
            CollisionTriangles.Add(CollisionVertices.Count - 2);
            CollisionTriangles.Add(CollisionVertices.Count - 1);
        }
    }
    
    public enum FaceDirection
    {
        North,
        East,
        South,
        West,
        Up,
        Down
    }
}