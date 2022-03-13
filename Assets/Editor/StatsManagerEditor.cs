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
            
            StatsManager sm = (StatsManager) target;
            base.OnInspectorGUI();
            if (GUILayout.Button("Calculate Value Iteration stats Stats"))
            {
               
            }
            
        }
    }
