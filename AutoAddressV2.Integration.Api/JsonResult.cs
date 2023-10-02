namespace AutoAddressV2.Integration.Api;
public class JsonResult : JsonResult<object>
{
    public JsonResult() { }
    public JsonResult(bool success) : base(success) { }
}
public class JsonResult<T> 
{
    public bool Success { get; set; }
    public T Result { get; set; }
    public string Error { get; set; }

    public JsonResult() { }
    public JsonResult(bool success) { Success = true; }

    public int StatusCode { get; set; } = 0;
}