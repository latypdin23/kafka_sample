using Confluent.Kafka;
using ServiceB.Services;

// Проверка доступности Kafka
await WaitForKafkaAsync("kafka:9092");

var cts=new CancellationTokenSource();

var consumer = new ConsumerService("kafka:9092", "kek", "test-group");

consumer.Consume(cts.Token);


static async Task WaitForKafkaAsync(string bootstrapServers, int retryDelayMs = 10000, int maxRetries = 10)
{
    Console.WriteLine($"Waiting for Kafka at {bootstrapServers}...");
    int retries = 0;

    while (retries < maxRetries)
    {
        try
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync("health-check", new Message<Null, string> { Value = "health-check" });
            Console.WriteLine("Kafka is available!");
            return;
        }
        catch (Exception ex)
        {
            retries++;
            Console.WriteLine($"Kafka is not ready. Retry {retries}/{maxRetries} in {retryDelayMs / 1000} seconds...");
            await Task.Delay(retryDelayMs);
        }
    }

    throw new Exception("Kafka is not available after maximum retries.");
}