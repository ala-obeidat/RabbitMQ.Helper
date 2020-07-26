using RabbitMQ.Client;
using System.Collections.Generic;

namespace RabbitMQHelper
{
    public class RabbitMQPublisher
    {
        private IConnection _rabbitConnection;
        private IModel _publishModel;
        private IBasicProperties _basicProperties;
        private static Dictionary<string, PublicationAddress> _addresses;
        private RabbitMQPublisher()
        {

        }
        public RabbitMQPublisher(string host, int port, string username, string password, bool isPersistent = true)
        {

            _rabbitConnection = RabbitMQUtility.CreateConnection(host, port, username, password);
            _publishModel = _rabbitConnection.CreateModel();
            _basicProperties = _publishModel.CreateBasicProperties();
            _basicProperties.Persistent = isPersistent;
            _addresses = new Dictionary<string, PublicationAddress>();
        }
        public void BindQueue(string exchangeName, string queueName, string routingKey)
        {
            _publishModel.QueueBind(queueName, exchangeName, routingKey);
        }
        public void BindAddress(string routingKey, string exchangeName, string exchangeType = "direct")
        {
            _addresses[routingKey] = new PublicationAddress(exchangeType, exchangeName, routingKey);
        }

        public bool pushToQueue<T>(string routingKey, T data)
        {
            try
            {
                _publishModel.BasicPublish(_addresses[routingKey], _basicProperties, RabbitMQUtility.ConvertToBinary(data));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
