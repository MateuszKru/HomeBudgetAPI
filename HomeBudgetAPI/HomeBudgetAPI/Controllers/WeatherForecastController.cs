using HomeBudget.Service.Actions.WeatherForecastActions.GetWeatherForecast;
using HomeBudget.Service.ModelsDTO.WeatherForecastModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudgetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecastDTO>> Get()
        {
            return await _mediator.Send(new GetWeatherForecastQuery());
        }
    }
}