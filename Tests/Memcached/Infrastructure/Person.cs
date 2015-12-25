using System;
using System.Runtime.Serialization;

namespace ReusableLibrary.Memcached.Tests.Infrastructure
{
    [Serializable]
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public int PostalCode { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Person that)
        {
            return that != null 
                && Id == that.Id
                && Name == that.Name
                && FirstName == that.FirstName
                && LastName == that.LastName
                && Age == that.Age
                && PostalCode == that.PostalCode;
        }
    }
}
