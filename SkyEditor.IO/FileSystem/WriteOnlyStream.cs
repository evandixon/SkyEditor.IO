using System;
using System.IO;

namespace SkyEditor.IO.FileSystem
{
    public class WriteOnlyStream : Stream
    {
        public WriteOnlyStream(Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        private Stream? stream;

        public override bool CanRead => false;

        public override bool CanSeek => stream?.CanSeek ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override bool CanWrite => stream?.CanWrite ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override long Length => stream?.Length ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));

        public override long Position
        {
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
            if (stream == null)
            {
                throw new ObjectDisposedException(nameof(ReadOnlyStream));
            }

            stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream?.Seek(offset, origin) ?? throw new ObjectDisposedException(nameof(ReadOnlyStream));
        }

        public override void SetLength(long value)
        {
            if (stream == null)
            {
                throw new ObjectDisposedException(nameof(ReadOnlyStream));
            }

            stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (stream == null)
            {
                throw new ObjectDisposedException(nameof(ReadOnlyStream));
            }

            stream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            stream?.Dispose();
            stream = null;
        }
    }
}