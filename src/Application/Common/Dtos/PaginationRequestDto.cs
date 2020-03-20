namespace Application.Common.Dtos
{
    public class PaginationRequestDto
    {
        public int NumberPage { get; set; } = 1;

        public int PageSize { get; set; } = 15;
    }
}