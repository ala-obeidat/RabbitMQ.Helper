using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RabbitMQHelper
{
    internal static class RabbitMQUtility
    {
        internal static IConnection CreateConnection(string host, int port, string username, string password)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                UserName = username,
                Password = password,
                //RequestedHeartbeat = 10,
            };

            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            return connectionFactory.CreateConnection();
        }
        internal static byte[] ConvertToBinary(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, obj);
                var bytes = ms.ToArray();
                return bytes;
            }
        }
        internal static T Deserialize<T>(byte[] arrBytes)
        {
            try
            {
                var binForm = new BinaryFormatter();
                using (var memStream = new MemoryStream(arrBytes))
                {
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    return (T)binForm.Deserialize(memStream);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
