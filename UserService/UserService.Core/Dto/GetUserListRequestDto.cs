namespace UserService.Core.Dto
{
    public class GetUserListRequestDto
    {
        // search criteria
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ZipCode { get; set; }

        // pagination & sorting
        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public string? SortOrder { get; set; }
        
        public string? SortColumn { get; set; }
    }
}
