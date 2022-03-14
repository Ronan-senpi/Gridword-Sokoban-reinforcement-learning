using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace classes
{
    [System.Serializable]
    public class Stats
    {
        public Stats(int level)
        {
            this.level = level;
        }

        private int level;
        private List<TimeSpan> executionTimes = new List<TimeSpan>();

        public TimeSpan MinExecutionTime
        {
            get { return executionTimes.Min(); }
        }

        public TimeSpan MaxExecutionTime
        {
            get { return executionTimes.Max(); }
        }

        public TimeSpan AverageExecutionTime
        {
            get { return new TimeSpan(Convert.ToInt64(executionTimes.Average(t => t.Ticks))); }
        }

        public int NbExecution
        {
            get { return executionTimes.Count; }
        }

        public void AddTime(TimeSpan execTime)
        {
            executionTimes.Add(execTime);
        }
        
        public void Write()
        {
            List<string> lines = new List<string>();
            lines.Add("============== Start ==============");
            lines.Add(string.Join(" ", "level : ", level));
            lines.Add(string.Join(" ", nameof(MaxExecutionTime), MaxExecutionTime));
            lines.Add(string.Join(" ", nameof(MinExecutionTime), MinExecutionTime));
            lines.Add(string.Join(" ", nameof(MaxExecutionTime), MaxExecutionTime));
            lines.Add(string.Join(" ", nameof(AverageExecutionTime), AverageExecutionTime));
            string allLine = string.Empty;
            foreach (var sp in executionTimes)
            {
                allLine += sp.ToString() + ";";
            }
            lines.Add("AllExecutionTime : " + allLine);
            lines.Add("============== End ==============");

           // System.IO.File.WriteAllLines(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + ".txt", lines);
        }

        public void ToJson()
        {
            string json = JsonUtility.ToJson(this);
            string path = Application.persistentDataPath + "/" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm") + ".json";
            Debug.Log(path);
            System.IO.File.WriteAllText(path, json);
        }
    }
}