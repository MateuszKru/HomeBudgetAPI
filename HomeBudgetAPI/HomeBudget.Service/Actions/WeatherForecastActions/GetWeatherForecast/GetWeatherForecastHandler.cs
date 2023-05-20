using HomeBudget.Service.ModelsDTO.WeatherForecastModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Service.Actions.WeatherForecastActions.GetWeatherForecast
{
    public class GetWeatherForecastHandler : IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecastDTO>>
    {
        public async Task<IEnumerable<WeatherForecastDTO>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecastDTO
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = request.Summaries[Random.Shared.Next(request.Summaries.Length)]
            })
            .ToArray();
        }
    }
}
