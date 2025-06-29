using UnityEngine;
using Myth.World;
using Myth.World.Blocks;

namespace Myth.UI
{
    [System.Serializable]
    public readonly struct HotbarSlot
    {
        public readonly byte BlockID;
        public readonly int Quantity;

        public HotbarSlot(byte blockID, int quantity)
        {
            BlockID = blockID;
            Quantity = quantity;
        }

        public bool IsEmpty => BlockID == 0 || Quantity == 0;

        public string SetDisplayName(byte blockID) => IsEmpty ? "" : BlockRegistry.GetBlockName(blockID);
        public Sprite SetDisplayIcon(byte blockID) => IsEmpty ? null : BlockRegistry.GetHotbarIcon(blockID);
    } 
}