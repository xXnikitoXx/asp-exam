const cpuStatsCtx = document.querySelector("#cpu-stats").getContext("2d");
const cpuChart = new Chart(cpuStatsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "",
				data: [],
				backgroundColor: "#dc354577",
				borderColor: "#dc3545",
				borderWidth: 2
			}
		]
	},
	options: {
		scales: {
			yAxes: [{
				ticks: {
					min: 0,
					max: 100,
					callback: value => value + "%",
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}]
		}
	}
});