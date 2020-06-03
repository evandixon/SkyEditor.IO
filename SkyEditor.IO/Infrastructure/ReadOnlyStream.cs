using System;
using System.IO;

namespace SkyEditor.IO.Infrastructure
{
    public class ReadOnlyStream : Stream
    {
        public ReadOnlyStream(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        private Stream? stream;

        public override bool CanRead => stream?.CanRead ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override bool CanSeek => stream?.CanSeek ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override bool CanWrite => false;

        public override long Length => stream?.Length ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override long Position { 
            get => stream?.Position ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));
            set 
            {
                if (stream == null) 
                { 
                    throw new ObjectDisposedException(nameof(ReadOnlyStream)); 
                }
                stream.Position = value; 
            }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return stream?.Read(buffer, offset, count) ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream?.Seek(offset, origin) ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            stream?.Dispose();
            stream = null;
        }
    }
}
