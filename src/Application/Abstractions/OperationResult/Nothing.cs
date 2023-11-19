using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public sealed class Nothing
    {
        public static Nothing AtAll { get { return null; } }
    }
}
