using System.Text;

namespace Otus.Reflection;

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
        csv.AppendLine(string.Join(',', properties.Select(p => FormatValue(p.GetValue(value)))));

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
            return string.Join(";", enumerable);

        return value.ToString();
    }
}