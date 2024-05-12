using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _4.WPFClientChat
{

    public class UploadResponseDTO
    {
        public string Image { get; set; } = string.Empty;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPhotoSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            var filePath = dlg.FileName;
            var bytes = File.ReadAllBytes(filePath);
            var base64 = Convert.ToBase64String(bytes);
            string json = JsonConvert.SerializeObject(new
            {
                Photo = base64
            });
            bytes = Encoding.UTF8.GetBytes(json);
            string serverUrl = "https://npr211.itstep.click";
            WebRequest request = WebRequest.Create($"{serverUrl}/api/Gallery/upload");
            request.Method = "POST";
            request.ContentType = "application/json";
            using(var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            try
            {
                var response = request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    var resp = JsonConvert.DeserializeObject<UploadResponseDTO>(data);
                    MessageBox.Show(serverUrl+resp.Image);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}