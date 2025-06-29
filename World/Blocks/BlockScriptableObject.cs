using UnityEngine;

namespace Myth.World.Blocks
{
    [CreateAssetMenu(fileName = "New Block", menuName = "Myth/Block")]
    public class BlockScriptableObject : ScriptableObject
    {
        [Header("Base Block")]
        public byte id;
        public string blockName = "ERROR";
        public Sprite hotbarIcon;
        public Vector2Int[] textureOffset = new Vector2Int[6]; // Top, Bottom, North, East, South, West
        public bool isSolid;
    }
}