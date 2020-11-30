using Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Services.Native
{
	public interface IPlanClient
	{
		List<Plan> GetPlans();
		Task RegisterPlan(Plan plan);
		Task RemovePlan(int number);
		Task RemovePlan(Plan plan);
	}
}
