
using SWFToHTML5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourceToWebUC.Model.xslt
{
    class FlashConvertSettings : IConvertSettings
    {
        /// <summary>
        /// высота выходного видео 
        /// в пикселях
        /// </summary>
        public int HeightPx { get;  private set; }
        /// <summary>
        /// качество видео
        /// </summary>
        public int Quality {get;  private set;}
        /// <summary>
        /// ширина видео в пикселях
        /// </summary>
        public int WidthPx{get; private set; }
        /// <summary>
        /// удалять ли оригинал
        /// </summary>
        public bool IsOrigDel{ get; private set; }

        /// <summary>
        /// путь к  папке
        /// для временных файлов
        /// </summary>
        public string TmpDirectory {  get; private set;}
        /// <summary>
        /// параметры конвертации
        /// </summary>
        public FlashConvertSettings(int wd, int hg, string tmpDir)
        {
            HeightPx = 576;
            Quality = 100;
            WidthPx = 1024;
            TmpDirectory = tmpDir;
            IsOrigDel = true;
        }

        /// <summary>
        /// обновить параметры
        /// </summary>
        /// <param name="_settings">новые параметры</param>
        public void UpdateSettings(IConvertSettings _settings)
        {
            if (_settings == null)
                throw new ArgumentNullException("_settings");

            this.HeightPx = _settings.HeightPx;
            this.WidthPx = _settings.WidthPx;
            this.Quality = _settings.Quality;
            this.TmpDirectory = _settings.TmpDirectory;
            this.IsOrigDel = _settings.IsOrigDel;
        }


        
    }
}
