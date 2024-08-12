using Microsoft.EntityFrameworkCore;

namespace Erm.DataAccess;

public sealed class RiskProfileRepository : IRiskProfileRepository
{
    private readonly ErmDbContext _db = new();
    public async Task CreateAsync(RiskProfile entity, CancellationToken token = default)
    {
        await _db.RiskProfiles.AddAsync(entity, token);
        await _db.SaveChangesAsync(token);
    }

    public int[] RpOccurrenceProbability()
    {
        int[] res = _db.RiskProfiles
            .Select(i => i.OccurrenceProbability)
            .ToArray();
        return res;
    }
    
    
    public int[] RpPotentialBusinessImpact()
    {
        int[] res = _db.RiskProfiles
            .Select(i => i.PotentialBusinessImpact)
            .ToArray();
        return res;
    }

    public async Task DeleteAsync(string name, CancellationToken token = default)
    {
        await _db.RiskProfiles.Where(x => x.RiskName.Equals(name)).ExecuteDeleteAsync(token);
        await _db.SaveChangesAsync(token);
    }

    public async Task<RiskProfile> GetAsync(string name, CancellationToken token = default)
{
    return await _db.RiskProfiles
        .FromSqlRaw("SELECT * FROM risk_profile WHERE risk_name = {0}", name)
        .Include(i => i.BusinessProcess)
        .AsNoTracking()
        .SingleAsync(token);
}


    public async Task<IEnumerable<RiskProfile>> QueryAsync(string query, CancellationToken token = default)
        => await _db.RiskProfiles
            .AsNoTracking()
            .Include(i => i.BusinessProcess)
            .Where(x => x.RiskName.Contains(query) || x.Description.Contains(query))
            .ToArrayAsync(token);

    public async Task UpdateAsync(string name, RiskProfile riskProfile, CancellationToken token = default)
    {
        RiskProfile profileToUpdate = await _db.RiskProfiles.SingleAsync(x => x.RiskName.Equals(name), token);
        profileToUpdate.RiskName = riskProfile.RiskName;
        profileToUpdate.Description = riskProfile.Description;
        profileToUpdate.PotentialBusinessImpact = riskProfile.PotentialBusinessImpact;
        profileToUpdate.PotentialSolution = riskProfile.PotentialSolution;
        profileToUpdate.OccurrenceProbability = riskProfile.OccurrenceProbability;
        await _db.SaveChangesAsync(token);
    }
}