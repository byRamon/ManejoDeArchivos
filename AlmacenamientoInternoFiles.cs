using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Threading.Tasks;

namespace ManejoDeArchivos
{
    class AlmacenamientoInternoFiles
    {
        string pathAlmacenamientoInterno;
        string pathAlmacenamientoInternoTemp;
        public AlmacenamientoInternoFiles(string filesDir, string cacheDir)
        {
            pathAlmacenamientoInterno = filesDir;
            pathAlmacenamientoInternoTemp = cacheDir;
        }

        public async Task<string> leerArchivo(string fileName)
        {
            string pathFile = Path.Combine(pathAlmacenamientoInterno, fileName);
            string texto = "";
            if (File.Exists(pathFile))
            {
                var reader = new StreamReader(pathFile);

                string linea = "";
                while ((linea = await reader.ReadLineAsync()) != null)
                    texto += linea;
            }
            return texto;
        }

        public void escribirArchivo(string fileName, string contenido)
        {
            string pathFile = Path.Combine(pathAlmacenamientoInterno, fileName);
            File.WriteAllText(pathFile, contenido);
        }

        public string leerArchivoTemporal(string fileNameTmp)
        {
            fileNameTmp = fileNameTmp.Insert(fileNameTmp.IndexOf('.'), "_tmp");
            string pathFileTmp = Path.Combine(pathAlmacenamientoInternoTemp, fileNameTmp);
            if (File.Exists(pathFileTmp))
                return File.ReadAllText(pathFileTmp);
            return "";
        }

        public void escribirArchivoTemporal(string fileNameTmp, string contenido)
        {
            fileNameTmp = fileNameTmp.Insert(fileNameTmp.IndexOf('.'), "_tmp");
            string pathFileTmp = Path.Combine(pathAlmacenamientoInternoTemp, fileNameTmp);
            File.WriteAllText(pathFileTmp, contenido);
        }
        public void eliminarArchivoTemporal(string fileNameTmp)
        {
            fileNameTmp = fileNameTmp.Insert(fileNameTmp.IndexOf('.'), "_tmp");
            string pathFileTmp = Path.Combine(pathAlmacenamientoInternoTemp, fileNameTmp);
            File.Delete(fileNameTmp);
        }
    }
}