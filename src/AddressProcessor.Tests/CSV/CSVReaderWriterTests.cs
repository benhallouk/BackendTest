using AddressProcessing.CSV;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {

        CSVReaderWriter _CSVReaderWriter = null;
        MemoryStream _stream = null;
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string Column1Value = @"Value 1";
        private const string Column2Value = @"Value 2";

        [SetUp]
        public void SetUp()
        {
            _stream = new MemoryStream();
            _CSVReaderWriter = new CSVReaderWriter(_stream);
        }

        [TearDown]
        public void Dispose()
        {
            _CSVReaderWriter.Dispose();
            _stream = null;
        }

        [Test]
        public void Should_open_when_parameters_are_clean()
        {
            Assert.DoesNotThrow(() =>
            {
                _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            });
        }

        [Test]
        public void Should_throw_exception_on_open_when_fileInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _CSVReaderWriter.Open(null, CSVReaderWriter.Mode.Read);
            });
        }

        [Test]
        public void Should_throw_exception_when_open_an_open_state()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
                _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
            });
        }

        [Test]
        public void Can_be_open_for_read()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);

            Assert.That(_stream.CanRead, Is.EqualTo(true));
        }

        [Test]
        public void Can_be_open_for_write()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Write);

            Assert.That(_stream.CanWrite, Is.EqualTo(true));
        }

        [Test]
        public void Should_write_line()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Write);
            var columns = new string[] { Column1Value, Column2Value };

            _CSVReaderWriter.Write(columns);

            _stream.Position = 0;

            var addedLine = new StreamReader(_stream).ReadToEnd();

            Assert.That(addedLine, Is.EqualTo(string.Join("\t", columns) + "\r\n"));
        }

        [Test]
        public void Should_read_line()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Write);

            var columns = new string[] { Column1Value, Column2Value };
            
            _CSVReaderWriter.Write(columns);

            _stream.Position = 0;

            var canRead = _CSVReaderWriter.Read(string.Empty, string.Empty);

            Assert.That(canRead, Is.EqualTo(true));
        }

        [Test]
        public void Should_read_columns()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Write);

            var columns = new string[] { Column1Value, Column2Value };
            var column1 = "";
            var column2 = "";

            _CSVReaderWriter.Write(columns);

            _stream.Position = 0;

            var canRead = _CSVReaderWriter.Read(out column1, out column2);

            Assert.That(canRead, Is.EqualTo(true));

            Assert.That(column1, Is.EqualTo(columns[0]));

            Assert.That(column2, Is.EqualTo(columns[1]));
        }

        [Test]
        public void Should_close_the_state()
        {
            _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Write);
            _CSVReaderWriter.Close();

            Assert.That(_stream.CanRead, Is.EqualTo(false));
        }

        //can do that if close implement some logic that is bit complex than that
        //[Test]
        //public void Should_throw_exception_when_close_a_closed_state()
        //{            
        //    Assert.Throws<InvalidOperationException>(() =>
        //    {
        //        _CSVReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read);
        //        _CSVReaderWriter.Close();
        //        _CSVReaderWriter.Close();
        //    });
        //}
    }
}
