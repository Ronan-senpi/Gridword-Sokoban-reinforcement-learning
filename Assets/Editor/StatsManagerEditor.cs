    using System.Collections.Generic;
    using classes;
    using log4net.Core;
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

            if (GUILayout.Button(" Stat all levels"))
            {
                List<Stats> s = new List<Stats>();
                for (int i = 1; i <= LevelManager.Instance.Levels.Count; i++)
                {
                    GridManager.Instance.LoadMap();
                    
                    Stats.ToJson(sm.GenerateStats(i),"level" + i.ToString());
                }
                
                GUILayout.Label("Files savec at : " + Application.persistentDataPath+ "/<MM-dd-yyyy-HH-mm>.json");
            }
            
            if (!GridManager.Instance.LevelLoaded)
            {
                GUILayout.Label("More options when a level is loaded");
                return;
            }
            
            if (GUILayout.Button(" Stat load level"))
            {
                
                int idx = LevelManager.Instance.GetLevelIndex();
                Stats.ToJson(sm.GenerateStats(idx));
                
                GUILayout.Label("Files savec at : " + Application.persistentDataPath+ "/<MM-dd-yyyy-HH-mm>.json");
            }
            
        }
    }
