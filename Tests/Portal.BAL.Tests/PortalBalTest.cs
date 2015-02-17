using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.BAL.Azure;
using Portal.BAL.Azure.Project;
using Portal.BAL.Parameters;

namespace Portal.BAL.Tests
{
    [TestClass]
    public class PortalBalTest
    {
        [TestMethod]
        public void TestBal()
        {
            IParametersCommandContext<ProjectParameters> context = new ParametersCommandContext<ProjectParameters>();
            Task<ParametersCommandResult<ProjectParameters>> task = context.ExecuteAsync<CreateProjectCommand>(new ProjectParameters());
        }
    }
}