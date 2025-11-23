using Microsoft.EntityFrameworkCore;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Persistence.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        // Entidades de la BD
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<RespuestaCuestionario> RespuestasCuestionarios { get; set; }
        public DbSet<Cuestionario> Cuestionarios { get; set; }
        public DbSet<SesionNota> SesionNotas { get; set; }

        // llaves primarias compuestas y otras configuraciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cita>(c =>
            {
                c.HasKey(c => c.Id);
            });
            modelBuilder.Entity<Cita>().Property(c => c.Modo).HasConversion<string>();
            modelBuilder.Entity<Cita>().Property(c => c.Estado).HasConversion<string>();

            modelBuilder.Entity<Sesion>(s =>
            {
                s.HasKey(s => s.Id);
                s.HasMany(s => s.Notas)
                 .WithOne(n => n.Sesion)
                 .HasForeignKey(n => n.SesionId);
            });
            modelBuilder.Entity<SesionNota>(sn =>
            {
                sn.HasKey(sn => sn.Id);
            });
            modelBuilder.Entity<Paciente>(p =>
            {
                p.HasKey(p => p.Id);
            });
            modelBuilder.Entity<Cuestionario>(q =>
            {
                q.HasKey(q => q.Id);
            });
            modelBuilder.Entity<RespuestaCuestionario>(rc =>
            {
                rc.HasKey(rc => rc.Id);
                rc.HasOne(rc => rc.Paciente)
                  .WithMany()
                  .HasForeignKey(rc => rc.PacienteId);
                rc.HasOne(rc => rc.Sesion)
                  .WithMany()
                  .HasForeignKey(rc => rc.SesionId);
                rc.HasOne(rc => rc.Cuestionario)
                  .WithMany()
                  .HasForeignKey(rc => rc.CuestionarioId);
            });
            // Additional configurations can be added here
        }

    }
}
