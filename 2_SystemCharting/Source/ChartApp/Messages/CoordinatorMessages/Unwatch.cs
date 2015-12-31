using ChartApp.Actors;

namespace ChartApp.Messages.CoordinatorMessages
{
    /// <summary>
    ///     Unsubscribe the <see cref="ChartingActor" /> to updates for <see cref="Counter" />.
    /// </summary>
    public class Unwatch
    {
        public Unwatch(CounterType counter)
        {
            Counter = counter;
        }

        public CounterType Counter { get; set; }
    }
}