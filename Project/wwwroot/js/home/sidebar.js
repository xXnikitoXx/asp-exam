let menu = new Vue({
	el: "#sidebar",
	data() {
		return {
			get user() { return user; },
			homeButton: [
				{ text: "Начало", icon: "fas fa-home", link: "/panel", active: false }
			],
			clientDropdown: [
				{ text: "VPS Servers", link: "/vps", active: false }
			],
			buyButton: [
				{ text: "Купи VPS Сървър", icon: "fas fa-server", link: "/", active: false }
			],
			mainButton: [
				{ text: "Главен сайт", icon: "fas fa-link", link: "https://uhost.pw", target: "_blank", active: false }
			],
			helpButtons: [
				{ text: "Discord Сървър", link: "https://discord.uhost.pw/", target: "_blank", active: false },
				{ text: "Support Билети", link: "https://uhost.pw/clientarea/submitticket.php", target: "_blank", active: false }
			]
		};
	},
});