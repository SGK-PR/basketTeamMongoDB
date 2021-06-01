using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basketTeam
{
    class Program
    {
        static void Main(string[] args)
        {

            FindDocs();

            Console.WriteLine("чтобы добавить информацию напишите 1, \nчтобы изменить информацию напишите 2, \nчтобы удалить информацию напишите 3");
            string selection = Console.ReadLine();

            if (selection == "1")
            {
                SaveDocs();
            }
            if (selection == "2")
            {
                UpdatePlayers();
            }
            if (selection == "3")
            {
                DeletePlayers();
            }



            Console.ReadKey();
        }
        private static async Task FindDocs()
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("basketTeam");
            var collection = database.GetCollection<BsonDocument>("players");
            var filter = new BsonDocument();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var people = cursor.Current;
                    foreach (var doc in people)
                    {
                        Console.WriteLine(doc);

                    }
                }
            }
        }
        private static async Task DeletePlayers()
        {
            Console.WriteLine("Введите имя игрока которого нужно удалить.");
            string Name = Console.ReadLine();
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("basketTeam");
            var collection = database.GetCollection<BsonDocument>("players");

            var filter = Builders<BsonDocument>.Filter.Eq("Name", Name);
            await collection.DeleteOneAsync(filter);

            var people = await collection.Find(new BsonDocument()).ToListAsync();
            foreach (var p in people)
                Console.WriteLine(p);
            Console.WriteLine("Игрок удалён");
        }
        private static async Task SaveDocs()
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("basketTeam");
            var collection = database.GetCollection<Players>("players");
            Console.WriteLine("введите количество игроков которых хотите добавить в бд");
            int ColUch = Convert.ToInt32(Console.ReadLine());



            for (int i = 0; i <= ColUch; i++)
            {
                Console.WriteLine("Напишите имя игрока ");
                string name = Console.ReadLine();
                Console.WriteLine("Напишите фамилию игрока ");
                string surname = Console.ReadLine();
                Console.WriteLine("Напишите Возраст игрока ");
                string age = Convert.ToString(Console.ReadLine());

                Console.WriteLine("Напишите команду");
                string nameTeam = Console.ReadLine();

                Players players = new Players
                {
                    Name = name,
                    Surname = surname,
                    Age = age,
                    nameTeam = nameTeam


                };
                await collection.InsertOneAsync(players);
                Console.WriteLine("сохранено");
            }



        }
        private static async Task UpdatePlayers()// изменение бд
        {
            Console.WriteLine("Напишите имя игрока которого хотите изменить");
            string NameIsm = Console.ReadLine();
            Console.WriteLine("Введите новое имя");
            string Name = Console.ReadLine();

            Console.WriteLine("Напишите фамилию игрока которого хотите изменить");
            string SurnameIsm = Console.ReadLine();
            Console.WriteLine("Введите новую фамилию");
            string Surname = Console.ReadLine();

            Console.WriteLine("Введите возраст");
            string age = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Введите команду");
            string nameTeam = Convert.ToString(Console.ReadLine());


            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("basketTeam");
            var collection = database.GetCollection<BsonDocument>("players");
            var result = await collection.ReplaceOneAsync(new BsonDocument("Name", NameIsm),
            new BsonDocument
            {
{"Name", Name},
{"Surname", Surname },
{"Age", age},
{"nameTeam", nameTeam},

            });
            Console.WriteLine("Изменения успешно сохранены");
        }
    }
}
