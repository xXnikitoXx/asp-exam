namespace Project.ViewModels {
	public abstract class ListViewModel
	{
		public int Page { get; set; }
		public int Pages { get; set; }
		public int Total { get; set; }
		public int Show { get; set; }
	}
}