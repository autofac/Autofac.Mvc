// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web;
using System.Web.Hosting;
using System.Web.Routing;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Dependency injection module that registers abstractions for common
/// web application properties.
/// </summary>
/// <remarks>
/// <para>
/// This <see cref="Module"/> is primarily used during
/// application startup (in <c>Global.asax</c>) to register
/// mappings from commonly referenced contextual application properties
/// to their corresponding abstraction.
/// </para>
/// <para>
/// The following mappings are made:
/// </para>
/// <list type="table">
/// <listheader>
/// <term>Common Construct</term>
/// <description>Abstraction</description>
/// </listheader>
/// <item>
/// <term><c>HttpContext.Current</c></term>
/// <description><see cref="HttpContextBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Application</c></term>
/// <description><see cref="HttpApplicationStateBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Request</c></term>
/// <description><see cref="HttpRequestBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Request.Browser</c></term>
/// <description><see cref="HttpBrowserCapabilitiesBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Request.Files</c></term>
/// <description><see cref="HttpFileCollectionBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Request.RequestContext</c></term>
/// <description><see cref="RequestContext"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Response</c></term>
/// <description><see cref="HttpResponseBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Response.Cache</c></term>
/// <description><see cref="HttpCachePolicyBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Server</c></term>
/// <description><see cref="HttpServerUtilityBase"/></description>
/// </item>
/// <item>
/// <term><c>HttpContext.Current.Session</c></term>
/// <description><see cref="HttpSessionStateBase"/></description>
/// </item>
/// <item>
/// <term><c>HostingEnvironment.VirtualPathProvider</c></term>
/// <description><see cref="VirtualPathProvider"/></description>
/// </item>
/// </list>
/// <para>
/// In addition, the <see cref="UrlHelper"/> type is registered
/// for construction based on the current <see cref="RequestContext"/>.
/// </para>
/// <para>
/// The lifetime for each of these items is one web request.
/// </para>
/// </remarks>
public class AutofacWebTypesModule : Module
{
    /// <summary>
    /// Registers web abstractions with dependency injection.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ContainerBuilder"/> in which registration
    /// should take place.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method registers mappings between common current context-related
    /// web constructs and their associated abstract counterparts. See
    /// <see cref="AutofacWebTypesModule"/> for the complete
    /// list of mappings that get registered.
    /// </para>
    /// </remarks>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "A lot of types get registered, but there isn't much complexity.")]
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "The complexity is in the registration lambdas. They're not actually hard to maintain.")]
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new HttpContextWrapper(HttpContext.Current))
            .As<HttpContextBase>()
            .InstancePerRequest();

        // HttpContext properties
        builder.Register(c => c.Resolve<HttpContextBase>().Request)
            .As<HttpRequestBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpContextBase>().Response)
            .As<HttpResponseBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpContextBase>().Server)
            .As<HttpServerUtilityBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpContextBase>().Session)
            .As<HttpSessionStateBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpContextBase>().Application)
            .As<HttpApplicationStateBase>()
            .InstancePerRequest();

        // HttpRequest properties
        builder.Register(c => c.Resolve<HttpRequestBase>().Browser)
            .As<HttpBrowserCapabilitiesBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpRequestBase>().Files)
            .As<HttpFileCollectionBase>()
            .InstancePerRequest();

        builder.Register(c => c.Resolve<HttpRequestBase>().RequestContext)
            .As<RequestContext>()
            .InstancePerRequest();

        // HttpResponse properties
        builder.Register(c => c.Resolve<HttpResponseBase>().Cache)
            .As<HttpCachePolicyBase>()
            .InstancePerRequest();

        // HostingEnvironment properties
        builder.Register(c => HostingEnvironment.VirtualPathProvider)
            .As<VirtualPathProvider>()
            .InstancePerRequest();

        // MVC types
        builder.Register(c => new UrlHelper(c.Resolve<RequestContext>()))
            .As<UrlHelper>()
            .InstancePerRequest();
    }
}
