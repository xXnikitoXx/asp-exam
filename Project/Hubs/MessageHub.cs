using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Hubs {
	public class MessageHub : Hub {
		protected IHubContext<MessageHub> _context;
		private readonly IUserStatusClient _userStatus;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public MessageHub(
			IHubContext<MessageHub> context,
			IUserStatusClient userStatus,
			UserManager<ApplicationUser> userManager,
			IMapper mapper
		) {
			this._context = context;
			this._userStatus = userStatus;
			this._userManager = userManager;
			this._mapper = mapper;
		}

		public override async Task OnConnectedAsync() {
			if ((Context.User != null) && Context.User.Identity.IsAuthenticated)
				this._userStatus.AddConnection(await this._userManager.GetUserAsync(Context.User), Context.ConnectionId);
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			if ((Context.User != null) && Context.User.Identity.IsAuthenticated)
				this._userStatus.RemoveConnection(Context.ConnectionId);
			await base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(Message message) {
			IClientProxy target = this._context.Clients.Clients(this._userStatus.ConnectionsOf(message.User).ToList());
			NotificationMessageViewModel model = this._mapper.Map<NotificationMessageViewModel>(message);
			await target.SendAsync("ReceiveMessage", model);
		}
	}
}