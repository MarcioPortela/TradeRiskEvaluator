using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradeRiskEvaluator.Application.Features.CalculateRisk;
using TradeRiskEvaluator.Application.Features.CalculateRiskDistribution;

namespace TradeRiskEvaluator.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class TradesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Calculates the risk category for a batch of financial trades.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/trades/calculate-risk
        ///     [
        ///        { "value": 2000000, "clientSector": "Private" },
        ///        { "value": 400000, "clientSector": "Public" }
        ///     ]
        /// 
        /// Note: The ClientSector field only accepts the exact values "Public" or "Private".
        /// </remarks>
        /// <param name="trades">A list of financial trades to be evaluated.</param>
        /// <returns>A list of strings representing the risk classification (e.g., HIGHRISK, MEDIUMRISK, LOWRISK) in the same order as the input.</returns>
        /// <response code="200">Risks were successfully calculated and returned.</response>
        /// <response code="400">Returned when the payload is invalid (e.g., negative values, missing or incorrect sector).</response>
        [HttpPost("calculate-risk")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateRisk([FromBody] List<TradeRequest> trades)
        {
            var command = new CalculateRiskCommand { Trades = trades };
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Evaluates a large batch of trades and returns a statistical distribution grouped by risk category.
        /// </summary>
        /// <remarks>
        /// This endpoint is highly optimized to efficiently handle up to 100,000 trades in a single request.
        /// 
        /// Sample request:
        /// 
        ///     POST /api/v1/trades/summary
        ///     [
        ///        { "value": 2000000, "clientSector": "Private", "clientId": "CLI001" },
        ///        { "value": 400000, "clientSector": "Public", "clientId": "CLI002" },
        ///        { "value": 5000000, "clientSector": "Public", "clientId": "CLI001" }
        ///     ]
        /// 
        /// Note: The ClientId field is required for this endpoint to calculate the top client exposure.
        /// </remarks>
        /// <param name="trades">A list of financial trades, including the ClientId, to be aggregated.</param>
        /// <returns>A summary object containing the risk categories, aggregated stats (count, total value, top client), and total processing time.</returns>
        /// <response code="200">Summary was successfully calculated and returned.</response>
        /// <response code="400">Returned when the payload is invalid (e.g., negative values, missing sector or clientId).</response>
        [HttpPost("distribution")]
        [ProducesResponseType(typeof(CalculateRiskDistributionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateRiskDistribution([FromBody] List<TradeDistributionRequest> trades)
        {
            var result = await _mediator.Send(new CalculateRiskDistributionCommand { Trades = trades });
            return Ok(result);
        }
    }
}
