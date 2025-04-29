
using StorageApi.Controllers.Models.Validators;

namespace StorageApi.Controllers.Models.Request
{
    public class AddItemRequest
    {
        public string Name { get; set; }

        public IFormFile Picture { get; set; }

        public string PartNumber { get; set; }

        public string Category { get; set; }

        public string Place { get; set; }

        public string Description { get; set; }

        public string Supplier { get; set; }

        public ushort Quantity { get; set; }

        public List<string> Validate()
        {
            var errors = PictureValidator.Validate(this.Picture);

            if (string.IsNullOrWhiteSpace(this.Name))
                errors.Add("Nome é obrigatório");

            return errors;
        }
    }
}
