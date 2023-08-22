
using LibraryApp.API.Data;
using LibraryApp.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace LibraryApp.API.Services
{
    public class LibraryService
    {

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private DataContext _dataContext;
       

        public LibraryService(DataContext dataContext, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _dataContext = dataContext;
            _environment = environment;

        }
        public List<LibraryResponseTable> GetOrderedResponseData()
        {
            try
            {//response ordered data
                var orderedResponseData = new List<LibraryResponseTable>();
                var idNum = 1;
                var orderedData = (from s in _dataContext.LibraryTable
                                   select new LibraryTable
                                   {
                                       Id = s.Id,
                                       BookName = s.BookName,
                                       AuthorsName = s.AuthorsName,
                                       IsBorrowed = s.IsBorrowed,
                                       DeliveryDate = s.DeliveryDate,
                                       BorrowerUser = s.BorrowerUser
                                   }).OrderBy(x => x.BookName).ToList();



                foreach (var i in orderedData)
                {
                    var resTab = new LibraryResponseTable
                    {
                        Id = i.Id,
                        BookName = i.BookName,
                        AuthorsName = i.AuthorsName,
                        BorrowedMessage = (i.IsBorrowed == false) ? "Kütüphanede" : "Ödünç Alýndý",
                        IsBorrowed = i.IsBorrowed,
                        DeliveryDate = i.DeliveryDate,
                        BorrowerUser = i.BorrowerUser,
                        OrderedId = idNum
                    };

                    orderedResponseData.Add(resTab);
                    idNum++;
                }

                return orderedResponseData;
            }

            catch (Exception ex)
            {
                var logRec = new LibraryLog
                {
                    LogMessage = ex.ToString(),//add data to LibraryLog for exception errors
                    LogDate = DateTime.Now

                };
                _dataContext.LibraryLog.Add(logRec);
                _dataContext.SaveChanges();
                throw;
            }
        }
        public int getLength()

        {
            try
            {//length of data used in frontend
                var length = 0;
                var data = (from s in _dataContext.LibraryTable
                            select new LibraryTable
                            {
                                Id = s.Id,
                                BookName = s.BookName,
                                AuthorsName = s.AuthorsName,
                                IsBorrowed = s.IsBorrowed,
                                DeliveryDate = s.DeliveryDate,
                                BorrowerUser = s.BorrowerUser
                            }).ToList();
              
                
                    foreach (var i in data)
                    {
                        length++;
                    }

                    return length;
            
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
        public List<LibraryResponseTable> GetOrderedResponseDataById(int OrderedId)
        {
            try
            {
               
                var data = (from s in GetOrderedResponseData()
                            where s.OrderedId == OrderedId
                            select new LibraryResponseTable
                            {
                                Id = s.Id,
                                BookName = s.BookName,
                                AuthorsName = s.AuthorsName,
                                BorrowedMessage = s.BorrowedMessage,
                                IsBorrowed = s.IsBorrowed,
                                DeliveryDate = s.DeliveryDate,
                                BorrowerUser = s.BorrowerUser,
                                OrderedId = s.OrderedId
                            }).ToList();

                return data;
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

  
      
        public bool UpdateLibraryData([FromBody] LibraryTable newData)//Update borrowed status
        {
            try
            {
                var item = _dataContext.LibraryTable.FirstOrDefault(o => o.Id == newData.Id);
                if (newData != null && item !=null)
                {
                    if (newData.BorrowerUser != null && newData.DeliveryDate != null)
                    {
                        item.Id = item.Id;
                        item.BookName = item.BookName;
                        item.AuthorsName = item.AuthorsName;
                        item.IsBorrowed = true;
                        item.BorrowerUser = newData.BorrowerUser;
                        item.DeliveryDate = newData.DeliveryDate;
                        _dataContext.LibraryTable.Update(item);
                        _dataContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                return false;
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
                return false;
            }

        }
        public bool UploadImage([FromForm] IFormFile files_, [FromForm] string bookName_, [FromForm] string authorsName_)//add book
        {
            try
            {
                if (files_ != null && bookName_ != null && authorsName_ != null)
                {
                    var libTab = new LibraryTable();
                    libTab.BookName = bookName_;
                    libTab.AuthorsName = authorsName_;
                    libTab.IsBorrowed = false;
                    libTab.DeliveryDate = null;
                    libTab.BorrowerUser = null;
                    _dataContext.LibraryTable.Add(libTab);
                    _dataContext.SaveChanges();
                    var dmm = new ImageTable();

                    var dataDoc = (from s in _dataContext.ImageTable

                                   select new ImageTable
                                   {
                                       ImageId = s.ImageId,
                                       LibraryTableId = s.LibraryTableId,
                                       FileName = s.FileName,
                                       ContentType = s.ContentType,


                                   }).ToList();


                    var roothPath = Path.Combine(_environment.ContentRootPath, "ImageFile");
                    if (!Directory.Exists(roothPath))
                        Directory.CreateDirectory(roothPath);


                    var filePath = Path.Combine(roothPath, files_.FileName.Replace("ð", "g").Replace("ý", "i").Replace("þ", "s").Replace("Ð", "G").Replace("Ý", "I").Replace("Þ", "S"));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        var item = _dataContext.ImageTable.FirstOrDefault(o => o.LibraryTableId == libTab.Id);
                        if (item == null)
                        { //Add to ImageTable
                            var document = new ImageTable
                            {

                                FileName = files_.FileName,
                                ContentType = files_.ContentType,

                                LibraryTableId = libTab.Id,
                            };
                             files_.CopyTo(stream);
                            _dataContext.ImageTable.Add(document);
                            _dataContext.SaveChanges();
                        }
                        else
                        {
                            item.ImageId = item.ImageId;
                            item.FileName = files_.FileName;
                            item.ContentType = files_.ContentType;

                            item.LibraryTableId = libTab.Id;
                            files_.CopyTo(stream);
                            _dataContext.ImageTable.Update(item);
                            _dataContext.SaveChanges();
                        }



                    }
                    return true;
                }
                else { return false; }

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
