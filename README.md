# Autofac.Mvc

ASP.NET MVC integration for [Autofac](https://autofac.org).

[![Build status](https://ci.appveyor.com/api/projects/status/bw1p26wbae0jeye5?svg=true)](https://ci.appveyor.com/project/Autofac/autofac-mvc)

Please file issues and pull requests for this package [in this repository](https://github.com/autofac/Autofac.Mvc/issues) rather than in the Autofac core repo.

If you're working with ASP.NET Core, you want [Autofac.Extensions.DependencyInjection](https://www.nuget.org/packages/Autofac.Extensions.DependencyInjection), not this package.

- [Documentation](https://autofac.readthedocs.io/en/latest/integration/mvc.html)
- [NuGet](https://www.nuget.org/packages/Autofac.Mvc5)
- [Contributing](https://autofac.readthedocs.io/en/latest/contributors.html)
- [Open in Visual Studio Code](https://open.vscode.dev/autofac/Autofac.Mvc)

## Quick Start

To get Autofac integrated with MVC you need to reference this MVC integration NuGet package, register your controllers, and set the dependency resolver. You can optionally enable other features as well.

```c#
protected void Application_Start()
{
  var builder = new ContainerBuilder();

  // Register your MVC controllers. (MvcApplication is the name of
  // the class in Global.asax.)
  builder.RegisterControllers(typeof(MvcApplication).Assembly);

  // OPTIONAL: Register model binders that require DI.
  builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
  builder.RegisterModelBinderProvider();

  // OPTIONAL: Register web abstractions like HttpContextBase.
  builder.RegisterModule<AutofacWebTypesModule>();

  // OPTIONAL: Enable property injection in view pages.
  builder.RegisterSource(new ViewRegistrationSource());

  // OPTIONAL: Enable property injection into action filters.
  builder.RegisterFilterProvider();

  // OPTIONAL: Enable action method parameter injection (RARE).
  builder.InjectActionInvoker();

  // Set the dependency resolver to be Autofac.
  var container = builder.Build();
  DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
}
```

If you are using MVC [as part of an OWIN application](https://autofac.readthedocs.io/en/latest/integration/owin.html), you may be interested in the [ASP.NET MVC OWIN integration](https://github.com/autofac/Autofac.Mvc.Owin).

Check out the [Autofac ASP.NET MVC integration documentation](https://autofac.readthedocs.io/en/latest/integration/mvc.html) for more information.

## Get Help

**Need help with Autofac?** We have [a documentation site](https://autofac.readthedocs.io/) as well as [API documentation](https://autofac.org/apidoc/). We're ready to answer your questions on [Stack Overflow](https://stackoverflow.com/questions/tagged/autofac) or check out the [discussion forum](https://groups.google.com/forum/#forum/autofac).
