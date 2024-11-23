using Renci.SshNet;
using System.Text.RegularExpressions;

namespace dnstools
{
    public partial class Form1 : Form
    {
        string host = "10.40.50.201";
        string user = "uczen";
        string pass = "uczenpti";

        public Form1()
        {
            InitializeComponent();
        }

        private string exec(string cmd)
        {
            string res = "";
            using (var ssh = new SshClient(host, user, pass))
            {
                ssh.Connect();
                var sshCommand = ssh.CreateCommand(cmd);
                res = sshCommand.Execute();
                ssh.Disconnect();
            }
            return res;
        }

        private string whois(string domainname)
        {
            string res = "";

            TimeSpan zaGodzine = TimeSpan.FromHours(1);

            if (!Directory.Exists("domeny"))
                Directory.CreateDirectory("domeny");

            string filename = Path.Combine("domeny", domainname);
            if (File.Exists(filename))
            {
                var czasZmianyWPliku = File.GetLastWriteTime(filename);

                if (DateTime.Now - czasZmianyWPliku > zaGodzine)
                {
                    res = exec($"whois {domainname}");
                    File.WriteAllText(filename, res);

                } else res = File.ReadAllText(filename);
            } else
            {
                res = exec($"whois {domainname}");
                File.WriteAllText(filename, res);
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox1.Text = whois("wsi.edu.pl").Replace("\n","\r\n");
            var reg = new Regex("(([a-zA-Z0-9_\\.])+([a-zA-Z0-9_\\.]{1,3})\\.)$");
            var  r = reg.Matches(textBox1.Lines[4].Trim());

        }
    }
}
