using System.ComponentModel;
using System.Text;
using Confluent.Kafka;
using Kafka.Configurations;
using LZ4;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceA.Model;

namespace ServiceИ.Services;

public class ConsumerService
{
    private IProducer<string, string> _producer;
    
    private readonly string _bootstrapServers;
    private readonly string _topic;
    
    public Person Person { get; set; }

    public ConsumerService(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        
        Init();
    }
    private void Init()
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = _bootstrapServers,
            ClientId = "Kafka.Dotnet.Sample",

            SecurityProtocol = SecurityProtocol.Plaintext,
            EnableDeliveryReports = false,
            QueueBufferingMaxMessages = 10000000,
            QueueBufferingMaxKbytes = 100000000,
            BatchNumMessages = 500,
            Acks = Acks.All,
            DeliveryReportFields = "none"
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public Message<string,string> CreateMessage(Person person)
    {
        var json = person.ToString();
        var msg = new Message<string, string>
        {
            Key = person.Id.ToString(),
            Value = Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(json)))
        };
        return msg;
    }
    
    public async Task Produce(Person person)
    {
        try
        {
            var msg = CreateMessage(person);
            var result = await _producer.ProduceAsync(_topic, msg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}