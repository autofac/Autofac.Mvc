using System.Reflection;
using Xunit;

// Tests can't run in parallel because they use the static DependencyResolver.Current.
[assembly: CollectionBehavior(DisableTestParallelization = true)]