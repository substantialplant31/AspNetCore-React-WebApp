using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DSX.ProjectTemplate.Data;
using Microsoft.DSX.ProjectTemplate.Data.Exceptions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.Command.Trade
{
    public class DeleteTradeCommand : IRequest<bool>
    {
        public int TradeId { get; set; }
    }

    public class DeleteTradeCommandHandler : CommandHandlerBase,
        IRequestHandler<DeleteTradeCommand, bool>
    {
        public DeleteTradeCommandHandler(
            IMediator mediator,
            ProjectTemplateDbContext database,
            IMapper mapper,
            IAuthorizationService authorizationService)
            : base(mediator, database, mapper, authorizationService)
        {
        }

        public async Task<bool> Handle(DeleteTradeCommand request, CancellationToken cancellationToken)
        {
            if (request.TradeId <= 0)
            {
                throw new BadRequestException($"A valid {nameof(Data.Models.Trade)} Id must be provided.");
            }

            var Trade = await Database.Trades.FindAsync(new object[] { request.TradeId }, cancellationToken);
            if (Trade == null)
            {
                throw new EntityNotFoundException($"{nameof(Data.Models.Trade)} not found.");
            }

            Database.Trades.Remove(Trade);

            await Database.SaveChangesAsync(cancellationToken);

            Debug.Assert(Database.Trades.Find(Trade.Id) == null);

            return true;
        }
    }
}
