using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DSX.ProjectTemplate.Command.Trade;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DSX.ProjectTemplate.API.Controllers
{
    /// <summary>
    /// Controller for Trade APIs.
    /// </summary>
    public class TradesController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TradesController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator instance from dependency injection.</param>
        public TradesController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Get all Trades.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeDto>>> GetAllTrades()
        {
            return Ok(await Mediator.Send(new GetAllTradesQuery()));
        }

        /// <summary>
        /// Get a Trade by its Id.
        /// </summary>
        /// <param name="id">ID of the Trade to get.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeDto>> GetTrade(int id)
        {
            return Ok(await Mediator.Send(new GetTradeByIdQuery() { TradeId = id }));
        }

        /// <summary>
        /// Create a new Trade.
        /// </summary>
        /// <param name="dto">A Trade DTO.</param>
        [HttpPost]
        public async Task<ActionResult<TradeDto>> CreateTrade([FromBody] TradeDto dto)
        {
            return Ok(await Mediator.Send(new CreateTradeCommand() { Trade = dto }));
        }

        /// <summary>
        /// Update an existing Trade.
        /// </summary>
        /// <param name="dto">Updated Trade DTO.</param>
        [HttpPut]
        public async Task<ActionResult<TradeDto>> UpdateTrade([FromBody] TradeDto dto)
        {
            return Ok(await Mediator.Send(new UpdateTradeCommand() { Trade = dto }));
        }

        /// <summary>
        /// Delete an existing Trade.
        /// </summary>
        /// <param name="id">Id of the Trade to be deleted.</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TradeDto>> DeleteTrade([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteTradeCommand() { TradeId = id }));
        }
    }
}
