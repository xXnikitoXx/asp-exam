plans = new Vue({
	el: "#plans",
	data() {
		return {
			plans: [
				{
					id: "0",
					name: "VPS-01",
					cores: 1,
					ram: 2,
					ssd: 20,
					price: 9.50,
				},
				{
					id: "1",
					name: "VPS-02",
					cores: 2,
					ram: 2,
					ssd: 40,
					price: 12,
				},
				{
					id: "2",
					name: "VPS-03",
					cores: 2,
					ram: 4,
					ssd: 40,
					price: 15,
				},
				{
					id: "3",
					name: "VPS-04",
					cores: 3,
					ram: 4,
					ssd: 80,
					price: 20,
				},
				{
					id: "4",
					name: "VPS-05",
					cores: 4,
					ram: 8,
					ssd: 160,
					price: 35,
				},
				{
					id: "5",
					name: "VPS-06",
					cores: 8,
					ram: 16,
					ssd: 240,
					price: 60,
				},
			]
		};
	},
})