using BarberClub.Dominio.ModuloServico;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberClub.Infraestrutura.Orm.RepositorioServicos;

public class MapeamentoServicoEmOrm : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("TBSERVICO");

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Titulo)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(x => x.Duracao)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.ValorFinal)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(x => x.IsPromocao)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(x => x.PorcentagemPromocao)
            .HasColumnType("int")
            .IsRequired(false);

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Funcionario)
               .WithMany()
               .HasForeignKey(x => x.FuncionarioId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}