const parent = "div.main";
const destination = "main.content";

const titleInput = $("#titleInput");
const contentInput = $("#contentInput");
const typeInput = $("#typeInput");

const TestAnnouncement = () => {
	$(`${parent} > .alert`).remove();
	let htmlStruct =
	`<div class="alert alert-${typeInput.val().toLowerCase()}">
		<div class="alert-message">
			<span style="font-weight: bold;">${titleInput.val()}</span>
			${contentInput.val()}
		</div>
	</div>`;
	$(htmlStruct).insertBefore(destination);
};