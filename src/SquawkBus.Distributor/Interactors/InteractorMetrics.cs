using System;

using Prometheus;

using SquawkBus.Distributor.Utilities;

namespace SquawkBus.Distributor.Interactors
{
    public class InteractorMetrics
    {
        public InteractorMetrics(string host, string user, Guid id, string application)
        {
            var labelValues = new string[] { host, user, id.ToString(), application };

            ReadsReceived = _Metrics.Instance.ReadsReceived.WithLabels(labelValues);

            WritesSent = _Metrics.Instance.WritesSent.WithLabels(labelValues);

            WriteQueueLength = _Metrics.Instance.WriteQueueLength.WithLabels(labelValues);

            Faulted = _Metrics.Instance.Faulted;

            AuthorizationRequests = _Metrics.Instance.AuthorizationRequests.WithLabels(labelValues);

            AuthorizationResponses = _Metrics.Instance.AuthorizationResponses.WithLabels(labelValues);

            Interactors = _Metrics.Instance.Interactors;

            ForwardedSubscriptions = new CounterSelector(
                _Metrics.Instance.ForwardedSubscriptions,
                labelValues);

            FeedRequests = new GaugeSelector(_Metrics.Instance.FeedRequests, labelValues);

            UnicastMessages = new CounterSelector(
                _Metrics.Instance.UnicastMessages,
                labelValues);

            MulticastMessages = new CounterSelector(
                _Metrics.Instance.MulticastMessages,
                labelValues);

            Subscriptions = new GaugeSelector(
                _Metrics.Instance.Subscriptions,
                labelValues);
        }

        public Counter.Child ReadsReceived { get; }
        public Counter.Child WritesSent { get; }
        public Gauge.Child WriteQueueLength { get; }
        public Counter Faulted { get; }
        public Counter.Child AuthorizationRequests { get; }
        public Counter.Child AuthorizationResponses { get; }
        public Gauge Interactors { get; }
        public CounterSelector ForwardedSubscriptions { get; }
        public GaugeSelector FeedRequests { get; }
        public CounterSelector UnicastMessages { get; }
        public CounterSelector MulticastMessages { get; }
        public GaugeSelector Subscriptions { get; }

        private class _Metrics
        {
            public const string Feed = "feed", Host = "host", User = "user", Id = "id", Application = "application";

            private static _Metrics? _instance;

            public static _Metrics Instance => _instance ?? (_instance = new _Metrics());

            public _Metrics()
            {
                ReadsReceived = Metrics.CreateCounter(
                    "squawkbus_reads",
                    "The number of read messages read by an interactor",
                    Host, User, Id, Application);

                WritesSent = Metrics.CreateCounter(
                    "squawkbus_writes",
                    "The number of write messages sent from an interactor",
                    Host, User, Id, Application);

                WriteQueueLength = Metrics.CreateGauge(
                    "squawkbus_write_queue_length",
                    "The number of messages on an interactor write queue",
                    Host, User, Id, Application);

                Faulted = Metrics.CreateCounter(
                    "squawkbus_interactor_faults",
                    "The number of interactor faults",
                    Host, User, Id, Application);

                AuthorizationRequests = Metrics.CreateCounter(
                    "squawkbus_authorization_requests",
                    "The number of authorization requests",
                    Host, User, Id, Application);

                AuthorizationResponses = Metrics.CreateCounter(
                    "squawkbus_authorization_responses",
                    "The number of authorization responses",
                    Host, User, Id, Application);

                Interactors = Metrics.CreateGauge(
                    "squarkbus_interactors",
                    "The number of interactors",
                    Host, User, Id, Application);

                ForwardedSubscriptions = Metrics.CreateCounter(
                    "squawkbus_forwarded_subscriptions",
                    "The number of forwarded subscriptions",
                    Feed, Host, User, Id, Application);

                FeedRequests = Metrics.CreateGauge(
                    "squawkbus_notification_requests",
                    "The number of notification requests",
                    Feed, Host, User, Id, Application);

                UnicastMessages = Metrics.CreateCounter(
                    "squawkbus_published_unicast_messages",
                    "The number of unicast messages sent",
                    Feed, Host, User, Id, Application);

                MulticastMessages = Metrics.CreateCounter(
                    "squawkbus_published_multicast_messages",
                    "The number of multicast messages sent",
                    Feed, Host, User, Id, Application);

                Subscriptions = Metrics.CreateGauge(
                    "squawkbus_subscriptions",
                    "The number of subscriptions",
                    Feed, Host, User, Id, Application);
            }

            public Counter ReadsReceived { get; }
            public Counter WritesSent { get; }
            public Gauge WriteQueueLength { get; }
            public Counter Faulted { get; }
            public Counter AuthorizationRequests { get; }
            public Counter AuthorizationResponses { get; }
            public Gauge Interactors { get; }
            public Counter ForwardedSubscriptions { get; }
            public Gauge FeedRequests { get; }
            public Counter UnicastMessages { get; }
            public Counter MulticastMessages { get; }
            public Gauge Subscriptions { get; }
        }
    }
}