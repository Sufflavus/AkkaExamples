namespace ChartApp
{
    /// <summary>
    ///     Metric data at the time of sample
    /// </summary>
    public class Metric
    {
        public Metric(string series, float counterValue)
        {
            Series = series;
            CounterValue = counterValue;
        }

        public float CounterValue { get; set; }
        public string Series { get; set; }
    }
}