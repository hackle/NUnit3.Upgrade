using System;

namespace NUnit3.Upgrade.UnitTest
{
    internal class ExpectedExceptionAttribute : Attribute
    {
        public ExpectedExceptionAttribute(Type exceptionType)
        {

        }
    }
}