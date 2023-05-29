using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace app_system
{
    class Program
    {
        public static string localhost(string one,string two)//识别本地文件
        {
            string str = System.Environment.CurrentDirectory;
            if (one == "language")
            {
                str = str + "\\language";
                StreamReader reader = new StreamReader(str, System.Text.Encoding.Default);
                string line = reader.ReadLine();
                string rtn = "";
                if (line == "CN" || line == "cn")
                {
                    rtn = "cn";
                }
                else if (line == "EN" || line == "en")
                {
                    rtn = "en";
                }
                reader.Close();
                return rtn;
            }
            else if (one == "local")
            {
                str = str + "\\modal";
                try
                {
                    DirectoryInfo di = new DirectoryInfo(str);
                    Directory.CreateDirectory(str);
                    string[] folders = Directory.GetDirectories(str);//获取base文件夹下所有的文件夹路径
                    for (int i = 0; i < folders.Length; i++)
                    {
                        folders[i] = Path.GetFileName(folders[i]);//在路径中提取文件夹名
                    }
                    string a = "";
                    foreach (string shuchu in folders)
                    {
                        a += "\n" + shuchu;
                    }
                    return a;
                }
                catch (Exception)
                {
                    Console.WriteLine("Warring!!!");
                }
                return "Show命令出现未知错误";
            }
            else
            {
                return "";
            }
        }
        static void help(string str)
        {
            string[] sp = str.Split(' ');
            string path = System.Environment.CurrentDirectory;
            StreamReader reader = new StreamReader(path + "\\language", System.Text.Encoding.UTF7);
            string line = reader.ReadLine();
            reader.Close();
            path = path + "\\base\\" + line + "\\help";
            if (sp[0] == "")
            {
                StreamReader reader1 = new StreamReader(path + "\\any", System.Text.Encoding.UTF8);
                string line1 = "";
                while (line1 != null)
                {
                    line1 = reader1.ReadLine();
                    Console.Write("{0}\r\n",line1);
                }
            }
            else
            {
                try
                {
                    StreamReader reader1 = new StreamReader(path + "\\" + sp[0], System.Text.Encoding.UTF8);

                    string line1 = "";
                    while (line1 != null)
                    {
                        line1 = reader1.ReadLine();
                        Console.Write("{0}\r\n", line1);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("未找到关于{0}的帮助", sp[0]);
                }
            }
        }
        static void show(string a)
        {
            if (a == "modal")
            {
                Console.WriteLine(localhost("local", ""));
            }
        }
        static void set(string str)
        {
            string[] splitted = str.Split(' ');
            if (splitted[0] == "language")
            {
                string path = System.Environment.CurrentDirectory;
                path = path + "\\language";
                Console.WriteLine("请稍后...\nPlease wait...");
                Thread.Sleep(1000);
                File.WriteAllText(path, splitted[1]);
                Thread.Sleep(2000);
                Console.Write("立即重启程序生效（Y/N）\nImmediate restart of the program takes effect (Y/N)\nEnter:");
                string a = Console.ReadLine();
                if (a == "y" || a == "Y")
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);//重启
                }
            }
        }
        static void print(string str)
        {
            if (str == "ip")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine("IP Address = " + ip.ToString());
                    }
                }
            }
            else if(str == "sys_language")
            {
                Console.WriteLine(localhost("language",""));
            }
            else
            {
                Console.WriteLine("\n" + str);
            }
        }
        static void panduan(string bianliang)
        {
            string [] splitted = bianliang.Split(' ');
            int i = 0;
            if (splitted[i] == "help")
            {
                if (splitted.Length == 1)
                {
                    help("");
                }
                else
                {
                    string shuchu = "";
                    for (int e = 1; e < splitted.Length; e++)
                    {
                        shuchu += splitted[e];
                    }
                    help(shuchu);
                }
            }
            else if (splitted[i] == "show")
            {
                if (splitted.Length == 1 || splitted[1] == "")
                {
                    Console.WriteLine("\n你的语法存在错误：“show”的后面是否需要一个参数？\nThere is an error in your grammar: do you need a parameter after 'show'?");
                }
                else
                {
                    string shuchu = "";
                    for (int e = 1; e < splitted.Length; e++)
                    {
                        shuchu += splitted[e];
                    }
                    show(shuchu);
                }
            }
            else if (splitted[i] == "clear")
            {
                Console.Clear();
            }
            else if (splitted[i] == "print")
            {
                //i++;
                if (splitted.Length == 1 || splitted[1] == "")
                {
                    Console.WriteLine("\n你的语法存在错误：“print”的后面是否需要一个参数？\nThere is an error in your grammar: do you need a parameter after 'print'?");
                }
                else
                {
                    string shuchu = "";
                    for (int e = 1; e < splitted.Length; e++)
                    {
                        shuchu += splitted[e];
                    }
                    print(shuchu);
                }
            }
            else if (splitted[i] == "set")
            {
                if (splitted.Length == 1 || splitted[1] == "")
                {
                    Console.WriteLine("\n你的语法存在错误：“set”的后面是否需要一个参数？\nThere is an error in your grammar: do you need a parameter after 'set'?");
                }
                else
                {
                    string shuchu = "";
                    for (int e = 1; e < splitted.Length; e++)
                    {
                        if (e == 1)
                        {
                        shuchu = splitted[1];
                        }
                        else if (e > 1)
                        {
                            shuchu = shuchu + " " + splitted[e];
                        }
                    }
                    Console.WriteLine(shuchu);
                    set(shuchu);
                }
            }
            else if (splitted[i] == "tcp-chat")
            {
                string str = System.Environment.CurrentDirectory + "\\modal\\tcp-chat\\tcp-chat.exe";
                try
                {
                    System.Diagnostics.Process.Start(str);
                }
                catch (Exception)
                {
                    Console.WriteLine("Dox无法找到tcp-chat模块，请联系开发者获取相应模块。微信：sxcxy_");
                }
            }
            
        }
       

        static void Main(string[] args)
        {
            ///log_system可获得本地的base文件夹修改权限
            //string str = System.Environment.CurrentDirectory;
            //string a = localhost(1);
            //Console.WriteLine(a);
            string language = localhost("language", "");
            Console.WriteLine(language);
            string quanju = "\nDox:\\\\\\>";//定义输出的路径
            string bianliang = "";//用于时刻获取用户的输入

            Console.WriteLine("Yunqingli Dox [版本 1.0.0000.00.0]\n(c) Yunqingli Corporation。保留所有权利。\n");

            do
            {
                bianliang = "";//用于时刻获取用户的输入
                
                Console.Write(quanju);
                bianliang = Console.ReadLine();
                panduan(bianliang);
            }
            while (bianliang != "exit");
        }
    }
}
