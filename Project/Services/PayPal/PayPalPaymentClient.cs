using Microsoft.AspNetCore.Hosting;
using PayPal.Api;
using System;
using System.Collections.Generic;

namespace Project.Services.PayPal {
	public class PayPalPaymentClient {
		private readonly string currency;
		private readonly double rate;
		private readonly PayPalConfiguration _config;

		public PayPalPaymentClient(IWebHostEnvironment env) {
			this._config = new PayPalConfiguration(env);
			this.currency = "EUR";
			this.rate = 1.92;
		}

		public Payment CreatePayment(Models.Order order, string baseUrl, string intent) {
			APIContext apiContext = this._config.GetAPIContext();
			Payment payment = new Payment() {
				intent = intent,
				payer = new Payer() { payment_method = "paypal" },
				transactions = GetTransactionsList(order),
				redirect_urls = GetReturnUrls(order, baseUrl, intent)
			};
			return payment.Create(apiContext);
		}

		private List<Transaction> GetTransactionsList(Models.Order order) {
			List<Transaction> transactionList = new List<Transaction>();
			List<string> planContents = new List<string> {
				$"{order.Plan.Cores} Ядр{(order.Plan.Cores == 1 ? "о" : "а")}",
				$"{order.Plan.RAM}GB RAM",
				$"{order.Plan.SSD}GB SSD",
			};
			transactionList.Add(new Transaction() {
				invoice_number = GetRandomInvoiceNumber(),
				amount = new Amount() {
					currency = this.currency,
					total = (order.FinalPrice / this.rate).ToString("F2").Replace(",", ".")
				},
				item_list = new ItemList() {
					items = new List<Item>() {
						new Item() {
							name = $"{order.Plan.Name} × ({order.Amount})",
							currency = this.currency,
							price = ((order.FinalPrice) / this.rate).ToString("F2").Replace(",", "."),
							quantity = "1",
							description = $"{string.Join(", ", planContents)}",
						}
					}
				}
			});
			return transactionList;
		}

		private RedirectUrls GetReturnUrls(Models.Order order, string baseUrl, string intent) =>
			new RedirectUrls() {
				cancel_url = baseUrl + "/Payment/Cancelled?OrderId=" + order.Id,
				return_url = baseUrl + (intent == "sale" ? "Payment/Successful?OrderId=" + order.Id : "Payment/Authorized?OrderId=" + order.Id),
			};

		public Payment ExecutePayment(string paymentId, string payerId) {
			APIContext apiContext = this._config.GetAPIContext();
			PaymentExecution paymentExecution = new PaymentExecution() { payer_id = payerId };
			Payment payment = new Payment() { id = paymentId };
			Payment executedPayment = payment.Execute(apiContext, paymentExecution);
			return executedPayment;
		}

		public static string GetRandomInvoiceNumber() =>
			new Random().Next(999999).ToString();
	}
}