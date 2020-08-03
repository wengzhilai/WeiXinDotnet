
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cors;
using Models;
using Models.Entity;
using IRepository;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 授权管理
    /// </summary>
    [Route("file/[controller]/[action]")]
    [EnableCors("AllowSameDomain")]
    [ApiController]
    public class UpFileController : ControllerBase
    {
        IFileRepository file;
        private readonly IWebHostEnvironment _env;
        /// <summary>
        /// 
        /// </summary>
        public UpFileController(
             IWebHostEnvironment hostingEnvironment, IFileRepository file)
        {
            _env = hostingEnvironment;
            this.file = file;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        async public Task<ResultObj<SysFilesEntity>> UploadPhotos()
        {

            ResultObj<SysFilesEntity> reEnt = new ResultObj<SysFilesEntity>();

            var files = Request.Form.Files;
            var fileFolder = string.Format("{0}", DateTime.Now.ToString("yyyyMM"));
            if (!Directory.Exists(Path.Combine(Global.appConfig.FileCfg.path, fileFolder)))
                Directory.CreateDirectory(Path.Combine(Global.appConfig.FileCfg.path, fileFolder));

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = formFile.FileName;
                    var fileName_min = formFile.FileName.Replace(".","_min.");
                    var filePath = Path.Combine(fileFolder, fileName);
                    var filePath_min = Path.Combine(fileFolder, fileName_min);
                    
                    var allPath = Path.Combine(_env.ContentRootPath, Path.Combine(Global.appConfig.FileCfg.path , filePath));
                    using (var stream = new FileStream(allPath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        reEnt.msg = filePath;
                        byte[] bytes=Helper.ImageHelper.ResizeByte(stream,100,100);

                        using (var stream_min = new FileStream(Path.Combine(_env.ContentRootPath, Path.Combine(Global.appConfig.FileCfg.path,filePath_min)), FileMode.Create))
                        {
                            stream_min.Write(bytes);
                            stream_min.Close();
                        }

                        var fileEnt=new SysFilesEntity
                        {
                            id=0,
                            name = fileName,
                            path = allPath,
                            url = filePath,                            
                            length = stream.Length,
                            uploadTime = Helper.DataTimeHelper.getDateLong(DateTime.Now),
                            fileType = Path.GetExtension(formFile.FileName),
                            md5Str=Helper.Fun.Md5Hash(bytes),
                            base64Str=Convert.ToBase64String(bytes),
                            isUse=0
                        };
                        fileEnt.id=await file.Save(new DtoSave<SysFilesEntity>{data=fileEnt});
                        reEnt.success = fileEnt.id>0;
                        // BinaryReader r = new BinaryReader(stream);
                        // r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                        // bytes = r.ReadBytes((int)r.BaseStream.Length);
                        
                        reEnt.data = fileEnt;
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
            var allPath = Path.Combine(_env.ContentRootPath, Global.appConfig.FileCfg.path, dir, fileName + "." + type);
            return File(System.IO.File.ReadAllBytes(allPath), @"image/png");
        }

    }
}