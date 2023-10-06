using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DSX.ProjectTemplate.Data;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using Microsoft.DSX.ProjectTemplate.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.Command.Trade
{
    public class GetAllTradesQuery : IRequest<IEnumerable<TradeDto>> { }

    public class GetTradeByIdQuery : IRequest<TradeDto>
    {
        public int TradeId { get; set; }
    }

    public class TradeQueryHandler : QueryHandlerBase,
        IRequestHandler<GetAllTradesQuery, IEnumerable<TradeDto>>,
        IRequestHandler<GetTradeByIdQuery, TradeDto>
    {
        public TradeQueryHandler(
            IMediator mediator,
            ProjectTemplateDbContext database,
            IMapper mapper,
            IAuthorizationService authorizationService)
            : base(mediator, database, mapper, authorizationService)
        {
        }

        // GET ALL
        public async Task<IEnumerable<TradeDto>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
        {
            return await Database.Trades
                .Select(x => Mapper.Map<TradeDto>(x))
                .ToListAsync(cancellationToken);
        }

        // GET BY ID
        public async Task<TradeDto> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TradeId <= 0)
            {
                throw new BadRequestException($"A valid {nameof(Data.Models.Trade)} Id must be provided.");
            }

            var innerResult = await Database.Trades.FindAsync(new object[] { request.TradeId }, cancellationToken);
            if (innerResult == null)
            {
                throw new EntityNotFoundException($"{nameof(Data.Models.Trade)} with Id {request.TradeId} cannot be found.");
            }

            return Mapper.Map<TradeDto>(innerResult);
        }
    }
}
