using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BarChartCanvars.Models
{
    public partial class InteractionsContext : DbContext
    {
        public InteractionsContext()
        {
        }

        public InteractionsContext(DbContextOptions<InteractionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CurrentInteraction> CurrentInteractions { get; set; } = null!;
        public virtual DbSet<DeviceDetail> DeviceDetails { get; set; } = null!;
        public virtual DbSet<InterationDatum> InterationData { get; set; } = null!;
        public virtual DbSet<PageEventDatum> PageEventData { get; set; } = null!;
        public virtual DbSet<WhoIsInformation> WhoIsInformations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentInteraction>(entity =>
            {
                entity.Property(e => e.Ip).HasColumnName("IP");
            });

            modelBuilder.Entity<InterationDatum>(entity =>
            {
                entity.HasIndex(e => e.DeviceInfoId, "IX_InterationData_DeviceInfoId");

                entity.HasIndex(e => e.GeoIpid, "IX_InterationData_GeoIPId");

                entity.HasIndex(e => e.PageEventId, "IX_InterationData_PageEventId");

                entity.Property(e => e.GeoIpid).HasColumnName("GeoIPId");

                entity.HasOne(d => d.DeviceInfo)
                    .WithMany(p => p.InterationData)
                    .HasForeignKey(d => d.DeviceInfoId);

                entity.HasOne(d => d.GeoIp)
                    .WithMany(p => p.InterationData)
                    .HasForeignKey(d => d.GeoIpid);

                entity.HasOne(d => d.PageEvent)
                    .WithMany(p => p.InterationData)
                    .HasForeignKey(d => d.PageEventId);
            });

            modelBuilder.Entity<WhoIsInformation>(entity =>
            {
                entity.ToTable("WhoIsInformation");

                entity.Property(e => e.Dns).HasColumnName("DNS");

                entity.Property(e => e.Ip).HasColumnName("IP");

                entity.Property(e => e.Isp).HasColumnName("ISP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
