using System;
using System.Collections.Generic;

namespace student.Models;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Studentcourse> Studentcourses { get; } = new List<Studentcourse>();
}
