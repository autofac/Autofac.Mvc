using System.Web.Mvc;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class ExtensibleActionInvokerPropertyFixture : IClassFixture<ExtensibleActionInvokerPropertyTestContext>
    {
        public ExtensibleActionInvokerPropertyFixture(ExtensibleActionInvokerPropertyTestContext testContext)
        {
            this.TestContext = testContext;
        }

        public ExtensibleActionInvokerPropertyTestContext TestContext { get; private set; }

        [Fact]
        public void ActionInjection_DependencyRegistered_ServiceResolved()
        {
            var invoker = this.TestContext.Container.Resolve<IActionInvoker>();
            invoker.InvokeAction(this.TestContext.ControllerContext, "Index");
            var dependency = Assert.IsAssignableFrom<ExtensibleActionInvokerPropertyTestContext.IActionDependency>(this.TestContext.Controller.Dependency);
            Assert.NotNull(dependency.Property);
        }
    }
}
