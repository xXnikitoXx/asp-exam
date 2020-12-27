import { xAxes } from "./timeAxes.js";
const networkTransferCtx = document.querySelector("#network-transfer").getContext("2d");
window.networkTransferChart = new Chart(networkTransferCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "вход",
				data: [],
				lineTension: 0,
				backgroundColor: "#8dc7",
				borderColor: "#8dc",
				borderWidth: 2
			},
			{
				label: "изход",
				data: [],
				lineTension: 0,
				backgroundColor: "#ffc10777",
				borderColor: "#ffc107",
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
				ticks: {
					callback: value => value + " MBi/s",
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