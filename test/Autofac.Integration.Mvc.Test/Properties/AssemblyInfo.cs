// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

// Tests can't run in parallel because they use the static DependencyResolver.Current.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
