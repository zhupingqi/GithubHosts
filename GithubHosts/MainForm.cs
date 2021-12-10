using RestSharp;
using System.Diagnostics;

namespace GithubHosts
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public string LoadHostFromGitee()
        {
            var client = new RestClient();
            var request = new RestRequest("https://gitee.com/ineo6/hosts/raw/master/hosts");

            var response = client.Get(request);
            var hosts = response.Content.Split("\n",StringSplitOptions.RemoveEmptyEntries);

            var n = "";
            foreach (var host in hosts)
            {
                n = host.Trim();
                if (n.ToLower().EndsWith(" github.com"))
                    break;
            }

            var ip = n.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];

            return ip;
        }

        public string LoadHosts()
        {
            var path = @"C:\WINDOWS\system32\drivers\etc";
            var file = path + @"\hosts";

            var lines = File.ReadAllLines(file);
            var n = "";

            foreach (var line in lines)
            {
                if (line.Trim().ToLower().EndsWith("github.com"))
                {
                    n = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0];
                    break;
                }
            }

            return n;
        }

        public void WriteNewHost(string ip)
        {
            var path = @"C:\WINDOWS\system32\drivers\etc";
            var file = path + @"\hosts";

            var lines = File.ReadAllLines(file);
            var n = "";
            var find = false;

            for (int i = 0; i < lines.Length; i++)
            {
                n = lines[i].Trim();
                if (n.ToLower().EndsWith("github.com"))
                {
                    lines[i] = ip + " github.com";
                    find = true;
                    break;
                }
            }

            if(!find)
            {
                lines.Append(ip + " github.com");
            }

            File.WriteAllLines(file, lines);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var old = LoadHosts();
            var ip = LoadHostFromGitee();
            
            if (ip != old)
            {
                WriteNewHost(ip);
                label2.Text = "github.com 更新地址为 " + ip;
                RunCmd("ipconfig /flushdns");
            }
        }

        private void RunCmd(string command)
        {
            //例Process
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";         //确定程序名
            p.StartInfo.Arguments = "/c " + command;   //确定程式命令行
            p.StartInfo.UseShellExecute = false;      //Shell的使用
            p.StartInfo.RedirectStandardInput = true;  //重定向输入
            p.StartInfo.RedirectStandardOutput = true; //重定向输出
            p.StartInfo.RedirectStandardError = true;  //重定向输出错误
            p.StartInfo.CreateNoWindow = true;        //设置置不显示示窗口
            p.Start();
            p.Close();
        }

    }
}