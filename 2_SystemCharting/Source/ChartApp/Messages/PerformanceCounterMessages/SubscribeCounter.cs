using Akka.Actor;

namespace ChartApp.Messages.PerformanceCounterMessages
{
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
}