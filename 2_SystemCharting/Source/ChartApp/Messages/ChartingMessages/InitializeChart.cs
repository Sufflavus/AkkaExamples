using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages.ChartingMessages
{
    public class InitializeChart
    {
        public InitializeChart(Dictionary<string, Series> initialSeries)
        {
            InitialSeries = initialSeries;
        }

        public Dictionary<string, Series> InitialSeries { get; private set; }
    }
}