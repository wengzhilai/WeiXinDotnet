using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

namespace Helper
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageHelper
    {
        private static List<int> ZoomImage(int sourWidth, int sourHeight, int destHeight, int destWidth)
        {
            int width = 0, height = 0;
            if (sourHeight > destHeight || sourWidth > destWidth)
            {
                if ((sourWidth * destHeight) > (sourHeight * destWidth))
                {
                    width = destWidth;
                    height = (destWidth * sourHeight) / sourWidth;
                }
                else
                {
                    height = destHeight;
                    width = (sourWidth * destHeight) / sourHeight;
                }
            }
            else
            {
                width = sourWidth;
                height = sourHeight;
            }
            return new List<int> { width, height };
        }
        /// <summary>
        /// 重置图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="minW"></param>
        /// <param name="minH"></param>
        public static string Resize(string filePath, int minW, int minH)
        {
            var file = filePath;
            var fileName="";
            if(file.LastIndexOf("\\")>0){
                fileName=filePath.Substring(file.LastIndexOf("\\")+1);
            }
            else{
                fileName=filePath.Substring(file.LastIndexOf("/")+1);
            }
            Console.WriteLine($"Loading {file}");
            using (FileStream pngStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Bitmap resized = ResizeBitmap(pngStream, minW, minH);
                resized.Save($"resized-{fileName}", ImageFormat.Png);
                Console.WriteLine($"Saving resized-{fileName} thumbnail");
            }
            return $"resized-{fileName}";
        }
        /// <summary>
        /// 重置图片
        /// </summary>
        /// <param name="pngStream"></param>
        /// <param name="minW"></param>
        /// <param name="minH"></param>
        /// <returns></returns>
        public static Bitmap ResizeBitmap(FileStream pngStream, int minW, int minH)
        {
            int width = 0;
            int height = 0;
            using (var image = new Bitmap(pngStream))
            {
                var sizeList = ZoomImage(image.Width, image.Height, minW, minH);
                width = sizeList[0];
                height = sizeList[1];
                Bitmap resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                }
                Bitmap resized1 = resized;
                return resized1;
            }
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="pngStream"></param>
        /// <param name="minW"></param>
        /// <param name="minH"></param>
        /// <returns></returns>
        public static byte[] ResizeByte(FileStream pngStream, int minW, int minH)
        {
            Bitmap b = ResizeBitmap(pngStream, minW, minH);
            MemoryStream stream = new MemoryStream();
            b.Save(stream, ImageFormat.Png);
            byte[] byteImage =stream.ToArray();
            return byteImage;
        }

    }
}