using Project.Models;
using Project.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public interface IPlanClient
	{
		Task<Plan> Find(int number);
		List<Plan> GetPlans();
		List<Plan> GetPlans(PlansViewModel pageInfo);
		Task RegisterPlan(Plan plan);
		Task RemovePlan(int number);
		Task RemovePlan(Plan plan);
	}
}
