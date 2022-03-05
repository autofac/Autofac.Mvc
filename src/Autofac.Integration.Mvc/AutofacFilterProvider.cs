// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;
using System.Web.Mvc.Async;
using Autofac.Features.Metadata;

namespace Autofac.Integration.Mvc;

/// <summary>
/// Defines a filter provider for filter attributes that performs property injection.
/// </summary>
public class AutofacFilterProvider : FilterAttributeFilterProvider
{
    /// <summary>
    /// Metadata location with information about action filters.
    /// </summary>
    internal const string ActionFilterMetadataKey = "AutofacMvcActionFilter";

    /// <summary>
    /// Metadata location with information about action filter overrides.
    /// </summary>
    internal const string ActionFilterOverrideMetadataKey = "AutofacMvcActionFilterOverride";

    /// <summary>
    /// Metadata location with information about authentication filters.
    /// </summary>
    internal const string AuthenticationFilterMetadataKey = "AutofacMvcAuthenticationFilter";

    /// <summary>
    /// Metadata location with information about authentication filter overrides.
    /// </summary>
    internal const string AuthenticationFilterOverrideMetadataKey = "AutofacMvcAuthenticationFilterOverride";

    /// <summary>
    /// Metadata location with information about authorization filters.
    /// </summary>
    internal const string AuthorizationFilterMetadataKey = "AutofacMvcAuthorizationFilter";

    /// <summary>
    /// Metadata location with information about authorization filter overrides.
    /// </summary>
    internal const string AuthorizationFilterOverrideMetadataKey = "AutofacMvcAuthorizationFilterOverride";

    /// <summary>
    /// Metadata location with information about exception filters.
    /// </summary>
    internal const string ExceptionFilterMetadataKey = "AutofacMvcExceptionFilter";

    /// <summary>
    /// Metadata location with information about exception filter overrides.
    /// </summary>
    internal const string ExceptionFilterOverrideMetadataKey = "AutofacMvcExceptionFilterOverride";

    /// <summary>
    /// Metadata location with information about result filters.
    /// </summary>
    internal const string ResultFilterMetadataKey = "AutofacMvcResultFilter";

    /// <summary>
    /// Metadata location with information about result filter overrides.
    /// </summary>
    internal const string ResultFilterOverrideMetadataKey = "AutofacMvcResultFilterOverride";

    /// <summary>
    /// Initializes a new instance of the <see cref="AutofacFilterProvider"/> class.
    /// </summary>
    /// <remarks>
    /// The <c>false</c> constructor parameter passed to base here ensures that attribute instances are not cached.
    /// </remarks>
    public AutofacFilterProvider()
        : base(false)
    {
    }

    /// <summary>
    /// Aggregates the filters from all of the filter providers into one collection.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="actionDescriptor">The action descriptor.</param>
    /// <returns>
    /// The collection filters from all of the filter providers with properties injected.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="controllerContext" /> is <see langword="null" />.
    /// </exception>
    public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
    {
        if (controllerContext == null)
        {
            throw new ArgumentNullException(nameof(controllerContext));
        }

        var filters = base.GetFilters(controllerContext, actionDescriptor).ToList();
        var lifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;

        if (lifetimeScope != null)
        {
            foreach (var filter in filters)
            {
                lifetimeScope.InjectProperties(filter.Instance);
            }

            var controllerType = controllerContext.Controller.GetType();

            var filterContext = new FilterContext
            {
                ActionDescriptor = actionDescriptor,
                LifetimeScope = lifetimeScope,
                ControllerType = controllerType,
                Filters = filters,
            };

            ResolveControllerScopedFilters(filterContext);

            ResolveActionScopedFilters<ReflectedActionDescriptor>(filterContext, d => d.MethodInfo);
            ResolveActionScopedFilters<ReflectedAsyncActionDescriptor>(filterContext, d => d.AsyncMethodInfo);
            ResolveActionScopedFilters<TaskAsyncActionDescriptor>(filterContext, d => d.TaskMethodInfo);

            ResolveControllerScopedFilterOverrides(filterContext);

            ResolveActionScopedFilterOverrides<ReflectedActionDescriptor>(filterContext, d => d.MethodInfo);
            ResolveActionScopedFilterOverrides<ReflectedAsyncActionDescriptor>(filterContext, d => d.AsyncMethodInfo);
            ResolveActionScopedFilterOverrides<TaskAsyncActionDescriptor>(filterContext, d => d.TaskMethodInfo);

            ResolveControllerScopedEmptyOverrideFilters(filterContext);

            ResolveActionScopedEmptyOverrideFilters<ReflectedActionDescriptor>(filterContext, d => d.MethodInfo);
            ResolveActionScopedEmptyOverrideFilters<ReflectedAsyncActionDescriptor>(filterContext, d => d.AsyncMethodInfo);
            ResolveActionScopedEmptyOverrideFilters<TaskAsyncActionDescriptor>(filterContext, d => d.TaskMethodInfo);
        }

        return filters.ToArray();
    }

    private static bool FilterMatchesAction(FilterContext filterContext, MethodInfo methodInfo, FilterMetadata metadata)
    {
        return metadata.ControllerType != null
               && metadata.ControllerType.IsAssignableFrom(filterContext.ControllerType)
               && metadata.FilterScope == FilterScope.Action
               && metadata.MethodInfo.GetBaseDefinition() == methodInfo.GetBaseDefinition();
    }

    private static bool FilterMatchesController(FilterContext filterContext, FilterMetadata metadata)
    {
        return metadata.ControllerType != null
               && metadata.ControllerType.IsAssignableFrom(filterContext.ControllerType)
               && metadata.FilterScope == FilterScope.Controller
               && metadata.MethodInfo == null;
    }

    private static void ResolveActionScopedEmptyOverrideFilters<T>(FilterContext filterContext, Func<T, MethodInfo> methodSelector)
        where T : ActionDescriptor
    {
        if (filterContext.ActionDescriptor is not T actionDescriptor)
        {
            return;
        }

        var methodInfo = methodSelector(actionDescriptor);

        ResolveActionScopedOverrideFilter(filterContext, methodInfo, ActionFilterOverrideMetadataKey);
        ResolveActionScopedOverrideFilter(filterContext, methodInfo, AuthenticationFilterOverrideMetadataKey);
        ResolveActionScopedOverrideFilter(filterContext, methodInfo, AuthorizationFilterOverrideMetadataKey);
        ResolveActionScopedOverrideFilter(filterContext, methodInfo, ExceptionFilterOverrideMetadataKey);
        ResolveActionScopedOverrideFilter(filterContext, methodInfo, ResultFilterOverrideMetadataKey);
    }

    private static void ResolveActionScopedFilter<TFilter>(FilterContext filterContext, MethodInfo methodInfo, string metadataKey, Func<TFilter, TFilter> wrapperFactory = null)
        where TFilter : class
    {
        var actionFilters = filterContext.LifetimeScope.Resolve<IEnumerable<Meta<Lazy<TFilter>>>>();

        foreach (var actionFilter in actionFilters.Where(a => a.Metadata.ContainsKey(metadataKey) && a.Metadata[metadataKey] is FilterMetadata))
        {
            var metadata = (FilterMetadata)actionFilter.Metadata[metadataKey];
            if (!FilterMatchesAction(filterContext, methodInfo, metadata))
            {
                continue;
            }

            var instance = actionFilter.Value.Value;

            if (wrapperFactory != null)
            {
                instance = wrapperFactory(instance);
            }

            var filter = new Filter(instance, FilterScope.Action, metadata.Order);
            filterContext.Filters.Add(filter);
        }
    }

    private static void ResolveActionScopedFilterOverrides<T>(FilterContext filterContext, Func<T, MethodInfo> methodSelector)
        where T : ActionDescriptor
    {
        if (filterContext.ActionDescriptor is not T actionDescriptor)
        {
            return;
        }

        var methodInfo = methodSelector(actionDescriptor);

        ResolveActionScopedFilter<IActionFilter>(filterContext, methodInfo, ActionFilterOverrideMetadataKey, filter => new ActionFilterOverride(filter));
        ResolveActionScopedFilter<IAuthenticationFilter>(filterContext, methodInfo, AuthenticationFilterOverrideMetadataKey, filter => new AuthenticationFilterOverride(filter));
        ResolveActionScopedFilter<IAuthorizationFilter>(filterContext, methodInfo, AuthorizationFilterOverrideMetadataKey, filter => new AuthorizationFilterOverride(filter));
        ResolveActionScopedFilter<IExceptionFilter>(filterContext, methodInfo, ExceptionFilterOverrideMetadataKey, filter => new ExceptionFilterOverride(filter));
        ResolveActionScopedFilter<IResultFilter>(filterContext, methodInfo, ResultFilterOverrideMetadataKey, filter => new ResultFilterOverride(filter));
    }

    private static void ResolveActionScopedFilters<T>(FilterContext filterContext, Func<T, MethodInfo> methodSelector)
        where T : ActionDescriptor
    {
        if (filterContext.ActionDescriptor is not T actionDescriptor)
        {
            return;
        }

        var methodInfo = methodSelector(actionDescriptor);

        ResolveActionScopedFilter<IActionFilter>(filterContext, methodInfo, ActionFilterMetadataKey);
        ResolveActionScopedFilter<IAuthenticationFilter>(filterContext, methodInfo, AuthenticationFilterMetadataKey);
        ResolveActionScopedFilter<IAuthorizationFilter>(filterContext, methodInfo, AuthorizationFilterMetadataKey);
        ResolveActionScopedFilter<IExceptionFilter>(filterContext, methodInfo, ExceptionFilterMetadataKey);
        ResolveActionScopedFilter<IResultFilter>(filterContext, methodInfo, ResultFilterMetadataKey);
    }

    private static void ResolveActionScopedOverrideFilter(FilterContext filterContext, MethodInfo methodInfo, string metadataKey)
    {
        var actionFilters = filterContext.LifetimeScope.Resolve<IEnumerable<Meta<IOverrideFilter>>>();

        foreach (var actionFilter in actionFilters.Where(a => a.Metadata.ContainsKey(metadataKey) && a.Metadata[metadataKey] is FilterMetadata))
        {
            var metadata = (FilterMetadata)actionFilter.Metadata[metadataKey];
            if (!FilterMatchesAction(filterContext, methodInfo, metadata))
            {
                continue;
            }

            var filter = new Filter(actionFilter.Value, FilterScope.Action, metadata.Order);
            filterContext.Filters.Add(filter);
        }
    }

    private static void ResolveControllerScopedEmptyOverrideFilters(FilterContext filterContext)
    {
        ResolveControllerScopedOverrideFilter(filterContext, ActionFilterOverrideMetadataKey);
        ResolveControllerScopedOverrideFilter(filterContext, AuthenticationFilterOverrideMetadataKey);
        ResolveControllerScopedOverrideFilter(filterContext, AuthorizationFilterOverrideMetadataKey);
        ResolveControllerScopedOverrideFilter(filterContext, ExceptionFilterOverrideMetadataKey);
        ResolveControllerScopedOverrideFilter(filterContext, ResultFilterOverrideMetadataKey);
    }

    private static void ResolveControllerScopedFilter<TFilter>(FilterContext filterContext, string metadataKey, Func<TFilter, TFilter> wrapperFactory = null)
        where TFilter : class
    {
        var actionFilters = filterContext.LifetimeScope.Resolve<IEnumerable<Meta<Lazy<TFilter>>>>();

        foreach (var actionFilter in actionFilters.Where(a => a.Metadata.ContainsKey(metadataKey) && a.Metadata[metadataKey] is FilterMetadata))
        {
            var metadata = (FilterMetadata)actionFilter.Metadata[metadataKey];
            if (!FilterMatchesController(filterContext, metadata))
            {
                continue;
            }

            var instance = actionFilter.Value.Value;

            if (wrapperFactory != null)
            {
                instance = wrapperFactory(instance);
            }

            var filter = new Filter(instance, FilterScope.Controller, metadata.Order);
            filterContext.Filters.Add(filter);
        }
    }

    private static void ResolveControllerScopedFilterOverrides(FilterContext filterContext)
    {
        ResolveControllerScopedFilter<IActionFilter>(filterContext, ActionFilterOverrideMetadataKey, filter => new ActionFilterOverride(filter));
        ResolveControllerScopedFilter<IAuthenticationFilter>(filterContext, AuthenticationFilterOverrideMetadataKey, filter => new AuthenticationFilterOverride(filter));
        ResolveControllerScopedFilter<IAuthorizationFilter>(filterContext, AuthorizationFilterOverrideMetadataKey, filter => new AuthorizationFilterOverride(filter));
        ResolveControllerScopedFilter<IExceptionFilter>(filterContext, ExceptionFilterOverrideMetadataKey, filter => new ExceptionFilterOverride(filter));
        ResolveControllerScopedFilter<IResultFilter>(filterContext, ResultFilterOverrideMetadataKey, filter => new ResultFilterOverride(filter));
    }

    private static void ResolveControllerScopedFilters(FilterContext filterContext)
    {
        ResolveControllerScopedFilter<IActionFilter>(filterContext, ActionFilterMetadataKey);
        ResolveControllerScopedFilter<IAuthenticationFilter>(filterContext, AuthenticationFilterMetadataKey);
        ResolveControllerScopedFilter<IAuthorizationFilter>(filterContext, AuthorizationFilterMetadataKey);
        ResolveControllerScopedFilter<IExceptionFilter>(filterContext, ExceptionFilterMetadataKey);
        ResolveControllerScopedFilter<IResultFilter>(filterContext, ResultFilterMetadataKey);
    }

    private static void ResolveControllerScopedOverrideFilter(FilterContext filterContext, string metadataKey)
    {
        var actionFilters = filterContext.LifetimeScope.Resolve<IEnumerable<Meta<IOverrideFilter>>>();

        foreach (var actionFilter in actionFilters.Where(a => a.Metadata.ContainsKey(metadataKey) && a.Metadata[metadataKey] is FilterMetadata))
        {
            var metadata = (FilterMetadata)actionFilter.Metadata[metadataKey];
            if (!FilterMatchesController(filterContext, metadata))
            {
                continue;
            }

            var filter = new Filter(actionFilter.Value, FilterScope.Controller, metadata.Order);
            filterContext.Filters.Add(filter);
        }
    }

    private class FilterContext
    {
        public ActionDescriptor ActionDescriptor { get; set; }

        public Type ControllerType { get; set; }

        public List<Filter> Filters { get; set; }

        public ILifetimeScope LifetimeScope { get; set; }
    }
}
