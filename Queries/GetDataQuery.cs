namespace Queries
{
    public class GetDataQuery
    {
        public string? Match { get; set; }
        public string? CollectionName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int SortOrder { get; set; }
        public string? SortKey { get; set; }

        public GetDataQuery()
        {
            this.PageNumber = 0;
            this.PageSize = 10;
        }
    }
}