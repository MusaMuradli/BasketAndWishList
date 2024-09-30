namespace FastKart.Extencions
{
    public static class FileExtencionMethods
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image");//kontentinde image varsa demeli sekildi

        }

        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length <= mb*1024*1024;
        }

        public static async Task<string> GenerateFileAsync(this IFormFile file, string path)
        {
            var imageName = $"{Guid.NewGuid()} - {file.FileName}";
             path = Path.Combine( path,imageName);

            var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();

            return imageName;  
        } 
    }
}
