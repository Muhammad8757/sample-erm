using System.Text.Json.Serialization;

namespace Erm.DataAccess;

public class BusinessProcess
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Domain { get; set; }
    [JsonIgnore]
    public ICollection<RiskProfile> RiskProfiles { get; set; } = null!;
}