using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquawkBus.Messages.Test
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void TestMultiple()
        {
            using (var stream = new MemoryStream())
            {
                var source = new SubscriptionRequest("FOO", "bar", true);
                source.Write(new DataWriter(stream));
                stream.Seek(0, SeekOrigin.Begin);
                var dest = Message.Read(new DataReader(stream));
                Assert.AreEqual(source, dest);
            }
        }
    }
}
