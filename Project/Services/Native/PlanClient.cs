using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Data;
using Project.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Project.ViewModels;

namespace Project.Services.Native {
	public class PlanClient : IPlanClient {
		private readonly ApplicationDbContext _context;

		public PlanClient(ApplicationDbContext context) {
			this._context = context;
			string[] names = typeof(Project.Enums.Plan).GetEnumNames();
			if (names.Length > this._context.Plans.Count()) {
				Array valueObjects = typeof(Project.Enums.Plan).GetEnumValues();
				int[] values = new int[valueObjects.Length];
				valueObjects.CopyTo(values, 0);
				Dictionary<string, int> plans = new Dictionary<string, int>();
				for (int i = 0; i < names.Length; i++)
					plans.Add(names[i], values[i]);
				RegisterPlans(plans);
			}
		}

		public async Task<Plan> Find(int number) =>
			await this._context.Plans.FirstOrDefaultAsync(plan => plan.Number == number);

		public List<Plan> GetPlans() =>
			this._context.Plans
				.OrderBy(plan => plan.Price)
				.ToList();

		public List<Plan> GetPlans(PlansViewModel pageInfo) {
			IQueryable<Plan> plans = this._context.Plans
				.OrderBy(plan => plan.Price)
				.AsQueryable();
			pageInfo.Total = plans.Count();
			pageInfo.Pages = (pageInfo.Total / pageInfo.Show) + (pageInfo.Total % pageInfo.Show != 0 ? 1 : 0);
			return plans.Skip(pageInfo.Show * (pageInfo.Page - 1)).Take(pageInfo.Show).ToList();
		}

		public void RegisterPlans(Dictionary<string, int> plans) {
			foreach (KeyValuePair<string, int> plan in plans) {
				Plan p = PlanFor(plan.Value);
				p.Name = plan.Key;
				this._context.Plans.Add(p);
			}
			this._context.SaveChanges();
		}

		public async Task RegisterPlan(Plan plan) {
			this._context.Plans.Add(plan);
			await this._context.SaveChangesAsync();
		}

		public async Task UpdatePlan(Plan plan) {
			this._context.Update(plan);
			await this._context.SaveChangesAsync();
		}

		public async Task RemovePlan(int number) =>
			await this.RemovePlan(this._context.Plans.FirstOrDefault(plan => plan.Number == number));

		public async Task RemovePlan(Plan plan) {
			this._context.Plans.Remove(plan);
			await this._context.SaveChangesAsync();
		}

		protected static byte CoresFor(int n) {
			switch (n) {
				case 0: return 1;
				case 1: return 2;
				case 2: return 2;
				case 3: return 3;
				case 4: return 4;
				default: return 8;
			}
		}

		protected static byte RAMFor(int n) {
			switch (n) {
				case 0: return 2;
				case 1: return 2;
				case 2: return 4;
				case 3: return 4;
				case 4: return 8;
				default: return 16;
			}
		}

		protected static ushort SSDFor(int n) {
			switch (n) {
				case 0: return 20;
				case 1: return 40;
				case 2: return 40;
				case 3: return 80;
				case 4: return 160;
				default: return 240;
			}
		}

		protected static double PriceFor(int n) {
			switch (n) {
				case 0: return 9.5;
				case 1: return 12;
				case 2: return 15;
				case 3: return 20;
				case 4: return 35;
				default: return 60;
			}
		}

		public static Plan PlanFor(int n) => new Plan() {
			Name = typeof(Project.Enums.Plan).GetEnumNames()[n],
			Cores = CoresFor(n),
			RAM = RAMFor(n),
			SSD = SSDFor(n),
			Price = PriceFor(n),
		};
	}
}