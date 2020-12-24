const setup = new Vue({
	el: "#setup",
	data() {
		return {
			distro: "ubuntu",
			version: 20.04,
			defaultDistro: "ubuntu",
			defaultVersion: 20.04,
			name: null,
			versions: {
				ubuntu: [ 20.04, 18.04, 16.04 ],
				debian: [ 9, 10 ],
				fedora: [ 32, 33 ],
				centos: [ 7, 8 ],
			},
			lastSelected: {
				ubuntu: 0,
				debian: 0,
				fedora: 0,
				centos: 0,
			},
		};
	},
});