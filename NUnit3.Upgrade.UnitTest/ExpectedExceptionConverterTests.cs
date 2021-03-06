﻿using System;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace NUnit3.Upgrade.UnitTest
{
    [TestFixture]
    public class ExpectedExceptionConverterTests
    {
        [Test]
        public void Can_convert_correctly()
        {
            var input = @"
public class AnyClass 
{
    [Test]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AnyTest() 
    {
        Assert.IsTrue(true);
    }
}
";
            var expected = @"
public class AnyClass 
{
    [Test]
    public void AnyTest() 
    {
        Assert.Throws<ArgumentNullException>(() => 
        {
            Assert.IsTrue(true);
        }

        );
    }
}
";

            var actual = new ExpectedExceptionConverter().Convert(input);

             Assert.That(NoWhiteSpace(actual), Is.EqualTo(NoWhiteSpace(expected)));
        }

        private string NoWhiteSpace(string actual)
        {
            return Regex.Replace(actual, @"\s", "");
        }
    }
}
