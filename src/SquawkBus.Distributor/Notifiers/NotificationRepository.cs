using System.Collections.Generic;
using System.Linq;

using SquawkBus.Distributor.Interactors;

namespace SquawkBus.Distributor.Notifiers
{
    internal class NotificationRepository
    {
        private readonly Dictionary<string, ISet<Interactor>> _feedToNotifiables = new Dictionary<string, ISet<Interactor>>();

        public NotificationRepository()
        {
        }

        public void RemoveInteractor(Interactor interactor)
        {
            // Remove the interactor where it appears in the notifiables, remembering any topics which are left without any interactors.
            var topicsWithoutInteractors = new HashSet<string>();
            foreach (var (feed, feedInteractors) in _feedToNotifiables.Where(x => x.Value.Contains(interactor)))
            {
                interactor.Metrics.FeedRequests[feed].Dec();
                feedInteractors.Remove(interactor);
                if (feedInteractors.Count == 0)
                    topicsWithoutInteractors.Add(feed);
            }

            // Remove any topics left without interactors.
            foreach (var topic in topicsWithoutInteractors)
            {
                _feedToNotifiables.Remove(topic);
            }
        }

        public bool AddRequest(Interactor notifiable, string feed)
        {
            // Find or create the set of notifiables for this feed.
            if (!_feedToNotifiables.TryGetValue(feed, out var notifiables))
                _feedToNotifiables.Add(feed, notifiables = new HashSet<Interactor>());
            else if (notifiables.Contains(notifiable))
                return false;

            // Add to the notifiables for this topic pattern and inform the subscription manager of the new notification request.
            notifiables.Add(notifiable);
            notifiable.Metrics.FeedRequests[feed].Inc();
            return true;
        }

        public void RemoveRequest(Interactor notifiable, string feed)
        {
            // Does this feed have any notifiable interactors?
            if (!_feedToNotifiables.TryGetValue(feed, out var notifiables))
                return;

            // Is this interactor in the set of notifiables for this feed?
            if (!notifiables.Contains(notifiable))
                return;

            // Remove the interactor from the set of notifiables.
            notifiables.Remove(notifiable);
            notifiable.Metrics.FeedRequests[feed].Dec();

            // Are there any interactors left listening to this feed?
            if (notifiables.Count != 0)
                return;

            // Remove the empty pattern from the caches.
            _feedToNotifiables.Remove(feed);
        }

        public ISet<Interactor>? FindNotifiables(string feed)
        {
            return !_feedToNotifiables.TryGetValue(feed, out var interactors) ? null : interactors;
        }
    }
}
