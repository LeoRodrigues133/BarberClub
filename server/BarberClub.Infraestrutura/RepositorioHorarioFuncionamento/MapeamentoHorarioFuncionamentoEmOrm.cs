using BarberClub.Dominio.ModuloHorarioFuncionamento;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberClub.Infraestrutura.Orm.RepositorioHorarioFuncionamento;

public class MapeamentoHorarioFuncionamentoEmOrm : IEntityTypeConfiguration<HorarioFuncionamento>
{
    public void Configure(EntityTypeBuilder<HorarioFuncionamento> builder)
    {
        builder.ToTable("TBHORARIOFUNCIONAMENTO");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(x => x.ConfiguracaoEmpresa)
            .WithMany(x => x.HorarioDeExpediente)
            .HasForeignKey(a => a.ConfiguracaoEmpresaId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(x => x.DiaSemana)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.HoraAbertura)
            .IsRequired(false);

        builder.Property(x => x.HoraFechamento)
            .IsRequired(false);

        builder.Property(x => x.Fechado)
            .HasColumnType("bit")
            .IsRequired();

        builder.HasIndex(x => new { x.ConfiguracaoEmpresaId, x.DiaSemana })
            .IsUnique();
    }
}