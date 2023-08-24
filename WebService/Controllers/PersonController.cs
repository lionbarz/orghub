using System.Collections;
using InterfaceAdapters;
using InterfaceAdapters.Models;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;

namespace WebService.Controllers;

[ApiController]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonController(PersonService personService)
    {
        _personService = personService;
    }
    
    [HttpPost]
    [Route("api/person/addPerson")]
    public async Task<UxPerson> AddPerson(AddPersonRequest request)
    {
        return await _personService.AddPerson(request.UserName, request.Email);
    }
    
    [HttpGet]
    [Route("api/person")]
    public async Task<IEnumerable<UxPerson>> GetPersons()
    {
        return await _personService.GetPersons();
    }
    
    [HttpGet]
    [Route("api/person/{id}")]
    public async Task<UxPerson> GetPersons(string id)
    {
        return await _personService.GetPersonAsync(Guid.Parse(id));
    }
}