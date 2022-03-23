#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simple_api.Contexts;
using simple_api.Models;
using simple_api.ViewModels;

namespace simple_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly MyContext _context;

        public StudentsController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents()
        {
            return _context.Students.Select(student => new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentViewModel>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return NotFound();

            return new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id != student.Id) return BadRequest();

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

        [HttpPost]
        public async Task<ActionResult<StudentViewModel>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
