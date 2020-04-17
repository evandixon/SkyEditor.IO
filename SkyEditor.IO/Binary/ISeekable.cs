using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.IO.Binary
{
    public interface ISeekable
    {
        /// <summary>
        /// Current position of data for use with sequential reads/writes
        /// </summary>
        long Position { get; set; }
    }
}
