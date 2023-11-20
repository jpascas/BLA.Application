using Application;
using Application.Abstractions;
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

        public async Task<OperationResult<TResult>> Send<T, TResult>(T message) where T : ICommand
        {
            var handler = (ICommandHandler<T, TResult>)this._serviceProvider.GetService(typeof(ICommandHandler<T, TResult>));
            return await handler.Handle(message);
        }        
    }
}
