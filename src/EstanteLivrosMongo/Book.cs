using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EstanteLivrosMongo
{
    internal class Book
    {
        public ObjectId Id { get; set; }
        public string? ISBN { get; set; }
        public string NameBook { get; set; }
        public Author Author { get; set; }
        public string Publisher { get; set; }
        public int Edition { get; set; }
        public int Year { get; set; }
        public bool Reading { get; set;}
        public bool Borrewed { get; set; }

        public Book(string iSBN, string nameBook, Author author, string publisher, int edition, int year, bool reading, bool borrewed)
        {
            Id = ObjectId.GenerateNewId();
            ISBN = iSBN;
            NameBook = nameBook;
            Author = author;
            Publisher = publisher;
            Edition = edition;
            Year = year;
            Reading = reading;
            Borrewed = borrewed;
        }

        public override string ToString()
        {
            return $"ISBN: {ISBN}, \nNome do Livro: {NameBook}, \nAuthor:{Author}," +
                $" \nEditora: {Publisher}, \nEdição:{Edition}, \nAno: {Year}, \nLendo o livro: {Reading}, \nLivro Emprestado: {Borrewed}";
        }
    }
}
