using Application.Abstractions;
using System;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommandHandler<T, TResult> where T : ICommand
    {
        Task<OperationResult<TResult>> Handle(T command);
    }
}
