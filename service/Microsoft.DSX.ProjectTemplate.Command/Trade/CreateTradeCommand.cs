using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DSX.ProjectTemplate.Data;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using Microsoft.DSX.ProjectTemplate.Data.Events;
using Microsoft.DSX.ProjectTemplate.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.Command.Trade
{
    public class CreateTradeCommand : IRequest<TradeDto>
    {
        public TradeDto Trade { get; set; }
    }

    public class CreateTradeCommandHandler : CommandHandlerBase,
        IRequestHandler<CreateTradeCommand, TradeDto>
    {
        public CreateTradeCommandHandler(
            IMediator mediator,
            ProjectTemplateDbContext database,
            IMapper mapper,
            IAuthorizationService authorizationService)
            : base(mediator, database, mapper, authorizationService)
        {
        }

        public async Task<TradeDto> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Trade;

            bool nameAlreadyUsed = await Database.Trades.AnyAsync(e => e.Name.Trim() == dto.Name.Trim(), cancellationToken);
            if (nameAlreadyUsed)
            {
                throw new BadRequestException($"{nameof(dto.Name)} '{dto.Name}' already used.");
            }

            var model = new Data.Models.Trade()
            {
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            Database.Trades.Add(model);

            await Database.SaveChangesAsync(cancellationToken);

            await Mediator.Publish(new TradeCreatedDomainEvent(model), cancellationToken);

            return Mapper.Map<TradeDto>(model);
        }
    }
}
