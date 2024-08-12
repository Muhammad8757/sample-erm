using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Erm.BusinessLayer;
using Erm.DataAccess;

namespace Erm.PresentationLayer.WebApi;
[ApiController]
[Route("api/analysis")]
public sealed class AnalysisController : ControllerBase
{
    private readonly Analysis _analysis;
    public AnalysisController()
    {
        _analysis = new();
    }

    [HttpGet]
    [Route("correlationAnalyzerAsync")]
    public async Task<IActionResult> CorrelationAnalyzerAsync([FromQuery] int[] x, [FromQuery] int[] y)
    {
        // Создаем объект для сериализации с поддержкой бесконечных чисел
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        // Вызываем метод из вашего сервиса, передаем параметры и сериализуем результат
        var result = await _analysis.CorrelationAnalyzerAsync(x, y);
        string json = JsonSerializer.Serialize(result, options);

        return Ok(json);
    }

    [HttpGet]
    [Route("timeSeriesAnalys")]
    public IActionResult TimeSeriesAnalys([FromQuery] int[] ananlizeValueArray)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        // Вызываем метод из вашего сервиса, передаем параметры и сериализуем результат
        var result = _analysis.TimeSeriesAnalysisAsync(ananlizeValueArray);
        string json = JsonSerializer.Serialize(result, options);

        return Ok(json);
    }

    [HttpGet]
    [Route("clusterAnalysis")]
    public IActionResult ClusterAnalysisAsync([FromQuery] int[][] data, int k)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        // Вызываем метод из вашего сервиса, передаем параметры и сериализуем результат
        var result = _analysis.ClusterAnalysisAsync(data, k);
        string json = JsonSerializer.Serialize(result, options);

        return Ok(json);
    }
}