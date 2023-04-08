using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace com.fabioscagliola.OAuthSwagger.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    readonly WebApiDbContext dbContext;

    public PersonController(WebApiDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Creates a person.
    /// </summary>
    [HttpPost]
    [Route("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Person))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(PersonCreateData personCreateData)
    {
        Person person = new() { FName = personCreateData.FName, LName = personCreateData.LName, };
        PersonValidation personValidation = new();
        ValidationResult validationResult = personValidation.Validate(person);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToString());
        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync();
        return Ok(person);
    }

    /// <summary>
    /// Retrieves a person.
    /// </summary>
    [HttpGet]
    [Route("[action]/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Person))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Read(int id)
    {
        Person? person = await dbContext.People.SingleOrDefaultAsync(x => x.Id == id);
        if (person == null)
            return BadRequest(Properties.Resources.PersonNotFound);
        return Ok(person);
    }

    /// <summary>
    /// Retrieves all the people.
    /// </summary>
    [HttpGet]
    [Route("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Person>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ReadList()
    {
        List<Person> people = await dbContext.People.ToListAsync();
        return Ok(people);
    }

    /// <summary>
    /// Updates a person.
    /// </summary>
    [HttpPost]
    [Route("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Person))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(Person person)
    {
        Person? existing = await dbContext.People.SingleOrDefaultAsync(x => x.Id == person.Id);
        if (existing == null)
            return BadRequest(Properties.Resources.PersonNotFound);
        PersonValidation personValidation = new();
        ValidationResult validationResult = personValidation.Validate(person);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToString());
        existing.FName = person.FName;
        existing.LName = person.LName;
        await dbContext.SaveChangesAsync();
        return Ok(existing);
    }

    /// <summary>
    /// Deletes a person.
    /// </summary>
    [HttpDelete]
    [Route("[action]/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(int id)
    {
        Person? person = await dbContext.People.SingleOrDefaultAsync(x => x.Id == id);
        if (person == null)
            return BadRequest(Properties.Resources.PersonNotFound);
        dbContext.People.Remove(person);
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
