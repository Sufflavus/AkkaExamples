using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Akka.Actor;
using ChartApp.Messages;
using ChartApp.Messages.ChartingMessages;

namespace ChartApp.Actors
{
    public class ChartingActor : ReceiveActor, IWithUnboundedStash
    {
        /// <summary>
        ///     Maximum number of points we will allow in a series
        /// </summary>
        public const int MaxPoints = 250;

        private readonly Chart _chart;
        private readonly Button _pauseButton;
        private Dictionary<string, Series> _seriesIndex;

        /// <summary>
        ///     Incrementing counter we use to plot along the X-axis
        /// </summary>
        private int xPosCounter;

        public ChartingActor(Chart chart, Button pauseButton)
            : this(chart, new Dictionary<string, Series>(), pauseButton)
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;
            _pauseButton = pauseButton;
            Charting();
        }

        public IStash Stash { get; set; }

        private void Charting()
        {
            Receive<InitializeChart>(ic => HandleInitialize(ic));
            Receive<AddSeries>(addSeries => HandleAddSeries(addSeries));
            Receive<RemoveSeries>(removeSeries => HandleRemoveSeries(removeSeries));
            Receive<Metric>(metric => HandleMetrics(metric));

            Receive<TogglePause>(pause =>
                {
                    SetPauseButtonText(true);
                    BecomeStacked(Paused);
                });
        }

        private void HandleAddSeries(AddSeries series)
        {
            if (!string.IsNullOrEmpty(series.Series.Name) && !_seriesIndex.ContainsKey(series.Series.Name))
            {
                _seriesIndex.Add(series.Series.Name, series.Series);
                _chart.Series.Add(series.Series);
                SetChartBoundaries();
            }
        }

        private void HandleInitialize(InitializeChart ic)
        {
            if (ic.InitialSeries != null)
            {
                //swap the two series out
                _seriesIndex = ic.InitialSeries;
            }

            //delete any existing series
            _chart.Series.Clear();

            ChartArea area = _chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            SetChartBoundaries();

            //attempt to render the initial chart
            if (_seriesIndex.Any())
            {
                foreach (var series in _seriesIndex)
                {
                    //force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    _chart.Series.Add(series.Value);
                }
            }

            SetChartBoundaries();
        }

        private void HandleMetrics(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && _seriesIndex.ContainsKey(metric.Series))
            {
                Series series = _seriesIndex[metric.Series];
                series.Points.AddXY(xPosCounter++, metric.CounterValue);
                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }
                SetChartBoundaries();
            }
        }

        private void HandleMetricsPaused(Metric metric)
        {
            if (!string.IsNullOrEmpty(metric.Series) && _seriesIndex.ContainsKey(metric.Series))
            {
                Series series = _seriesIndex[metric.Series];
                series.Points.AddXY(xPosCounter++, 0.0d); //set the Y value to zero when we're paused
                while (series.Points.Count > MaxPoints)
                {
                    series.Points.RemoveAt(0);
                }
                SetChartBoundaries();
            }
        }

        private void HandleRemoveSeries(RemoveSeries series)
        {
            if (!string.IsNullOrEmpty(series.SeriesName) && _seriesIndex.ContainsKey(series.SeriesName))
            {
                Series seriesToRemove = _seriesIndex[series.SeriesName];
                _seriesIndex.Remove(series.SeriesName);
                _chart.Series.Remove(seriesToRemove);
                SetChartBoundaries();
            }
        }

        private void Paused()
        {
            // while paused, we need to stash AddSeries & RemoveSeries messages
            Receive<AddSeries>(addSeries => Stash.Stash());
            Receive<RemoveSeries>(removeSeries => Stash.Stash());
            Receive<Metric>(metric => HandleMetricsPaused(metric));
            Receive<Metric>(metric => HandleMetricsPaused(metric));
            Receive<TogglePause>(pause =>
                {
                    SetPauseButtonText(false);
                    UnbecomeStacked();

                    // ChartingActor is leaving the Paused state, put messages back
                    // into mailbox for processing under new behavior
                    Stash.UnstashAll();
                });
        }

        private void SetChartBoundaries()
        {
            HashSet<DataPoint> allPoints = _seriesIndex.Values.Aggregate(new HashSet<DataPoint>(),
                                                                         (set, series) =>
                                                                         new HashSet<DataPoint>(set.Concat(series.Points)));
            List<double> yValues = allPoints.Aggregate(new List<double>(),
                                                       (list, point) => list.Concat(point.YValues).ToList());
            double maxAxisX = xPosCounter;
            double minAxisX = xPosCounter - MaxPoints;
            double maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
            double minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;
            if (allPoints.Count > 2)
            {
                ChartArea area = _chart.ChartAreas[0];
                area.AxisX.Minimum = minAxisX;
                area.AxisX.Maximum = maxAxisX;
                area.AxisY.Minimum = minAxisY;
                area.AxisY.Maximum = maxAxisY;
            }
        }

        private void SetPauseButtonText(bool paused)
        {
            _pauseButton.Text = string.Format("{0}", !paused ? "PAUSE ||" : "RESUME ->");
        }
    }
}