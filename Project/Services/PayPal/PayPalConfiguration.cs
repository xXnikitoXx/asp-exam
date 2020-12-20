using System;
using PayPal.Api;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Project.Services.PayPal {
	public class PayPalConfiguration {
		private readonly IWebHostEnvironment _env;
		public readonly string ClientId;
		public readonly string ClientSecret;
		public readonly bool Production;

		public PayPalConfiguration(IWebHostEnvironment env) {
			this._env = env;
			string mode = Environment.GetEnvironmentVariable("PAYPAL_MODE");
			if (mode == null)
				this.Production = !this._env.IsDevelopment();
			else
				this.Production = mode == "live";
			Dictionary<string, string> config = GetConfig();
			this.ClientId = config["clientId"];
			this.ClientSecret = config["clientSecret"];
		}

		public Dictionary<string, string> GetConfig() =>
			new Dictionary<string, string>() {
				{ "clientId", Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") },
				{ "clientSecret", Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") },
				{ "mode", this.Production ? "live" : "sandbox" },
			};

		private string GetAccessToken() =>
			new OAuthTokenCredential(this.ClientId, this.ClientSecret, GetConfig())
				.GetAccessToken();

		public APIContext GetAPIContext(string accessToken = "") {
			APIContext apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ?
				GetAccessToken() : accessToken);
			apiContext.Config = GetConfig();
			return apiContext;
		}
	}
}