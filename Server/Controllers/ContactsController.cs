namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController(ISender sender) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("data")]
    public async Task<IActionResult> GetData([FromBody] GetDataRequest request, CancellationToken token)
    {
        var result = await sender.Send(new GetDataQuery(request), token);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Contact contact, CancellationToken token, [FromServices] ContactValidator contactValidator)
    {
        if (contactValidator.Validate(contact) is { IsValid: false } validationResult)
        {
            return BadRequest(validationResult.Errors);
        }
        var result = await sender.Send(new UpdateContactCommand(id, contact), token);
        return result.Success ? Ok() : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Post(IFormFile csv, CancellationToken token, [FromServices] ContactValidator contactValidator)
    {
        if (csv.ContentType != "text/csv")
        {
            return BadRequest("Invalid file type");
        }

        var result = await sender.Send(new CreateContactsCommand(csv), token);
        return result.Success ? RedirectToAction(nameof(Index)) : BadRequest(result.Errors);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var result = await sender.Send(new DeleteContactCommand(id), token);
        return result.Success ? Ok() : NotFound(result.Error);
    }
}