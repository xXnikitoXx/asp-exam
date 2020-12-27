import { xAxes } from "./timeAxes.js";
const networkPacketsCtx = document.querySelector("#network-packets").getContext("2d");
window.networkPacketsChart = new Chart(networkPacketsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "вход",
				data: [],
				lineTension: 0,
				backgroundColor: "#3f67",
				borderColor: "#3f6",
				borderWidth: 2
			},
			{
				label: "изход",
				data: [],
				lineTension: 0,
				backgroundColor: "#f35b0477",
				borderColor: "#f35b04",
				borderWidth: 2
			},
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
				beginAtZero: true,
				ticks: {
					callback: value => value + " P/s",
					stepSize: 1,
					min: 0
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