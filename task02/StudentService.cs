using System.Linq;
using System.Collections.Generic;

namespace task02;

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        => _students.Where(s => s.Faculty == faculty);

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        => _students.Where(s => s.Grades != null && s.Grades.Any() && s.Grades.Average() >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(s => s.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(s => s.Faculty);

    public string GetFacultyWithHighestAverageGrade()
        => _students
            .GroupBy(s => s.Faculty)
            .Select(g => new { Faculty = g.Key, Avg = g.Average(s => s.Grades.Any() ? s.Grades.Average() : 0) })
            .OrderByDescending(f => f.Avg)
            .Select(f => f.Faculty)
            .FirstOrDefault() ?? string.Empty;
}
