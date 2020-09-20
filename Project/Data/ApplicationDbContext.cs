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
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }
        public DbSet<UserVPS> UserVPSs { get; set; }
        public DbSet<VPS> VPSs { get; set; }
        public DbSet<VPSActivity> VPSActivities { get; set; }
        public DbSet<VPSState> VPSStates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.HasMany(user => user.VPSs)
                .WithOne(vps => vps.User)
                .HasForeignKey(userVps => userVps.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Posts)
                .WithOne(post => post.User)
                .HasForeignKey(userPost => userPost.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Tickets)
                .WithOne(ticket => ticket.User)
                .HasForeignKey(userTicket => userTicket.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(user => user.Messages)
                .WithOne(message => message.User);

                entity.HasMany(user => user.Activities)
                .WithOne(activity => activity.User)
                .HasForeignKey(userActivity => userActivity.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VPS>(entity =>
            {
                entity.HasMany(vps => vps.Activities)
                .WithOne(activity => activity.VPS)
                .HasForeignKey(vpsActivity => vpsActivity.VPSId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(vps => vps.States)
                .WithOne(state => state.VPS)
                .HasForeignKey(vpsState => vpsState.VPSId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Post>()
            .HasMany(post => post.Answers)
            .WithOne(answer => answer.ParentPost)
            .HasForeignKey(answer => answer.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserVPS>()
            .HasKey(userVps => new { userVps.UserId, userVps.VPSId });

            modelBuilder.Entity<UserPost>()
            .HasKey(userPost => new { userPost.UserId, userPost.PostId });

            modelBuilder.Entity<UserTicket>()
            .HasKey(userTicket => new { userTicket.UserId, userTicket.TicketId });

            modelBuilder.Entity<UserMessage>()
            .HasKey(userMessage => new { userMessage.UserId, userMessage.MessageId });

            modelBuilder.Entity<UserActivity>()
            .HasKey(userActivity => new { userActivity.UserId, userActivity.ActivityId });

            modelBuilder.Entity<VPSActivity>()
            .HasKey(vpsActivity => new { vpsActivity.VPSId, vpsActivity.ActivityId });

            modelBuilder.Entity<VPSState>()
            .HasKey(vpsState => new { vpsState.VPSId, vpsState.StateId });
        }
    }
}
