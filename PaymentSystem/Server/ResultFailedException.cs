using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSystem.Server
{
    public class ResultFailedException : ApplicationException
    {
        public ResultFailedException() : base("Transfer Failed")
        {

        }
    }
}
