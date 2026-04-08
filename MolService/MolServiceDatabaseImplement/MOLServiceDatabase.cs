using Microsoft.EntityFrameworkCore;
using MolServiceDatabaseImplement.Models;

namespace MolServiceDatabaseImplement
{
    public class MOLServiceDatabase : DbContext
    {
        public MOLServiceDatabase(DbContextOptions<MOLServiceDatabase> options) : base(options)
        {
        }

        public virtual DbSet<Classroom> Classrooms { get; set; }
        public virtual DbSet<MaterialResponsiblePerson> MaterialResponsiblePersons { get; set; }
        public virtual DbSet<MaterialTechnicalValue> MaterialTechnicalValues { get; set; }
        public virtual DbSet<Software> Softwares { get; set; }
        public virtual DbSet<SoftwareRecord> SoftwareRecords { get; set; }
        public virtual DbSet<EquipmentMovementHistory> EquipmentMovementHistories { get; set; }
        public virtual DbSet<MaterialTechnicalValueGroup> MaterialTechnicalValueGroups { get; set; }
        public virtual DbSet<MaterialTechnicalValueRecord> MaterialTechnicalValueRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("classrooms");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.CoreSystemId).IsRequired();
                entity.Property(x => x.Number).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Capacity).IsRequired();
                entity.Property(x => x.NotUseInSchedule).IsRequired();
                entity.HasIndex(x => x.CoreSystemId).IsUnique();
                entity.HasIndex(x => x.Number);
            });

            modelBuilder.Entity<MaterialResponsiblePerson>(entity =>
            {
                entity.ToTable("material_responsible_persons");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Position).HasMaxLength(150);
                entity.Property(x => x.Phone).HasMaxLength(30);
                entity.Property(x => x.Email).HasMaxLength(150);
            });

            modelBuilder.Entity<MaterialTechnicalValue>(entity =>
            {
                entity.ToTable("material_technical_values");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(100);
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.Location).HasMaxLength(200);

                entity.HasOne(x => x.Classroom)
                    .WithMany(x => x.MaterialTechnicalValues)
                    .HasForeignKey(x => x.ClassroomId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.MaterialResponsiblePerson)
                    .WithMany(x => x.MaterialTechnicalValues)
                    .HasForeignKey(x => x.MaterialResponsiblePersonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Software>(entity =>
            {
                entity.ToTable("softwares");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SoftwareName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.SoftwareDescription).HasMaxLength(1000);
                entity.Property(x => x.SoftwareKey).HasMaxLength(500);
                entity.Property(x => x.SoftwareK).HasMaxLength(500);
            });

            modelBuilder.Entity<SoftwareRecord>(entity =>
            {
                entity.ToTable("software_records");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.SetupDescription).HasMaxLength(1000);
                entity.Property(x => x.ClaimNumber).HasMaxLength(200);

                entity.HasIndex(x => new { x.MaterialTechnicalValueId, x.SoftwareId })
                    .IsUnique();

                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany(x => x.SoftwareRecords)
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Software)
                    .WithMany(x => x.SoftwareRecords)
                    .HasForeignKey(x => x.SoftwareId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EquipmentMovementHistory>(entity =>
            {
                entity.ToTable("equipment_movement_histories");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.MoveDate)
                    .IsRequired()
                    .HasColumnType("timestamp without time zone");

                entity.Property(x => x.Reason).IsRequired().HasMaxLength(1000);
                entity.Property(x => x.Quantity).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Comment).HasMaxLength(1000);

                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany(x => x.EquipmentMovementHistories)
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MaterialTechnicalValueGroup>(entity =>
            {
                entity.ToTable("material_technical_value_groups");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.GroupName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Order).IsRequired();
            });

            modelBuilder.Entity<MaterialTechnicalValueRecord>(entity =>
            {
                entity.ToTable("material_technical_value_records");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FieldName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.FieldValue).HasMaxLength(1000);
                entity.Property(x => x.Order).IsRequired();

                entity.HasOne(x => x.MaterialTechnicalValueGroup)
                    .WithMany(x => x.MaterialTechnicalValueRecords)
                    .HasForeignKey(x => x.MaterialTechnicalValueGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany(x => x.MaterialTechnicalValueRecords)
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}