namespace Project.ViewModels {
	public class ServerDataViewModel {
		public string Id { get; set; }
		public string ExternalId { get; set; }
		public string Status { get; set; }
		public string IPv4 { get; set; }
		public string IPv4DNSPointer { get; set; }
		public bool IPv4Blocked { get; set; }
		public string IPv6 { get; set; }
		public string IPv6DNSPointer { get; set; }
		public bool IPv6Blocked { get; set; }
		public string Distro { get; set; }
		public string DistroVersion { get; set; }
	}
}