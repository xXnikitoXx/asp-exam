using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using System.Linq;

public class PlanClient : IPlanClient
{
	private ApplicationDbContext _context;

	public PlanClient(ApplicationDbContext context) {
		this._context = context;
		
		int[] preRegisteredPlans = new int[0];
		typeof(Project.Enums.Plan).GetEnumValues().CopyTo(preRegisteredPlans, 0);
		if (this.GetPlans().Count < preRegisteredPlans.Length)
			foreach (int n in preRegisteredPlans)
				Task.Run(async () => await this.RegisterPlan(PlanFor(n)));
	}

	public List<Plan> GetPlans() => this._context.Plans.ToList();

	public async Task RegisterPlan(Plan plan)
	{
		this._context.Plans.Add(plan);
		await this._context.SaveChangesAsync();
	}

	public async Task RemovePlan(int number) => await this.RemovePlan(this._context.Plans.FirstOrDefault(plan => plan.Number == number));

	public async Task RemovePlan(Plan plan)
	{
		this._context.Plans.Remove(plan);
		await this._context.SaveChangesAsync();
	}

	protected static byte CoresFor(int n)
	{
		switch (n) {
			case 0: return 1;
			case 1: return 2;
			case 2: return 2;
			case 3: return 3;
			case 4: return 4;
			default: return 8;
		}
	}

	protected static byte RAMFor(int n)
	{
		switch (n) {
			case 0: return 2;
			case 1: return 2;
			case 2: return 4;
			case 3: return 4;
			case 4: return 8;
			default: return 16;
		}
	}

	protected static ushort SSDFor(int n)
	{
		switch (n) {
			case 0: return 20;
			case 1: return 40;
			case 2: return 40;
			case 3: return 80;
			case 4: return 160;
			default: return 240;
		}
	}

	protected static double PriceFor(int n)
	{
		switch (n) {
			case 0: return 9.5;
			case 1: return 12;
			case 2: return 15;
			case 3: return 20;
			case 4: return 35;
			default: return 60;
		}
	}

	protected static Plan PlanFor(int n) => new Plan()
	{
		Number = n,
		Cores = CoresFor(n),
		RAM = RAMFor(n),
		SSD = SSDFor(n),
		Price = PriceFor(n),
	};
}