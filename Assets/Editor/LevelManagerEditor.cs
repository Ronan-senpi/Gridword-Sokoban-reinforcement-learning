    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                GUILayout.Label("More option in play mode");
                return;
            }
            if (GUILayout.Button("Laod level"))
            {
                GridManager.Instance.LoadMap();
            }
        }
    }
