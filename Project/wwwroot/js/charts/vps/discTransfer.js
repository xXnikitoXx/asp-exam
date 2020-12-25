const discTransferCtx = document.querySelector("#disc-transfer").getContext("2d");
const discTransferChart = new Chart(discTransferCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "четене",
				data: [],
				backgroundColor: "#c057",
				borderColor: "#c05",
				borderWidth: 2
			},
			{
				label: "записване",
				data: [],
				backgroundColor: "#0cf7",
				borderColor: "#0cf",
				borderWidth: 2
			},
		]
	},
	options: {
		scales: {
			yAxes: [{
				ticks: {
					callback: value => value + " MB/s",
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}]
		}
	}
});