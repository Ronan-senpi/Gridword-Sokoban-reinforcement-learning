    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(StatsManager))]
    public class StatsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("More option in play mode");
                return;
            }
            if (!GridManager.Instance.LevelLoaded)
            {
                GUILayout.Label("More options when a level is loaded");
                return;
            }
            StatsManager sm = (StatsManager) target;
            base.OnInspectorGUI();
            if (GUILayout.Button("Calculate Value Iteration stats Stats"))
            {
                sm.GenerateStats();
            }
            GUILayout.Label("Files savec at : " + Application.persistentDataPath+ "/<MM-dd-yyyy-HH-mm>.json");
        }
    }
