namespace Erm.DataAccess;

public interface IRiskProfileRepository
{
    public Task CreateAsync(RiskProfile entity, CancellationToken token = default);
    public int[] RpOccurrenceProbability();
    public int[] RpPotentialBusinessImpact();
    public Task<RiskProfile> GetAsync(string name, CancellationToken token = default);
    public Task UpdateAsync(string name, RiskProfile riskProfile, CancellationToken token = default);
    public Task<IEnumerable<RiskProfile>> QueryAsync(string query, CancellationToken token = default);
    public Task DeleteAsync(string name, CancellationToken token = default);
}