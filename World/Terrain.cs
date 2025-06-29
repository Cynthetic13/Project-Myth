using UnityEngine;
using Myth.World.Blocks;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Myth.World
{
    public static class Terrain
    {
        public static bool SetBlock(RaycastHit hit, byte blockID, bool adjacent = false)
        {
            Chunk chunk = ChunkManager.Instance.GetChunkFromCollider(hit.collider);

            if (chunk == null)
                return false;

            WorldPosition position = GetBlockPosition(hit, adjacent);

            ChunkManager.Instance.SetBlock(position.x, position.y, position.z, blockID, true);

            return true;
        }

        public static byte GetBlock(RaycastHit hit, bool adjacent = false)
        {
            Chunk chunk = ChunkManager.Instance.GetChunkFromCollider(hit.collider);
            
            if (chunk == null)
                return 0;

            WorldPosition position = GetBlockPosition(hit, adjacent);

            return ChunkManager.Instance.GetBlock(position.x, position.y, position.z);
        }

        public static Vector3 GetBlockWorldPosition(RaycastHit hit)
        {
            Chunk chunk = ChunkManager.Instance.GetChunkFromCollider(hit.collider);

            if (chunk == null)
                return Vector3.zero;

            WorldPosition position = GetBlockPosition(hit);
            
            return new Vector3(position.x, position.y, position.z);
        }
        
        /* TODO... Remove old function
        private static WorldPosition GetBlockPosition(Vector3 position)
        {
            WorldPosition blockPosition = new(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                Mathf.RoundToInt(position.z));

            return blockPosition;
        }*/

        private static WorldPosition GetBlockPosition(RaycastHit hit, bool adjacent = false)
        {
            Vector3 point = hit.point;
            Vector3Int normal = new Vector3Int(
                Mathf.RoundToInt(hit.normal.x),
                Mathf.RoundToInt(hit.normal.y),
                Mathf.RoundToInt(hit.normal.z));
            Vector3Int position = Vector3Int.FloorToInt(point);

            if (IsNearWhole(point.x))
                position.x -= normal.x > 0 ? 1 : 0;

            if (IsNearWhole(point.y))
                position.y -= normal.y > 0 ? 1 : 0;

            if (IsNearWhole(point.z))
                position.z -= normal.z > 0 ? 1 : 0;

            if (adjacent)
                position += normal;
            
            return new WorldPosition(position.x, position.y, position.z);
        }

        private static bool IsNearWhole(float value, float threshold = 0.001f)
        {
            float frac = Mathf.Abs(value % 1f);
            return frac < threshold || frac > 1f - threshold;
        }

        /* TODO... Remove old Function
        private static float MoveWithinBlock(float position, float normal, bool adjacent = false)
        {
            if (position - (int)position == 0.5f || position - (int)position == -0.5f)
            {
                if (adjacent)
                    position += (normal / 2);
                else
                    position -= (normal / 2);
            }

            return position;
        }*/
    }
}