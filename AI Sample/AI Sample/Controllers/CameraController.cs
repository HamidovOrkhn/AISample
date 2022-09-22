using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using AI_Sample.AIClassification.BLL;
using AI_Sample.AIClassification.BLL.Types;
using AI_Sample.Data;
using AI_Sample.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
////using System.Drawing.;
//using System.Drawing;
//using AI_Sample.AIClassification.Common;
//using AI_Sample.AIClassification.BLL.Types;
//using Accord.Vision.Detection;

namespace AI_Sample.Controllers
{
    public class CameraController : Controller
    {

        private readonly DatabaseContext _context;
        private readonly IHostingEnvironment _environment;
        public float ScaleFactor { get; set; } = 1.1f;
        public int MinSize { get; set; } = 5;
        public ObjectDetectorScalingMode ScaleMode { get; set; } = ObjectDetectorScalingMode.GreaterToSmaller;
        public ObjectDetectorSearchMode SearchMode { get; set; } = ObjectDetectorSearchMode.Average;
        public bool Parallel { get; set; } = true;
        public CameraController(IHostingEnvironment hostingEnvironment, DatabaseContext context)
        {
            _environment = hostingEnvironment;
            _context = context;
        }

        [HttpGet]
        public IActionResult Capture()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Capture(string name)
        {
            var file = HttpContext.Request.Form.Files[0];
            if (file != null)
            {
                if (file.Length > 0)
                {
                    // Getting Filename  
                    var fileName = file.FileName;
                    // Unique filename "Guid"  
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    // Getting Extension  
                    var fileExtension = Path.GetExtension(fileName);
                    // Concating filename + fileExtension (unique filename)  
                    var newFileName = string.Concat(myUniqueFileName, fileExtension);
                    //  Generating Path to store photo   
                    var filepath = Path.Combine(_environment.WebRootPath, "CameraPhotos") + $@"\{newFileName}";



                    if (!string.IsNullOrEmpty(filepath))
                    {
                        // Storing Image in Folder  
                        StoreInFolder(file, filepath);
                    }

                    var imageBytes = System.IO.File.ReadAllBytes(filepath);

                    string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);

                    Stream fileStream = file.OpenReadStream();

                    Bitmap preProcessedImage = new Bitmap(fileStream);

                    var faceDetector = new FaceDetector();

                    //var datads = new ImageProcessor(preProcessedImage).Grayscale().EqualizeHistogram().Result,
                    var data = faceDetector.ExtractFaces(
                    new ImageProcessor(preProcessedImage).Grayscale().EqualizeHistogram().Result,
                    FaceDetectorParameters.Create(ScaleFactor, MinSize, ScaleMode, SearchMode, Parallel));
                    var returnedImage = preProcessedImage;
                    byte[] byt = ImageToByte(returnedImage);
                    foreach (var face in data)
                    {
                        DrawLineInt(returnedImage, face.FaceRectangle);
                    }
                    returnedImage.Save(Path.Combine(_environment.WebRootPath, "CameraPhotos") + $@"\AiImageExampleş.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    string tst = returnedImage.ToString();
                    if (imageBytes != null)
                    {
                        // Storing Image in Folder  
                        StoreInDatabase(byt);
                    }

                }
                _context.SaveChanges();
                return Ok(_context.ImageStore.OrderByDescending(a=>a.ImageId).FirstOrDefault());
                //return Json(true);
            }
            else
            {
                return Json(false);
            }

        }
        
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        /// <summary>  
        /// Saving captured image into Folder.  
        /// </summary>  
        /// <param name="file"></param>  
        /// <param name="fileName"></param>  
        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
        public void DrawLineInt(Bitmap bmp, Rectangle rectangle)
        {
            Pen blackPen = new Pen(Color.Black, 3);
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawRectangle(blackPen, rectangle);
            }

        }

        /// <summary>  
        /// Saving captured image into database.  
        /// </summary>  
        /// <param name="imageBytes"></param>  
        private void StoreInDatabase(byte[] imageBytes)
        {
            try
            {
                if (imageBytes != null)
                {
                    string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
                    string imageUrl = string.Concat("data:image/jpg;base64,", base64String);

                    ImageStore imageStore = new ImageStore()
                    {
                        CreateDate = DateTime.Now,
                        ImageBase64String = imageUrl,
                        ImageId = 0
                    };

                    _context.ImageStore.Add(imageStore);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }





        internal class ImageProcessor
        {
            private Bitmap _bitmap;
            public Bitmap Result { get => _bitmap; }
            public ImageProcessor(Bitmap bitmap)
            {
                _bitmap = bitmap;
            }
            internal ImageProcessor Grayscale()
            {
                var grayscale = new Grayscale(0.2125, 0.7154, 0.0721);
                _bitmap = grayscale.Apply(_bitmap);
                return this;
            }

            internal ImageProcessor EqualizeHistogram()
            {
                HistogramEqualization filter = new HistogramEqualization();
                filter.ApplyInPlace(_bitmap);
                return this;
            }

            internal ImageProcessor Resize(Size size)
            {
                _bitmap = new Bitmap(_bitmap, size);
                return this;
            }
        }



    }
}
