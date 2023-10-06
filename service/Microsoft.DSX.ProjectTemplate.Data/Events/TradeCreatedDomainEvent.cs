using MediatR;
using Microsoft.DSX.ProjectTemplate.Data.Models;

namespace Microsoft.DSX.ProjectTemplate.Data.Events
{
    public class TradeCreatedDomainEvent : INotification
    {
        public Trade Trade{ get; }

        public TradeCreatedDomainEvent(Trade trade)
        {
            Trade = trade;
        }
    }
}
