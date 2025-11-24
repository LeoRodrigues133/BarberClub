using BarberClub.Dominio.ModuloHorario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberClub.Infraestrutura.Orm.RepositorioHorarioDisponivel;

public class MapeamentoHorarioDisponivelEmOrm : IEntityTypeConfiguration<HorarioDisponivel>
{
    public void Configure(EntityTypeBuilder<HorarioDisponivel> builder)
    {
        builder.ToTable("TBHORARIOSDISPONIVEIS");

        builder.HasOne(x => x.Funcionario)
            .WithMany(f => f.HorarioDisponivels) 
            .HasForeignKey(x => x.FuncionarioId)
            .OnDelete(DeleteBehavior.Cascade) 
            .IsRequired();

        builder.Property(x => x.HorarioInicio)
            .HasColumnType("time")
            .IsRequired();
            
        builder.Property(x=> x.HorarioFim)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x =>x.Ativo)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(x => x.DiaSemana)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.DataEspecifica)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasMany(x => x.Agendamentos)
            .WithOne(a => a.HorarioDisponivel)
            .HasForeignKey(a => a.HorarioDisponivelId)
            .OnDelete(DeleteBehavior.Restrict) 
            .IsRequired(false);
    }
}