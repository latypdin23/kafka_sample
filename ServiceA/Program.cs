using Confluent.Kafka;
using ServiceA.Model;
using ServiceА.Services;

// Проверка доступности Kafka
await WaitForKafkaAsync("kafka:9092");

var producer = new ProducerService("kafka:9092", "kek");
Person person = new Person("Test", "Testov", new DateTime(1996, 09, 23));
await producer.Produce(person);

static async Task WaitForKafkaAsync(string bootstrapServers, int retryDelayMs = 10000, int maxRetries = 10)
{
    Console.WriteLine($"Ожидание кафки: {bootstrapServers}...");
    int retries = 0;

    while (retries < maxRetries)
    {
        try
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync("health-check", new Message<Null, string> { Value = "health-check" });
            Console.WriteLine("Kafka доступна!");
            return;
        }
        catch (Exception ex)
        {
            retries++;
            Console.WriteLine($"Kafka is not ready. Retry {retries}/{maxRetries} in {retryDelayMs / 1000} seconds...");
            Console.WriteLine($"Error: {ex.Message}");  // Логирование ошибки
            await Task.Delay(retryDelayMs);
        }

    }

    throw new Exception("Kafka is not available after maximum retries.");
}