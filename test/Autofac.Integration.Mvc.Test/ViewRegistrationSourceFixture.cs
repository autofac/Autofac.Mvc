// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc.Test.Stubs;

namespace Autofac.Integration.Mvc.Test
{
    public class ViewRegistrationSourceFixture
    {
        [Fact]
        public void IsNotAdapterForIndividualComponents()
        {
            Assert.False(new ViewRegistrationSource().IsAdapterForIndividualComponents);
        }

        [Fact]
        public void RegistrationFoundForViewMasterPageWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewMasterPage = lifetimeScope.Resolve<GeneratedViewMasterPageWithModel>();
            Assert.NotNull(viewMasterPage);
            Assert.NotNull(viewMasterPage.Dependency);
        }

        [Fact]
        public void RegistrationFoundForViewMasterPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewMasterPage = lifetimeScope.Resolve<GeneratedViewMasterPage>();
            Assert.NotNull(viewMasterPage);
            Assert.NotNull(viewMasterPage.Dependency);
        }

        [Fact]
        public void RegistrationFoundForViewPageWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewPage = lifetimeScope.Resolve<GeneratedViewPageWithModel>();
            Assert.NotNull(viewPage);
            Assert.NotNull(viewPage.Dependency);
        }

        [Fact]
        public void RegistrationFoundForViewPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewPage = lifetimeScope.Resolve<GeneratedViewPage>();
            Assert.NotNull(viewPage);
            Assert.NotNull(viewPage.Dependency);
        }

        [Fact]
        public void RegistrationFoundForViewUserControlWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewUserControl = lifetimeScope.Resolve<GeneratedViewUserControlWithModel>();
            Assert.NotNull(viewUserControl);
            Assert.NotNull(viewUserControl.Dependency);
        }

        [Fact]
        public void RegistrationFoundForViewUserControlWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewUserControl = lifetimeScope.Resolve<GeneratedViewUserControl>();
            Assert.NotNull(viewUserControl);
            Assert.NotNull(viewUserControl.Dependency);
        }

        [Fact]
        public void RegistrationFoundForWebViewPageWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var webViewPage = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
            Assert.NotNull(webViewPage);
            Assert.NotNull(webViewPage.Dependency);
        }

        [Fact]
        public void RegistrationFoundForWebViewPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var webViewPage = lifetimeScope.Resolve<GeneratedWebViewPage>();
            Assert.NotNull(webViewPage);
            Assert.NotNull(webViewPage.Dependency);
        }

        [Fact]
        public void ViewCanHaveInstancePerHttpRequestDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf().InstancePerRequest();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewPage1 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
            var viewPage2 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();

            Assert.Same(viewPage2.Dependency, viewPage1.Dependency);
        }

        [Fact]
        public void ViewRegistrationIsInstancePerDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            var viewPage1 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
            var viewPage2 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();

            Assert.NotSame(viewPage2, viewPage1);
        }
    }
}
