public interface ILogWriter
{
    string Name {get;}
    void LogError(string message);
    void LogInfo(string message);
}

public class ConsoleLogWriter : ILogWriter
{
    public string Name { get { return "Console"; } }
    public void LogError(string message)
    {
        Console.WriteLine("Error: " + message);
    }

    public void LogInfo(string message)
    {
        Console.WriteLine("Info: " + message);
    }
}

public class ColorfulConsoleLogWriter : ConsoleLogWriter
{
    public override void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        base.LogError(message);
        Console.ResetColor();
    }

    public override void LogInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        base.LogInfo(message);
        Console.ResetColor();
    }
}

public class FileLogWriter : ILogWriter
{
    public string Name { get { return "File"; } }
    public void LogError(string message)
    {
        // Pretend
        Console.WriteLine("FileError: " + message);
    }
    
    public void LogInfo(string message)
    {
        // Pretend
        Console.WriteLine("FileInfo: " + message);
    }
}

public class ThingThatDoesStuff
{
    public ThingThatDoesStuff(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public void DoStuff()
    {
        _logWriter.LogInfo("Stuff was done");
    }
}

public class Program
{
    public static void Main()
    {
        ILogWriter logWriter = new FileLogWriter();
        ThingThatDoesStuff thing = new ThingThatDoesStuff(logWriter);
        thing.DoStuff();
    }
}