using System;

namespace NUnit3.Upgrade.UnitTest
{
    [SimpleAttr(5, "Hello World")]
    public class SimpleClassToAnalyze : IDisposable
    {
        public int MySimpleProperty { get; set; }

        public event Action<object> MySimpleEvent;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        [ExpectedException(typeof(ArgumentNullException))]
        public int MySimpleMethod(string str, out bool flag, int i = 5)
        {
            flag = true;

            return 5;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
