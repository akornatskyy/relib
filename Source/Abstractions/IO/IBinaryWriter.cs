using System;
using System.Collections.Generic;

namespace ReusableLibrary.Abstractions.IO
{
    public interface IBinaryWriter
    {
        int Write(byte[] bytes, int offset, int count);

        int Write(IList<ArraySegment<byte>> buffers);
    }
}
