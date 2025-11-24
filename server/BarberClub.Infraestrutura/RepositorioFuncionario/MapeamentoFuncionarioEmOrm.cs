using BarberClub.Dominio.ModuloFuncionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberClub.Infraestrutura.Orm.RepositorioFuncionario;

public class MapeamentoFuncionarioEmOrm : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable("TBFUNCIONARIOS");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnType("varchar(14)")
            .IsRequired();

        builder.Property(x => x.TempoAtendimento)
            .HasColumnType("int")
            .HasDefaultValue(25)
            .IsRequired();

        builder.Property(x => x.TempoIntervalo)
            .HasColumnType("int")
            .HasDefaultValue(5)
            .IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.HorarioDisponivels)
            .WithOne(h => h.Funcionario)
            .HasForeignKey(h => h.FuncionarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Agendamentos)
            .WithOne(a => a.Funcionario)
            .HasForeignKey(a => a.FuncionarioId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}