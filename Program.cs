using Microsoft.EntityFrameworkCore;
using Npgsql;
using Oakton;
using ReproduceWolverineIssue;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine((context, options) =>
{
    var connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = "localhost",
        Port = 5432,
        Username = "postgres",
        Database = "test-wolverine",
        Password = "ER4PnCJz6q"
    }.ToString();

    options.Services.AddDbContextWithWolverineIntegration<AppDbContext>(ef => ef.UseNpgsql(connectionString));
    options.PersistMessagesWithPostgresql(connectionString);
    options.UseEntityFrameworkCoreTransactions();
    options.Policies.AutoApplyTransactions();

    // set up messaging
    options.PublishMessage<FollowUpHandler.FollowUpMessage>().Locally().UseDurableInbox();
}).ApplyOaktonExtensions();

var app = builder.Build();

app.MapGet("/", async (IMessageBus bus) => 
    await bus.InvokeAsync<AppHandler.Response>(new AppHandler.Request()));

await app.RunOaktonCommands(args);