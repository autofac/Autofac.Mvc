using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac.Core.Lifetime;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class AutofacModelBinderProviderFixture
    {
        [Fact]
        public void ModelBinderHasDependenciesInjected()
        {
            using (var httpRequestScope = BuildContainer().BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var modelBinder = httpRequestScope.Resolve<IEnumerable<IModelBinder>>()
                    .OfType<ModelBinder>()
                    .FirstOrDefault();
                Assert.NotNull(modelBinder);
                Assert.NotNull(modelBinder.Dependency);
            }
        }

        [Fact]
        public void ModelBindersAreRegistered()
        {
            using (var httpRequestScope = BuildContainer().BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var modelBinders = httpRequestScope.Resolve<IEnumerable<IModelBinder>>();
                Assert.Equal(1, modelBinders.Count());
            }
        }

        [Fact]
        public void MultipleTypesCanBeDeclaredWithMultipleAttribute()
        {
            BuildContainer();
            var provider = (AutofacModelBinderProvider)DependencyResolver.Current.GetService<IModelBinderProvider>();
            Assert.IsType<ModelBinder>(provider.GetBinder(typeof(string)));
            Assert.IsType<ModelBinder>(provider.GetBinder(typeof(DateTime)));
        }

        [Fact]
        public void MultipleTypesCanBeDeclaredWithSingleAttribute()
        {
            BuildContainer();
            var provider = (AutofacModelBinderProvider)DependencyResolver.Current.GetService<IModelBinderProvider>();
            Assert.IsType<ModelBinder>(provider.GetBinder(typeof(Model)));
            Assert.IsType<ModelBinder>(provider.GetBinder(typeof(string)));
        }

        [Fact]
        public void ProviderIsRegisteredAsSingleInstance()
        {
            var container = BuildContainer();
            var modelBinderProvider = container.Resolve<IModelBinderProvider>();
            Assert.IsType<AutofacModelBinderProvider>(modelBinderProvider);

            using (var httpRequestScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                Assert.Equal(modelBinderProvider, httpRequestScope.Resolve<IModelBinderProvider>());
            }
        }

        [Fact]
        public void ReturnsNullWhenModelBinderRegisteredWithoutMetadata()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ModelBinderWithoutAttribute>().As<IModelBinder>().InstancePerRequest();
            builder.RegisterModelBinderProvider();
            var container = builder.Build();

            using (var httpRequestScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var modelBinders = httpRequestScope.Resolve<IEnumerable<IModelBinder>>().ToList();
                Assert.Equal(1, modelBinders.Count());
                Assert.IsType<ModelBinderWithoutAttribute>(modelBinders.First());

                var provider = (AutofacModelBinderProvider)httpRequestScope.Resolve<IModelBinderProvider>();
                Assert.Null(provider.GetBinder(typeof(object)));
            }
        }

        private static ILifetimeScope BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            var container = builder.Build();
            var lifetimeScopeProvider = new StubLifetimeScopeProvider(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container, lifetimeScopeProvider));
            return container;
        }
    }

    public class Dependency
    {
    }

    public class Model
    {
    }

    [ModelBinderType(typeof(Model), typeof(string))]
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

    public class ModelBinderWithoutAttribute : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return "Bound";
        }
    }
}