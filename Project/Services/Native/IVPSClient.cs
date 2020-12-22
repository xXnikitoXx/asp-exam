using Project.Enums;
using Project.Models;
using Project.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Services.Native {
	public interface IVPSClient {
		Task<List<VPS>> GetVPSs(ClaimsPrincipal user, VPSsViewModel pageInfo);
		List<VPS> GetVPSs(ApplicationUser user, VPSsViewModel pageInfo);
		Task<List<VPS>> GetVPSs(ClaimsPrincipal user);
		List<VPS> GetVPSs(ApplicationUser user);
		List<VPS> GetVPSs(VPSsViewModel pageInfo);
		Task RegisterVPS(VPS vps);
		Task<VPS> RegisterVPSFor(Order order, string name);
		Task<VPS> RegisterVPSFor(Order order, int index);
		Task UpdateStatus(string id, ServerStatus status, float cpu, float ram);
		Task UpdateStatus(VPS vps, ServerStatus status, float cpu, float ram);
		Task RegisterActivity(string id, string message, string url);
		Task RegisterActivity(VPS vps, string message, string url);
		Task RemoveVPS(string id);
		Task RemoveVPS(VPS vps);
	}
}
