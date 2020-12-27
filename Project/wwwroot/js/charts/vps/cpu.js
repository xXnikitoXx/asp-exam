import { xAxes } from "./timeAxes.js";
const cpuStatsCtx = document.querySelector("#cpu-stats").getContext("2d");
window.cpuChart = new Chart(cpuStatsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "",
				data: [],
				lineTension: 0,
				backgroundColor: "#dc354577",
				borderColor: "#dc3545",
				borderWidth: 2
			}
		]
	},
	options: {
		elements: {
			lines: {
				bezierCurve: false,
				tension: 0,
			},
		},
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
			}],
			xAxes,
		}
	}
});