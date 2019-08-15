using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;

namespace ManejoDeArchivos
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        AlmacenamientoInternoFiles aiFiles;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            EditText etContentFile = FindViewById<EditText>(Resource.Id.etContentFile);
            Button btnAbrirArchivo = FindViewById<Button>(Resource.Id.btnAbrirArchivo);

            //AlmacenamientoInternoFiles aiFiles = new AlmacenamientoInternoFiles(FilesDir.AbsolutePath, CacheDir.AbsolutePath);
            aiFiles = new AlmacenamientoInternoFiles(FilesDir.AbsolutePath, CacheDir.AbsolutePath);
            aiFiles.escribirArchivo("archivo1.txt", "Este es el contenido del archivo.\nEsta es la línea 2.\n Y esta es la línea 3.");

            btnAbrirArchivo.Click += async (sender, e) => {
                etContentFile.Text = "";
                etContentFile.Text = await aiFiles.leerArchivo("archivo1.txt");
            };

            Button btnAbrirArchivoTmp = FindViewById<Button>(Resource.Id.btnAbrirArchivoTmp);
            aiFiles.escribirArchivoTemporal("archivo1.txt", "Este es el contenido del archivo temporal.\nEsta es la línea 2.\n Y esta es la línea 3.");
            btnAbrirArchivoTmp.Click += delegate {
                etContentFile.Text = "";
                etContentFile.Text = "Archivo archivo1_tmp.txt almacenado en: " + System.IO.Path.Combine(CacheDir.AbsolutePath, "archivo1_tmp.txt") + "\n";
                etContentFile.Text += aiFiles.leerArchivoTemporal("archivo1.txt");
            };

            Button btnVEAE = FindViewById<Button>(Resource.Id.btnVEAE);
            btnVEAE.Click += delegate {
                bool escribible = Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMounted;
                bool leible = escribible || Android.OS.Environment.ExternalStorageState == Android.OS.Environment.MediaMountedReadOnly;
                etContentFile.Text = string.Format("El almacenamiento externo {0} esta disponible, {1} es escribible y {2} es leíble", escribible ? "si" : "no", escribible ? "si" : "no", leible ? "si" : "no");
                etContentFile.Text += string.Format("\nLa ruta del almacenamiento externo es: {0}", GetExternalFilesDir(null).AbsolutePath);
                etContentFile.Text += string.Format("\nLa ruta del almacenamiento externo temporal es: {0}", ExternalCacheDir.AbsolutePath);
            };
            var localFolder = Android.App.Application.Context.GetExternalFilesDir(null).ToString();
            string[] files = Directory.GetFiles(FilesDir.AbsolutePath, "*.txt", SearchOption.AllDirectories);
            files = Directory.GetFiles("/storage/emulated/0/", "*.jpeg", SearchOption.AllDirectories);
            string archivos = "";
            foreach(string archivo in files)
            {
                archivos += (archivos.Length > 0 ? "\n" : "") + archivo;
            }
            EditText txtFiles = FindViewById<EditText>(Resource.Id.etFiles);
            txtFiles.Text = archivos;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            aiFiles.eliminarArchivoTemporal("archivo1.txt");
        }        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}