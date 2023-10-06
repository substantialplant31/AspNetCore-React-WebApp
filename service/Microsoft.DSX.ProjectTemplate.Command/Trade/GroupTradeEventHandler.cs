using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DSX.ProjectTemplate.Data;
using Microsoft.DSX.ProjectTemplate.Data.Abstractions;
using Microsoft.DSX.ProjectTemplate.Data.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.Command.Trade
{
    public class TradeCreatedEventHandler : HandlerBase, INotificationHandler<TradeCreatedDomainEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<TradeCreatedEventHandler> _logger;

        public TradeCreatedEventHandler(
            IMediator mediator,
            ProjectTemplateDbContext database,
            IMapper mapper,
            IAuthorizationService authorizationService,
            IEmailService emailService,
            ILogger<TradeCreatedEventHandler> logger)
            : base(mediator, database, mapper, authorizationService)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(TradeCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // As a subscriber, we run on the thread pool so we need to handle our own failures appropriately.
            try
            {
                await _emailService.SendEmailAsync("a@microsoft.com", "b@microsoft.com", $"New Trade '{notification.Trade.Name}' was created.", "lorem ipsum");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email.");
            }
        }
    }
}
