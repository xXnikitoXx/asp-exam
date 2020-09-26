using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Tables

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<VPS> VPSs { get; set; }
        public DbSet<PromoCodePlan> PromoCodePlans { get; set; }

        #endregion

        #region Configuration

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region EntityModels

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.HasMany(user => user.VPSs)
                .WithOne(vps => vps.User)
                .HasForeignKey(vps => vps.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(post => post.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Tickets)
                .WithOne(ticket => ticket.User)
                .HasForeignKey(ticket => ticket.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Messages)
                .WithOne(message => message.User);

                entity.HasMany(user => user.Activities)
                .WithOne(activity => activity.User)
                .HasForeignKey(activity => activity.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VPS>(entity =>
            {
                entity.HasMany(vps => vps.Activities)
                .WithOne(activity => activity.VPS)
                .HasForeignKey(activity => activity.VPSId);

                entity.HasMany(vps => vps.States)
                .WithOne(state => state.VPS)
                .HasForeignKey(state => state.VPSId);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasMany(post => post.Answers)
                .WithOne(answer => answer.ParentPost)
                .HasForeignKey(answer => answer.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(post => post.Time)
                .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Plan>()
            .HasNoKey()
            .HasIndex(plan => plan.Number)
            .IsUnique();

            modelBuilder.Entity<Activity>()
            .Property(activity => activity.Time)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<State>()
            .Property(state => state.Time)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Message>()
            .Property(message => message.Time)
            .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(order => order.TimeStarted)
                .HasDefaultValueSql("getdate()");

                entity.HasMany(order => order.VPSs)
                .WithOne(vps => vps.Order)
                .HasForeignKey(vps => vps.OrderId);
            });
            
            #endregion

            #region MappingModels

            modelBuilder.Entity<UserPromoCode>(entity =>
            {
                entity.HasKey(userPromoCode => new { userPromoCode.UserId, userPromoCode.PromoCodeId });

                entity.HasOne(userPromoCode => userPromoCode.User)
                .WithMany(user => user.PromoCodes)
                .HasForeignKey(userPromoCode => userPromoCode.UserId);

                entity.HasOne(userPromoCode => userPromoCode.PromoCode)
                .WithMany(promoCode => promoCode.Users)
                .HasForeignKey(userPromoCode => userPromoCode.PromoCodeId);
            });

            modelBuilder.Entity<PromoCodePlan>(entity =>
            {
                entity.HasKey(promoCodePlan => promoCodePlan.PromoCodeId);

                entity.HasOne(promoCodePlan => promoCodePlan.PromoCode)
                .WithMany(promoCode => promoCode.Plans)
                .HasForeignKey(promoCodePlan => promoCodePlan.PromoCodeId);
            });

            #endregion
        }
    }
}
