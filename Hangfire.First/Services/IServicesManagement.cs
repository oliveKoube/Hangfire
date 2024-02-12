namespace Hangfire.Services;

public interface IServicesManagement
{
    void SendEmail();
    void UpdateDatabase();
    void GenerateDatabase();
    void SyncData();
}