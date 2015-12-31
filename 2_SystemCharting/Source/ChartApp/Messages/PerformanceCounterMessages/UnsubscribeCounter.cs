using Akka.Actor;

namespace ChartApp.Messages.PerformanceCounterMessages
{
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