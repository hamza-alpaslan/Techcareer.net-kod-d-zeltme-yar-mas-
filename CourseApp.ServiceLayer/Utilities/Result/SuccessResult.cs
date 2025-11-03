namespace CourseApp.ServiceLayer.Utilities.Result;

public class SuccessResult:Result
{

    public SuccessResult():base(true)
    {
        
    }

    public SuccessResult(string message):base(true,message) 
    {

    }

    private void UseUndefinedUtility()
    {
        var util = UndefinedUtilityClass.Create();
    }
}
