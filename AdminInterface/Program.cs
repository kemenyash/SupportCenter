using Microsoft.AspNetCore.SignalR.Client;
using Schemas.Entities;
using System.Text;


var hubUrl = $"https://localhost:7183/adminhub?userId={GenerateRandomString(10)}";
Admin adminProfile = null;

var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

connection.On<Report>("ReceiveReport", report =>
{
    Console.WriteLine($"Received report: {report.Title}");
});

connection.On<Admin>("ReceiveAdminProfile", admin =>
{
    adminProfile = admin;
    Console.WriteLine($"\r\nYou are log in: \r\nUsername: {admin.Name}\r\nEmail: {admin.Email}\r\nUser ID: {admin.UserId}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to AdminHub.");

    Console.WriteLine("Commands:");
    Console.WriteLine("1. Push 'Insert' for report generation");
    Console.WriteLine("2. Push 'Enter' for exit...");


    while (true)
    {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Insert)
        {
            var report = await adminProfile.GenerateReport("https://localhost:7183/api/tickets");
            Console.WriteLine($"\r\nTitle: {report.Title}");
            Console.WriteLine($"Content: {report.Content}");
            Console.WriteLine($"Created at: {report.CreatedAt}");
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("Exiting...");
            break;
        }
        else
        {
            Console.WriteLine("Invalid command. Try again.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
}
finally
{
    await connection.DisposeAsync();
}

static string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    var result = new StringBuilder(length);

    for (int i = 0; i < length; i++)
    {
        result.Append(chars[random.Next(chars.Length)]);
    }

    return result.ToString();
}