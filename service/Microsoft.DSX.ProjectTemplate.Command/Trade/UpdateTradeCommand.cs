using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DSX.ProjectTemplate.Data;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using Microsoft.DSX.ProjectTemplate.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.Command.Trade
{
    public class UpdateTradeCommand : IRequest<TradeDto>
    {
        public TradeDto Trade { get; set; }
    }

    public class UpdateTradeCommandHandler : CommandHandlerBase,
        IRequestHandler<UpdateTradeCommand, TradeDto>
    {
        public UpdateTradeCommandHandler(
            IMediator mediator,
            ProjectTemplateDbContext database,
            IMapper mapper,
            IAuthorizationService authorizationService)
            : base(mediator, database, mapper, authorizationService)
        {
        }

        public async Task<TradeDto> Handle(UpdateTradeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Trade;

            if (dto.Id <= 0)
            {
                throw new BadRequestException($"{nameof(Data.Models.Trade)} Id must be greater than zero.");
            }

            var model = await Database.Trades
                .Where(x => x.Id == dto.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
            {
                throw new EntityNotFoundException($"{nameof(Data.Models.Trade)} with Id {dto.Id} not found.");
            }

            // ensure uniqueness of name
            bool nameAlreadyUsed = await Database.Trades.AnyAsync(e => e.Name.Trim() == dto.Name.Trim(), cancellationToken) && dto.Name != (model.Name);
            if (nameAlreadyUsed)
            {
                throw new BadRequestException($"{nameof(dto.Name)} {dto.Name} already used.");
            }

            model.Name = dto.Name;
            model.IsActive = dto.IsActive;

            Database.Trades.Update(model);

            await Database.SaveChangesAsync(cancellationToken);

            return Mapper.Map<TradeDto>(model);
        }
    }
}
