using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages.ChartingMessages
{
    /// <summary>
    ///     Remove an existing <see cref="Series" /> from the chart
    /// </summary>
    public class RemoveSeries
    {
        public RemoveSeries(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; private set; }
    }
}