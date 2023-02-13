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
    public class StudentcoursesController : ControllerBase
    {
        private readonly studentContext _context;

        public StudentcoursesController(studentContext context)
        {
            _context = context;
        }

        // GET: api/Studentcourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Studentcourse>>> GetStudentcourses()
        {
            return await _context.Studentcourses.ToListAsync();
        }

        // GET: api/Studentcourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Studentcourse>> GetStudentcourse(int id)
        {
            var studentcourse = await _context.Studentcourses.FindAsync(id);

            if (studentcourse == null)
            {
                return NotFound();
            }

            return studentcourse;
        }

        // PUT: api/Studentcourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentcourse(int id, Studentcourse studentcourse)
        {
            if (id != studentcourse.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentcourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentcourseExists(id))
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

        // POST: api/Studentcourses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Studentcourse>> PostStudentcourse(Studentcourse studentcourse)
        {
            _context.Studentcourses.Add(studentcourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentcourse", new { id = studentcourse.Id }, studentcourse);
        }

        // DELETE: api/Studentcourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentcourse(int id)
        {
            var studentcourse = await _context.Studentcourses.FindAsync(id);
            if (studentcourse == null)
            {
                return NotFound();
            }

            _context.Studentcourses.Remove(studentcourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("AddStudentWithCourses")]
        public IActionResult AddStudentWithCourses([FromBody] Student student, [FromQuery] int[] courseIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            if (courseIds != null)
            {
                foreach (int courseId in courseIds)
                {
                    Studentcourse sc = new Studentcourse
                    {
                        StudentId = student.Id,
                        CourseId = courseId
                    };

                    _context.Studentcourses.Add(sc);
                }
            }

            _context.SaveChanges();
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }




        private bool StudentcourseExists(int id)
        {
            return _context.Studentcourses.Any(e => e.Id == id);
        }
    }
}
