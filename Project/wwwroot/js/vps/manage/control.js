export const control = parent => ({
	serverId: "",
	status: "unknown",
	controlUrl: "",
	powerOn: action => {
		fetch(parent.controlUrl, {
			method: "POST",
			body: parent.JsonToForm({
				id: parent.serverId,
				action
			}),
		})
		.then(response => {
			console.log(response.status);
		});
	}
});