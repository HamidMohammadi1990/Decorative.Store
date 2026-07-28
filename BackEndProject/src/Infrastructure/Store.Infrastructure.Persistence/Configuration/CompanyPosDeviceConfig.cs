using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyPosDeviceConfig : IEntityTypeConfiguration<CompanyPosDevice>
{
	public void Configure(EntityTypeBuilder<CompanyPosDevice> builder)
	{
		builder
			.Property(x => x.Name)
			.HasNVarcharMaxLength(50)
			.IsRequired();

		builder
			.Property(x => x.Description)
			.HasNVarcharMaxLength(200);

		builder
			.Property(x => x.IP)
			.HasVarcharMaxLength(16);

		builder
			.HasOne(x => x.Bank)
			.WithMany(x => x.CompanyPosDevices)
			.HasForeignKey(x => x.BankId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Company)
			.WithMany(x => x.CompanyPosDevices)
			.HasForeignKey(x => x.CompanyId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(x => x.PosTransactions)
			.WithOne(x => x.CompanyPosDevice)
			.HasForeignKey(x => x.CompanyPosDeviceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasIndex(x => x.CompanyId);

		builder
			.HasIndex(x => x.BankId);
	}
}