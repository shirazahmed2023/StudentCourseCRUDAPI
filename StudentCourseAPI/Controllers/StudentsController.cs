using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using student.Data;
using student.Models;

namespace StudentCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly studentContext _context;

        public StudentsController(studentContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("GetStudentbyID")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetStudentsWithCoursesbyID")]
        public ActionResult<object> GetStudentsWithCoursesbyID(int id)
        {
            var StudentsWithCourses = _context.Students
                .Include(c => c.Studentcourses)
                .ThenInclude(sc => sc.Course)
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.FatherName,
                    c.Address,
                    Course = c.Studentcourses.Select(sc => new { 
                        sc.Course.Id,
                        sc.Course.Name
                    })
                })
                .FirstOrDefault();

            if (StudentsWithCourses == null)
            {
                return NotFound();
            }

            return StudentsWithCourses;
        }

        [HttpGet("GetStudentsWithCourses")]
        public ActionResult<object> GetStudentsWithCourses()
        {
            var StudentsWithCourses = _context.Students
                .Include(c => c.Studentcourses)
                .ThenInclude(sc => sc.Course)
                .Where(c => c.Id == c.Id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.FatherName,
                    c.Address,
                    Course = c.Studentcourses.Select(sc => sc.Course.Name)
                })
                .ToList();

            if (StudentsWithCourses == null)
            {
                return NotFound();
            }

            return StudentsWithCourses;
        }


        [HttpPost("AddStudentWithCourses")]
        public async Task<ActionResult<Student>> AddStudentWithCourses(
            [FromBody] Student student,
            [FromQuery] int[] courseIds)
        {

            // Add the student to the database
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Add the courses for the student
            foreach (var courseId in courseIds)
            {
                var studentCourse = new Studentcourse
                {
                    StudentId = student.Id,
                    CourseId = courseId
                };
                _context.Studentcourses.Add(studentCourse);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        [HttpPut("UpdateStudentWithCourses")]
        public async Task<IActionResult> UpdateStudentWithCourses(int id, [FromBody] SModelClass model)
        {
            if (id != model.Student.Id)
            {
                return BadRequest();
            }

            _context.Entry(model.Student).State = EntityState.Modified;

            // Keep all existing courses for this student
            var existingStudentCourses = _context.Studentcourses.Where(sc => sc.StudentId == id);

            // Add new courses for this student
            if (model.CourseIds != null)
            {
                foreach (var courseId in model.CourseIds)
                {
                    if (!existingStudentCourses.Any(sc => sc.CourseId == courseId))
                    {
                        _context.Studentcourses.Add(new Studentcourse
                        {
                            StudentId = id,
                            CourseId = courseId
                        });
                    }
                }
            }

            // Remove courses for this student
            foreach (var existingCourse in existingStudentCourses)
            {
                if (model.CourseIds == null || !model.CourseIds.Any(c => c == existingCourse.CourseId))
                {
                    _context.Studentcourses.Remove(existingCourse);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        public class SModelClass
        {
            public Student Student { get; set; }
            public int[] CourseIds { get; set; }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
