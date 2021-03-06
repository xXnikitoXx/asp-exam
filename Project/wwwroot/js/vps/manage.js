import { charts } from "./manage/charts.js";
import { tabs } from "./manage/tabs.js";
import { setupModule } from "./manage/setup.js";
import { control } from "./manage/control.js";

window.manage = new Vue({
	el: "#manage",
	data() {
		return {
			Vue,
			url: window.location.href,
			JsonToForm: window.JsonToForm,
			...charts,
			...tabs,
			...setupModule(this),
			...control(this),
		};
	},
})