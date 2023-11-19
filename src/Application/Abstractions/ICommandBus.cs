using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommandBus
    {        
        Task<OperationResult<TResult>> Send<T, TResult>(T message) where T : ICommand;        
    }
}
