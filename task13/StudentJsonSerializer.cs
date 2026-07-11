using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public static class StudentJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        
        WriteIndented = true
    };

    static StudentJsonSerializer()
    {
        Options.Converters.Add(new CustomDateTimeConverter());
    }

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, Options);
    }

    public static Student? Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, Options);
        
        if (student != null && string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new JsonException("Ошибка валидации: Имя студента не может быть пустым.");
        }
        
        return student;
    }
}

public class CustomDateTimeConverter : JsonConverter<System.DateTime>
{
    private const string Format = "yyyy-MM-dd";

    public override System.DateTime Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
    {
        return System.DateTime.ParseExact(reader.GetString()!, Format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, System.DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
