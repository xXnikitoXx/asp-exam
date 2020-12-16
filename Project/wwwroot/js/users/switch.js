const adminsLabel = $(".badge.bg-primary");
const usersLabel = $(".badge.bg-secondary");

const SwitchRole = (event, id) => {
	event.preventDefault();
	let target = $(event.target);
	let checkboxes = $(".form-check-input");
	checkboxes.attr("disabled", true);
	fetch("/Admin/User/Switch", {
		method: "POST",
		body: JsonToForm({ id }),
	}).then(response => {
		setTimeout(() => checkboxes.attr("disabled", false), 250);
		if (response.status == 200)
			response.json()
			.then(json => {
				target.prop("checked", json);
				if (adminsLabel.length > 0 && usersLabel.length > 0) {
					let adminsText = adminsLabel.text().split(" - ");
					let usersText = usersLabel.text().split(" - ");
					let adminsCount = Number(adminsText[1]);
					let usersCount = Number(usersText[1]);
					adminsLabel.text(adminsText[0] + " - " + (adminsCount + (json ? 1 : -1)));
					usersLabel.text(usersText[0] + " - " + (usersCount + (json ? -1 : 1)));
				}
			});
		else {
			let htmlStruct = `<div id="result" class="alert alert-danger alert-dismissible" role="alert" style="transition: all .25s ease-in-out; opacity: 0;">
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
		}
	});
}