// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Autofac.Integration.Mvc.Test.Stubs;

[ModelBinderType(typeof(TestModel1), typeof(string))]
[ModelBinderType(typeof(DateTime))]
public class ModelBinder : IModelBinder
{
    public ModelBinder(Dependency dependency)
    {
        this.Dependency = dependency;
    }

    public Dependency Dependency { get; private set; }

    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        return "Bound";
    }
}
