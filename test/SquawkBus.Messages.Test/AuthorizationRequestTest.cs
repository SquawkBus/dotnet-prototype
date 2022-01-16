using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquawkBus.Messages.Test
{
    [TestClass]
    public class AuthorizationRequestTest
    {
        [TestMethod]
        public void RoundTrip()
        {
            using (var stream = new MemoryStream())
            {
                var source = new AuthorizationRequest(Guid.NewGuid(), "HOST", "USER", "FEED", "TOPIC");
                source.Write(new DataWriter(stream));
                stream.Seek(0, SeekOrigin.Begin);
                var dest = Message.Read(new DataReader(stream));
                Assert.AreEqual(source, dest);
            }
        }
    }
}
