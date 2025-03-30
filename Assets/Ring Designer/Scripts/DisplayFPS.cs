using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TownOfTrials
{
    public sealed class DisplayFPS : MonoBehaviour
    {
        private float timer;
        private bool update;
        private float deltaTime = 0.0f;

        private GUIStyle style;

        private void Awake()
        {
            // Initialize GUI style
            style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.black;
            style.fontSize = 20;
        }

        private void OnEnable()
        {
            update = true;
        }

        private void OnDisable()
        {
            update = false;
        }

        private void Update()
        {
            if (!update)
                return;

            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if (!update)
                return;

            float fps = 1.0f / deltaTime;
            // Display text at top-left corner (10, 10 are pixel offsets from top-left)
            GUI.Label(new Rect(10, 10, 100, 20), Mathf.Round(fps).ToString(), style);
        }
    }
}



