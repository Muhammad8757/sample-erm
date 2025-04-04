﻿namespace Erm.DataAccess;

public sealed class RiskProfile
{
    private int _occurrenceProbability;
    private int _potentialBusinessImpact;
    public int Id { get; set; }
    public int BusinessProcessId { get; set; }
    public required string RiskName { get; set; }
    public string Description { get; set; } = null!;
    public required BusinessProcess BusinessProcess { get; set; }
    public required int OccurrenceProbability
    {
        get => _occurrenceProbability;
        set => _occurrenceProbability = (value < 1 || value > 10)
            ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
    }
    public required int PotentialBusinessImpact
    {
        get => _potentialBusinessImpact;
        set => _potentialBusinessImpact = (value < 1 || value > 10)
            ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
    }
    public string? PotentialSolution { get; set; }
    public ICollection<Risk>? Risks { get; set; }
}