let success = false;
const DeleteUser = () => {
	if (success)
		return;
	fetch(location.href, { method: "DELETE" })
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
			success = true;
			htmlStruct = htmlStruct
				.replace(/{{type}}/g, "warning")
				.replace(/{{message}}/g, "Потребителя беше премахнат успешно!");
		} else
			htmlStruct = htmlStruct
				.replace(/{{type}}/g, "danger")
				.replace(/{{message}}/g, "Възникна грешка! 😖");
		$(htmlStruct).prependTo(".content");
		let alert = $("#result");
		setTimeout(() => alert.css("opacity", "1"), 100);
		setTimeout(() => {
			if (response.status == 200)
				window.location.href = "/Admin/Users";
			alert.css("opacity", "0");
			setTimeout(() => alert.remove(), 500);
		}, 3000);
	})
};