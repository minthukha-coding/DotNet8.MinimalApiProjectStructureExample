﻿namespace DotNet8.MinimalApiProjectStructureExampleBackend.Endpoints.EmailService;

public class EmailServiceEndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        //app.MapPost("/api/email/service",
        //    async (EmailRequestModel reqModel,
        //    [FromServices] EmailService _service) =>
        //    {
        //        return await ClassList(reqModel, _service);
        //    });
    }   
}
