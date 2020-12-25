const discOperationsCtx = document.querySelector("#disc-operations").getContext("2d");
const discOperationsChart = new Chart(discOperationsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "четене",
				data: [],
				backgroundColor: "#3b7ddd88",
				borderColor: "#3b7ddd",
				borderWidth: 2
			},
			{
				label: "записване",
				data: [],
				backgroundColor: "#28a74588",
				borderColor: "#28a745",
				borderWidth: 2
			},
		]
	},
	options: {
		scales: {
			yAxes: [{
				ticks: {
					callback: value => value + " IOP/s",
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}]
		}
	}
});