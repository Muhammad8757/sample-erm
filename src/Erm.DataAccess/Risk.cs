namespace Erm.DataAccess;

public sealed class Risk
{
    public int Id { get; set; }
    public int RiskProfileId { get; set; } // Внешний ключ
    private int _probability;
    private int _businessImpact;
    public required string Type { get; set; }
    public string? Description { get; set; }
    public int Probability 
    { 
        get => _probability; 
        set
        {
            if(value < 1 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(value));
            _probability = value;
        }
    }
    public int BusinessImpact 
    { 
        get => _businessImpact; 
        set
        {
            if(value < 1 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(value));
            _businessImpact = value;
        } 
    }
    public DateTime? OccurenceData { get; set; }
    
    public RiskProfile? RiskProfile { get; set; } // Навигационное свойство
}
