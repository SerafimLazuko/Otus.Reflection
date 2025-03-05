# Otus.Reflection

## Результаты замеров

### На 1000 итераций

- Среднее время сериализации (GenericSerializer): 0,0080525 мс
- Среднее время десериализации (GenericSerializer): 0,0140274 мс
- Среднее время сериализации (Newtonsoft.Json): 0,1227129 мс
- Среднее время десериализации (Newtonsoft.Json): 0,0261722 мс

### На 100000 итераций

- Среднее время сериализации (GenericSerializer): 0,001531778 мс
- Среднее время десериализации (GenericSerializer): 0,005161598 мс
- Среднее время сериализации (Newtonsoft.Json): 0,00698545899 мс
- Среднее время десериализации (Newtonsoft.Json): 0,005391395 мс

### На 1000000 итераций

- Среднее время сериализации (GenericSerializer): 0,0014830391 мс
- Среднее время десериализации (GenericSerializer): 0,0022175826 мс
- Среднее время сериализации (Newtonsoft.Json): 0,0014393384 мс
- Среднее время десериализации (Newtonsoft.Json): 0,0020026345 мс


## Модель

```csharp
public class UserProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Interests { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}
```

## Код Сериализатора

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A generic serializer class for converting objects to and from CSV format.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to serialize and deserialize.</typeparam>
internal static class GenericSerializer<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// Serializes an object to a CSV string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <returns>A CSV string representing the object.</returns>
    public static string Serialize(TEntity value) 
    {
        var properties = typeof(TEntity).GetProperties();
        var csv = new StringBuilder();

        csv.AppendLine(string.Join(",", properties.Select(p => p.Name)));
        csv.AppendLine(string.join(',', properties.Select(p => FormatValue(p.GetValue(value)))));

        return csv.ToString();
    }

    /// <summary>
    /// Deserializes a CSV string to an object.
    /// </summary>
    /// <param name="csv">The CSV string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    public static TEntity Deserialize(string csv) 
    {
        var lines = csv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        var header = lines[0].Split(',');
        var values = lines[1].Split(',');

        var properties = typeof(TEntity).GetProperties();
        var entity = new TEntity();

        for (int i = 0; i < header.Length; i++)
        {
            var property = properties.FirstOrDefault(p => p.Name == header[i]);
            if (property != null)
            {
                object value = property.PropertyType == typeof(List<string>) 
                    ? values[i].Split(';').ToList() 
                    : Convert.ChangeType(values[i], property.PropertyType);

                property.SetValue(entity, value, null);
            }
        }

        return entity;
    }

    /// <summary>
    /// Formats a value for CSV output.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A formatted string representing the value.</returns>
    private static string FormatValue(object value)
    {
        if (value == null)
            return string.Empty;

        if (value is DateTime dateTime)
            return dateTime.ToString("yyyy-MM-dd");

        if (value is IEnumerable<string> enumerable)
            return string.join(";", enumerable);

        return value.ToString();
    }
}
```
