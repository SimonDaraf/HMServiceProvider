using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMServiceProvider.Core
{
    public class InstanceAlreadyRegisteredException : Exception
    {
        public InstanceAlreadyRegisteredException() : base() { }

        public InstanceAlreadyRegisteredException(string message) : base(message) { }
    }
}
