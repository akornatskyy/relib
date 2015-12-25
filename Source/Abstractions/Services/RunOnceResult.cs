using System;

namespace ReusableLibrary.Abstractions.Services
{
    [Serializable]
    public sealed class RunOnceResult<T>
    {
        public bool IsCompleted { get; set; }

        public bool HasError
        {
            get
            {
                return Error != null;
            }
        }

        public string Error { get; set; }

        public T Result { get; set; }

        public RunOnceState State()
        {
            return IsCompleted 
                ? HasError ? RunOnceState.Error : RunOnceState.Done 
                : RunOnceState.Wait;
        }
    }
}
