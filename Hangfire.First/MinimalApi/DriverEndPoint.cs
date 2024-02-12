using Carter;
using Hangfire.Filters;
using Hangfire.Models;
using Hangfire.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hangfire.MinimalApi;

public class DriverEndPoint : CarterModule
{
    private static List<Driver> drivers = new List<Driver>();
    public DriverEndPoint() : base("/drivers")
    {

    }
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", AddDrivers).AddEndpointFilter<PostValidationFilter>();
        app.MapGet("/{id}", GetDriver).WithName("GetDriver");
        app.MapDelete("/{id}", DeleteDriver);
    }

    private IResult DeleteDriver(Guid id)
    {
        var driver = drivers.FirstOrDefault(o => o.Id == id);
        if(driver == null) Results.NotFound();
        driver.Status = 0;
        var jobId = BackgroundJob.Enqueue<IServicesManagement>(x => x.UpdateDatabase());
        return Results.NoContent();
    }

    private IResult GetDriver(Guid id)
    {
        var driver = drivers.FirstOrDefault(o => o.Id == id);
        return driver == null ? Results.NotFound() : Results.Ok(driver);
    }

    private IResult AddDrivers(Driver driver)
    {
        drivers.Add(driver);
        var jobId = BackgroundJob.Enqueue<IServicesManagement>(x => x.SendEmail());
        return Results.CreatedAtRoute("GetDriver", new { id = driver.Id }, driver);
    }
}