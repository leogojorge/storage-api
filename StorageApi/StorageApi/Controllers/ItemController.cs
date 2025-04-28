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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var item = await this.ItemRepository.GetById(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [Authorize]
    [Route("filters")]
    [HttpGet(Name = "GetFiltered")]
    public async Task<IActionResult> GetFiltered([FromQuery] GetItemByFilterRequest request)
    {
        var items = await this.ItemRepository.GetByFilters(request);

        return Ok(items);
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Post([FromForm] AddItemRequest request)
    {
        var validationErros = request.Validate();

        if (validationErros.Count > 0)
            return BadRequest(validationErros);

        var pictureContent = await GetPictureAsByteArray(request.Picture);

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

    [Authorize]
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put([FromForm] UpdateItemRequest request)
    {
        var validationErros = request.Validate();

        if (validationErros.Count > 0)
            return BadRequest(validationErros);

        var item = await this.ItemRepository.GetById(request.Id);

        var pictureContent = await GetPictureAsByteArray(request.Picture);

        item.Update(request.Name, pictureContent, request.PartNumber, request.Category, request.Place, request.Description, request.Supplier, request.Quantity);

        try
        {
            await this.ItemRepository.Update(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

        return Ok("Item created successfully.");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Ok();

        await this.ItemRepository.Delete(id);

        return Ok("Item deleted successfully.");
    }

    private static async Task<byte[]> GetPictureAsByteArray(IFormFile picture)
    {
        using var ms = new MemoryStream();
        await picture.CopyToAsync(ms);
        return ms.ToArray();
    }
}
