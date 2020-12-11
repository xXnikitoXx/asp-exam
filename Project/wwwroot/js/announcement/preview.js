$("#preview").on("shown.bs.modal", () => $("#preview").focus());

const PreviewAnnouncement = (title, content, type) => {
	type = type.toLowerCase();
	$(".modal-content").attr("class", `modal-content alert-${type} bg-${type}`);
	$("#preview .modal-title").text(title);
	$("#preview .modal-body").html(content);
};