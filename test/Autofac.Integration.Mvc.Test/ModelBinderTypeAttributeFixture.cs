using System;
using Xunit;

namespace Autofac.Integration.Mvc.Test
{
    public class ModelBinderTypeAttributeFixture
    {
        [Fact]
        public void NullTargetTypesThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new ModelBinderTypeAttribute((Type[])null));
        }

        [Fact]
        public void NullTargetTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new ModelBinderTypeAttribute((Type)null));
        }
    }
}
