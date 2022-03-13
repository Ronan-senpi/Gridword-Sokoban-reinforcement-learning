using System;
using System.Collections.Generic;
using System.Linq;

namespace classes
{
    public class Stats
    {
        public List<TimeSpan> ExecutionTimes { get; private set; } = new List<TimeSpan>();

        public TimeSpan MinExecutionTime
        {
            get { return ExecutionTimes.Min(); }
        }

        public TimeSpan MaxExecutionTime
        {
            get { return ExecutionTimes.Max(); }
        }

        public TimeSpan AverageExecutionTime
        {
            get { return new TimeSpan(Convert.ToInt64(ExecutionTimes.Average(t => t.Ticks))); }
        }

        public int NbExecution
        {
            get { return ExecutionTimes.Count; }
        }

        public void AddTime(TimeSpan execTime)
        {
            ExecutionTimes.Add(execTime);
        }

        public async void Write()
        {
            List<string> lines = new List<string>();
            lines.Add(string.Join(" ", nameof(MaxExecutionTime), MaxExecutionTime));
            lines.Add(string.Join(" ", nameof(MinExecutionTime), MinExecutionTime));
            lines.Add(string.Join(" ", nameof(MaxExecutionTime), MaxExecutionTime));
            lines.Add(string.Join(" ", nameof(AverageExecutionTime), AverageExecutionTime));
            string allLine = string.Empty;
            foreach (var sp in ExecutionTimes)
            {
                allLine += sp.ToString() + ";";
            }

            lines.Add("AllExecutionTime");
            System.IO.File.WriteAllLines(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + ".txt", lines);
        }
    }
}