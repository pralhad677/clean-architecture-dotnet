using Bookify.Infrastructure.OutBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configuration;

public class OutBoxMessageConfiguration:IEntityTypeConfiguration<OutBoxMessage>
{
    public void Configure(EntityTypeBuilder<OutBoxMessage> builder)
    {
            builder.ToTable("OutBoxMessages");
            builder.HasKey(x => x.Id);
            builder.Property(outBoxMessage => outBoxMessage.Content).HasColumnName("jsonb");
    }
}