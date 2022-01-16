using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquawkBus.Messages.Test
{
    [TestClass]
    public class NotificationRequestTest
    {
        [TestMethod]
        public void TestAddNotificationRequest()
        {
            using (var stream = new MemoryStream())
            {
                var source = new NotificationRequest("FOO", true);
                source.Write(new DataWriter(stream));
                stream.Seek(0, SeekOrigin.Begin);
                var dest = Message.Read(new DataReader(stream));
                Assert.AreEqual(source, dest);
            }
        }

        [TestMethod]
        public void TestRemoveNotificationRequest()
        {
            using (var stream = new MemoryStream())
            {
                var source = new NotificationRequest("FOO", false);
                source.Write(new DataWriter(stream));
                stream.Seek(0, SeekOrigin.Begin);
                var dest = Message.Read(new DataReader(stream));
                Assert.AreEqual(source, dest);
            }
        }
    }
}
