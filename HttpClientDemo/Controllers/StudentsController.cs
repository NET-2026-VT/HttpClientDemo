using AutoMapper;
using AutoMapper.QueryableExtensions;
using HttpClientDemo.Data;
using HttpClientDemo.Models.Dtos;
using HttpClientDemo.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClientDemo.Controllers
{
    [Route("api/students")]
    [ApiController]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityContext _context;
        private readonly IMapper mapper;

        public StudentsController(UniversityContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets all students
        /// </summary>
        /// <returns>List of StudentDtos</returns>
        // GET: api/Students
        [HttpGet]
        [SwaggerOperation(Summary = "Get all students", Description = "Gets all students with their address city.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudentDto>))]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudent()
        {
            var test = await _context.Students
                .Where(s => EF.Property<DateTime>(s, "Edited") > DateTime.UtcNow.AddDays(-1)).ToListAsync();

            var dto = await mapper.ProjectTo<StudentDto>(_context.Students).ToListAsync();

            return Ok(dto);
        }

        /// <summary>
        /// Gets a specific student by ID with details.
        /// </summary>
        /// <param name="id">The student ID.</param>
        /// <returns>Student details DTO.</returns>
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get student by ID", Description = "Returns full details of a student.")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDetailsDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [SwaggerResponse(StatusCodes.Status200OK, "Find Student", typeof(StudentDetailsDto))]
        public async Task<ActionResult<StudentDetailsDto>> GetStudent([FromRoute, Range(0 , int.MaxValue)]int id)
        {
            //var student = await _context.Students
            //    .Where(s => s.Id == id)
            //    .ProjectTo<StudentDetailsDto>(mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync();

            var student = await mapper.ProjectTo<StudentDetailsDto>(_context.Students.Where(s => s.Id == id))
                                      .FirstOrDefaultAsync();

            if (student == null)
                return NotFound();

            return Ok(student);
        }

       
        /// <summary>
        /// Updates an existing student by ID.
        /// </summary>
        /// <param name="id">The student ID.</param>
        /// <param name="dto">The updated student data.</param>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update student", Description = "Updates an existing student by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> PutStudent([FromRoute]int id, [FromBody] UpdateStudentDto dto)
        {
            var student = await _context.Students
                                       .Include(s => s.Address)
                                       .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null) return NotFound();

            mapper.Map(dto, student);

            //student.FirstName = dto.FirstName;
            //student.LastName = dto.LastName;
            //student.Avatar = dto.Avatar;
            //student.Address.Street = dto.Street;
            //student.Address.City = dto.City;
            //student.Address.ZipCode = dto.ZipCode;

           // _context.Entry(student).State = EntityState.Modified;

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

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="dto">The student creation DTO.</param>
        /// <returns>The created student DTO.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Create student", Description = "Creates a new student.", Tags = ["Student"])]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDto>> PostStudent(CreateStudentDto dto)
        {
            var student = mapper.Map<Student>(dto);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentDto = mapper.Map<StudentDto>(student);

            return CreatedAtAction(nameof(GetStudent), new { id = studentDto.Id }, studentDto);
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="id">The student ID.</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete student", Description = "Deletes a student by ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
