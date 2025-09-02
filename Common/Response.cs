public class Response<T>
{
    public bool Status { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Object { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public long TotalCount { get; set; } = 0;
}