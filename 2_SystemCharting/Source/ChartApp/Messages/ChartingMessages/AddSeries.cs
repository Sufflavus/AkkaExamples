using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages.ChartingMessages
{
    /// <summary>
    ///     Add a new <see cref="Series" /> to the chart
    /// </summary>
    public class AddSeries
    {
        public AddSeries(Series series)
        {
            Series = series;
        }

        public Series Series { get; set; }
    }
}