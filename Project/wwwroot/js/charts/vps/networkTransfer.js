const networkTransferCtx = document.querySelector("#network-transfer").getContext("2d");
const networkTransferChart = new Chart(networkTransferCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "вход",
				data: [],
				backgroundColor: "#8dc7",
				borderColor: "#8dc",
				borderWidth: 2
			},
			{
				label: "изход",
				data: [],
				backgroundColor: "#ffc10777",
				borderColor: "#ffc107",
				borderWidth: 2
			},
		]
	},
	options: {
		scales: {
			yAxes: [{
				ticks: {
					callback: value => value + " MBi/s",
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}]
		}
	}
});