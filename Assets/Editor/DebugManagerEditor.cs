using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIManager))]
class DebugManagerEditor : Editor {
    public override void OnInspectorGUI() {
    
        base.DrawDefaultInspector();
        
        if (GUILayout.Button("Compute AI"))
        {
            AIManager.Instance.AIStart();
        }
        
        if (GUILayout.Button("Play"))
        {
            AIManager.Instance.PlayActions();
        }

        if (GUILayout.Button("Show state value"))
        {
            AIManager.Instance.DisplayStateValue();
        }
                
    }
}