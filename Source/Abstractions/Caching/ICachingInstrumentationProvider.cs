namespace ReusableLibrary.Abstractions.Caching
{
    public interface ICachingInstrumentationProvider
    {
        void FireFailed();

        void FireAccessed(bool hit);

        void FireStored(bool succeed);

        void FireRemoved(bool succeed);
    }
}
