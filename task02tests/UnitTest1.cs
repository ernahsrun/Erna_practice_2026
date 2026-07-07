global using Xunit;
using task02;
using System.Linq;
using System.Collections.Generic;

namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 4, 5, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3} },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
        };
        
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {

        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

     [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result); 
    }

     [Fact]
    public void GetStudentsOrderedByName_ReturnCorrect()
    {
        var result = _service.GetStudentsOrderedByName().ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal("Анна", result[0].Name);
        Assert.Equal("Иван", result[1].Name);
        Assert.Equal("Петр", result[2].Name);
    }

     [Fact]
    public void GroupStudentsByFaculty_ReturnCorrect()
    {
        var lookup = _service.GroupStudentsByFaculty();

        Assert.Equal(2, lookup.Count);
        Assert.True(lookup.Contains("ФИТ"));
        Assert.True(lookup.Contains("Экономика"));
        Assert.Equal(2, lookup["ФИТ"].Count());
        Assert.Single(lookup["Экономика"]);
    }

     [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnCorrect()
    {
        var result = _service.GetStudentsWithMinAverageGrade(4.0).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Name == "Иван");
        Assert.Contains(result, s => s.Name == "Петр");
        Assert.DoesNotContain(result, s => s.Name == "Анна");
    }
}
