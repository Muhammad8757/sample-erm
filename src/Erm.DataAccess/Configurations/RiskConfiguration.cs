using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erm.DataAccess;

public sealed class RiskConfiguration : IEntityTypeConfiguration<Risk>
{
    public void Configure(EntityTypeBuilder<Risk> builder)
    {
        builder
            .ToTable("risk");
        
        builder
            .Property(p => p.Id)
            .HasColumnName("id")
            .IsRequired();
        
        builder
            .Property(p => p.Type)
            .HasColumnName("type")
            .HasColumnType("VARCHAR(10)")
            .IsRequired();

        builder
            .Property(p => p.Description)
            .HasColumnName("description")
            .HasColumnType("VARCHAR(500)")
            .IsRequired();
        
        builder
            .Property(p => p.Probability)
            .HasColumnName("probability")
            .HasColumnType("INTEGER")
            .IsRequired();
        
        builder
            .Property(p => p.BusinessImpact)
            .HasColumnName("business_impact")
            .HasColumnType("INTEGER")
            .IsRequired();
        
        builder
            .Property(p => p.OccurenceData)
            .HasColumnName("occurence_data")
            .HasColumnType("DATE")
            .IsRequired();
        
        builder
            .HasOne(rp => rp.RiskProfile)
            .WithMany(r => r.Risks)
            .HasForeignKey(fk => fk.RiskProfileId)
            .IsRequired();

        builder
            .Property(p => p.RiskProfileId)
            .HasColumnName("risk_profile_id");
        
        builder
            .HasKey(k => k.Id);
    }
}