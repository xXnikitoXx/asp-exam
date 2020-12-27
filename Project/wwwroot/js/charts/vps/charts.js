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
			let time = new Date(data.time);
			cpuChart.data.labels.shift();
			discTransferChart.data.labels.shift();
			discOperationsChart.data.labels.shift();
			networkTransferChart.data.labels.shift();
			networkPacketsChart.data.labels.shift();

			cpuChart.data.labels.push(time);
			discTransferChart.data.labels.push(time);
			discOperationsChart.data.labels.push(time);
			networkTransferChart.data.labels.push(time);
			networkPacketsChart.data.labels.push(time);

			cpuChart.data.datasets[0].data.shift();
			discTransferChart.data.datasets[0].data.shift();
			discTransferChart.data.datasets[1].data.shift();
			discOperationsChart.data.datasets[0].data.shift();
			discOperationsChart.data.datasets[1].data.shift();
			networkTransferChart.data.datasets[0].data.shift();
			networkTransferChart.data.datasets[1].data.shift();
			networkPacketsChart.data.datasets[0].data.shift();
			networkPacketsChart.data.datasets[1].data.shift();

			cpuChart.data.datasets[0].data.push({ t: time, y: (data.CPU * Math.pow(10,-14)).toFixed(2) });
			discTransferChart.data.datasets[0].data.push({ t: time, y: (data.diskRead / Math.pow(1024, 2)).toFixed(2) });
			discTransferChart.data.datasets[1].data.push({ t: time, y: (data.diskWrite / Math.pow(1024, 2)).toFixed(2) });
			discOperationsChart.data.datasets[0].data.push({ t: time, y: data.operationsRead.toFixed(2) });
			discOperationsChart.data.datasets[1].data.push({ t: time, y: data.operationsWrite.toFixed(2) });
			networkTransferChart.data.datasets[0].data.push({ t: time, y: (data.networkIn / Math.pow(1024, 2)).toFixed(2) });
			networkTransferChart.data.datasets[1].data.push({ t: time, y: (data.networkOut / Math.pow(1024, 2)).toFixed(2) });
			networkPacketsChart.data.datasets[0].data.push({ t: time, y: data.packetsIn.toFixed(2) });
			networkPacketsChart.data.datasets[1].data.push({ t: time, y: data.packetsOut.toFixed(2) });

			cpuChart.update();
			discTransferChart.update();
			discOperationsChart.update();
			networkTransferChart.update();
			networkPacketsChart.update();
		});
	}, 15000);
}
