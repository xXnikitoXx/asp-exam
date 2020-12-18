using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data {
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
		#region Tables

		public DbSet<Activity> Activities { get; set; }
		public DbSet<Announcement> Announcements { get; set; }
		public new DbSet<ApplicationUser> Users { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Plan> Plans { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<PromoCode> PromoCodes { get; set; }
		public DbSet<PromoCodeOrder> PromoCodeOrders { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<UserPromoCode> UserPromoCodes { get; set; }
		public DbSet<VPS> VPSs { get; set; }

		#endregion

		#region Configuration

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			#region EntityModels

			modelBuilder.Entity<ApplicationUser>(entity => {
				entity.HasKey(user => user.Id);

				entity.Property(user => user.JoinDate)
					.HasDefaultValueSql("getdate()");

				entity.Property(user => user.LastLoginDate)
					.HasDefaultValueSql("getdate()");

				entity.HasMany(user => user.VPSs)
					.WithOne(vps => vps.User)
					.HasForeignKey(vps => vps.UserId)
					.OnDelete(DeleteBehavior.SetNull);

				entity.HasMany(user => user.Posts)
					.WithOne(post => post.User)
					.HasForeignKey(post => post.UserId)
					.OnDelete(DeleteBehavior.SetNull);

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

			modelBuilder.Entity<VPS>(entity => {
				entity.HasAlternateKey(vps => vps.ExternalId);

				entity.HasMany(vps => vps.Activities)
					.WithOne(activity => activity.VPS)
					.HasForeignKey(activity => activity.VPSId);

				entity.HasMany(vps => vps.States)
					.WithOne(state => state.VPS)
					.HasForeignKey(state => state.VPSId);
			});

			modelBuilder.Entity<Post>(entity => {
				entity.HasMany(post => post.Answers)
					.WithOne(answer => answer.ParentPost)
					.HasForeignKey(answer => answer.ParentId)
					.OnDelete(DeleteBehavior.NoAction);

				entity.Property(post => post.Time)
					.HasDefaultValueSql("getdate()");
			});

			modelBuilder.Entity<Plan>(entity => {
				entity.HasKey(plan => plan.Number);

				entity.HasMany(plan => plan.Orders)
					.WithOne(order => order.Plan)
					.HasForeignKey(order => order.PlanNumber);
			});

			modelBuilder.Entity<Activity>()
				.Property(activity => activity.Time)
				.HasDefaultValueSql("getdate()");

			modelBuilder.Entity<State>()
				.Property(state => state.Time)
				.HasDefaultValueSql("getdate()");

			modelBuilder.Entity<Message>(entity => {
				entity.Property(message => message.Time)
					.HasDefaultValueSql("getdate()");
				
				entity.HasOne(message => message.User)
					.WithMany(user => user.Messages)
					.HasForeignKey(message => message.UserId);

				entity.HasOne(message => message.Sender)
					.WithMany(sender => sender.SentMessages)
					.HasForeignKey(message => message.SenderId);

				entity.HasOne<Ticket>(message => message.Ticket)
					.WithOne(ticket => ticket.Answer)
					.HasForeignKey<Ticket>(ticket => ticket.AnswerId);

				entity.HasOne(message => message.Reply)
					.WithOne(reply => reply.PreviousMessage)
					.HasForeignKey<Message>(reply => reply.PreviousMessageId);

				entity.HasOne(message => message.PreviousMessage)
					.WithOne(prev => prev.Reply)
					.HasForeignKey<Message>(prev => prev.ReplyId);
			});

			modelBuilder.Entity<Ticket>()
				.HasOne<Message>(ticket => ticket.Answer)
				.WithOne(message => message.Ticket)
				.HasForeignKey<Message>(message => message.TicketId);

			modelBuilder.Entity<Order>(entity => {
				entity.Property(order => order.TimeStarted)
					.HasDefaultValueSql("getdate()");

				entity.HasOne(order => order.Plan)
					.WithMany(plan => plan.Orders)
					.HasForeignKey(order => order.PlanNumber);

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

			modelBuilder.Entity<PromoCodeOrder>(entity =>
			{
				entity.HasKey(promoCodeOrder => new { promoCodeOrder.PromoCodeId, promoCodeOrder.OrderId });

				entity.HasOne(promoCodeOrder => promoCodeOrder.PromoCode)
					.WithMany(promoCode => promoCode.Orders)
					.HasForeignKey(promoCodeOrder => promoCodeOrder.PromoCodeId);

				entity.HasOne(promoCodeOrder => promoCodeOrder.Order)
					.WithMany(order => order.PromoCodes)
					.HasForeignKey(promoCodeOrder => promoCodeOrder.OrderId);
			});

			#endregion
		}
	}
}
