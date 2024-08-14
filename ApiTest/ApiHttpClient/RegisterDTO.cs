using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.Test.ExternalApiTests.GenerateApiUserTokenTest
{
    public class RegisterInputDTO
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class RegisterOutputDTO : HttpResponseMessage
    {

        public int id { get; set; }
        public string token { get; set; }
        public string error { get; set; }

    }  

    public class GetResponceOutputDTO : HttpResponseMessage
    {
        public int page { get; set; }
    }
}
