namespace FWO.Data.Report
{
    public abstract class RowBase { }

    public class GroupHeaderRow : RowBase
    {
        public string GroupKey { get; set; }
        public bool IsCollapsed { get; set; } = false;
    }

    public class DataRow : RowBase
    {
        public Rule Rule { get; set; }
        public GroupHeaderRow ParentGroup { get; set; }
    }
}
