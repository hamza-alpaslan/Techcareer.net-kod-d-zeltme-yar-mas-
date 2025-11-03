namespace CourseApp.ServiceLayer.Utilities.Result;

public class DataResult<T> : Result, IDataResult<T>
{
    public T? Data { get; }



    public DataResult(T data,bool isSuccess):base(isSuccess)
    {
        Data = data;    
    }

    public DataResult(T data,string message):base(default,message)
    {
        Data = data;  
    }

    public DataResult(T data,bool isSuccess,string message):base(isSuccess,message)
    {
        Data = data;    
    }
}
