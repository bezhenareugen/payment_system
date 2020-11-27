using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() : base("Object not found")
        {

        }
    }
}
