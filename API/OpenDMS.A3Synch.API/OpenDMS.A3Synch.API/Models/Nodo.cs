namespace OpenDMS.A3Synch.API.Models
{
	public class Nodo
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }
		public int LeftBound { get; set; }
		public int RightBound { get; set; }
		public List<Nodo> Figli { get; set; } = new List<Nodo>();
		public string? oldId { get; set; }
	}
}
