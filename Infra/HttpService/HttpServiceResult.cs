using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.Infra.HttpService
{
    public class HttpServiceResult<T>
    {
        public HttpServiceResult(HttpStatusCode httpStatus, T result)
        {
            HttpStatus = httpStatus;

            Result = result;
        }

        public HttpStatusCode HttpStatus { get; private set; }

        public T Result { get; private set; }
    }
}
