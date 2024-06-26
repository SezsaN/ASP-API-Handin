﻿using ASP_API.Filters;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
   

 
    
    public class CourseController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;


        #region Create

        [HttpPost]

        public async Task<IActionResult> Create(CourseDto course)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.Courses.AnyAsync(x => x.Title == course.Title))
                {
                    var newCourse = new CourseEntity
                    {
                        Title = course.Title,
                        Price = course.Price,
                        DiscountPrice = course.DiscountPrice,
                        Hours = course.Hours,
                        IsBestSeller = course.IsBestSeller,
                        LikesInNumbers = course.LikesInNumbers,
                        LikesInPercent = course.LikesInPercent,
                        Author = course.Author,
                        CoursePictureUrl = course.CoursePictureUrl
                    };
                    await _context.Courses.AddAsync(newCourse);
                    await _context.SaveChangesAsync();
                    return Created("Course has been created", newCourse);

                }

                return Conflict("Course with the same title already exist");
            }
            return BadRequest();


        }


        #endregion

        #region GET

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var totalCount = await _context.Courses.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var courses = await _context.Courses
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            var paginationMetadata = new
            {
                totalCount,
                totalPages,
                currentPage = pageNumber,
                pageSize
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            return Ok(course);
        }

        #endregion

        #region UPDATE

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, CourseDto dto)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            course.Title = dto.Title;
            course.Price = dto.Price;
            course.DiscountPrice = dto.DiscountPrice;
            course.Hours = dto.Hours;
            course.Author = dto.Author;
            course.IsBestSeller = dto.IsBestSeller;
            course.LikesInNumbers = dto.LikesInNumbers;
            course.LikesInPercent = dto.LikesInPercent;
            course.CoursePictureUrl = dto.CoursePictureUrl;

            await _context.SaveChangesAsync();

            return Ok("Course was updated");
        }
        #endregion

        #region DELETE

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound("Course not found");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok("Course was deleted");
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteAll()
        {
            var courses = await _context.Courses.ToListAsync();

            if (courses.Count == 0)
            {
                return NotFound("No courses found");
            }

            _context.Courses.RemoveRange(courses);
            await _context.SaveChangesAsync();

            return Ok("All courses were deleted");
        }
        #endregion
    }
}

