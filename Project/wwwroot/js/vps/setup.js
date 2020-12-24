const setup = new Vue({
	el: "#setup",
	data() {
		return {
			url: window.location.href,
			JsonToForm: window.JsonToForm,
			distro: "ubuntu",
			version: 20.04,
			defaultDistro: "ubuntu",
			defaultVersion: 20.04,
			name: "",
			ip: "",
			password: "",
			hiddenPassword: "",
			showPassword: false,
			processing: false,
			versions: {
				ubuntu: [ 20.04, 18.04, 16.04 ],
				debian: [ 9, 10 ],
				fedora: [ 32, 33 ],
				centos: [ 7, 8 ],
			},
			lastSelected: {
				ubuntu: 0,
				debian: 0,
				fedora: 0,
				centos: 0,
			},
			changeName: () => this.name = this.name.replace(/([^a-zA-Z0-9\.]|\ )/g, ""),
			setup: () => {
				this.processing = true;
				fetch(window.location.href, {
					method: "POST",
					body: this.JsonToForm({
						Name: this.name,
						Distro: this.distro,
						Version: this.version,
					})
				})
				.then(response => {
					if (response.status != 200) {
						this.processing = false;
						let htmlStruct = `<div id="result" class="alert alert-danger alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;">
							<div class="alert-icon">
								<i class="fa fa-info"></i>
							</div>
							<div class="alert-message">
								Възникна грешка! 😖
							</div>
						</div>`;
						$(htmlStruct).prependTo(".content");
						let alert = $("#result");
						setTimeout(() => alert.css("opacity", "1"), 100);
						setTimeout(() => {
							if (response.status == 200)
								window.location.href = "/Admin/Users";
							alert.css("opacity", "0");
							setTimeout(() => alert.remove(), 500);
						}, 3000);
					} else
						response.json()
						.then(json => {
							this.processing = false;
							this.password = json[0];
							this.ip = json[1];
							this.hiddenPassword = "*".repeat(this.password.length);
						});
				});
			},
		};
	},
});