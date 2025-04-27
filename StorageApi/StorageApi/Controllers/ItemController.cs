using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Controllers.Models.Request;
using StorageApi.Domain;
using StorageApi.Infrastructure.Repository;

namespace StorageApi.Controllers;

[Authorize]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository ItemRepository;

    public ItemController(IItemRepository itemRepository)
    {
        ItemRepository = itemRepository;
    }

    //[Authorize]
    //[Route("/detail/{id}")]
    //[HttpGet(Name = "GetItem")]
    //public async Task<IActionResult> GetDetailed(string id)
    //{
    //    var item = await this.ItemRepository.GetById(id);

    //    if (item is null)
    //        return NotFound("Item not found.");

    //    return Ok(item);
    //}

    [Authorize]
    [HttpGet(Name = "GetAllItems")]
    public async Task<IActionResult> GetFiltered([FromQuery] GetItemByFilterRequest request)
    {
        var items = await this.ItemRepository.GetByFilters(request);

        return Ok(items);
    }

    [Authorize]
    [HttpPost(Name = "AddItem")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] AddItemRequest request)
    {
        var validationErros = request.Validate();

        if (validationErros.Count > 0)
            return BadRequest(validationErros);

        var pictureContent = await request.GetPictureAsByteArray();

        var item = new Item(request.Name, pictureContent, request.PartNumber, request.Category, request.Place, request.Description, request.Supplier, request.Quantity);

        try
        {
            await this.ItemRepository.Save(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

        return Ok("Item created successfully.");
    }
}
