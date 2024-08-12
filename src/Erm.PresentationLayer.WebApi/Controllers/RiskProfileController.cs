namespace Erm.PresentationLayer.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Erm.BusinessLayer;
using Erm.DataAccess;

[ApiController]
[Route("api/[controller]")]
public sealed class RiskProfileController : ControllerBase
{
    private readonly RiskProfileService _riskProfileService;
    private readonly string? Auth = "Auth";
    public RiskProfileController()
    {
        _riskProfileService = new();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateRiskProfile(RiskProfileInfo riskProfileInfo, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskProfileService.CreateAsync(riskProfileInfo);
        return Ok();
    }



    [HttpGet]
    public async Task<IActionResult> Query([FromQuery] string? query, [FromQuery(Name = "apiKey")] string apiKey) 
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        if(!string.IsNullOrEmpty(query))
            return Ok(await _riskProfileService.QueryAsync(query));
        else
            return BadRequest();
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetById([FromRoute] string name, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        return Ok(await _riskProfileService.GetAsync(name));
    }
    
    [HttpGet]
    [Route("RpOccurrenceProbabilityTimeSeriesAnalysis")]
    public Task<double> RpOccurrenceProbabilityTimeSeriesAnalysis()
        => _riskProfileService.RpOccurrenceProbabilityTimeSeriesAsync();
    
    [HttpGet]
    [Route("RpPotentialBusinessImpactTimeSeriesAnalysis")]
    public Task<double> RpPotentialBusinessImpactTimeSeriesAnalysis()
        => _riskProfileService.RpPotentialBusinessImpactTimeSeries(); 
    
    [HttpGet]
    [Route("CorrelationAnalyzerAsync")]
    public Task<double> CorrelationAnalyzerAsync()
        => _riskProfileService.CorrelationAnalyzerAsync(); 
        
    
    [HttpGet]
    [Route("ClusterAnalysisAsync")]
    public Task<double> ClusterAnalysisAsync()
        => _riskProfileService.ClusterAnalysisAsync();  

    [HttpDelete]
    [Route("deleteRiskProfile")]
    public async Task<IActionResult> DeleteRiskProfile(string name, [FromQuery(Name = "apiKey")] string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskProfileService.DeleteAsync(name);
        return Ok();
    }
    

    [HttpPut]
    [Route("updateRiskProfile")]
    public async Task<IActionResult> UpdateRiskProfile(string name, RiskProfileInfo riskProfile, [FromQuery(Name = "apiKey")] string apiKey) 
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey != Auth) // Замените на ваш реальный API-ключ
        {
            return Unauthorized();
        }
        await _riskProfileService.UpdateAsync(name, riskProfile);
        return Ok();
    }   
}
