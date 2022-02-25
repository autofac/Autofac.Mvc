// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test;

public class DependencyResolverReplacementContext : IDisposable
{
    private readonly IDependencyResolver _originalResolver;

    public DependencyResolverReplacementContext()
    {
        this._originalResolver = DependencyResolver.Current;
    }

    public virtual void Dispose()
    {
        DependencyResolver.SetResolver(this._originalResolver);
    }
}
