using ChartApp.Actors;

namespace ChartApp.Messages.CoordinatorMessages
{
    /// <summary>
    ///     Subscribe the <see cref="ChartingActor" /> to updates for <see cref="Counter" />.
    /// </summary>
    public class Watch
    {
        public Watch(CounterType counter)
        {
            Counter = counter;
        }

        public CounterType Counter { get; set; }
    }
}