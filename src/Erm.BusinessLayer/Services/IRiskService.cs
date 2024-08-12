using Erm.DataAccess;

namespace Erm.BusinessLayer;

public interface IRiskService
{
    Task CreateRiskAsync(RiskDTO riskDTO, CancellationToken token = default);
    Task DeleteAsync(string type, CancellationToken token = default);
    Task UpdateAsync(string type, RiskDTO risk, CancellationToken token = default);
    Task<RiskDTO> GetAsync(string type, CancellationToken token = default);
    Task<double> ROccurrenceProbabilityTimeSeriesAsync();
    Task<double> RPotentialBusinessImpactTimeSeries();
    Task<double> CorrelationAnalyzerAsync();
    Task<double> ClusterAnalysisAsync();
}
