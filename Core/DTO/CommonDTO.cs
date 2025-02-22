namespace Core.DTO
{
    public class CommonDTO
    {
        public CustomResponseStatus Status { get; set; }
        public CommonView View { get; set; } = new CommonView();
    }
    public class CommonContentDTO<T>
    {
        public CustomResponseStatus Status { get; set; }
        public CommonContentView<T> View { get; set; } = new CommonContentView<T>();
    }

    public class CommonListDTO<T>
    {
        public CustomResponseStatus Status { get; set; }
        public CommonListView<T> View { get; set; } = new CommonListView<T>();
    }

    public class CommonView 
    {
        public string Message { get; set; }
    }

    public class CommonContentView<T> : CommonView
    {
        public T Content { get; set; }
    }

    public class CommonListView<T> : CommonView
    {
        public List<T> Content { get; set; }
    }

    public enum CustomResponseStatus
    {
        OK,
        NoContent,
        BadRequest,
        Unauthorized,
    }
}
