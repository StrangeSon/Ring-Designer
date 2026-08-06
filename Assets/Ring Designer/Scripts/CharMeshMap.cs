using System.Collections.Generic;
using UnityEngine;

namespace RingDesigner
{
    [System.Serializable]
    public struct CharMeshMapping
    {
        public char character;
        public GameObject meshPrefab;
    }

    [CreateAssetMenu(menuName = "Ring Designer/Character Mesh Map")]
    public class CharMeshMap : ScriptableObject
    {
        public List<CharMeshMapping> CharMeshMappings = new();
    }
}