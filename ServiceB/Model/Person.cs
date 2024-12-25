using Newtonsoft.Json;

namespace ServiceA.Model;

public class Person
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    /// <summary>
    /// Инициализирует объект Person с указаннами параметрами
    /// </summary>
    /// <param name="firstName">Имя</param>
    /// <param name="lastName">Фамилия</param>
    /// <param name="dateOfBirth">Дата рождения</param>
    public Person(string firstName, string lastName, DateTime? dateOfBirth)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
    /// <summary>
    /// Приводит объект к JSON формату
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
    }
}