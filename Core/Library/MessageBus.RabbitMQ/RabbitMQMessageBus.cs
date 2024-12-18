using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQMessageBus<T> : IMessaging<T>
    {
        public event MessageReceived<T> OnMessageReceived;

        private IModel channel;
        private IConnection connection;
        private EventingBasicConsumer consumer;
        private string tagConsumer;
        private string connectionString; //amqp://guest:guest@localhost:5672
                                         //private readonly MobileAppContext _context;
                                         //  private readonly IConfiguration _configuration;
        public RabbitMQMessageBus()
        {
        }

        //public RabbitMQMessageBus(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    this.connectionString = _configuration.GetSection("RabbitMQ_Uri").Value;
        //}

        public RabbitMQMessageBus(string connectionstring)
        {
            this.connectionString = connectionstring ?? throw new ArgumentNullException(nameof(connectionstring));
        }

        public RabbitMQMessageBus(RabbitMQMessageBusInfo info)
        {
            this.connectionString = info.connectionString ?? throw new ArgumentNullException(nameof(info.connectionString));
        }

        public async void PushMessage(T message, string queue)
        {
            try
            {

                //controllo dell'esistenza del queue name
                if (string.IsNullOrEmpty(queue))
                {
                    throw new ArgumentException("queue name cannot be null or empty");
                }

                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

                using (var connection = factory.CreateConnection())
                {

                    using (var channel = connection.CreateModel())
                    {

                        //dichiara la coda in caso non esista, nel caso in cui esista non esegue alcun operazione
                        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: true, arguments: null);

                        //trasforma il messaggio in byte[]
                        var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        //publica sulla queue
                        channel.BasicPublish("", queue, properties, body);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine("Errore durante l'invio del messaggio: " + ex.Message);
            }
        }


        public T BasicGetSingleMessage(string queue)
        {
            try
            {
                T message = default(T);

                //controllo dell'esistenza del queue name
                if (string.IsNullOrEmpty(queue))
                {
                    throw new ArgumentException("queue name cannot be null or empty");
                }

                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        //dichiara la coda in caso non esista, nel caso in cui esista non esegue alcun operazione
                        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: true, arguments: null);

                        //var consumer = new EventingBasicConsumer(channel);

                        var data = channel.BasicGet(queue, true);
                        if (data != null)
                        {
                            message = System.Text.Json.JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data.Body.ToArray()));
                        }

                    }
                }

                return message;
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine("Errore durante l'invio del messaggio: " + ex.Message);
            }

        }


        public T GetSingleMessage(string queue, Action<T> beforeAck)
        {
            try
            {
                T message = default(T);

                //controllo dell'esistenza del queue name
                if (string.IsNullOrEmpty(queue))
                {
                    throw new ArgumentException("queue name cannot be null or empty");
                }

                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        //dichiara la coda in caso non esista, nel caso in cui esista non esegue alcun operazione
                        channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: true, arguments: null);

                        var consumer = new EventingBasicConsumer(channel);

                        var properties = channel.BasicGet(queue, false);
                        if (properties != null)
                        {
                            var body = properties.Body.ToArray();
                            message = System.Text.Json.JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                            beforeAck?.Invoke(message);

                            channel.BasicAck(properties.DeliveryTag, multiple: false);
                        }
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine("Errore durante l'invio del messaggio: " + ex.Message);
            }

        }


        public async Task Listening(string queue)
        {
            try
            {
                T message = default(T);

                //controllo dell'esistenza del queue name
                if (string.IsNullOrEmpty(queue))
                {
                    throw new ArgumentException("queue name cannot be null or empty");
                }

                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };

                connection = factory.CreateConnection();

                channel = connection.CreateModel();


                //dichiara la coda in caso non esista, nel caso in cui esista non esegue alcun operazione
                channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: true, arguments: null);


                consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (ch, ea) =>
                {

                    var body = ea.Body.ToArray();
                    //if (typeof(T) == typeof(String))
                    //    message = (T)Convert.ChangeType(Encoding.UTF8.GetString(body), typeof(T));
                    //else
                        message = System.Text.Json.JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                    try
                    {
                        await LaunchEventOnMessageReceived(message);
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                };

                tagConsumer = channel.BasicConsume(queue, false, consumer);
                //if OnMessageReceived is not null then call delegate
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine("Errore durante l'invio del messaggio: " + ex.Message);
            }


        }

        public void StopListening()
        {
            channel.Close();
        }

        protected virtual async Task LaunchEventOnMessageReceived(T message)
        {
            //fai qua qualcosa che serve prima di lanciare l'evento
            await OnMessageReceived?.Invoke(message);
        }

        public void Dispose()
        {
            if (channel != null)
            {
                channel.Close();
                channel.Dispose();
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }

        }


    }

    public class RabbitMQMessageBusInfo
    {

        public string connectionString;

    }

}
