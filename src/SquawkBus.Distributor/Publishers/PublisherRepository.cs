using SquawkBus.Distributor.Interactors;
using SquawkBus.Messages;

namespace SquawkBus.Distributor.Publishers
{
    internal class PublisherRepository
    {
        private readonly TwoWaySet<FeedTopic, Interactor> _topicsAndPublishers = new TwoWaySet<FeedTopic, Interactor>();

        public PublisherRepository()
        {
        }

        public void AddPublisher(Interactor publisher, string feed, string topic)
        {
            _topicsAndPublishers.Add(publisher, new FeedTopic(feed, topic));
        }

        public IEnumerable<FeedTopic> RemovePublisher(Interactor publisher)
        {
            return _topicsAndPublishers.Remove(publisher) ?? new FeedTopic[0];
        }
    }
}
