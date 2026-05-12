using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MayTinhServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server =
                new TcpListener(IPAddress.Any, 5000);

            server.Start();

            Console.WriteLine(
                "Server đang chay..."
            );

            while (true)
            {
                try
                {
                    // Chờ client kết nối
                    TcpClient client =
                        server.AcceptTcpClient();

                    Console.WriteLine(
                        "Client đã kết nối!"
                    );

                    NetworkStream stream =
                        client.GetStream();

                    byte[] data =
                        new byte[1024];

                    int byteCount =
                        stream.Read(
                            data,
                            0,
                            data.Length
                        );

                    string message =
                        Encoding.UTF8.GetString(
                            data,
                            0,
                            byteCount
                        );

                    Console.WriteLine(
                        "Nhận: " + message
                    );

                    string[] parts =
                        message.Split('|');

                    double a =
                        double.Parse(parts[0]);

                    string pheptoan =
                        parts[1];

                    double b =
                        double.Parse(parts[2]);

                    string ketqua = "";

                    switch (pheptoan)
                    {
                        case "+":
                            ketqua =
                                (a + b).ToString();
                            break;

                        case "-":
                            ketqua =
                                (a - b).ToString();
                            break;

                        case "*":
                            ketqua =
                                (a * b).ToString();
                            break;

                        case "/":
                            if (b == 0)
                                ketqua =
                                    "Không chia cho 0";
                            else
                                ketqua =
                                    (a / b).ToString();
                            break;

                        default:
                            ketqua =
                                "Phép toán không hợp lệ";
                            break;
                    }

                    byte[] gui =
                        Encoding.UTF8.GetBytes(
                            ketqua
                        );

                    stream.Write(
                        gui,
                        0,
                        gui.Length
                    );

                    Console.WriteLine(
                        "Kết quả: " +
                        ketqua
                    );

                    stream.Close();
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "Lỗi: " +
                        ex.Message
                    );
                }
            }
        }
    }
}
