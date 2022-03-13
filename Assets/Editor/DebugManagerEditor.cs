using classes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIManager))]
class DebugManagerEditor : Editor {
    public override void OnInspectorGUI() {
        
        base.OnInspectorGUI();
        
        if (!Application.isPlaying)
        {
            GUILayout.Label("More options in play mode");
            return;
        }

        if (!GridManager.Instance.LevelLoaded)
        {
            GUILayout.Label("More options when a level is loaded");
            return;
        }
        AIManager ai = (AIManager) target;
        
        if (GUILayout.Button("Compute AI"))
        {
            AIManager.Instance.AIStart();
        }

        bool needEpisode = ai.ReinforcementType == ReinforcementType.McEsPolicyOff;
        if (needEpisode)
        {
            ai.NbEpisode = EditorGUILayout.IntSlider("Number of episodes", ai.NbEpisode, 1, 9999);
        }
        
        GUILayout.Label("(More options show up when compute is over)");
        if (ai.ComputeIsOver)
        {
            if (GUILayout.Button("Play"))
            {
                AIManager.Instance.PlayActions();
            }

            if (GUILayout.Button("Show state value"))
            {
                AIManager.Instance.DisplayStateValue();
            }

            if (GUILayout.Button("Show all Arrows"))
            {
                AIManager.Instance.DisplayAllArrows();
            }

            if (GUILayout.Button("Show critical path Arrows"))
            {
                AIManager.Instance.DisplayCriticPath();
            }
        }
    }
}