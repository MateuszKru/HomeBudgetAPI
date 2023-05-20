using HomeBudget.Service.ModelsDTO.WeatherForecastModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.Service.Actions.WeatherForecastActions.GetWeatherForecast
{
    public class GetWeatherForecastQuery : IRequest<IEnumerable<WeatherForecastDTO>>
    {
        public readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }
}
