// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test.Stubs;

public class TestActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
    }

    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
    }
}
