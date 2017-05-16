//using FluentValidation;
//using MediatR;
//using Microsoft.Extensions.Logging;

//namespace AutoRenter.Api.Features.Logs
//{
//    public class Post
//    {
//        public class Validator : AbstractValidator<Command>
//        {
//            public Validator()
//            {
//                RuleFor(m => m.Message).NotNull();
//                RuleFor(m => m.Level).NotNull();
//            }
//        }

//        public class Command : IRequest
//        {
//            public string Message { get; set; }
//            // TODO: Switch to enum?
//            public string Level { get; set; }
//        }

//        public class CommandHanlder : RequestHandler<Command>
//        {
//            private readonly ILogger<LogController> _logger;

//            public CommandHanlder(ILogger<LogController> logger)
//            {
//                _logger = logger;
//            }

//            protected override void HandleCore(Command message)
//            {
//                _logger.LogInformation($"({message.Level}): {message.Message}");
//            }
//        }
//    }
//}