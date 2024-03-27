using ASP_API.Filters;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;

        #region CREATE
        [HttpPost]
        [UseApiKey]

        public async Task<IActionResult> Create(ContactFormDto dto)
        {
            if (ModelState.IsValid)
            {
                var contactForm = new ContactFormEntity
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Service = dto.Service,
                    Message = dto.Message
                };

                await _context.ContactForms.AddAsync(contactForm);
                await _context.SaveChangesAsync();

                return Created("Contact form was created and sent", contactForm);
            }
            return BadRequest("Invalid fields in the form, please try again.");
        }

        #endregion

        #region GET

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOne(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("Contact form not found");
            }

            return Ok(contactForm);
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var contactForms = await _context.ContactForms.ToListAsync();

            if (contactForms.Count == 0)
            {
                return NotFound("No contact forms found");
            }

            return Ok(contactForms);
        }
        #endregion

        #region UPDATE
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, ContactFormDto dto)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("Contact form not found");
            }

            contactForm.FullName = dto.FullName;
            contactForm.Email = dto.Email;
            contactForm.Service = dto.Service;
            contactForm.Message = dto.Message;

            await _context.SaveChangesAsync();

            return Ok("Contact form was updated");
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound("Contact form not found");
            }

            _context.ContactForms.Remove(contactForm);
            await _context.SaveChangesAsync();

            return Ok("Contact form was deleted");
        }

        #endregion
    }
}
