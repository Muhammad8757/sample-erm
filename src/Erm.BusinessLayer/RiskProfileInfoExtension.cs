using Erm.DataAccess;

namespace Erm.BusinessLayer;

internal static class RiskProfileInfoExtensions
{
    internal static RiskProfile ToRiskProfile(this RiskProfileInfo profileInfo)
        => new()
        {
            RiskName = profileInfo.Name,
            Description = profileInfo.Description,
            BusinessProcess = new()
            {
                Name = profileInfo.BusinessProcess,
                Domain = profileInfo.BusinessProcess
            },
            PotentialBusinessImpact = profileInfo.PotentialBusinessImpact,
            OccurrenceProbability = profileInfo.OccurrenceProbability
        };
    
    internal static Risk ToRisk(this RiskDTO riskDTO)
        => new()
        {
            Type = riskDTO.Type,
            Description = riskDTO.Description,
            Probability = riskDTO.Probability,
            BusinessImpact = riskDTO.BusinessImpact,
            OccurenceData = riskDTO.OccurenceData
        };
}