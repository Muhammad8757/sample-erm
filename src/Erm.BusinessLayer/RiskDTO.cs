namespace Erm.BusinessLayer;

public readonly record struct RiskDTO
(
    string Type,
    int RiskProfileId,
    string Description,
    int Probability,
    int BusinessImpact,
    DateTime? OccurenceData
);

