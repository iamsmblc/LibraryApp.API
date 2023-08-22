using LibraryApp.API.Data;
using LibraryApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using LibraryApp.API.Services;
using static System.Net.WebRequestMethods;

namespace LibraryApp.API.Controllers
{
    [Route("api")]
        public class LibraryController : Controller
        {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private DataContext _dataContext;
        private readonly LibraryService _libraryService;
        public LibraryController(DataContext dataContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, LibraryService libraryService)
        {
            _dataContext = dataContext;
            _environment = environment;
            _libraryService = libraryService;

        }
        [HttpGet("GetOrderedResponseData")]
        public List<LibraryResponseTable> GetOrderedResponseData()
        {
           var result =_libraryService.GetOrderedResponseData();
           return result;   
        }

        [HttpGet("GetOrderedResponseDataById/{OrderedId}")]
        public List<LibraryResponseTable> GetOrderedResponseDataById(int OrderedId)
        {
            var result = _libraryService.GetOrderedResponseDataById(OrderedId);
            return result;
        }

      
        
        [HttpPost("UpdateLibraryData")]
        public IActionResult UpdateLibraryData([FromBody]LibraryTable newData)
        {
            try
            {
                _libraryService.UpdateLibraryData(newData);
                if (_libraryService.UpdateLibraryData(newData) == true)
                {
                    return Ok();
                }
                return BadRequest("Error:Null Value");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }



        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromForm]IFormFile files_, [FromForm] string bookName_, [FromForm] string authorsName_)
        {
            try
            {
               
                if (files_ != null && bookName_ != null && authorsName_ != null)
                {
                    return Ok(_libraryService.UploadImage(files_, bookName_, authorsName_));
                }
                return BadRequest("Error:Null Value");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpGet("getLength")]
        public IActionResult getLength()
        {
            try
            {
                _libraryService.getLength();
                if (_libraryService.getLength()!=0)
                {
                    
                    return Ok(_libraryService.getLength());
                }
                return BadRequest("Error:Null Value");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
        [HttpGet("getImage/{OrderedId}")]
        public IActionResult GetImage(int OrderedId)

        {
            try
            {
                var orderedData = GetOrderedResponseData().FirstOrDefault(i => i.OrderedId == OrderedId);
                if (orderedData == null)
                {
                    return NotFound();

                }
                var image = _dataContext.ImageTable.FirstOrDefault(i => i.LibraryTableId == orderedData.Id);

                if (image == null)
                {
                    return NotFound();

                }
                var imagePath = Path.Combine(_environment.ContentRootPath, "ImageFile", image.FileName.Replace("ð", "g").Replace("ý", "i").Replace("þ", "s").Replace("Ð", "G").Replace("Ý", "I").Replace("Þ", "S"));
                //accesses image in ImageFile
                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound();
                }

                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, image.ContentType);//return file 
            }
            catch (Exception ex)
            {
                var logRec = new LibraryLog
                {
                    LogMessage = ex.ToString(),
                    LogDate = DateTime.Now
                };
                _dataContext.LibraryLog.Add(logRec);
                _dataContext.SaveChanges();
                throw;
            }
        }
    }
    }

