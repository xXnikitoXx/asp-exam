const order = new Vue({
	el: "#order",
	data() {
		return {
			SaveOrder: window.SaveOrder || (() => new Promise(resolve => resolve())),
			Having: (amount, price) => ({
				get Get() {
					let priceForOne = price / amount;
					return {
						PriceFor: amount => priceForOne * amount,
						AmountFor: price => price / priceForOne,
					};
				},
			}),
			update: (discount = null, keepSaved = false) => {
				this.midPrice = this.price * this.amount;
				let updateValues = (discount, keepSaved) => {
					this.saved = keepSaved;
					this.discount = discount;
					this.finalPrice = this.midPrice - this.discount;
				}
				if (discount == null || discount == -1)
					new Promise(async (resolve, reject) => {
						let response = await fetch(`/PromoCode/Discount?Price=${this.midPrice}&Codes=${encodeURIComponent(this.codes)}`);
						if (response.status == 200)
							return resolve(await response.json());
						reject(await response.text());
					})
					.then(d => updateValues(d, discount == -1 ? true : false))
					.catch(console.error);
				else updateValues(discount, keepSaved);
			},
			applyCodes: () => {
				this.SaveOrder()
				.then(() => {
					let input = $("#codesInput");
					let codes = [ input.val() ];
					new Promise(async (resolve, reject) => {
						let response = await fetch("/Order/SetCodes", {
							method: "POST",
							body: JsonToForm({
								orderId: this.id,
								codes
							})
						});
						if (response.status == 200)
							return resolve(await response.json());
						reject(await response.text());
					})
					.then(discount => {
						if (discount == 0 && codes.length != 0)
							return $("#codeError").show();
						input.val("");
						this.codes = codes;
						$("#codeError").hide();
						this.update(discount, true);
					})
					.catch(console.error);
				});
			},
			removeCodes: () => {
				new Promise(async (resolve, reject) => {
					await this.SaveOrder();
					let response = await fetch("/Order/SetCodes", {
						method: "POST",
						body: JsonToForm({
							orderId: this.id,
							codes: [],
						})
					});
					if (response.status == 200)
						return resolve(await response.json());
					reject(await response.text());
				})
				.then(() => {
					this.codes = [];
					this.update(-1);
				})
				.catch(console.error);
			},
			id: null,
			showCodeError: false,
			saved: true,
			amount: 0,
			price: 0,
			midPrice: 0,
			discount: 0,
			finalPrice: 0,
			location: null,
			codes: [],
		};
	},
});