using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System; 

namespace RabbitMQHelper
{
    public class RabbitMQConsumer
    {
        private IConnection _rabbitConnection;
        private IModel _subscribeModel;
        private EventingBasicConsumer _consumer;
        private string _queueName;
        private RabbitMQConsumer()
        {

        }
        public RabbitMQConsumer(string host, int port, string username, string password, string exchangeName, string queueName, string routingKey)
        {
            _queueName = queueName;
            _rabbitConnection = RabbitMQUtility.CreateConnection(host, port, username, password);
            _subscribeModel = _rabbitConnection.CreateModel();
            _subscribeModel.QueueBind(_queueName, exchangeName, routingKey);
        }
        public void Init<T>(Func<T, bool> Handler)
        {
            _consumer = new EventingBasicConsumer(_subscribeModel);
            _consumer.Received += (sender, msg) =>
            {
                T data = RabbitMQUtility.Deserialize<T>(msg.Body.ToArray());
                if (data != null)
                {
                    if (Handler(data))
                        _subscribeModel.BasicAck(msg.DeliveryTag, false);
                }
            };
        }
        public void Consume()
        {
            _subscribeModel.BasicConsume(_queueName, false, _consumer);
        }
    }
}
