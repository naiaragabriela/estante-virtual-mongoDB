using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstanteLivrosMongo
{
    internal class Author
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public Author(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}
