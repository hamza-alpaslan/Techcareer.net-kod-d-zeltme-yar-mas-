namespace CourseApp.ServiceLayer.Utilities.Result;

public class SuccessDataResult<T>:DataResult<T>    
{

    public SuccessDataResult(T data):base(data,true,default)
    {
        
    }
    public SuccessDataResult(T data,string message):base(data,true,message)
    {
        
    }

}
