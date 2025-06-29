using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Myth.World.Blocks
{
    [CreateAssetMenu(fileName = "New Custom Block", menuName = "Myth/Custom Block")]
    public class CustomBlockScriptableObject : BlockScriptableObject
    {
        [NonSerialized] private MeshData _cachedMeshData;
        
        [Header("Custom Block")]
        [Tooltip("Custom Mesh in FBX format")] public GameObject customBlockPrefab;

        [Tooltip("VFX Graph Array")] public VisualEffectAsset customEffects;
        [Tooltip("Does this Custom Block have a Point Light?")] public bool hasPointLight;
        [Tooltip("The Light Color")] public Color lightColor = Color.white;
        [Tooltip("The Light Intensity")] public float lightIntensity = 1.0f;
        [Tooltip("The Light Range")] public float lightRange = 5.0f;

        /// <summary>
        /// Gets the Cached MeshData
        /// Generates the Cached Data the first time
        /// </summary>
        /// <returns></returns>
        public MeshData GetMeshData()
        {
            if (_cachedMeshData != null)
                return _cachedMeshData;
            
            Mesh mesh = customBlockPrefab.GetComponent<MeshFilter>().sharedMesh;

            MeshData meshData = new MeshData();
            meshData.Vertices.AddRange(mesh.vertices);
            meshData.Triangles.AddRange(mesh.triangles);
            meshData.Uvs.AddRange(mesh.uv);

            _cachedMeshData = meshData;
            return _cachedMeshData;
        }
    }
}