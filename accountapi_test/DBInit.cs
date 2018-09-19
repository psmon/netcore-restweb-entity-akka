using System;
using Xunit;

namespace accountapi_test
{
    public class DBInit
    {
        [Fact]
        public void PrepareTestData()
        {
            int count = AccountControlerTest.PrepareTestData();
            Assert.Equal(10, count);
        }

    }
}
