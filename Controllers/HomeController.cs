using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestTaskWeb.Models;
using Microsoft.AspNetCore.Hosting;
using TestTaskWeb.ModelViews;
using System.IO;
using TestTaskWeb.Service;
using System.Text;
using Newtonsoft.Json;

namespace TestTaskWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly Reader reader;

        public HomeController(ILogger<HomeController> logger, Reader reader, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            this.reader = reader;
            this.hostingEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
          
            return View();
        }

        public async Task<IActionResult> Upload(UploadModelView uploadModelView)
        {
            if (ModelState.IsValid)
            {
                if (uploadModelView.FormFile != null)
                {
                   
                        var uniqueFileName = GetUniqueFileName(uploadModelView.FormFile.FileName);

                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploads))
                        {
                            Directory.CreateDirectory(uploads);
                        }
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        uploadModelView.FormFile.CopyTo(fileStream);
                        fileStream.Close();


                        var _json = await reader.GetJSON(filePath);
                        if (Encoding.ASCII.GetByteCount(_json) <= 4096)
                        {
                            TempData["Result"] = _json;
                        }


                        return RedirectToAction("ShowResult");
                    
                 

                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            }

        public IActionResult ShowResult()
        {
            if(TempData["Result"].ToString()!=null)
            {
                ResultView resultView = new ResultView();
               

                resultView.ResultJson = TempData["Result"].ToString();
                return View(resultView);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 10)
                      + Path.GetExtension(fileName);
        }
    }
}
