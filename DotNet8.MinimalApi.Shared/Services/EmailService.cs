namespace ClassLibrary1DotNet8.MinimalApi.Shared.Services;

public class EmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public record class EmailRequestModel(string body, string toEmail, string subject);

    public async Task<string> SendEmail(EmailRequestModel requestModel)
    {
        var response = await _fluentEmail
            .To(requestModel.toEmail)
            .Subject(requestModel.subject)
            .Body(requestModel.body)
            .SendAsync();

        return ("Email sent successfully.");
    }
}