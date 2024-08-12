using AutoMapper;
using Erm.BusinessLayer.Validators;
using Erm.DataAccess;
using FluentValidation;

namespace Erm.BusinessLayer;
public sealed class RiskProfileService : IRiskProfileService
{
    private readonly IRiskProfileRepository _repository;
    private readonly IMapper _mapper;
    private readonly RiskProfileInfoValidator _validationRules;
    private readonly IAnalysis _analysis;

    public RiskProfileService()
    {

        _validationRules = new();
        _repository = new RiskProfileRepositoryProxy(new RiskProfileRepository());
        _mapper = AutoMapperHelper.MapperConfiguration.CreateMapper();
        _analysis = new Analysis();
    }

    public async Task CreateAsync(RiskProfileInfo profileInfo, CancellationToken token = default)
    {
        await _validationRules.ValidateAndThrowAsync(profileInfo, token);

        RiskProfile riskProfile = _mapper.Map<RiskProfile>(profileInfo);
        await _repository.CreateAsync(riskProfile, token);
    }


    public async Task DeleteAsync(string name, CancellationToken token = default)
    {
        await _repository.DeleteAsync(name, token);
    }

    public async Task<RiskProfileInfo> GetAsync(string name, CancellationToken token = default)
    {
        return _mapper.Map<RiskProfileInfo>(await _repository.GetAsync(name, token));
    }

    public async Task<IEnumerable<RiskProfileInfo>> QueryAsync(string query, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(query);
        return _mapper.Map<IEnumerable<RiskProfileInfo>>(await _repository.QueryAsync(query, token));
    }

    public async Task UpdateAsync(string name, RiskProfileInfo profileInfo, CancellationToken token = default)
    {
        await _validationRules.ValidateAndThrowAsync(profileInfo, token);
        RiskProfile riskProfile = _mapper.Map<RiskProfile>(profileInfo);
        await _repository.UpdateAsync(name, riskProfile, token);
    }

    public async Task<double> RpOccurrenceProbabilityTimeSeriesAsync()
    {
        int[] resultFormDb = _repository.RpOccurrenceProbability();
        double result = await _analysis.TimeSeriesAnalysisAsync(resultFormDb);
        return result;
    }


    public async Task<double> RpPotentialBusinessImpactTimeSeries()
    {
        int[] resultFormDb = _repository.RpPotentialBusinessImpact();
        double result = await _analysis.TimeSeriesAnalysisAsync(resultFormDb);
        return result;
    }

    public async Task<double> CorrelationAnalyzerAsync()
    {
        int[] OccurrenceProbability = _repository.RpOccurrenceProbability();
        int[] potentialSolution = _repository.RpPotentialBusinessImpact();
        double correlationCoefficient = await _analysis.CorrelationAnalyzerAsync(OccurrenceProbability, potentialSolution);
        return correlationCoefficient;
    }

    public async Task<double> ClusterAnalysisAsync()
    {
    
        int[] businessImpact = _repository.RpPotentialBusinessImpact();
        int[] OccurrenceProbability = _repository.RpOccurrenceProbability();
        int[][] ClusterAnalysisAsyncArr = new int[][]
        {
            businessImpact,
            OccurrenceProbability
        };
        return await _analysis.ClusterAnalysisAsync(ClusterAnalysisAsyncArr, 5);
    }
}