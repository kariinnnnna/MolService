using Microsoft.EntityFrameworkCore;
using MolServiceDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MolServiceDatabaseImplement
{
    public class MOLServiceDatabase : DbContext
    {
        public MOLServiceDatabase(DbContextOptions<MOLServiceDatabase> options) : base(options)
        {
        }

        // DbSet для всех сущностей
        public virtual DbSet<Classroom> Classrooms { get; set; }
        public virtual DbSet<MaterialResponsiblePerson> MaterialResponsiblePersons { get; set; }
        public virtual DbSet<MaterialTechnicalValue> MaterialTechnicalValues { get; set; }
        public virtual DbSet<Software> Softwares { get; set; }
        public virtual DbSet<SoftwareRecord> SoftwareRecords { get; set; }
        public virtual DbSet<EquipmentMovementHistory> EquipmentMovementHistories { get; set; }
        public virtual DbSet<MaterialTechnicalValueGroup> MaterialTechnicalValueGroups { get; set; }
        public virtual DbSet<MaterialTechnicalValueRecord> MaterialTechnicalValueRecords { get; set; }

        // Конфигурация моделей через Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка сущности Classroom
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

            // Настройка сущности MaterialResponsiblePerson
            modelBuilder.Entity<MaterialResponsiblePerson>(entity =>
            {
                entity.ToTable("material_responsible_persons");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Position).HasMaxLength(150);
                entity.Property(x => x.Department).HasMaxLength(150);
                entity.Property(x => x.Phone).HasMaxLength(30);
                entity.Property(x => x.Email).HasMaxLength(150);
            });

            // Настройка сущности MaterialTechnicalValue
            modelBuilder.Entity<MaterialTechnicalValue>(entity =>
            {
                entity.ToTable("material_technical_values");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.InventoryNumber).IsRequired().HasMaxLength(100);
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Description).HasMaxLength(1000);
                entity.Property(x => x.Location).HasMaxLength(200);
                entity.Property(x => x.Cost).IsRequired().HasColumnType("decimal(18,2)");

                // Связь с Classroom
                entity.HasOne(x => x.Classroom)
                    .WithMany()
                    .HasForeignKey(x => x.ClassroomId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Связь с MaterialResponsiblePerson
                entity.HasOne(x => x.MaterialResponsiblePerson)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialResponsiblePersonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка сущности Software
            modelBuilder.Entity<Software>(entity =>
            {
                entity.ToTable("softwares");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SoftwareName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.SoftwareDescription).HasMaxLength(1000);
                entity.Property(x => x.SoftwareKey).HasMaxLength(500);
                entity.Property(x => x.SoftwareK).HasMaxLength(500);
            });

            // Настройка сущности SoftwareRecord
            modelBuilder.Entity<SoftwareRecord>(entity =>
            {
                entity.ToTable("software_records");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SetupDescription).HasMaxLength(1000);
                entity.Property(x => x.ClaimNumber).HasMaxLength(200);

                // Связь с MaterialTechnicalValue
                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Связь с Software
                entity.HasOne(x => x.Software)
                    .WithMany()
                    .HasForeignKey(x => x.SoftwareId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка сущности EquipmentMovementHistory
            modelBuilder.Entity<EquipmentMovementHistory>(entity =>
            {
                entity.ToTable("equipment_movement_histories");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MoveDate).IsRequired();
                entity.Property(x => x.Reason).HasMaxLength(1000);

                // Связь с MaterialTechnicalValue
                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка сущности MaterialTechnicalValueGroup
            modelBuilder.Entity<MaterialTechnicalValueGroup>(entity =>
            {
                entity.ToTable("material_technical_value_groups");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.GroupName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Order).IsRequired();
            });

            // Настройка сущности MaterialTechnicalValueRecord
            modelBuilder.Entity<MaterialTechnicalValueRecord>(entity =>
            {
                entity.ToTable("material_technical_value_records");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.FieldName).IsRequired().HasMaxLength(200);
                entity.Property(x => x.FieldValue).HasMaxLength(1000);
                entity.Property(x => x.Order).IsRequired();

                // Связь с MaterialTechnicalValueGroup
                entity.HasOne(x => x.MaterialTechnicalValueGroup)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialTechnicalValueGroupId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Связь с MaterialTechnicalValue
                entity.HasOne(x => x.MaterialTechnicalValue)
                    .WithMany()
                    .HasForeignKey(x => x.MaterialTechnicalValueId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
