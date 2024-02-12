namespace Hangfire.Services;

public class ServiceManagement : IServicesManagement
{
    public void SendEmail()
    {
        Console.WriteLine($"Send Email : short running Task {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
    }

    public void UpdateDatabase()
    {
        Console.WriteLine($"Update Database : Long running Task {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
    }

    public void GenerateDatabase()
    {
        Console.WriteLine($"Generate Database : Long running Task {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
    }

    public void SyncData()
    {
        Console.WriteLine($"Sync Data : short running Task {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
    }
}