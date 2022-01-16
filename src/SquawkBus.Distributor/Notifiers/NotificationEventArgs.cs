using SquawkBus.Distributor.Interactors;

namespace SquawkBus.Distributor.Notifiers
{
    public class NotificationEventArgs : InteractorEventArgs
    {
        public NotificationEventArgs(Interactor interactor, string feed)
            : base(interactor)
        {
            Feed = feed;
        }

        public string Feed { get; }

        public override string ToString()
        {
            return $"{base.ToString()}, Feed={Feed}";
        }
    }
}
