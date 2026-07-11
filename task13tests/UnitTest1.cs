using Xunit;
using System;
using System.Collections.Generic;
using System.Text.Json;
using task13;

namespace task13tests;

public class JsonSerializationTests
{
    [Fact]
    public void Serialization_ShouldIgnoreNullValuesAndFormatDate()
    {
        var student = new Student
        {
            FirstName = "Иван",
            LastName = null,
            BirthDate = new DateTime(2005, 5, 20),
            Grades = new List<Subject> { new Subject { Name = "Программирование", Grade = 5 } }
        };

        // Act
        string json = StudentJsonSerializer.Serialize(student);

        // Assert
        Assert.Contains("\"FirstName\": \"Иван\"", json);
        Assert.DoesNotContain("LastName", json); 
        Assert.Contains("\"BirthDate\": \"2005-05-20\"", json); 
    }

    [Fact]
    public void Deserialize_ShouldThrow_WhenFirstNameIsEmpty()
    {
        string invalidJson = "{ \"FirstName\": \"\", \"BirthDate\": \"2005-05-20\" }";

        Assert.Throws<JsonException>(() => StudentJsonSerializer.Deserialize(invalidJson));
    }
}
