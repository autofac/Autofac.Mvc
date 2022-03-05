// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web.Mvc.Filters;

namespace Autofac.Integration.Mvc.Test.Stubs;

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
