using Microsoft.AspNetCore.Mvc;
using StorageApi.Controllers.Models;
using StorageApi.Domain;
using StorageApi.Infrastructure.Repository;

namespace StorageApi.Controllers;

//[Authorize]
//[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository ItemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        ItemRepository = itemRepository;
    }

    [HttpGet(Name = "GetItem")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await this.ItemRepository.GetById(id);

        if (item is null)
            return NotFound("Item not found.");

        return Ok(item);
    }

    [HttpPost(Name = "AddItem")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] AddItemRequest request)
    {
        var validationErros = request.Validate();

        if (validationErros.Count > 0)
            return BadRequest(validationErros);

        var pictureContent = await request.GetPictureAsByteArray();

        var item = new Item(request.Name, pictureContent, request.PartNumber, request.Category, request.Place, request.Description, request.Supplier, request.Quantity);

        await this.ItemRepository.Save(item);

        return Ok("Item created successfully.");
    }
}
