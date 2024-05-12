using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace WebServerFiles.Controllers
{

    public class UploadImage
    {
        public string Photo { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromBody] UploadImage model)
        {
            try
            {
                string fileName = Guid.NewGuid().ToString()+".jpg";
                if(model.Photo.Contains(",")) {
                    model.Photo = model.Photo.Split(',')[1];
                }
                var bytes = Convert.FromBase64String(model.Photo);
                using(var image = Image.Load(bytes))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(600, 600),
                        Mode = ResizeMode.Max
                    }));
                    var dirSave = Path.Combine(Directory.GetCurrentDirectory(),"uploads", fileName);
                    await image.SaveAsync(dirSave, new JpegEncoder());
                }
                return Ok(new { image = $"/images/{fileName}" });


            }
            catch 
            {
                return BadRequest(new { error = "Помилка збереження фото" });
            }
        }
    }
}
