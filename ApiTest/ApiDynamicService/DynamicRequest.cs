using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensivePlayrightAuto.ApiTest.ApiDynamicService
{
    public class DynamicRequestInputDto
    {
        public string Endpoint { get; set; }
        public HttpMethod Method { get; set; } = HttpMethod.Post;
        public Dictionary<string, object> Body { get; set; } = new();
        public string Token { get; set; } = null;

    }
}
