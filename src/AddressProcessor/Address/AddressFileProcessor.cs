using System;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;

namespace AddressProcessing.Address
{
    public class AddressFileProcessor
    {
        private readonly IMailShot _mailShot;
        private readonly CSVReaderWriter _reader;
        public AddressFileProcessor(IMailShot mailShot, CSVReaderWriter reader=null)
        {
            if (mailShot == null) throw new ArgumentNullException("mailShot");
            _mailShot = mailShot;
            _reader = reader;
        }

        public void Process(string inputFile)
        {
            _reader.Open(inputFile, CSVReaderWriter.Mode.Read);

            string column1, column2;

            while(_reader.Read(out column1, out column2))
            {
                _mailShot.SendMailShot(column1, column2);
            }

            _reader.Close();
        }
    }
}
