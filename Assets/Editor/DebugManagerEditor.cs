using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIManager))]
class DebugManagerEditor : Editor {
    public override void OnInspectorGUI() {
    
        
        base.OnInspectorGUI();
        
        AIManager ai = (AIManager) target;
        
        if (GUILayout.Button("Compute AI"))
        {
            AIManager.Instance.AIStart();
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