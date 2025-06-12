using System.Security.Claims;
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

    protected string CurrentUserId => User.FindFirstValue("UserId")!;

    public ItemController(IItemRepository itemRepository)
    {
        ItemRepository = itemRepository;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var item = await this.ItemRepository.GetById(id, this.CurrentUserId);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [Authorize]
    [Route("filters")]
    [HttpGet(Name = "GetFiltered")]
    public async Task<IActionResult> GetFiltered([FromQuery] GetItemByFilterRequest request)
    {
        var items = await this.ItemRepository.GetByFilters(request, this.CurrentUserId);

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

        var item = new Item(request.Name, this.CurrentUserId, pictureContent, request.PartNumber, request.Category, request.Place, request.Description, request.Supplier, request.Quantity);

        try
        {
            await this.ItemRepository.Save(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

        return Ok(item);
    }

    [Authorize]
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put([FromRoute] string id, [FromForm] UpdateItemRequest request)
    {
        var validationErros = request.Validate();

        if (validationErros.Count > 0)
            return BadRequest(validationErros);

        var item = await this.ItemRepository.GetById(id, this.CurrentUserId);

        if (item is null)
            return NotFound();

        var pictureContent = await GetPictureAsByteArray(request.Picture);

        item.Update(request.Name, pictureContent, request.PartNumber, request.Category, request.Place, request.Description, request.Supplier, request.Quantity);

        try
        {
            await this.ItemRepository.Update(item, this.CurrentUserId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

        return Ok("Item updated successfully.");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Ok();

        await this.ItemRepository.Delete(id, this.CurrentUserId);

        return Ok("Item deleted successfully.");
    }

    private async Task<byte[]> GetPictureAsByteArray(IFormFile picture)
    {
        using var ms = new MemoryStream();
        await picture.CopyToAsync(ms);
        return ms.ToArray();
    }
}
