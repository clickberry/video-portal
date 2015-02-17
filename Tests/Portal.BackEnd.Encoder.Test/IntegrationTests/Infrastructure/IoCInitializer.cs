using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.BackEnd.IoC;
using RestSharp;
using SimpleInjector;

namespace Portal.BackEnd.Encoder.Test.IntegrationTests.Infrastructure
{
    public class IoCInitializer:BackEndInitializerBase
    {
        private readonly IRestClient _restClient;

        public IoCInitializer(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public override void Initialize(Container container)
        {
            Initialize(container, _restClient);
        }
    }
}
