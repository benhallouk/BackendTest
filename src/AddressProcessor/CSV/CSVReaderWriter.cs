using System;
using System.IO;
using System.Linq;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : IDisposable
    {
        private StreamReader _streamReader = null;
        private StreamWriter _streamWriter = null;
        private bool _isOpen = false;
        private const int FIRST_COLUMN = 0;
        private const int SECOND_COLUMN = 1;

        public CSVReaderWriter()
        {

        }

        public CSVReaderWriter(Stream stream)
        {
            _streamWriter = new StreamWriter(stream);
            _streamReader = new StreamReader(stream);
        }

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (_isOpen) throw new InvalidOperationException("can not open CSVReaderWriter it has already open state");

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("the parameter fileName can not be null or empty");
                       
            if ((_streamReader == null || _streamWriter == null)  && !File.Exists(fileName))
                throw new FileNotFoundException("Can not find the file " + fileName);
            
            if (mode == Mode.Read && _streamReader==null)
            {
                _streamReader = File.OpenText(fileName);                
            }

            if (mode == Mode.Write && _streamWriter == null)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _streamWriter = fileInfo.CreateText();                
            }

            _isOpen = true;
        }

        public void Write(params string[] columns)
        {            
            var line = string.Join("\t", columns);
            if (!string.IsNullOrEmpty(line))
            {
                _streamWriter.WriteLine(line);
            }
            _streamWriter.Flush();
        }

        public bool Read(string column1, string column2)
        {
            var line = _streamReader.ReadLine();

            if (string.IsNullOrEmpty(line)) return false;

            var columns = line.Split(new char[] { '\t' });

            return columns.Length>0;
        }

        public bool Read(out string column1, out string column2)
        {
            var line = _streamReader.ReadLine();
            
            column1 = null;
            column2 = null;

            if (string.IsNullOrEmpty(line))  return false;
            var columns = line.Split(new char[] { '\t' });

            var columnsLength = columns.Length;

            if (columnsLength > 1)
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];
                return true;
            }

            return false;            
        }

        public void Close()
        {
            //can do that if close implement some logic that is bit complex than that
            //if (_isOpen == false) throw new InvalidOperationException("can not close CSVReaderWriter it has already in close state");
            Dispose();
        }

        public void Dispose()
        {
            if (_streamReader!=null) ((IDisposable)_streamReader).Dispose();
            if(_streamWriter!=null) ((IDisposable)_streamWriter).Dispose();
            _isOpen = false;
        }
    }
}
