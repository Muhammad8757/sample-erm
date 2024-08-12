using Microsoft.EntityFrameworkCore;

namespace Erm.DataAccess;


public sealed class RiskRepository : IRiskRepository
{
    private readonly ErmDbContext _db = new();


    public async Task CreateAsync(Risk entity, CancellationToken token = default)
    {
        await _db.Risks.AddAsync(entity, token);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string type, CancellationToken token = default)
    {
        await _db.Risks.Where(x => x.Type.Equals(type)).ExecuteDeleteAsync(token);
        await _db.SaveChangesAsync(token);
    }
    public Task<Risk> GetAsync(string type, CancellationToken token = default) 
        => _db.Risks.AsNoTracking().SingleAsync(x => x.Type.Equals(type), token);


    public async Task UpdateAsync(string type, Risk risk, CancellationToken token = default)
    {
        Risk riskToUpdate = await _db.Risks.SingleAsync(x => x.Type.Equals(type), token);

        riskToUpdate.Type = risk.Type;
        riskToUpdate.Description = risk.Description;
        riskToUpdate.Probability = risk.Probability;
        riskToUpdate.BusinessImpact = risk.BusinessImpact;
        riskToUpdate.OccurenceData = risk.OccurenceData;

        await _db.SaveChangesAsync(token);
    }

    public int[] ROccurrenceProbability()
    {
        int[] res = _db.Risks
            .AsNoTracking()
            .Select(i => i.Probability)
            .ToArray();
        return res;
    }
    

    
    public int[] RPotentialBusinessImpact()
    {
        int[] res = _db.Risks
            .AsNoTracking()
            .Select(i => i.BusinessImpact)
            .ToArray();
        return res;
    }


}