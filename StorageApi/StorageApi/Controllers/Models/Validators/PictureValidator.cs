namespace StorageApi.Controllers.Models.Validators
{
    public static class PictureValidator
    {
        const int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

        readonly static string[] AllowedFileExtensions = { ".jpg", ".gif", ".png" };

        public static List<string> Validate(IFormFile picture)
        {
            var erros = new List<string>();

            if (picture == null || picture.Length == 0)
            {
                erros.Add("Foto é obrigatória.");
                return erros;
            }

            var ext = picture.FileName.Substring(picture.FileName.LastIndexOf('.'));
            var extension = ext.ToLower();

            if (!AllowedFileExtensions.Contains(extension))
            {
                erros.Add("Foto deve ser do tipo .jpg ou .png.");
            }
            else if (picture.Length > MaxContentLength)
            {
                erros.Add("Foto deve ter até 1 MB.");
            }

            return erros;
        }
    }
}
