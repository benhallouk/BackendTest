using AddressProcessing.Address;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace AddressProcessing.Tests
{
    [TestFixture]
    public class AddressFileProcessorTests
    {
        private FakeMailShotService _fakeMailShotService;
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string LineValue = "Value 1\tValue 2\t\r\n";

        [SetUp]
        public void SetUp()
        {
            _fakeMailShotService = new FakeMailShotService();
            
        }

        [Test]
        public void Should_send_mail_using_mailshot_service()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(LineValue));
            var cSVReaderWriter = new CSVReaderWriter(stream);

            var processor = new AddressFileProcessor(_fakeMailShotService, cSVReaderWriter);
            processor.Process(TestInputFile);            

            Assert.That(_fakeMailShotService.Counter, Is.EqualTo(1));
        }

        internal class FakeMailShotService : IMailShot
        {
            internal int Counter { get; private set; }

            public void SendMailShot(string name, string address)
            {
                Counter++;
            }
        }
    }
}