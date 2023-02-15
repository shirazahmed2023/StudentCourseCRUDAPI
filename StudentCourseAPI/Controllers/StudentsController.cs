using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;
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
        private readonly ILoggerManager _logger;


        public StudentsController(studentContext context,ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInfo("Getting Students");
            try
            {
                _logger.LogInfo("Returned Student Successfully");
                return await _context.Students.ToListAsync();
            }
            
            catch(Exception ex)
            {
                _logger.LogError($"Error getting students: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
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
            _logger.LogInfo("Into Deleting Process");
            try {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                _logger.LogInfo("Deleted Successfully");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Deleting: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }



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
            _logger.LogInfo("Into Method Get Students");
            try {
                _logger.LogInfo("Student Data Have Been Displayed");
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
                _logger.LogInfo("Student Data Have Been Displayed");
                return StudentsWithCourses;
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Fetching Students: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }


        }


        [HttpPost("AddStudentWithCourses")]
        public async Task<ActionResult<Student>> AddStudentWithCourses(
            [FromBody] Student student,
            [FromQuery] int[] courseIds)
        {
            _logger.LogInfo("Process Of Creation Started");
            try
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
                _logger.LogInfo("Student Created Successfully!");
                return CreatedAtAction("GetStudent", new { id = student.Id }, student);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Creating Controller Not Working: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut("UpdateStudentWithCourses")]
        public async Task<IActionResult> UpdateStudentWithCourses(int id, [FromBody] SModelClass model)
        {
            _logger.LogInfo("Working on Updation");            
            if (id != model.Student.Id)
            {
                _logger.LogWarning("Student ID Does not Exists");
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
                _logger.LogInfo("Data has been Updated");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    _logger.LogError("Student id does not matched");
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
