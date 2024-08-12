using Erm.DataAccess;
namespace Erm.BusinessLayer;
public interface IRiskProfileService
{
    Task CreateAsync(RiskProfileInfo profileInfo, CancellationToken token = default);
    Task<IEnumerable<RiskProfileInfo>> QueryAsync(string query, CancellationToken token = default);
    Task DeleteAsync(string name, CancellationToken token = default);
    Task<RiskProfileInfo> GetAsync(string name, CancellationToken token = default);
    Task UpdateAsync(string name, RiskProfileInfo riskProfile, CancellationToken token = default);
    Task<double> RpOccurrenceProbabilityTimeSeriesAsync();
    Task<double> RpPotentialBusinessImpactTimeSeries();
    Task<double> CorrelationAnalyzerAsync();
    Task<double> ClusterAnalysisAsync();
}