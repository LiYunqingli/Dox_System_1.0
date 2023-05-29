using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("请选择模式：1-服务端；2-客户端");
        int mode = int.Parse(Console.ReadLine());
        if (mode == 1)
        {
            // 启动服务端
            Server.Start();
        }
        else if (mode == 2)
        {
            // 启动客户端
            Client.Start();
        }
        else
        {
            Console.WriteLine("输入错误");
        }
    }
}

class Server
{
    private static TcpListener listener;
    private static Thread thread;

    public static void Start()
    {
        int duankou = 1234;
        while (true)
        {
            while (true)
            {
                try
                {
                    Console.Write("请设置服务器端口(四位数字):");
                    duankou = int.Parse(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("请输入正确的端口!!");
                }
            }
            try
            {
                listener = new TcpListener(IPAddress.Any, duankou);
                listener.Start();
                break;
            }
            catch (Exception)
            {
                Console.WriteLine("通常每个套接字地址(协议/网络地址/端口)只允许使用一次。请更换端口重新尝试");
            }
        }
        Console.WriteLine("服务端已启动，等待客户端连接...");

        thread = new Thread(new ThreadStart(Listen));
        thread.Start();
    }

    private static void Listen()
    {
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("客户端已连接：" + client.Client.RemoteEndPoint.ToString());

            Thread receiveThread = new Thread(new ParameterizedThreadStart(Receive));
            receiveThread.Start(client);

            Thread sendThread = new Thread(new ParameterizedThreadStart(Send));
            sendThread.Start(client);
        }
    }

    private static void Receive(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                int count = stream.Read(buffer, 0, buffer.Length);
                if (count == 0)
                {
                    //Console.WriteLine("客户端已断开：" + client.Client.RemoteEndPoint.ToString());
                    client.Close();
                    break;
                }
                string message = System.Text.Encoding.Default.GetString(buffer, 0, count);
                Console.WriteLine("收到消息：" + message);
            }
            catch (Exception)
            {
                Console.WriteLine("客户端已断开：" + client.Client.RemoteEndPoint.ToString());
                break;
            }
        }
    }

    private static void Send(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        while (true)
        {
            try
            {
                string message = Console.ReadLine();
                byte[] buffer = System.Text.Encoding.Default.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                break;
            }
        }
    }
}

class Client
{
    private static TcpClient client;
    private static Thread receiveThread;
    private static Thread sendThread;

    public static void Start()
    {
        Console.WriteLine("请输入服务端IP地址：");
        string ip = Console.ReadLine();
        int duankou = 1234;
        while (true)
        {
            try
            {
                Console.Write("请输入服务器端口:");
                duankou = int.Parse(Console.ReadLine());
                break;
            }
            catch (Exception)
            {
                Console.WriteLine("请输入正确的端口!!");
            }
        }
        client = new TcpClient();
        client.Connect(IPAddress.Parse(ip), duankou);
        Console.WriteLine("已连接到服务端：" + client.Client.RemoteEndPoint.ToString());

        receiveThread = new Thread(new ThreadStart(Receive));
        receiveThread.Start();

        sendThread = new Thread(new ThreadStart(Send));
        sendThread.Start();
    }

    private static void Receive()
    {
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                int count = stream.Read(buffer, 0, buffer.Length);
                if (count == 0)
                {
                    Console.WriteLine("与服务端的连接已断开");
                    client.Close();
                    break;
                }
                string message = System.Text.Encoding.Default.GetString(buffer, 0, count);
                Console.WriteLine("收到消息：" + message);
            }
            catch (Exception)
            {
                Console.WriteLine("与服务端的连接已断开：" + client.Client.RemoteEndPoint.ToString());
                break;
            }
        }
    }

    private static void Send()
    {
        NetworkStream stream = client.GetStream();

        while (true)
        {
            try
            {
                string message = Console.ReadLine();
                byte[] buffer = System.Text.Encoding.Default.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            { }
        }
    }
}
