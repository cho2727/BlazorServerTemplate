
using MediatR;

namespace BlazorServerApp.Handler;

public class AddCount
{
    public class Command : IRequest<Response>
    {
        public int CurrentValue { get; set; }
        public int AddValue { get; set; }
    }
    public class Response
    {
        public int CurrentValue { get; set; }
    }

    public class CounterHandler : IRequestHandler<Command, Response>
    {
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentValue = request.CurrentValue + request.AddValue;
            return await Task.FromResult(new Response() { CurrentValue = currentValue });
        }
    }
}
