using System;
using System.Collections.Generic;

namespace student.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? FatherName { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Studentcourse> Studentcourses { get; } = new List<Studentcourse>();
}
