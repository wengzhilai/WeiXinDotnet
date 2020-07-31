
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Models;
using Models.Entity;

namespace WebApi.Controllers
{
    /// <summary>
    /// 授权管理
    /// </summary>
    [Route("file/[controller]/[action]")]
    [EnableCors("AllowSameDomain")]
    [ApiController]
    public class UpFileController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        public UpFileController(
             IWebHostEnvironment hostingEnvironment)
        {
            _env = hostingEnvironment;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        async public Task<ResultObj<FilesEntity>> UploadPhotos()
        {
            ResultObj<FilesEntity> reEnt = new ResultObj<FilesEntity>();

            var files = Request.Form.Files;
            var fileFolder = string.Format("{0}", DateTime.Now.ToString("yyyyMM"));
            fileFolder = "PromotePic";
            if (!Directory.Exists(Path.Combine("wwwroot", fileFolder)))
                Directory.CreateDirectory(Path.Combine("wwwroot", fileFolder));

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;
                    var filePath = Path.Combine(fileFolder, fileName);
                    
                    var allPath = Path.Combine(_env.ContentRootPath, "wwwroot/" + filePath);
                    using (var stream = new FileStream(allPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        reEnt.success = true;
                        reEnt.msg = filePath;
                        byte[] bytes=new byte[stream.Length];
                        stream.Read(bytes,0,(int)stream.Length);
                        reEnt.data = new FilesEntity
                        {
                            id=0,
                            name = fileName,
                            path = allPath,
                            url = fileName,                            
                            length = stream.Length,
                            uploadTime = Helper.DataTimeHelper.getDateLong(DateTime.Now),
                            fileType = Path.GetExtension(formFile.FileName),
                            md5Str=Helper.Fun.Md5Hash(bytes),
                            base64Str=Convert.ToBase64String(bytes)
                        };
                        stream.Flush();
                    }
                }
            }
            return reEnt;
        }


        /// <summary>
        /// 根据路径查看图片
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{dir}/{fileName}.{type}")]
        public IActionResult LookfileByPath(string dir, string fileName, string type)
        {
            Response.Body.Dispose();
            var allPath = Path.Combine(_env.ContentRootPath, "UpFiles", dir, fileName + "." + type);
            return File(System.IO.File.ReadAllBytes(allPath), @"image/png");
        }

    }
}