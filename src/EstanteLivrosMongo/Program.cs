using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Text.Json;
using EstanteLivrosMongo;
using MongoDB.Bson;
using MongoDB.Driver;

internal class Program
{
    private static void Main(string[] args)
    {
        {
            var collection = CreateDataBase();
            int options = 0;

            do
            {
                options = Menu();

                switch (options)
                {
                    case 1:
                        var book = CreateBook();
                        collection.InsertOne(book);
                        break;
                    case 2:
                        FindBoookInShelf(collection);
                        break;
                    case 3:
                        UpdateBook(collection);
                        break;
                    case 4:
                        RemoveBook(collection);
                        break;
                    case 5:
                        PrintShelfBook(collection);
                        break;
                    case 6:
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção Inválida!");
                        Console.WriteLine("Digite Novamente: ");
                        break;
                }
            } while (options != 6);

        }
    }
    static IMongoCollection<Book> CreateDataBase()
    {
        MongoClient mongo = new MongoClient("mongodb://localhost:27017");


        var baseBook = mongo.GetDatabase("BookShelf");
        var collection = baseBook.GetCollection<Book>("MyBooks");

        Console.WriteLine("Bem vindo a Estante de Livros Virtual!");
        Console.WriteLine("Selecione a opção desejada:");

        return collection;
    }
    private static int Menu()
    {
        
        Console.WriteLine("Menu: ");
        Console.WriteLine("1-Cadastrar Livro");
        Console.WriteLine("2-Encontrar um Livro");
        Console.WriteLine("3-Update Livro");
        Console.WriteLine("4-Deletar Livro");
        Console.WriteLine("5-Mostrar Lista de Livros na Estante");
        Console.WriteLine("6-Sair do Menu");

        int options = int.Parse(Console.ReadLine());
        Console.WriteLine("\n\n");

        return options;

    }
    private static Book CreateBook()
    {

        Console.WriteLine("Vamos inserir um livro. Digite os campos abaixo: \n");

        Console.WriteLine("Digite o ISBN do livro: ");
        string isbn = Console.ReadLine();

        Console.WriteLine("Digite o Titulo do livro: ");
        string nameBook = Console.ReadLine();

        Console.WriteLine("Digite o primeiro nome do Autor do livro: ");
        string name = Console.ReadLine();

        Console.WriteLine("Digite o ultimo nome do Autor do livro: ");
        string lastName = Console.ReadLine();

        Console.WriteLine("Digite o nome da Editora do Livro: ");
        string publish = Console.ReadLine();

        Console.WriteLine("Digite a Edição do Livro: ");
        int edition = int.Parse(Console.ReadLine());

        Console.WriteLine("Digite o Ano do Livro: ");
        int year = int.Parse(Console.ReadLine());

        bool reading = false;
        bool borrewed = false;

        Author author = new(name, lastName);

        Book book = new(isbn, nameBook, author, publish, edition, year, reading, borrewed);

        return book;
    }
    private static int MenuUpdate()
    {
        
        Console.WriteLine("Digite o número que deseja realizar a atualização no livro: ");
        Console.WriteLine("1-ISBN do Livro");
        Console.WriteLine("2-Titulo do Livro");
        Console.WriteLine("3-Primeiro Nome do Autor do Livro");
        Console.WriteLine("4-Ultimo Nome do Autor do Livro");
        Console.WriteLine("5-Edicao do Livro");
        Console.WriteLine("6-Ano do Livro");
        Console.WriteLine("7-Status de leitura do Livro");
        Console.WriteLine("8-Sair do Menu de Atualizações");
        int option = int.Parse(Console.ReadLine());

        return option;
    }
    private static void UpdateBook(IMongoCollection<Book> collection)
    {
        
        int options = 0;
        bool bye = false;
        Console.WriteLine("Digite o título que você deseja modificar: ");
        var findBook = Console.ReadLine();
        var filter = Builders<Book>.Filter.Eq("NameBook", findBook);

        do
        {
            options = MenuUpdate();

            switch (options)
            {

                case 1:
                    Console.WriteLine("Digite o novo ISBN do Livro: ");
                    var newIsbn = Console.ReadLine();
                    var updateIsbn = Builders<Book>.Update.Set("ISBN", newIsbn);
                    collection.UpdateOne(filter, updateIsbn);
                    break;
                case 2:
                    Console.WriteLine("Digite o novo Título do Livro: ");
                    var newNameBook = Console.ReadLine();
                    var updateNameBook = Builders<Book>.Update.Set("NameBook", newNameBook);
                    collection.UpdateOne(filter, updateNameBook);
                    break;
                case 3:
                    Console.WriteLine("Digite o novo nome do autor do Livro: ");
                    var newName = Console.ReadLine();
                    Console.WriteLine("Digite o novo sobrenome do autor do Livro: ");
                    var newLastName = Console.ReadLine();
                    Author author = new Author(newName, newLastName);
                    var upDateAuthor = Builders<Book>.Update.Set("Author", author);
                    collection.UpdateOne(filter, upDateAuthor);
                    break;
                case 4:
                    Console.WriteLine("Digite o novo nome da editora do Livro: ");
                    var newPublisher = Console.ReadLine();
                    var updatePublisher = Builders<Book>.Update.Set("Publisher", newPublisher);
                    collection.UpdateOne(filter, updatePublisher);
                    break;
                case 5:
                    Console.WriteLine("Digite o novo número da edicao do Livro: ");
                    var newEdition = int.Parse(Console.ReadLine());
                    var updateEdition = Builders<Book>.Update.Set("Edition", newEdition);
                    collection.UpdateOne(filter, updateEdition);
                    break;
                case 6:
                    Console.WriteLine("Digite o novo ano do Livro: ");
                    var newYear = Console.ReadLine();
                    var updateYear = Builders<Book>.Update.Set("Year", newYear);
                    collection.UpdateOne(filter, updateYear);
                    break;
                case 7:
                    ChangeStatus(collection, filter);
                    break;
                case 8:
                    bye = true;
                    break;
                default:
                    Console.WriteLine("Opção Inválida!");
                    Console.WriteLine("Digite Novamente: ");
                    break;
            }
        } while (bye == false);
    }
    private static void RemoveBook(IMongoCollection<Book> collection)
    {

        Console.WriteLine("Digite o título que deseja deletar: ");
        string deleteBook = Console.ReadLine();

        var filter = Builders<Book>.Filter.Eq("NameBook", deleteBook);
        collection.DeleteOne(filter);
    }
    private static void PrintShelfBook(IMongoCollection<Book> collection)
    {
        foreach (var book in collection.Find(new BsonDocument()).ToList())
        {
            Console.WriteLine(book.ToString());
            Console.WriteLine("\n");
        }
    }
    private static void ChangeStatus(IMongoCollection<Book>collection, FilterDefinition<Book> filter)
    {
        var bookFind =collection.Find(filter).First();
      
        Console.WriteLine("O livro está: ");
        Console.WriteLine("(1) Na Estante");
        Console.WriteLine("(2) Emprestado");
        Console.WriteLine("(3) Estou Lendo");
        var valueDigited = int.Parse(Console.ReadLine());

        if (valueDigited == 1)
        {
            bookFind.Reading = false;
            bookFind.Borrewed = false;
        }
        if (valueDigited == 2)
        {
            bookFind.Reading = false;
            bookFind.Borrewed = true;
        }
        if (valueDigited == 3)
        {
            bookFind.Reading = false;
            bookFind.Borrewed = true;
        }

        var updateReading = Builders<Book>.Update.Set("Reading",bookFind.Reading);
        var updateBorrewed = Builders<Book>.Update.Set("Borrewed", bookFind.Borrewed);
        collection.UpdateOne(filter,updateReading);
        collection.UpdateOne(filter, updateBorrewed);
    }
    private static void FindBoookInShelf(IMongoCollection<Book> collection)
    {
        Console.WriteLine("Digite o titulo do livro que vc deseja encontrar na estante: ");
        var findBook = Console.ReadLine();
        var filter = Builders<Book>.Filter.Eq("NameBook", findBook);
        Console.WriteLine(collection.Find(filter).First());
    }
}