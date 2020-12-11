const activeLabel = $(".badge.bg-success");
const inactiveLabel = $(".badge.bg-danger");

const SwitchState = (event, id) => {
	event.preventDefault();
	let target = $(event.target);
	let checkboxes = $(".form-check-input");
	checkboxes.attr("disabled", true);
	fetch("/Admin/Codes/Switch", {
		method: "POST",
		body: JsonToForm({ id }),
	}).then(response => {
		setTimeout(() => checkboxes.attr("disabled", false), 250);
		if (response.status == 200)
			response.json()
			.then(json => {
				target.prop("checked", json);
				let activeText = activeLabel.text().split(" - ");
				let inactiveText = inactiveLabel.text().split(" - ");
				let activeCount = Number(activeText[1]);
				let inactiveCount = Number(inactiveText[1]);
				activeLabel.text(activeText[0] + " - " + (activeCount + (json ? 1 : -1)));
				inactiveLabel.text(inactiveText[0] + " - " + (inactiveCount + (json ? -1 : 1)));
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