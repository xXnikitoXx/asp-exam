const MessageToUI = message => {
	let title = "Ново съобщение";
	let avatar = "SYS";
	let content = message.content;
	let status = "secondary";
	switch (message.status) {
		case 1: status = "primary"; break;
		case 2: status = "success"; break;
		case 3: status = "warning"; break;
		case 4: status = "danger"; break;
	}
	if (content.length > 35)
		content = content.substring(0, 35) + "...";
	if (message.username != null) {
		title = "От: " + message.username;
		avatar = (message.username[0] + message.username.split("").reverse()[0]).toUpperCase();
	}
	return {
		url: message.url,
		time: new Date(message.time),
		status,
		content,
		title,
		avatar
	};
};

const FetchMessages = async () => {
	if (!sessionStorage["messages"])
		sessionStorage["messages"] = "[]";
	if (!sessionStorage["messagesTimestamp"])
		sessionStorage["messagesTimestamp"] = Date.now();
	else if (Date.now() - Number(sessionStorage["messagesTimestamp"]) <= 10000)
		return;
	sessionStorage["messagesTimestamp"] = Date.now();
	let response = await fetch("/Notifications/Messages/Last?Count=4");
	let json = await response.json();
	sessionStorage["messages"] = JSON.stringify(json.map(MessageToUI));
};

const RegisterMessage = message => {
	new Promise(async resolve => {
		await LoadMessages();
		resolve();
	})
	.then(() => {
		navbar.messagesSeen = false;
		SaveMessages();
	});
};

const LoadMessages = async () => {
	await FetchMessages();
	navbar.messages = JSON.parse(sessionStorage["messages"]);
}

const SaveMessages = () => sessionStorage["messages"] = JSON.stringify(navbar.messages);

const connection = new signalR.HubConnectionBuilder()
	.withUrl("/Messages")
	.build();

const StartMessageConnection = async () => {
	try { await connection.start(); }
	catch { setTimeout(StartMessageConnection, 5000); }
};

connection.on("ReceiveMessage", RegisterMessage);
connection.onclose(StartMessageConnection);

StartMessageConnection();
LoadMessages();

$("#messagesDropdown").click(() => navbar.messagesSeen = true);