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

        const int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
        
        string[] AllowedFileExtensions = { ".jpg", ".gif", ".png" };

        public List<string> Validate()
        {
            var erros = new List<string>();

            if (Picture == null || Picture.Length == 0)
            {
                erros.Add("Picture is empty");

                return erros;
            }

            var ext = Picture.FileName.Substring(Picture.FileName.LastIndexOf('.'));
            var extension = ext.ToLower();

            if (!AllowedFileExtensions.Contains(extension))
            {              
                erros.Add("Please Upload image of type .jpg, .gif or .png.");
            }
            else if (Picture.Length > MaxContentLength)
            {
                erros.Add("Please Upload a file up to 1 mb.");
            }

            return erros;
        }

        public async Task<byte[]> GetPictureAsByteArray()
        {
            using var ms = new MemoryStream();
            await Picture.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
