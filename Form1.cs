using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public const int EM_SETCUEBANNER = 0x1501;

        string cache_url = "";
        string samp_url = "";
        string gta_url = "";

        bool samp_instalado = false;
        bool cache_encontrado = false;
        bool download_ativo = false;

        string server_password = "";
        string server_ip = "147.135.117.196";
        string server_port = "7777";

        private void Form1_Load(object sender, EventArgs e)
        {
            SendMessage(this.textBox1.Handle, EM_SETCUEBANNER, 0, "Usuário");

            progressBar1.Hide();
            progressBar2.Hide();
            progressBar3.Hide();

            string myRegistryKey = Registry.CurrentUser.OpenSubKey(@"Software\\SAMP").GetValue("gta_sa_exe").ToString();
            myRegistryKey = myRegistryKey.Substring(0, myRegistryKey.LastIndexOf(@"\") + 1);
            textBox1.Text = Registry.CurrentUser.OpenSubKey(@"Software\SAMP", true).GetValue("PlayerName").ToString();


            if (!File.Exists(myRegistryKey + "gta_sa.exe"))
            {
                label8.Text = "Não";
                label8.ForeColor = Color.Red;
            }
            else if (!File.Exists(myRegistryKey + "samp.exe"))
            {
                samp_instalado = false;
                label4.Text = "Não";
                label4.ForeColor = Color.Red;

                // GTA
                label8.Text = "Sim";
                label8.ForeColor = Color.Green;
            }
            else
            {
                samp_instalado = true;
                label4.Text = "Sim";
                label4.ForeColor = Color.Green;

                // GTA
                label8.Text = "Sim";
                label8.ForeColor = Color.Green;
            }

            string cache_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GTA San Andreas User Files\\SAMP\\cache";
            string lsrp_cache_path = cache_path + "\\" + server_ip + "." + server_port;
            if (Directory.Exists(cache_path))
            {
                if(!Directory.Exists(lsrp_cache_path))
                {
                    Directory.CreateDirectory(lsrp_cache_path);
                }
                else
                {
                    if(!IsDirectoryEmpty(lsrp_cache_path))
                    {
                        cache_encontrado = true;
                        label3.Text = "Sim";
                        label3.ForeColor = Color.Green;
                    }
                    else
                    {
                        cache_encontrado = false;
                        label3.Text = "Não";
                        label3.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                cache_encontrado = false;
                label3.Text = "Não";
                label3.ForeColor = Color.Red;
            }
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(base.Handle, 0xa1, 2, "0");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!samp_instalado)
            {
                MessageBox.Show("O cliente SA-MP não está instalado para jogar no servidor, faça o download primeiro.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (!cache_encontrado)
            {
                MessageBox.Show("O cache do servidor não foi encontrado, faça o download primeiro.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (textBox1.Text.Length > 24 || textBox1.Text.Length < 4)
            {
                MessageBox.Show("O seu nome deve ter entre 4 e 24 caracteres.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                string sa_path = Registry.CurrentUser.OpenSubKey(@"Software\\SAMP").GetValue("gta_sa_exe").ToString();

                MessageBox.Show(sa_path);

                Registry.CurrentUser.OpenSubKey(@"Software\SAMP", true).SetValue("PlayerName", textBox1.Text);


                if (Environment.Is64BitOperatingSystem == true) 
                {
                    ProcessStartInfo start_info = new ProcessStartInfo();
                    //start_info.FileName = "samp.exe";
                    start_info.FileName = Path.GetPathRoot(Environment.SystemDirectory) + "\\Program Files (x86)\\Rockstar Games\\GTA San Andreas\\samp.exe";
                    start_info.Arguments = server_ip + ":" + server_port + " " + server_password;
                    Process.Start(start_info);
                }
                else
                {
                    ProcessStartInfo start_info = new ProcessStartInfo();
                    //start_info.FileName = "samp.exe";
                    start_info.FileName = Path.GetPathRoot(Environment.SystemDirectory) + "\\Program Files\\Rockstar Games\\GTA San Andreas";
                    start_info.Arguments = server_ip + ":" + server_port + " " + server_password;
                    Process.Start(start_info);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            progressBar1.Show();

            if(download_ativo == true)
            {
                MessageBox.Show("Você já está baixando algo.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            
            else
            {
                try
                {
                    using (WebClient web_client = new WebClient())
                    {


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Não foi possível baixar o cache.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete("samp_client.exe");
            progressBar2.Show();

            if (download_ativo == true)
            {
                MessageBox.Show("Você já está baixando algo.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                try
                {
                    using (WebClient web_client = new WebClient())
                    {
                        web_client.DownloadProgressChanged += SAMP_DownloadProgressChanged;
                        web_client.DownloadFileCompleted += new AsyncCompletedEventHandler(SAMP_DownloadFileCompleted);
                        web_client.DownloadFileAsync(new System.Uri(samp_url), "samp_client.exe");
                        download_ativo = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Não foi possível baixar o cliente SA-MP.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void SAMP_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytes_baixados = double.Parse(e.BytesReceived.ToString());
                double bytes_totais = double.Parse(e.TotalBytesToReceive.ToString());

                double porcentagem = bytes_baixados / bytes_totais * 100;
                progressBar2.Value = int.Parse(Math.Truncate(porcentagem).ToString());
            });
        }

        void SAMP_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker) delegate
            {
                progressBar2.Hide();
                MessageBox.Show("Download do cliente concluído com sucesso. Após instalação, abra novamente o launcher.", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ProcessStartInfo start_info = new ProcessStartInfo();
                start_info.FileName = "samp_client.exe";
                Process.Start(start_info);
                download_ativo = false;
            });
        }

        void Cache_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker) delegate 
            {
                double bytes_baixados = double.Parse(e.BytesReceived.ToString());
                double bytes_totais = double.Parse(e.TotalBytesToReceive.ToString());

                double porcentagem = bytes_baixados / bytes_totais * 100;
                progressBar1.Value = int.Parse(Math.Truncate(porcentagem).ToString());
            });
        }

        void Cache_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker) delegate 
            {
                string cache_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GTA San Andreas User Files\\SAMP\\cache";
                System.IO.DirectoryInfo di = new DirectoryInfo(cache_path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                ZipFile.ExtractToDirectory("cache.zip", cache_path);

                progressBar1.Hide();
                cache_encontrado = true;
                label3.Text = "Sim";
                label3.ForeColor = Color.Green;
                MessageBox.Show("Download do cache concluído com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                download_ativo = false;
            });
        }

        void GTA_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytes_baixados = double.Parse(e.BytesReceived.ToString());
                double bytes_totais = double.Parse(e.TotalBytesToReceive.ToString());

                double porcentagem = bytes_baixados / bytes_totais * 100;
                progressBar3.Value = int.Parse(Math.Truncate(porcentagem).ToString());
            });
        }

        void GTA_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                string sa_path = "";

                if (Environment.Is64BitOperatingSystem == true) 
                {
                    sa_path = Path.GetPathRoot(Environment.SystemDirectory) + "\\Program Files (x86)\\Rockstar Games\\GTA San Andreas";
                }
                else
                {
                    sa_path = Path.GetPathRoot(Environment.SystemDirectory) + "\\Program Files\\Rockstar Games\\GTA San Andreas";
                }

                bool existe = System.IO.Directory.Exists(sa_path);

                if (!existe)
                    System.IO.Directory.CreateDirectory(sa_path);

                System.IO.DirectoryInfo di = new DirectoryInfo(sa_path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                ZipFile.ExtractToDirectory("GTA.zip", sa_path);

                progressBar3.Hide();
                label8.Text = "Sim";
                label8.ForeColor = Color.Green;
                MessageBox.Show("Download do GTA concluído com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                download_ativo = false;
            });
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ls-rp.pt/registro");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.ls-rp.pt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            progressBar3.Show();

            if (download_ativo == true)
            {
                MessageBox.Show("Você já está baixando algo.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            else
            {
                try
                {
                    using (WebClient web_client = new WebClient())
                    {
                        web_client.DownloadProgressChanged += GTA_DownloadProgressChanged;
                        web_client.DownloadFileCompleted += new AsyncCompletedEventHandler(GTA_DownloadFileCompleted);
                        web_client.DownloadFileAsync(new System.Uri(gta_url), "GTA.zip");
                        download_ativo = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro de conexão!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
