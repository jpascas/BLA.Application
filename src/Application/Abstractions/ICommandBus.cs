using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommandBus
    {
        Task Send<T>(T message) where T : ICommand;
        Task Send<T>(IEnumerable<T> messages) where T : ICommand;
    }
}
