using Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Bus
{
    public class InmediateExecutionCommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;
        public InmediateExecutionCommandBus(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task Send<T>(IEnumerable<T> messages) where T : ICommand
        {
            foreach (var item in messages)
            {
                await Send(item);
            }
        }

        public async Task Send<T>(T message) where T : ICommand
        {
            var handler = (ICommandHandler<T>)this._serviceProvider.GetService(typeof(ICommandHandler<T>));
            await handler.Handle(message);
        }
    }
}
