using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers
{
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IOrderClient _service;
		private readonly IPlanClient _planService;
		private readonly IMapper _mapper;

		public OrderController(IOrderClient service,
			IPlanClient planService,
			IMapper mapper)
		{
			this._service = service;
			this._planService = planService;
			this._mapper = mapper;
		}

		public async Task<IActionResult> Index(int ProductId = 0)
		{
			Order existing = (await _service.GetOrders(User))
				.FirstOrDefault(order => order.State == Enums.OrderState.Created && ProductId == order.Plan.Number);
			if (existing != null)
				return this.View(existing);
			else
			{
				
			}
			return this.View();
		}

		public async Task<IActionResult> List(int Page = 1, int Show = 20)
		{
			List<Order> orders = await _service.GetOrders(User);
			int Total = orders.Count;
			int Pages = Total / Show + Total % Show != 0 ? 1 : 0;
			double TotalInvestments = orders.Sum(order => order.FinalPrice);
			orders = orders.Skip(Show * (Page - 1)).Take(Show).ToList();
			OrdersViewModel model = new OrdersViewModel
			{
				Orders = orders.Select(_mapper.Map<OrderViewModel>).ToList(),
				Total = Total,
				Page = Page,
				Pages = Pages,
				Show = Show,
				TotalInvestments = TotalInvestments
			};
			return this.View(model);
		}
	}
}
