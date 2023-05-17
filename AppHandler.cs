namespace ReproduceWolverineIssue;

using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Runtime.Handlers;

public class AppHandler
{
    public record Request;

    public record Response;

    public async Task<Response> Handle
    (
        Request request,
        IMessageBus messageBus
    )
    {
        await messageBus.PublishAsync(new FollowUpHandler.FollowUpMessage());
        return new Response();
    }
}

public class FollowUpHandler
{
    public record FollowUpMessage;

    public static void Configure(HandlerChain chain)
    {
        chain.OnAnyException()
            .RetryTimes(3)
            .Then
            .MoveToErrorQueue();
    }

    public async Task Handle
    (
        FollowUpMessage message,
        AppDbContext context
    )
    {
        throw new Exception();
    }
}