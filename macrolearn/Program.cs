using System.Runtime.CompilerServices;


[AttributeUsage(AttributeTargets.Method)]
public class PrintFunctionNamesAttribute : Attribute { }

public static class FunctionLogger
{
    public static void ExecuteWithLogging(Action action, [CallerMemberName] string callerName = "")
    {
        var actionMethod = action.Method;
        Console.WriteLine($"\"{callerName}\"\nCalled {actionMethod.Name}");
        action.Invoke();
    }
}


public class Program
{
    [PrintFunctionNames]
    public static void MyFunction()
    {
        Console.WriteLine("Inside MyFunction.");
    }

    public static void Main()
    {
        FunctionLogger.ExecuteWithLogging(MyFunction);
    }
}
