import { xAxes } from "./timeAxes.js";
const discTransferCtx = document.querySelector("#disc-transfer").getContext("2d");
window.discTransferChart = new Chart(discTransferCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "четене",
				data: [],
				lineTension: 0,
				backgroundColor: "#c057",
				borderColor: "#c05",
				borderWidth: 2
			},
			{
				label: "записване",
				data: [],
				lineTension: 0,
				backgroundColor: "#0cf7",
				borderColor: "#0cf",
				borderWidth: 2
			},
		]
	},
	options: {
		animation: false,
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
					callback: value => value + " MB/s",
					stepSize: 1,
					min: 0
				},
				scaleLabel: {
					display: true,
					labelString: ""
				}
			}],
		}
	}
});