namespace ReusableLibrary.Abstractions.IO
{
    public interface IBinaryReader
    {
        int Read(byte[] buffer, int offset, int count);
    }
}
