let menu = new Vue({
	el: "#sidebar",
	data() {
		return {
			homeButton: [
				{ text: "Начало", icon: "fas fa-home", link: "/Panel" }
			],
			clientDropdown: [
				{ text: "VPS Сървъри", link: "/VPS" },
				{ text: "Билети", link: "/Tickets" },
				{ text: "Форум", link: "/Posts" },
			],
			buyButton: [
				{ text: "Купи VPS Сървър", icon: "fas fa-server", link: "/" }
			],
			mainButton: [
				{ text: "Главен сайт", icon: "fas fa-link", link: "https://uhost.pw", target: "_blank" }
			],
			helpButtons: [
				{ text: "Discord Сървър", link: "https://discord.uhost.pw/", target: "_blank" },
				{ text: "Support Билети", link: "https://uhost.pw/clientarea/submitticket.php", target: "_blank" }
			]
		};
	},
});