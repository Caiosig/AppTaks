namespace Application.Utils
{
    public record QueryBase
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
