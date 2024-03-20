using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriberController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region CREATE

    [HttpPost]
    public async Task<IActionResult> Create(SubscriberDto dto)
    {
        if (ModelState.IsValid)
        {
            if (! await _context.Subscribers.AnyAsync(x => x.Email == dto.Email))
            {
                var subscriber = new SubscriberEntity
                {
                    Email = dto.Email,
                    DailyNewsletter = dto.DailyNewsletter,
                    AdvertisingUpdates = dto.AdvertisingUpdates,
                    WeekInReview = dto.WeekInReview,
                    EventUpdates = dto.EventUpdates,
                    StartupsWeekly = dto.StartupsWeekly,
                    Podcast = dto.Podcast
                };

                await _context.Subscribers.AddAsync(subscriber);
                await _context.SaveChangesAsync();

                return Created("Subscriber was created" , subscriber);
            }
            else
            {
                return Conflict("Subscriber already exists");
            }
        }
        return BadRequest("Invalid email");
    }
    #endregion

    #region DELETE

    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id)
    {
        var subscriber = await _context.Subscribers.FindAsync(id);

        if (subscriber == null)
        {
            return NotFound("Subscriber not found");
        }

        _context.Subscribers.Remove(subscriber);
        await _context.SaveChangesAsync();

        return Ok("Subscriber was deleted");
    }

    #endregion

    #region UPDATE

    [HttpPut("{id}")]

    public async Task<IActionResult> Update(int id, SubscriberDto dto)
    {
        var subscriber = await _context.Subscribers.FindAsync(id);

        if (subscriber == null)
        {
            return NotFound("Subscriber not found");
        }

        subscriber.Email = dto.Email;
        subscriber.DailyNewsletter = dto.DailyNewsletter;
        subscriber.AdvertisingUpdates = dto.AdvertisingUpdates;
        subscriber.WeekInReview = dto.WeekInReview;
        subscriber.EventUpdates = dto.EventUpdates;
        subscriber.StartupsWeekly = dto.StartupsWeekly;
        subscriber.Podcast = dto.Podcast;

        await _context.SaveChangesAsync();

        return Ok("Subscriber was updated");
    }
    #endregion

    #region GET

    [HttpGet]
	public async Task<IActionResult> GetAll()
    {
		var subscribers = await _context.Subscribers.ToListAsync();

		return Ok(subscribers);
	}

    [HttpGet("{id}")]
	public async Task<IActionResult> GetOne(int id)
    {
		var subscriber = await _context.Subscribers.FindAsync(id);

		if (subscriber == null)
        {
			return NotFound("Subscriber not found");
		}

		return Ok(subscriber);
	}

	#endregion
}
