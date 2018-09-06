using System;
using Xunit;

namespace accountapi_test
{
    public class DBInit
    {
        [Fact]
        public void Test1()
        {
            int count = AccountControlerTest.PrepareTestData();
            Assert.Equal(10, count);
        }

    }
}
