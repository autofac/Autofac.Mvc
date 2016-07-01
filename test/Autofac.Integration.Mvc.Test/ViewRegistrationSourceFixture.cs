using System.Web.Mvc;
using Autofac.Core.Lifetime;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public abstract class AbstractViewMasterPage : ViewMasterPage
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractViewMasterPageWithModel<T> : ViewMasterPage<T>
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractViewPage : ViewPage
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractViewPageWithModel<T> : ViewPage<T>
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractViewUserControl : ViewUserControl
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractViewUserControlWithModel<T> : ViewUserControl<T>
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractWebViewPage : WebViewPage
    {
        public ViewDependency Dependency { get; set; }
    }

    public abstract class AbstractWebViewPageWithModel<T> : WebViewPage<T>
    {
        public ViewDependency Dependency { get; set; }
    }

    public class GeneratedViewMasterPage : AbstractViewMasterPage { }

    public class GeneratedViewMasterPageWithModel : AbstractViewMasterPageWithModel<Model> { }

    public class GeneratedViewPage : AbstractViewPage { }

    public class GeneratedViewPageWithModel : AbstractViewPageWithModel<Model> { }

    public class GeneratedViewUserControl : AbstractViewUserControl { }

    public class GeneratedViewUserControlWithModel : AbstractViewUserControlWithModel<Model> { }

    public class GeneratedWebViewPage : AbstractWebViewPage
    {
        public override void Execute() { }
    }

    public class GeneratedWebViewPageWithModel : AbstractWebViewPageWithModel<Model>
    {
        public override void Execute() { }
    }

    public class ViewDependency { }

    public class ViewModel { }

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
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewMasterPage = lifetimeScope.Resolve<GeneratedViewMasterPageWithModel>();
                Assert.NotNull(viewMasterPage);
                Assert.NotNull(viewMasterPage.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForViewMasterPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewMasterPage = lifetimeScope.Resolve<GeneratedViewMasterPage>();
                Assert.NotNull(viewMasterPage);
                Assert.NotNull(viewMasterPage.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForViewPageWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewPage = lifetimeScope.Resolve<GeneratedViewPageWithModel>();
                Assert.NotNull(viewPage);
                Assert.NotNull(viewPage.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForViewPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewPage = lifetimeScope.Resolve<GeneratedViewPage>();
                Assert.NotNull(viewPage);
                Assert.NotNull(viewPage.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForViewUserControlWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewUserControl = lifetimeScope.Resolve<GeneratedViewUserControlWithModel>();
                Assert.NotNull(viewUserControl);
                Assert.NotNull(viewUserControl.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForViewUserControlWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewUserControl = lifetimeScope.Resolve<GeneratedViewUserControl>();
                Assert.NotNull(viewUserControl);
                Assert.NotNull(viewUserControl.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForWebViewPageWithModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var webViewPage = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
                Assert.NotNull(webViewPage);
                Assert.NotNull(webViewPage.Dependency);
            }
        }

        [Fact]
        public void RegistrationFoundForWebViewPageWithoutModel()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var webViewPage = lifetimeScope.Resolve<GeneratedWebViewPage>();
                Assert.NotNull(webViewPage);
                Assert.NotNull(webViewPage.Dependency);
            }
        }

        [Fact]
        public void ViewCanHaveInstancePerHttpRequestDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf().InstancePerRequest();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewPage1 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
                var viewPage2 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();

                Assert.Same(viewPage2.Dependency, viewPage1.Dependency);
            }
        }

        [Fact]
        public void ViewRegistrationIsInstancePerDependency()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ViewDependency>().AsSelf();
            builder.RegisterSource(new ViewRegistrationSource());

            var container = builder.Build();
            using (var lifetimeScope = container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                var viewPage1 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();
                var viewPage2 = lifetimeScope.Resolve<GeneratedWebViewPageWithModel>();

                Assert.NotSame(viewPage2, viewPage1);
            }
        }
    }
}