using System;
using Microsoft.AspNetCore.Http;

namespace Projet2.Models
{
    public class Upload
    {


        public string Name { get; set; }

        public IFormFile File { get; set; }


    }
}
