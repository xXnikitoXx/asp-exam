const SignOut = (id) => {
	fetch("/Admin/User/SignOut", {
		method: "POST",
		body: JsonToForm({ id }),
	}).then(response => {
		let htmlStruct;
		if (response.status == 200)
			htmlStruct = `<div id="result" class="alert alert-success alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;">
				<div class="alert-icon">
					<i class="fa fa-info"></i>
				</div>
				<div class="alert-message">
					Сесията на потребителя беше обновена! 🤩
				</div>
			</div>`;
		else
			htmlStruct = `<div id="result" class="alert alert-danger alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;">
				<div class="alert-icon">
					<i class="fa fa-info"></i>
				</div>
				<div class="alert-message">
					Възникна грешка! 😖
				</div>
			</div>`;
		$(htmlStruct).prependTo(".content");
		let alert = $("#result");
		setTimeout(() => alert.css("opacity", "1"), 100);
		setTimeout(() => {
			alert.css("opacity", "0");
			setTimeout(() => alert.remove(), 500);
		}, 5000);
	});
}