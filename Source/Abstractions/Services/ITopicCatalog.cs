using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Services
{
    public interface ITopicCatalog : IDisposable
    {
        T Get<T>(string name) where T : AbstractTopic;
    }
}
