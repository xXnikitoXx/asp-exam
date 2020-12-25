const networkPacketsCtx = document.querySelector("#network-packets").getContext("2d");
const networkPacketsChart = new Chart(networkPacketsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "вход",
				data: [],
				backgroundColor: "#3f67",
				borderColor: "#3f6",
				borderWidth: 2
			},
			{
				label: "изход",
				data: [],
				backgroundColor: "#f35b0477",
				borderColor: "#f35b04",
				borderWidth: 2
			},
		]
	},
	options: {
		scales: {
			yAxes: [{
				ticks: {
					callback: value => value + " P/s",
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}]
		}
	}
});