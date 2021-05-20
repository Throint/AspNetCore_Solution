using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestTaskWeb.Models;

namespace TestTaskWeb.ModelViews
{
    public class UploadModelView
    {
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]

        [AllowedExtensions(new string[] { ".xml" })]

        public IFormFile FormFile { get; set; }
    }
}
