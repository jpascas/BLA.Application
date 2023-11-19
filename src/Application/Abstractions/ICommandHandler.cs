using System;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task Handle(T command);
    }
}
