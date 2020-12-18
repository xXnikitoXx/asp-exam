let success = false;

const SendMessage = () => {
	if (success)
		return;
	fetch(location.href, {
		method: "POST",
		body: JsonToForm({
			URL: $("#urlInput").val(),
			Status: $("#statusInput").val(),
			Content: $("#contentInput").val(),
		})
	})
	.then(response => {
		$("#result").remove();
		let htmlStruct = `<div id="result" class="alert alert-{{type}} alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;">
			<div class="alert-icon">
				<i class="fa fa-info"></i>
			</div>
			<div class="alert-message">
				{{message}}
			</div>
		</div>`;

		if (response.status == 200) {
			htmlStruct = htmlStruct
				.replace(/{{type}}/g, "success")
				.replace(/{{message}}/g, "Съобщението беше изпратено успешно! 🤩");
			success = true;
		}
		else
			htmlStruct = htmlStruct
				.replace(/{{type}}/g, "danger")
				.replace(/{{message}}/g, "Възникна грешка! 😖");
		$(htmlStruct).prependTo(".content");
		let alert = $("#result");
		setTimeout(() => alert.css("opacity", "1"), 100);
		setTimeout(() => {
			alert.css("opacity", "0");
			setTimeout(() => alert.remove(), 500);
		}, 5000);
	});
};