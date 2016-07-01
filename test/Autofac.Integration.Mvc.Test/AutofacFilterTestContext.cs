using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Moq;


namespace Autofac.Integration.Mvc.Test
{
    public class AutofacFilterTestContext
    {
        public AutofacFilterTestContext()
        {
            this.BaseControllerContext = new ControllerContext { Controller = new TestController() };
            this.DerivedControllerContext = new ControllerContext { Controller = new TestControllerA() };
            this.MostDerivedControllerContext = new ControllerContext { Controller = new TestControllerB() };
            this.BaseMethodInfo = TestController.GetAction1MethodInfo<TestController>();
            this.DerivedMethodInfo = TestController.GetAction1MethodInfo<TestControllerA>();
            this.MostDerivedMethodInfo = TestController.GetAction1MethodInfo<TestControllerB>();
            this.ActionName = this.BaseMethodInfo.Name;
            this.ControllerDescriptor = new Mock<ControllerDescriptor>().Object;
            this.ReflectedActionDescriptor = new ReflectedActionDescriptor(this.BaseMethodInfo, this.ActionName, this.ControllerDescriptor);
            this.ReflectedAsyncActionDescriptor = new ReflectedAsyncActionDescriptor(this.BaseMethodInfo, this.BaseMethodInfo, this.ActionName, this.ControllerDescriptor);
            this.TaskAsyncActionDescriptor = new TaskAsyncActionDescriptor(this.BaseMethodInfo, this.ActionName, this.ControllerDescriptor);
            this.DerivedActionDescriptor = new ReflectedActionDescriptor(this.DerivedMethodInfo, this.ActionName, this.ControllerDescriptor);
            this.MostDerivedActionDescriptor = new ReflectedActionDescriptor(this.MostDerivedMethodInfo, this.ActionName, this.ControllerDescriptor);
        }

        public string ActionName { get; private set; }

        public ControllerContext BaseControllerContext { get; private set; }

        public MethodInfo BaseMethodInfo { get; private set; }

        public ControllerDescriptor ControllerDescriptor { get; private set; }

        public ReflectedActionDescriptor DerivedActionDescriptor { get; private set; }

        public ControllerContext DerivedControllerContext { get; private set; }

        public MethodInfo DerivedMethodInfo { get; private set; }

        public ReflectedActionDescriptor MostDerivedActionDescriptor { get; private set; }

        public ControllerContext MostDerivedControllerContext { get; private set; }

        public MethodInfo MostDerivedMethodInfo { get; private set; }

        public ReflectedActionDescriptor ReflectedActionDescriptor { get; private set; }

        public ReflectedAsyncActionDescriptor ReflectedAsyncActionDescriptor { get; private set; }

        public TaskAsyncActionDescriptor TaskAsyncActionDescriptor { get; private set; }
    }
}
