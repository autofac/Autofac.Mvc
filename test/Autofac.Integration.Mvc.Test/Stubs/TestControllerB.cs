// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test.Stubs;

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
