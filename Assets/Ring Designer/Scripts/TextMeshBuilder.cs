using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RingDesigner
{
    // Internal cache structure
    public class CharMeshData
    {
        public GameObject prefab;
        public float width;
        public float height;

        public CharMeshData(GameObject prefab, float width, float height)
        {
            this.prefab = prefab;
            this.width = width;
            this.height = height;
        }
    }

    public class TextMeshBuilder : MonoBehaviour
    {
        [Header("Text Mesh Renderer")]
        [SerializeField] private MeshFilter combinedMeshFilter;
        [SerializeField] private MeshRenderer combinedMeshRenderer;
        [SerializeField] private Material textMaterial;
        [SerializeField] private CircleSplineCreator circleSplineCreator;

        [Header("Character Mappings")]
        [SerializeField] private CharMeshMap CharMeshMap;

        [Header("Settings")]
        [SerializeField] private Transform textRoot;
        [SerializeField] private float characterSpacing = 0.1f;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private string bookendChar = "";
        [SerializeField] private float bookendSpacing = 0.1f;

        private Dictionary<char, CharMeshData> charToMesh = new();

        private string lastText = "";


        private void Awake()
        {
            foreach (var mapping in CharMeshMap.CharMeshMappings)
            {
                if (!charToMesh.ContainsKey(mapping.character))
                {
                    CalculatePrefabDimensions(mapping.meshPrefab, out float width, out float height);
                    charToMesh.Add(mapping.character, new CharMeshData(mapping.meshPrefab, width, height));
                }
            }
            inputField.onValueChanged.AddListener(OnTextChanged);
        }

        private void Start()
        {
            OnTextChanged(inputField.text);
        }

        private void CalculatePrefabDimensions(GameObject prefab, out float width, out float height)
        {
            var instance = Instantiate(prefab);
            instance.SetActive(false); // don't render
            var renderers = instance.GetComponentsInChildren<Renderer>();

            Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();
            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            Destroy(instance);

            width = bounds.size.x;
            height = bounds.size.y;
        }

        private void OnTextChanged(string newText)
        {
            if (newText == lastText)
                return;

            RebuildText(newText, bookendChar);

            lastText = newText;
        }

        private void RebuildText(string text, string bookendCharacter = null)
        {
            if (!string.IsNullOrEmpty(bookendCharacter))
                text = $"{bookendCharacter}{text}{bookendCharacter}";

            Vector3 cursor = Vector3.zero;

            List<CombineInstance> combineInstances = new();

            for (int i = 0; i < text.Length; i++)
            {
                bool isFirst = i == 0;
                bool nextIsLast = i == text.Length - 2;
                bool isLast = i == text.Length;

                char c = text[i];

                var uppercase = char.ToUpper(c);
                if (charToMesh.TryGetValue(uppercase, out var data))
                {
                    var instance = Instantiate(data.prefab, textRoot);
                    instance.transform.localPosition = cursor;

                    // Get MeshFilter
                    var meshFilter = instance.GetComponentInChildren<MeshFilter>();
                    if (meshFilter != null)
                    {
                        CombineInstance combine = new CombineInstance
                        {
                            mesh = meshFilter.sharedMesh,
                            transform = meshFilter.transform.localToWorldMatrix
                        };
                        combineInstances.Add(combine);
                    }
                    Destroy(instance);
                    if (!string.IsNullOrEmpty(bookendCharacter) && (isFirst || nextIsLast))
                        cursor += new Vector3(-(data.width + characterSpacing + bookendSpacing), 0f, 0f);
                    else
                        cursor += new Vector3(-(data.width + characterSpacing), 0f, 0f);
                }
            }

            // Combine
            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

            circleSplineCreator.Update3DText(combinedMesh);
        }



    }
}