import "./cpu.js";
import "./discTransfer.js";
import "./discOperations.js";
import "./networkTransfer.js";
import "./networkPackets.js";

window.SetupChartUpdate = (vpsId, admin) => {
	let url = (admin ? "/Admin/" : "/") + `VPS/State?Id=${vpsId}&Step=1`;
	setInterval(() => {
		fetch(url)
		.then(response => response.json())
		.then(json => {
			let data = json[0];
			cpuChart.data.labels.shift();
			discTransferChart.data.labels.shift();
			discOperationsChart.data.labels.shift();
			networkTransferChart.data.labels.shift();
			networkPacketsChart.data.labels.shift();

			cpuChart.data.datasets[0].data.shift();
			discTransferChart.data.datasets[0].data.shift();
			discTransferChart.data.datasets[1].data.shift();
			discOperationsChart.data.datasets[0].data.shift();
			discOperationsChart.data.datasets[1].data.shift();
			networkTransferChart.data.datasets[0].data.shift();
			networkTransferChart.data.datasets[1].data.shift();
			networkPacketsChart.data.datasets[0].data.shift();
			networkPacketsChart.data.datasets[1].data.shift();

			cpuChart.data.datasets[0].data.push({ y: (data.CPU * Math.pow(10,-14)).toFixed(2) });
			discTransferChart.data.datasets[0].data.push({ y: (data.diskRead / Math.pow(1024, 2)).toFixed(2) });
			discTransferChart.data.datasets[1].data.push({ y: (data.diskWrite / Math.pow(1024, 2)).toFixed(2) });
			discOperationsChart.data.datasets[0].data.push({ y: data.operationsRead.toFixed(2) });
			discOperationsChart.data.datasets[1].data.push({ y: data.operationsWrite.toFixed(2) });
			networkTransferChart.data.datasets[0].data.push({ y: (data.networkIn / Math.pow(1024, 2)).toFixed(2) });
			networkTransferChart.data.datasets[1].data.push({ y: (data.networkOut / Math.pow(1024, 2)).toFixed(2) });
			networkPacketsChart.data.datasets[0].data.push({ y: data.packetsIn.toFixed(2) });
			networkPacketsChart.data.datasets[1].data.push({ y: data.packetsOut.toFixed(2) });

			cpuChart.update();
			if (window.manage.discChart == 0)
				discTransferChart.update();
			if (window.manage.discChart == 1)
				discOperationsChart.update();
			if (window.manage.networkChart == 0)
				networkTransferChart.update();
			if (window.manage.networkChart == 1)
				networkPacketsChart.update();
		});
	}, 15000);
}
