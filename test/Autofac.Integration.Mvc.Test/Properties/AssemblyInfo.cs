using System.Reflection;
using Xunit;

[assembly: AssemblyTitle("Autofac.Integration.Mvc.Test")]

// Tests can't run in parallel because they use the static DependencyResolver.Current.
[assembly: CollectionBehavior(DisableTestParallelization = true)]