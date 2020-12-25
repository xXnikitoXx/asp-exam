import { setupModule } from "./manage/setup.js";

window.setup = new Vue({
	el: "#setup",
	data() {
		return {
			url: window.location.href,
			JsonToForm: window.JsonToForm,
			...setupModule(this),
		};
	},
});