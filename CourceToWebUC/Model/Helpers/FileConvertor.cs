using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourceToWebUC.Model.Helpers
{
    /// <summary>
    /// конвертор файлов
    /// </summary>
    class FileConvertor
    {
        /// <summary>
        /// конвертировать swf в Jpg
        /// </summary>
        /// <param name="_inputPath">путь к исходному файлу</param>
        /// <param name="_otputPath">путь к выходному файлу</param>
        public static void ConvertSWFToPng(string _inputPath,string _otputPath,int width=400,int height=400)
        {
            if(string.IsNullOrEmpty(_inputPath)|| string.IsNullOrEmpty(_otputPath))
            {
                throw new ArgumentNullException("ConvertSWFToJpg(string _inputPath,string _otputPath)");
            }
            if (!System.IO.File.Exists(_inputPath))
                throw new Exception("Файл для конвертации в формат jpg не найден: "+_inputPath);

            SWFToImage.SWFToImageObject obj = new SWFToImage.SWFToImageObject();
            obj.InitLibrary("demo", "demo");
            obj.ImageOutputType = SWFToImage.TImageOutputType.iotPNG;
            obj.JPEGQuality = 100;
            obj.ImageWidth = width;
            obj.ImageHeight = height;

            obj.InputSWFFileName = _inputPath;          

            try
            {
                obj.Execute_Begin();

                obj.FrameIndex = 0;
                obj.Execute_GetImage();
                obj.SaveToFile(_otputPath);
                obj.Execute_End();
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка конвертации из формата swf в формат png: "+ex.Message);
            }
        }
    }
}
