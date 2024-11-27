using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace netCoding
{
    public partial class Form1 : Form
    {
        public TcpListener listener;
        IPEndPoint endPoint;
        public Form1()
        {
            endPoint = new IPEndPoint(IPAddress.Any, 7777);
            listener = new TcpListener(endPoint);
            InitializeComponent();
        }
        public async Task ListenAsync()
        {
            listener.Start();
            try
            {
                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => ClientHandler(client));
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listener?.Stop();
            }
        }
        public async void ClientHandler(TcpClient client)
        {
            try
            {
                MessageBox.Show("Есть подключение");
                var stream = client.GetStream();
                
                List<byte> buffer = new List<byte>();   
                while (true)
                {
                    int readedByte = stream.ReadByte();
                    if (readedByte == -1) continue;
                    if(readedByte == '\n')
                    {
                        var message = Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count);
                        textBox1.Text += message + " ";
                        buffer.Clear();
                    }
                    buffer.Add((byte)readedByte);
                }
            }
            catch(Exception)
            { 
                MessageBox.Show($"Пользователь отключился");
            }
            finally
            {
                client.Close();
                MessageBox.Show("Пользователь отключился");
            }
        }
        public async void OpenConnection(object sender, EventArgs e)
        {
            await ListenAsync();
        }
        public async void CloseConnection(object sender, EventArgs e)
        {
            listener.Stop();
            MessageBox.Show("Все соединения разорваны");
        }
    }
}
