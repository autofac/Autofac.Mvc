using System.Web.Mvc;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class ExtensibleActionInvokerFixture : IClassFixture<ExtensibleActionInvokerTestContext>
    {
        public ExtensibleActionInvokerFixture(ExtensibleActionInvokerTestContext testContext)
        {
            this.TestContext = testContext;
        }

        public ExtensibleActionInvokerTestContext TestContext { get; private set; }

        [Fact]
        public void ActionInjection_DependencyRegistered_ServiceResolved()
        {
            var invoker = this.TestContext.Container.Resolve<IActionInvoker>();
            invoker.InvokeAction(this.TestContext.ControllerContext, "Index");
            Assert.IsAssignableFrom<ExtensibleActionInvokerTestContext.IActionDependency>(this.TestContext.Controller.Dependency);
        }
    }
}
