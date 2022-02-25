// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test;

public class ModelBinderTypeAttributeFixture
{
    [Fact]
    public void NullTargetTypesThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new ModelBinderTypeAttribute((Type[])null));
    }

    [Fact]
    public void NullTargetTypeThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new ModelBinderTypeAttribute((Type)null));
    }
}
