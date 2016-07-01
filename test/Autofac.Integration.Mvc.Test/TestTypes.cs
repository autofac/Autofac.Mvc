using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Autofac.Integration.Mvc.Test
{
    public class IsAControllerNot : Controller
    {
    }

    public class TestActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }

    public class TestActionFilter2 : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }

    public class TestActionInvoker : IActionInvoker
    {
        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            return true;
        }
    }

    public class TestAuthenticationFilter : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }

    public class TestAuthenticationFilter2 : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }

    public class TestAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
        }
    }

    public class TestAuthorizationFilter2 : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
        }
    }

    public class TestCombinationFilter : IActionFilter, IAuthenticationFilter, IAuthorizationFilter, IExceptionFilter, IResultFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
        }

        public void OnException(ExceptionContext filterContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }

    public class TestController : Controller
    {
        public object Dependency;

        public static MethodInfo GetAction1MethodInfo<T>() where T : TestController
        {
            return typeof(T).GetMethod(nameof(Action1));
        }

        public virtual ActionResult Action1(string value)
        {
            return new EmptyResult();
        }

        public virtual ActionResult Action2(int value)
        {
            return new EmptyResult();
        }
    }

    public class TestControllerA : TestController
    {
        public override ActionResult Action1(string value)
        {
            return new EmptyResult();
        }

        public override ActionResult Action2(int value)
        {
            return new EmptyResult();
        }
    }

    public class TestControllerB : TestControllerA
    {
        public override ActionResult Action1(string value)
        {
            return new EmptyResult();
        }

        public override ActionResult Action2(int value)
        {
            return new EmptyResult();
        }
    }

    public class TestExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
        }
    }

    public class TestExceptionFilter2 : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
        }
    }

    public class TestModel1
    {
    }

    public class TestModel2
    {
    }

    public class TestModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }

    public class TestResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }

    public class TestResultFilter2 : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }
}