namespace ReusableLibrary.Abstractions.Models
{
    public interface ILazy<T>
    {
        bool Loaded { get; }

        void Reset();

        T Object { get; }
    }
}
