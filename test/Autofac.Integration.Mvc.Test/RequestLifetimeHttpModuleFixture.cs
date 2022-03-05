// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test;

public class RequestLifetimeHttpModuleFixture
{
    [Fact]
    public void CannotSetNullLifetimeScopeProvider()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => RequestLifetimeHttpModule.SetLifetimeScopeProvider(null));
        Assert.Equal("lifetimeScopeProvider", exception.ParamName);
    }

    [Fact]
    public void CanSetNonNullLifetimeScopeProvider()
    {
        var provider = new Mock<ILifetimeScopeProvider>();
        RequestLifetimeHttpModule.SetLifetimeScopeProvider(provider.Object);
        Assert.Equal(provider.Object, RequestLifetimeHttpModule.LifetimeScopeProvider);
    }
}
