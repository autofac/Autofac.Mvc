// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

namespace Autofac.Integration.Mvc.Test.Stubs;

public class TestController : Controller
{
    public object Dependency { get; set; }

    public static MethodInfo GetAction1MethodInfo<T>()
        where T : TestController
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
