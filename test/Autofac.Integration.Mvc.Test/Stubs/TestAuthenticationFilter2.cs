// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Web.Mvc.Filters;

namespace Autofac.Integration.Mvc.Test.Stubs;

public class TestAuthenticationFilter2 : IAuthenticationFilter
{
    public void OnAuthentication(AuthenticationContext filterContext)
    {
    }

    public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    {
    }
}
