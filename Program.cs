using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Data;

namespace MayTinhServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lắng nghe tại cổng 5000
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();

            Console.WriteLine("Server dang chay tai cong 5000...");

            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    byte[] data = new byte[1024];
                    int byteCount = stream.Read(data, 0, data.Length);
                    string message = Encoding.UTF8.GetString(data, 0, byteCount);

                    Console.WriteLine("\nNhan tu Client: " + message);

                    // GỌI LOGIC XỬ LÝ TỔNG HỢP Ở ĐÂY
                    string phanHoi = ExecuteServerLogic(message);

                    // Gửi kết quả lại cho Client
                    byte[] gui = Encoding.UTF8.GetBytes(phanHoi);
                    stream.Write(gui, 0, gui.Length);

                    Console.WriteLine("Phan hoi: " + phanHoi);

                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Loi: " + ex.Message);
                }
            }
        }

        // Hàm xử lý chính: Phân loại lệnh CALC hoặc SAVE
        static string ExecuteServerLogic(string request)
        {
            try
            {
                string[] parts = request.Split('|');
                string command = parts[0];

                if (command == "CALC")
                {
                    double a = double.Parse(parts[1]);
                    string pt = parts[2];
                    double b = double.Parse(parts[3]);

                    return PerformMath(a, pt, b);
                }
                /*else if (command == "SAVE")
                {
                    string dataToSave = parts[1];
                    // Lưu vào file server_history.txt cùng thư mục với file .exe
                    File.AppendAllText("server_history.txt", dataToSave + Environment.NewLine);
                    Console.WriteLine("--> Da luu lich su: " + dataToSave);
                    return "SUCCESS";
                }*/
                else if (command == "SAVE")
                {
                    try
                    {
                        string dataToSave = parts[1];

                        // Lấy đường dẫn thư mục đang chạy .exe
                        string basePath = AppDomain.CurrentDomain.BaseDirectory;

                        // Tạo thư mục History
                        string folderPath = Path.Combine(basePath, "History");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // File lưu lịch sử
                        string filePath = Path.Combine(folderPath, "server_history.txt");

                        // Ghi file
                        File.AppendAllText(filePath, dataToSave + Environment.NewLine);

                        Console.WriteLine("--> Da luu lich su: " + dataToSave);
                        Console.WriteLine("--> Da luu tai: " + filePath);

                        return "SUCCESS";
                    }
                    catch (Exception ex)
                    {
                        return "Loi SAVE: " + ex.Message;
                    }
                }

                // Trường hợp Client gửi theo kiểu cũ (không có CALC|)
                return "Lenh khong hop le!";
            }
            catch
            {
                return "Loi dinh dang du lieu!";
            }
        }

        // Hàm thực hiện phép tính
        static string PerformMath(double a, string pheptoan, double b)
        {
            switch (pheptoan)
            {
                case "+": return (a + b).ToString();
                case "-": return (a - b).ToString();
                case "*": return (a * b).ToString();
                case "/":
                    return (b == 0) ? "Khong the chia cho 0" : (a / b).ToString();
                default: return "Phep toan sai";
            }
        }
    }
}