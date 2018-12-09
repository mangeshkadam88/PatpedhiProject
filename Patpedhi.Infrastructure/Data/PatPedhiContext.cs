using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patpedhi.Infrastructure.Identity;
using PatPedhi.Core.Entities;
using PatPedhi.Core.Entities.Identity;
using System;

namespace Patpedhi.Infrastructure.Data
{
    public class PatpedhiContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public PatpedhiContext(DbContextOptions<PatpedhiContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserProfile>(ConfigureUserProfile);
            builder.Entity<Savings>(ConfigureSavings);
        }

        private void ConfigureSavings(EntityTypeBuilder<Savings> builder)
        {
            builder.ToTable("Savings");
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.user_id)
                .IsRequired(true);

            builder.Property(ci => ci.amount)
                .IsRequired(true);

            builder.Property(ci => ci.is_active)
                .IsRequired(true);

            builder.Property(ci => ci.is_approved)
                .IsRequired(true);

            builder.Property(ci => ci.savings_type)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci => ci.added_by)
                .IsRequired(true);

            builder.Property(ci => ci.approved_by);

            builder.Property(ci => ci.description)
                .HasColumnType("nvarchar(max)");
        }

        private void ConfigureUserProfile(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfile");
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.first_name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci => ci.middle_name)
                .HasMaxLength(50);

            builder.Property(ci => ci.last_name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(ci => ci.date_of_birth)
                .IsRequired(true);

            builder.Property(ci => ci.is_active)
                .IsRequired(true);

            builder.Property(ci => ci.is_approved)
                .IsRequired(true);

            builder.Property(ci => ci.profile_photo_url)
                .HasColumnType("nvarchar(max)");

            builder.Property(ci => ci.signature_photo_url)
                .HasColumnType("nvarchar(max)");

            builder.Property(ci => ci.user_id)
                .IsRequired(true);

            builder.Property(ci => ci.account_no)
                .IsRequired(false);

            builder.Ignore(ci => ci.full_name);

        }
    }
}
