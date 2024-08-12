namespace Erm.PresentationLayer.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Erm.BusinessLayer;
using Erm.DataAccess;

[ApiController]
[Route("api/[controller]")]
public sealed class RiskController : ControllerBase
{
    private readonly RiskService _riskService;
    private readonly string Auth = "Auth";

    public RiskController()
    {
        _riskService = new();
    }

    

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateRiskAsync(RiskDTO riskDTO, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskService.CreateRiskAsync(riskDTO);
        return Ok();
    }

    [HttpGet]
    [Route("{type}")]
    public async Task<IActionResult> GetByType([FromRoute] string type, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }

        return Ok(await _riskService.GetAsync(type));  
    } 
    
    [HttpDelete]
    [Route("deleteRisk")]
    public async Task<IActionResult> DeleteRisk(string type, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskService.DeleteAsync(type);
        return Ok();
    }
    
    [HttpPut]
    [Route("updateRisk")]
    public async Task<IActionResult> UpdateRisk(string type, RiskDTO riskDTO, [FromQuery(Name = "apiKey")] string apiKey) 
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskService.UpdateAsync(type, riskDTO);
        return Ok();
    }


    [HttpGet]
    [Route("ROccurrenceProbabilityTimeSeriesAnalysis")]
    public Task<double> ROccurrenceProbabilityTimeSeriesAnalysis()
        => _riskService.ROccurrenceProbabilityTimeSeriesAsync();
    
    [HttpGet]
    [Route("RPotentialBusinessImpactTimeSeriesAnalysis")]
    public Task<double> RPotentialBusinessImpactTimeSeriesAnalysis()
        => _riskService.RPotentialBusinessImpactTimeSeries(); 

    
    [HttpGet]
    [Route("CorrelationAnalyzerAsync")]
    public Task<double> CorrelationAnalyzerAsync()
        => _riskService.CorrelationAnalyzerAsync(); 
        
    
    [HttpGet]
    [Route("ClusterAnalysisAsync")]
    public Task<double> ClusterAnalysisAsync()
        => _riskService.ClusterAnalysisAsync(); 
}

public static class ApiKeyPolicy
{
    public const string PolicyName = "Authorization";

    public static void Register(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("Authorization"); // Здесь вы можете указать имя вашего claim'а с API-ключом
            });
        });
    }
}