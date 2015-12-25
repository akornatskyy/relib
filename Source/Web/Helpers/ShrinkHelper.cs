using System;
using System.Text;

namespace ReusableLibrary.Web.Helpers
{
    public static class ShrinkHelper
    {
        private static readonly byte[] ForwardMatch = Encoding.ASCII.GetBytes(">;{}");
        private static readonly byte[] BackwardMatch = Encoding.ASCII.GetBytes("<{}");
        private static readonly byte[] IgnoreMatch = Encoding.ASCII.GetBytes(" \r\n\t");

        public static int LeaveAtLeast { get; set; }

        public static int Shrink(byte[] buffer, int offset, int count)
        {
            int length = ShrinkBackward(buffer, offset, count);
            length = ShrinkForward(buffer, offset, buffer.Length - length, length);
            return length;
        }

        public static int ShrinkForward(byte[] buffer, int resultOffset, int offset, int count)
        {
            var result = buffer;
            var resultIndex = resultOffset;
            var state = new ShrinkState();
            for (int bufferIndex = offset; bufferIndex < offset + count; bufferIndex++)
            {
                state.Current = buffer[bufferIndex];
                if (IgnoreCurrent(ForwardMatch, state))
                {
                    continue;
                }

                result[resultIndex++] = state.Current;
            }

            return resultIndex;
        }

        public static int ShrinkBackward(byte[] buffer, int offset, int count)
        {
            var result = buffer;
            var resultIndex = count - offset;
            var state = new ShrinkState();
            for (int bufferIndex = offset + count - 1; bufferIndex >= 0; bufferIndex--)
            {
                state.Current = buffer[bufferIndex];
                if (IgnoreCurrent(BackwardMatch, state))
                {
                    continue;
                }

                result[--resultIndex] = state.Current;
            }

            return result.Length - resultIndex;
        }

        public static bool IgnoreCurrent(byte[] match, ShrinkState state)
        {
            if (Array.IndexOf(match, state.Current) >= 0)
            {
                state.InsideTag = true;
                state.IgnoreCount = 0;
            }
            else
            {
                if (state.InsideTag)
                {
                    if (Array.IndexOf(IgnoreMatch, state.Current) >= 0)
                    {
                        // Leave at least a single space
                        if (++state.IgnoreCount > LeaveAtLeast)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        state.InsideTag = false;
                    }
                }
            }

            return false;
        }
    }
}
