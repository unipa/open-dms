using OpenDMS.Domain.Enumerators;
using OpenDMS.Domain.Models;

namespace Web.DTOs
{
    public class ReportFile
	{
        public string Icon { get; set; }
        public string Title { get; set; }
        public String Description { get; set; }
		public List<ReportView> Views { get; set; } = new();
		public bool DetailPanel { get; set; } = true;

    }

	public class ReportView
	{
        public string Id { get; set; }
        public string Title { get; set; }
		public int Width { get; set; } = 6;
		public ViewStyle DefaultViewMode { get; set; } = ViewStyle.List;
		public List<ViewStyle> ViewModes { get; set; } = new() { ViewStyle.List };
		public int PageSize { get; set; } = 0;
		public bool Selectable { get; set; } = true;
		public string DefaultRowAction { get; set; } = "";
		public string UserFilters { get; set; } = "";
		public string Keys { get; set; } = "";
        public List<ReportViewColumn> Columns { get; set; } = new();
        public List<SearchFilter> Filters { get; set; } = new();
        public List<ReportViewAction> Actions { get; set; }

    }

	public class ReportViewColumn
	{
        public string ColumnName { get; set; }
        public string Title { get; set; }
        public SortingType SortType { get; set; }
        public AggregateType AggregateType { get; set; }

    }
    public class ReportViewAction
    {
        public string Icon { get; set; } = "";
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public string Action { get; set; } = "";
        public int SelectionType { get; set; }

    }


	
}
