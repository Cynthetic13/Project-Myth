using System;
using Myth.World.Blocks;
using UnityEngine;
using UnityEngine.VFX;

namespace Myth.World
{
    public static class BlockRegistry
    {
        private static readonly BlockScriptableObject[] Registry = new BlockScriptableObject[256];
        
        public static void Initialize(BlockScriptableObject[] blocks)
        {
            Array.Clear(Registry, 0, Registry.Length);

            foreach (var block in blocks)
                Registry[block.id] = block;
            
            Debug.Log($"[BlockRegistry] Registered {blocks.Length} block types.");
        }

        /// <summary>
        /// Gets the BlockID from the Registry array and returns the ID
        /// </summary>
        /// <param name="id">The Block ID</param>
        /// <returns></returns>
        public static BlockScriptableObject GetBlock(byte id)
        {
            return Registry[id];
        }

        public static string GetBlockName(byte id)
        {
            return Registry[id].blockName;
        }

        public static Sprite GetHotbarIcon(byte id)
        {
            return Registry[id].hotbarIcon;
        }

        /// <summary>
        /// Gets the BlockID from the Registry and returns if the block is Solid
        /// </summary>
        /// <param name="id">The Block ID</param>
        /// <returns></returns>
        public static bool IsSolid(byte id)
        {
            var block = Registry[id];
            return block != null && block.isSolid;
        }

        /// <summary>
        /// Check if the Block is Air
        /// </summary>
        /// <param name="id">The Block ID</param>
        /// <returns></returns>
        public static bool IsAir(byte id)
        {
            return id == 0 || Registry[id] == null;
        }

        public static Vector2Int[] GetTextureOffset(byte id)
        {
            var block = Registry[id];
            return block.textureOffset;
        }

        public static MeshData GetCustomBlockMeshData(byte id)
        {
            if (Registry[id] is CustomBlockScriptableObject customBlock)
            {
                return customBlock.GetMeshData();
            }

            throw new Exception($"BlockID:{id} is not a custom block type!");
        }

        public static bool IsCustomBlock(byte id)
        {
            return Registry[id] is CustomBlockScriptableObject;
        }

        public static VisualEffectAsset GetCustomEffects(byte id)
        {
            if (Registry[id] is not CustomBlockScriptableObject customBlock)
                throw new Exception($"BlockID: {id} is not a custom block type!");
            
            return customBlock.customEffects;
        }

        public static PointLightData GetPointLightData(byte id)
        {
            if (Registry[id] is not CustomBlockScriptableObject customBlock)
                throw new Exception($"BlockID: {id} is not a custom block type!");

            PointLightData lightData = new PointLightData();
            lightData.Color = customBlock.lightColor;
            lightData.Intensity = customBlock.lightIntensity;
            lightData.Range = customBlock.lightRange;
            return lightData;
        }
    }
    
    public struct PointLightData
    {
        public Color Color;
        public float Intensity;
        public float Range;
    }
}