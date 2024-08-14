using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveAutomation.Test.Infra.BaseTest
{
    public class TestData
    {
        public API Api { get; set; }
        public WebUi WebUi { get; set; }
        public Mobile Mobile { get; set; }
    }

    public class API
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string BaseApiUrl { get; set; }

    }
    public class WebUi
    {
        public string WebUrl { get; set; }
        public string WebUserName { get; set; }
        public string WebPassword { get; set; }
    }
   
    public class Mobile
    {
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string appPackage { get; set; }
        public string appActivity { get; set; }

    }
}
