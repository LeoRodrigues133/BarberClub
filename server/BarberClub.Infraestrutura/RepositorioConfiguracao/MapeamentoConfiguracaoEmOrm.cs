using BarberClub.Dominio.ModuloConfiguracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BarberClub.Infraestrutura.Orm.RepositorioConfiguracao;

public class MapeamentoConfiguracaoEmOrm : IEntityTypeConfiguration<ConfiguracaoEmpresa>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoEmpresa> builder)
    {
        builder.ToTable("TBCONFIGURACAO");

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(x => x.NomeEmpresa)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.LogoUrl)
            .HasColumnType("varchar(500)")
            .IsRequired(false);

        builder.Property(x => x.BannerUrl)
            .HasColumnType("varchar(500)")
            .IsRequired(false);

        builder.Property(x => x.Ativo)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.DatasEspecificasFechado)
            .HasColumnType("varchar(max)")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<DateTime>>(v, (JsonSerializerOptions)null) ?? new List<DateTime>()
            )
            .Metadata
            .SetValueComparer(
                new ValueComparer<List<DateTime>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        builder.HasMany(x => x.HorarioDeExpediente)
            .WithOne(x => x.ConfiguracaoEmpresa)
            .HasForeignKey(x => x.ConfiguracaoEmpresaId)
            .IsRequired();
    }
}