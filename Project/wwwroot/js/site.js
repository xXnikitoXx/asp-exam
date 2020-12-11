const JsonToForm = (json) => {
	let formData = new FormData();
	for (let key in json)
		formData.append(key, json[key]);
	return formData;
}