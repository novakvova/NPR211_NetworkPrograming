using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        private string _userImage;
        private ChatMessage _message = new ChatMessage();
        //клієнт для взаємодії із сервером
        private TcpClient _tcpClient = new TcpClient();
        private NetworkStream _ns; //почилання на потік взаємодії
        private Thread _thread;

        private string _serverUrl = "https://npr211.itstep.click";

        public MainWindow()
        {
            InitializeComponent();
            _userImage = String.Empty;
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
            
            WebRequest request = WebRequest.Create($"{_serverUrl}/api/Gallery/upload");
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
                    //ViewMessage("Козак", resp.Image);
                    if(resp != null)
                        _userImage = resp.Image;
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
            BitmapImage bmp = new BitmapImage(new Uri($"{_serverUrl}{imageUrl}"));
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

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(_userImage))
            {
                MessageBox.Show("Оберіть фото для корристувача");
                return;
            }
            if(string.IsNullOrEmpty(txtUserName.Text)) 
            {
                MessageBox.Show("Вкажіть назву користувача");
                return;
            }
            try
            {
                //IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPAddress ip = IPAddress.Parse("4.232.129.33");
                int port = 1243;
                _message.UserId = Guid.NewGuid().ToString();
                _message.Name = txtUserName.Text;
                _message.Photo = _userImage;
                _tcpClient.Connect(ip, port);
                _ns=_tcpClient.GetStream();
                _thread = new Thread(obj => ResponseData((TcpClient)obj));
                _thread.Start(_tcpClient);
                btnSend.IsEnabled = true;
                btnConnect.IsEnabled = false;
                txtUserName.IsEnabled = false;
                _message.Text = "Приєднався до чату";
                var buffer = _message.Serialize();
                _ns.Write(buffer); //Віпадправляю на сервер повідомлення про підключення

            }
            catch (Exception ex)
            {
                MessageBox.Show("Проблема підключення до серверу " + ex.Message);
            }
        }

        private void ResponseData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] readBytes = new byte[16054400];
            int byte_count;
            //читаємо відповідь від сервера, поки не буде 0
            while((byte_count = ns.Read(readBytes))>0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ChatMessage msg = ChatMessage.Desserialize(readBytes);
                    string text = $"{msg.Name} -> {msg.Text}";
                    ViewMessage(text, msg.Photo);

                }));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Повідомлення надсилаю усім про завершення роботи в чаті
            _message.Text = "Покинув чат";
            var buffer = _message.Serialize();
            _ns.Write(buffer);

            _tcpClient.Client.Shutdown(SocketShutdown.Both);
            _tcpClient.Close();

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            _message.Text = txtText.Text;
            var buffer = _message.Serialize();
            _ns.Write(buffer);

            txtText.Text = "";
        }
    }
}