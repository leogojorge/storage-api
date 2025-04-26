namespace StorageApi.Controllers.Models
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

        const int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
        
        string[] AllowedFileExtensions = { ".jpg", ".gif", ".png" };

        public List<string> Validate()
        {
            var erros = new List<string>();

            if (this.Picture == null || this.Picture.Length == 0)
            {
                erros.Add("Picture is empty");

                return erros;
            }

            var ext = this.Picture.FileName.Substring(this.Picture.FileName.LastIndexOf('.'));
            var extension = ext.ToLower();

            if (!AllowedFileExtensions.Contains(extension))
            {              
                erros.Add("Please Upload image of type .jpg, .gif or .png.");
            }
            else if (this.Picture.Length > MaxContentLength)
            {
                erros.Add("Please Upload a file up to 1 mb.");
            }

            return erros;
        }

        public async Task<byte[]> GetPictureAsByteArray()
        {
            using var ms = new MemoryStream();
            await this.Picture.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
