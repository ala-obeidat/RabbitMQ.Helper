using RabbitMQHelper;
using System;

namespace RabbitCaller
{
    class Program
    {
        private const string server = "localhost";
        private const int port = 5672;
        private const string username = "admin";
        private const string password = "admin";
        private const string exchangeName = "Client_Exchange";
        private const string queueName = "Client_OC";
        private const string routingKey = "oc";
        private const bool push = true;
        private const bool consume = true;

        static void Main(string[] args)
        {
            if (push)
            {
                var publisher = new RabbitMQPublisher(server, port, username, password);
                publisher.BindAddress(routingKey, exchangeName);
                for (int i = 0; i < 100; i++)
                {
                    publisher.pushToQueue(routingKey, new PersonModel()
                    {
                        Id = i,
                        Name = $"Name_{i}",
                        CreatedDate = DateTime.Now.AddMinutes(i)
                    });
                }
            }
            if (consume)
            {
                var consumer = new RabbitMQConsumer(server, port, username, password, exchangeName, queueName, routingKey);
                consumer.Init((PersonModel item) =>
                {
                    Console.WriteLine($" Id: {item.Id}\t|\tName: {item.Id}\t|\tDate: {item.CreatedDate.ToString()}");
                    return true;
                });
                consumer.Consume();
            }
            Console.WriteLine("__________________________________________________________");
            Console.ReadKey();

        }
    }
}
