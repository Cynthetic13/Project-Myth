using System;

namespace Myth.World
{
    /// <summary>
    /// Helper function for World Positional Data for Chunks
    /// </summary>
    [Serializable]
    public struct WorldPosition : IEquatable<WorldPosition>
    {
        public int x, y, z;

        public WorldPosition(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && GetHashCode() == obj.GetHashCode())
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;

                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();

                return hash;
            }
        }

        public bool Equals(WorldPosition other)
        {
            return x == other.x && y == other.y && z == other.z;
        }
    }
}