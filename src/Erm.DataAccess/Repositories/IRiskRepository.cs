namespace Erm.DataAccess;

public interface IRiskRepository
{
    public Task CreateAsync(Risk RiskEntity, CancellationToken token = default);
    public Task<Risk> GetAsync(string type, CancellationToken token = default);
    public Task UpdateAsync(string type, Risk risk, CancellationToken token = default);
    public Task DeleteAsync(string type, CancellationToken token = default);
    public int[] ROccurrenceProbability();
    public int[] RPotentialBusinessImpact();
}