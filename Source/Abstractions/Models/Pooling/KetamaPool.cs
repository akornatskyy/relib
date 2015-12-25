using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using ReusableLibrary.Abstractions.Cryptography;

namespace ReusableLibrary.Abstractions.Models
{
    /// <summary>
    /// A consistent hashing algorithm
    /// http://www.last.fm/user/RJ/journal/2007/04/10/rz_libketama_-_a_consistent_hashing_algo_for_memcache_clients
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public class KetamaPool<T> : Disposable, IPool<T>
        where T : class
    {
        private const int NumberOfHashesPerItem = 100;

        private static readonly Encoding g_encoding = Encoding.ASCII;

        private readonly List<uint> m_hashes;
        private readonly IDictionary<uint, T> m_items;
        private readonly IHashAlgorithmProvider m_hashAlgorithmProvider;

        public KetamaPool(string name, IHashAlgorithmProvider hashAlgorithmProvider)
        {
            Name = name;
            m_hashAlgorithmProvider = hashAlgorithmProvider;
            m_hashes = new List<uint>();
            m_items = new Dictionary<uint, T>();
        }

        #region IPool<T> Members

        public string Name { get; set; }

        public virtual T Take(object state)
        {
            T item = state as T;
            if (item != null)
            {
                if (!Remove(item))
                {
                    return default(T);
                }
            }
            else
            {
                item = Lookup(state);
            }

            return item;
        }

        public virtual bool Return(T item)
        {
            var name = item.ToString();
            var hashes = GenerateHashes(name);

            Thread.BeginCriticalRegion();
            foreach (var hash in hashes)
            {
                // List of hashes must have uniqueue values only
                if (m_hashes.Contains(hash))
                {
                    continue;
                }

                m_hashes.Add(hash);
                m_items[hash] = item;
            }

            m_hashes.Sort(0, m_hashes.Count, null);
            Thread.EndCriticalRegion();

            return true;
        }

        public bool Clear()
        {
            Thread.BeginCriticalRegion();

            m_hashes.Clear();
            m_items.Clear();

            Thread.EndCriticalRegion();
            
            return true;
        }

        public int Size
        {
            get { return m_hashes.Count; }
        }

        public int Count
        {
            get { return m_hashes.Count; }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
            }
        }

        protected bool Remove(T item)
        {
            var name = item.ToString();
            if (!m_hashes.Contains(ComputeHashKey(name)))
            {
                return false;
            }

            var hashes = GenerateHashes(name);
            
            Thread.BeginCriticalRegion();
            foreach (var hash in hashes)
            {
                if (!m_hashes.Remove(hash)
                    || !m_items.Remove(hash))
                {
                    Thread.EndCriticalRegion();
                    throw new InvalidOperationException("Unable remove item");
                }
            }

            m_hashes.Sort(0, m_hashes.Count, null);
            Thread.EndCriticalRegion();

            return true;
        }

        protected T Lookup(object state)
        {
            var count = m_hashes.Count;
            if (count == 0)
            {
                return default(T);
            }

            uint hash;
            if (count == 1)
            {
                hash = m_hashes[0];
            }
            else
            {
                hash = ComputeHashKey((byte[])state);
                var index = m_hashes.BinarySearch(0, count, hash, null);
                if (index < 0)
                {
                    index = ~index;
                    hash = m_hashes[index >= count ? 0 : index];
                }
            }

            return m_items[hash];
        }

        private IEnumerable<uint> GenerateHashes(string name)
        {
            yield return ComputeHashKey(name);
            for (int i = NumberOfHashesPerItem; i < NumberOfHashesPerItem * 2; i++)
            {
                yield return ComputeHashKey(String
                    .Concat(i.ToString(CultureInfo.InvariantCulture), name));
            }
        }

        private uint ComputeHashKey(string key)
        {
            return ComputeHashKey(g_encoding.GetBytes(key));
        }

        private uint ComputeHashKey(byte[] key)
        {
            var hashAlgorithm = m_hashAlgorithmProvider.Create();
            var hash = hashAlgorithm.ComputeHash(key, 0, key.Length);
            var hashkey = BitConverter.ToUInt32(hash, 0);
            hashAlgorithm.Clear();
            return hashkey;
        }
    }
}
