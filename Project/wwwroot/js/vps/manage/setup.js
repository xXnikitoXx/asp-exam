export const setupModule = parent => ({
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
		debian: [ 10, 9 ],
		fedora: [ 33, 32 ],
		centos: [ 8, 7 ],
	},
	lastSelected: {
		ubuntu: 0,
		debian: 0,
		fedora: 0,
		centos: 0,
	},
	changeName: () => parent.name = parent.name.replace(/([^a-zA-Z0-9\.]|\ )/g, ""),
	setup: () => {
		parent.processing = true;
		fetch(window.location.href, {
			method: "POST",
			body: parent.JsonToForm({
				Name: parent.name,
				Distro: parent.distro,
				Version: parent.version,
			})
		})
		.then(response => {
			if (response.status != 200) {
				parent.processing = false;
				let htmlStruct = '<div id="result" class="alert alert-danger alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;"><div class="alert-icon"><i class="fa fa-info"></i></div><div class="alert-message">Възникна грешка! 😖</div></div>';
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
					parent.processing = false;
					parent.password = json[0];
					parent.ip = json[1];
					parent.hiddenPassword = "*".repeat(parent.password.length);
				});
		});
	},
});