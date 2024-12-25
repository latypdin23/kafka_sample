using Kafka.Extensions;
using Newtonsoft.Json;

namespace Kafka.Model;

public class SampleData
{
    public Guid Id { get; set; }
    public double Value { get; set; }
    public DateTime Created { get; set; }

    public SampleData()
    {
        Id = Guid.NewGuid();
        Value = NumberExtensions.RandomNumberBetween(0.0, 99.9);
        Created = DateTime.UtcNow;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
    }
}