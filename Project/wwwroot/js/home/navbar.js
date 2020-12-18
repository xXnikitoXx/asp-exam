const navbar = new Vue({
	el: "#navbar",
	data() {
		return {
			allMessages: "/Notifications/Messages",
			messages: [],
			messagesSeen: true,
		};
	},
});