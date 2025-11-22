using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace FactFinderWeb.Utils
{
    public class CommonUtillity
    {
        public CommonUtillity()
        {
            //test
        }

        public static string EncryptData(string data)
        {
            var secretData = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(data)).Select(s => s.ToString("x2")));
            return secretData;
        }

        public static string DecryptData(string data)
        {
            var secretData = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(data)).Select(s => s.ToString("x2")));
            return secretData;
        }
        public static bool CheckExtention(string data)
        {
            string[] arr = { ".png", ".jpg", ".jpeg", ".gif", ".pdf", ".rtf", ".txt", ".mp4", ".doc", ".docx", ".mkv" };
            bool res = arr.AsQueryable().Contains(data);
            return res;
        }
       
        public enum PriorityValue
        {
            Low,
            Medium,
            High
        }

 }

    /*
      var filePath = "";
            if (contentView.FormFile != null && contentView.FormFile.Length > 0)
            {
                //var repositoryPath = @"D:\Uploads";  //F:\Webminds\WebmindsSignage\WebmindsSignage
                var repositoryPath = System.IO.Directory.GetCurrentDirectory();
                //var projectFolderPath = Path.Combine(repositoryPath, "/assets/images/");
                var filename= "images_" + Guid.NewGuid().ToString() + "_" + contentView.FormFile.FileName;
                    filePath =  "/assets/images/" + filename;
                //filePath = Path.Combine(projectFolderPath, uniqueFileName);

                using (var stream = new FileStream(repositoryPath + filePath, FileMode.Create))
                {
                    contentView.FormFile.CopyToAsync(stream);
                }
            }
            else
            {
                return;
            }
    */


}

