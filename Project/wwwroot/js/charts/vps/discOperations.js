import { xAxes } from "./timeAxes.js";
const discOperationsCtx = document.querySelector("#disc-operations").getContext("2d");
window.discOperationsChart = new Chart(discOperationsCtx, {
	type: "line",
	data: {
		labels: [
			
		],
		datasets: [
			{
				label: "четене",
				data: [],
				lineTension: 0,
				backgroundColor: "#3b7ddd88",
				borderColor: "#3b7ddd",
				borderWidth: 2
			},
			{
				label: "записване",
				data: [],
				lineTension: 0,
				backgroundColor: "#28a74588",
				borderColor: "#28a745",
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
					callback: value => value + " IOP/s",
					stepSize: 1,
					min: 0
				},
				scaleLabel: {
					display: true,
					labelString: ""
				},
			}],
		}
	}
});