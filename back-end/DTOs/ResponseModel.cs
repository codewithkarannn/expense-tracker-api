namespace Budget_Tracker_WebAPI.DTOs
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }

        // Constructor for success response
        public ResponseModel(T data, string message = "", int statusCode = 200)
        {
            Success = true;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        // Constructor for error response
        public ResponseModel(string message, int statusCode = 400)
        {
            Success = false;
            Message = message;
            Data = default;
            StatusCode = statusCode;
        }
    }

    public class PaginatedResponseModel<T>
    {
        public List<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }


        public PaginatedResponseModel(List<T> data, int currentPage, int pageSize, int totalItems)
        {
            Data = data;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }

    public class TransactionQueryParameters
    {
        // Pagination parameters with defaults
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Sorting parameters with defaults
        public string? SortColumn { get; set; } = "transactionDate";
        public string? SortDirection { get; set; } = "desc";

        // Filtering parameters (all nullable)
        public string? FilterType { get; set; }
        public string? FilterCategory { get; set; }
        public string? SearchText { get; set; }

        // Use nullable types for dates and numbers
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
