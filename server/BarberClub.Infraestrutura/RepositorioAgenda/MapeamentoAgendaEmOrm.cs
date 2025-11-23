using BarberClub.Dominio.ModuloAgenda;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberClub.Infraestrutura.Orm.RepositorioConfiguracao;

public class MapeamentoAgendaEmOrm : IEntityTypeConfiguration<ConfiguracaoAgenda>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoAgenda> builder)
    {
        builder.ToTable("TBCONFIGURACAOAGENDA");

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(x => x.Funcionario)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(x => x.TempoAtendimento)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.IntervaloEntreAtendimento)
            .HasColumnType("int")
            .IsRequired();
    }
}