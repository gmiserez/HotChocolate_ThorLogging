using Demo.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Demo
{
    public class Subscription
    {
        [Subscribe]
        public EventHubTrace OnEvent(
            [Topic] string eventType,
            [EventMessage] EventHubTrace message) =>
            message;
    }
}
