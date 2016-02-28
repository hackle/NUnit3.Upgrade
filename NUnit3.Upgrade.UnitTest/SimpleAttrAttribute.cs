using System;

namespace NUnit3.Upgrade.UnitTest
{
    internal class SimpleAttrAttribute : Attribute
    {
        private int v1;
        private string v2;

        public SimpleAttrAttribute(int v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }
}