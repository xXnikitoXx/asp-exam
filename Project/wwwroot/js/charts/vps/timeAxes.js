export const xAxes = [{
	type: "time",
	ticks: {
		callback: (value, index, values) => (index == (values.length - 1)) ? undefined : value,
		get max() { return new Date(); },
	},
	time: {
		unit: "second",
		unitStepSize: 15,
		displayFormats: {
			second: "HH:mm"
		}
	}
}];