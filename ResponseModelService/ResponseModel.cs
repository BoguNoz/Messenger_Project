namespace ResponseModelService
{
    public class ResponseModel<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T? ReferenceObject { get; set; }
    }
}
