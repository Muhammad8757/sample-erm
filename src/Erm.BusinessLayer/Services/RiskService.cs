using Erm.BusinessLayer.Validators;
using Erm.DataAccess;
using FluentValidation;
using AutoMapper;

namespace Erm.BusinessLayer;

public sealed class RiskService : IRiskService
{
    private readonly RiskDTOValidation _riskDTOValidation;
    private readonly IRiskRepository _repository;
    private readonly IMapper _mapper;
    private readonly IAnalysis _analysis;

    

    public RiskService()
    {
        _riskDTOValidation = new();
        _repository = new RiskRepositoryProxy(new RiskRepository());
        _mapper = AutoMapperHelper.MapperConfiguration.CreateMapper();
        _analysis = new Analysis();
    }

    public async Task CreateRiskAsync(RiskDTO riskDTO, CancellationToken token = default)
    {
        _riskDTOValidation.ValidateAndThrow(riskDTO);

        Risk risk = _mapper.Map<Risk>(riskDTO);

        await _repository.CreateAsync(risk, token);
    }

    public async Task DeleteAsync(string type, CancellationToken token = default)
    {
        await _repository.DeleteAsync(type, token);
    }

    public async Task<RiskDTO> GetAsync(string type, CancellationToken token = default)
    {
        return _mapper.Map<RiskDTO>(await _repository.GetAsync(type, token));
    }

    public async Task UpdateAsync(string type, RiskDTO riskDTO, CancellationToken token = default)
    {
        await _riskDTOValidation.ValidateAndThrowAsync(riskDTO, token);
        Risk risk = _mapper.Map<Risk>(riskDTO);
        await _repository.UpdateAsync(type, risk, token);
    }

    


    public async Task<double> ROccurrenceProbabilityTimeSeriesAsync()
    {
        int[] resultFormDb = _repository.ROccurrenceProbability();
        double result = await _analysis.TimeSeriesAnalysisAsync(resultFormDb);
        return result;
    }

    public async Task<double> RPotentialBusinessImpactTimeSeries()
    {
        int[] resultFormDb = _repository.RPotentialBusinessImpact();
        double result = await _analysis.TimeSeriesAnalysisAsync(resultFormDb);
        return result;
    }

    public async Task<double> CorrelationAnalyzerAsync()
    {
        
        int[] OccurrenceProbability = _repository.ROccurrenceProbability();
        int[] potentialSolution = _repository.RPotentialBusinessImpact();
        double correlationCoefficient = await _analysis.CorrelationAnalyzerAsync(OccurrenceProbability, potentialSolution);
        return correlationCoefficient;    
    }

    public async Task<double> ClusterAnalysisAsync()
    {
        int[] OccurrenceProbability = _repository.ROccurrenceProbability();
        int[] PotentialBusinessImpact = _repository.RPotentialBusinessImpact();
        int[][] ClusterAnalysisAsyncArr = new int[][]
        {
            OccurrenceProbability,
            PotentialBusinessImpact
        };
        return await _analysis.ClusterAnalysisAsync(ClusterAnalysisAsyncArr, 5);
    }
}