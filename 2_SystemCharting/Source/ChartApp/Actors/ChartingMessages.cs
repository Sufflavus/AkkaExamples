using Akka.Actor;

namespace ChartApp.Actors
{
    /// <summary>
    ///     All types of counters supported by this example
    /// </summary>
    public enum CounterType
    {
        Cpu,
        Memory,
        Disk
    }

    /// <summary>
    ///     Signal used to indicate that it's time to sample all counters
    /// </summary>
    public class GatherMetrics
    {
    }

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

    /// <summary>
    ///     Enables a counter and begins publishing values to <see cref="Subscriber" />.
    /// </summary>
    public class SubscribeCounter
    {
        public SubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Counter = counter;
            Subscriber = subscriber;
        }

        public CounterType Counter { get; set; }
        public IActorRef Subscriber { get; set; }
    }

    /// <summary>
    ///     Unsubscribes <see cref="Subscriber" /> from receiving updates for a given counter
    /// </summary>
    public class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Counter = counter;
            Subscriber = subscriber;
        }

        public CounterType Counter { get; set; }
        public IActorRef Subscriber { get; set; }
    }
}