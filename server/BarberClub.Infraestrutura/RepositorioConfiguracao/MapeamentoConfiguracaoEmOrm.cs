using BarberClub.Dominio.ModuloConfiguracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.HasMany(x => x.HorarioDeExpediente)
            .WithOne(x => x.ConfiguracaoEmpresa)
            .HasForeignKey(x => x.ConfiguracaoEmpresaId)
            .IsRequired();
    }
}