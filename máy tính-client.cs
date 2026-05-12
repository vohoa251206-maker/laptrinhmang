using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TCP_Client_Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            cbPhepToan.Items.Add("+");
            cbPhepToan.Items.Add("-");
            cbPhepToan.Items.Add("*");
            cbPhepToan.Items.Add("/");

            cbPhepToan.SelectedIndex = 0;
        }

        private void btnTinh_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu
                string a = txtA.Text;
                string b = txtB.Text;
                string pheptoan = cbPhepToan.Text;

                // Chuỗi gửi lên server
                string dulieu = a + "|" + pheptoan + "|" + b;

                // Kết nối server
                TcpClient client = new TcpClient("127.0.0.1", 5000);

                // Tạo stream
                NetworkStream stream = client.GetStream();

                // Chuyển chuỗi thành byte
                byte[] sendData =
                    Encoding.UTF8.GetBytes(dulieu);

                // Gửi dữ liệu
                stream.Write(sendData, 0, sendData.Length);

                // Bộ nhớ nhận dữ liệu
                byte[] receiveData = new byte[1024];

                // Nhận dữ liệu từ server
                int byteNhan =
                    stream.Read(receiveData, 0, receiveData.Length);

                // Chuyển byte sang string
                string ketqua =
                    Encoding.UTF8.GetString(
                        receiveData,
                        0,
                        byteNhan
                    );

                // Hiển thị kết quả
                lblKetQua.Text = "Kết quả: " + ketqua;

                // Lưu lịch sử
                string lichsu =
                    a + " " +
                    pheptoan + " " +
                    b + " = " +
                    ketqua;

                lstHistory.Items.Add(lichsu);

                // Ghi file
                File.AppendAllText(
                    "history.txt",
                    lichsu + Environment.NewLine
                );

                // Đóng kết nối
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi kết nối Server!\n" + ex.Message
                );
            }
        }
    }
}