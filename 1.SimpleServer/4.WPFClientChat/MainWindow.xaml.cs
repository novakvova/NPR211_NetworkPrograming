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
                    //MessageBox.Show(serverUrl+resp.Image);
                    ViewMessage("Козак", resp.Image);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ViewMessage(string text, string imageUrl)
        {
            var grid = new Grid();
            for (int i = 0; i < 2; i++)
            {
                var colDef = new ColumnDefinition();
                colDef.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(colDef);
            }
            BitmapImage bmp = new BitmapImage(new Uri($"https://npr211.itstep.click{imageUrl}"));
            var image = new Image();
            image.Source = bmp;
            image.Width = 50;
            image.Height = 50;

            var textBlock = new TextBlock();
            Grid.SetColumn(textBlock, 1);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(5, 0, 0, 0);
            textBlock.Text = text;
            grid.Children.Add(image);
            grid.Children.Add(textBlock);

            lbInfo.Items.Add(grid);
            lbInfo.Items.MoveCurrentToLast();
            lbInfo.ScrollIntoView(lbInfo.Items.CurrentItem);
        }
    }
}