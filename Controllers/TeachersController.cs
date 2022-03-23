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
    public class TeachersController : ControllerBase
    {
        private readonly MyContext _context;

        public TeachersController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherViewModel>>> GetTeachers()
        {
            return await _context
                .Teachers
                .Select(teacher => new TeacherViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherViewModel>> GetTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            return new TeacherViewModel
            {
                Id = teacher.Id,
                Name = teacher.Name
            };
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(Guid id, TeacherViewModel teacherViewModel)
        {
            var teacher = _context.Teachers.Find(id);

            _context.Entry(teacher).State = EntityState.Modified;

            teacher.AddOrUpdateName(teacherViewModel.Name);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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
        public async Task<ActionResult<TeacherViewModel>> PostTeacher(TeacherViewModel teacherViewModel)
        {
            var teacher = new Teacher(teacherViewModel.Name);

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents(Guid id)
        {
            var teacher = await _context.Teachers.Include(teacher => teacher.Students).SingleOrDefaultAsync(teacher => teacher.Id == id);

            if (teacher == null) return NotFound();

            return teacher.Students.Select(student => new StudentViewModel 
            { 
                Id = student.Id, 
                Name = student.Name 
            }).ToList();
        }

        [HttpGet("{id}/students/{studentId}")]
        public async Task<ActionResult<StudentViewModel>> GetStudent(Guid id, Guid studentId)
        {
            var teacher = await _context.Teachers.Include(teacher => teacher.Students).SingleOrDefaultAsync(teacher => teacher.Id == id);

            if (teacher == null) return NotFound();

            var student = teacher.Students.SingleOrDefault(student => student.Id == studentId);

            if (student == null) return NotFound();

            return new StudentViewModel
            {
                Id = student.Id,
                Name = student.Name
            };
        }

        [HttpPost("{id}/students/{studentId}")]
        public async Task<ActionResult<StudentViewModel>> AddStudents(Guid id, Guid studentId)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            var student = await _context.Students.FindAsync(studentId);

            if (teacher == null || student == null) return NotFound();

            teacher.AddStudent(student);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = teacher.Id, studentId = student.Id }, student);
        }

        [HttpDelete("{id}/student/{studentId}")]
        public async Task<IActionResult> RemoveStudent(Guid id, Guid studentId)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            var student = teacher.Students.SingleOrDefault(student => student.Id == studentId);

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(Guid id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
