using Amazon.DynamoDBv2.DataModel;
using Dynamo_Manager.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dynamo_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //injeção de dependencia
        private readonly IDynamoDBContext _context;
        public StudentsController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _context.LoadAsync<Students>(studentId);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var student = await _context.ScanAsync<Students>(default).GetRemainingAsync();
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Students students) 
        {
            var student = await _context.LoadAsync<Students>(students.Id);
            if (student != null) return BadRequest($"Student with Id {student.Id} Already Exists");
            await _context.SaveAsync(students);
            return Ok(student);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _context.LoadAsync<Students>(studentId);
            if (student == null) return NotFound();
            await _context.DeleteAsync(student);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Students students)
        {
            var student = await _context.LoadAsync<Students>(students.Id);
            if (student == null) return NotFound();
            await _context.SaveAsync(students);
            return Ok(students);
        }
        
    }
}
