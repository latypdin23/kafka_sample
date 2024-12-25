using System.Text;
using Confluent.Kafka;
using LZ4;
using ServiceA.Model;

namespace ServiceА.Services;

public class  ProducerService
{
    private IProducer<string, string> _producer;
    
    private readonly string _bootstrapServers;
    private readonly string _topic;
    
    public Person Person { get; set; }

    public ProducerService(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        
        Init();
    }
    private void Init()
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = _bootstrapServers,
            Acks = Acks.All,

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
            Console.WriteLine("Sending message to Kafka");
            var result = await _producer.ProduceAsync(_topic, msg);
            Console.WriteLine($"Message sent: {result.Value}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}