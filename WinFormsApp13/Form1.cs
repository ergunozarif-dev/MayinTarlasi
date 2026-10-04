using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp13
{
    public partial class Form1 : Form
    {
        Button btnEmoji = new Button();
        Button btnBasla = new Button();
        TextBox txtX = new TextBox();
        TextBox txtY = new TextBox();
        TextBox txtMayin = new TextBox();
        Panel pnlOyunAlani = new Panel();
        FlowLayoutPanel pnlUstBar = new FlowLayoutPanel();

        int satir = 0;
        int sutun = 0;
        int mayin = 0;
        Button[,] kutular = new Button[0, 0];
        bool[,] mayinVarMi = new bool[0, 0];
        bool oyunBitti;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Mayın Tarlası";
            this.Size = new Size(800, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

            pnlUstBar.Dock = DockStyle.Top;
            pnlUstBar.AutoSize = true;
            pnlUstBar.Padding = new Padding(10);
            pnlUstBar.WrapContents = false;
            this.Controls.Add(pnlUstBar);

            pnlOyunAlani.Dock = DockStyle.Fill;
            pnlOyunAlani.AutoScroll = true;
            pnlOyunAlani.Padding = new Padding(10);
            this.Controls.Add(pnlOyunAlani);

            btnEmoji.Text = "😊";
            btnEmoji.Font = new Font("Segoe UI", 16);
            btnEmoji.Size = new Size(50, 50);
            btnEmoji.Margin = new Padding(0, 0, 20, 0);
            btnEmoji.Click += BtnEmoji_Click;
            pnlUstBar.Controls.Add(btnEmoji);

            Label lblX = new Label() { Text = "Satır:", AutoSize = true, Margin = new Padding(5, 15, 0, 0) };
            txtX.Text = "10";
            txtX.Width = 50;
            txtX.Margin = new Padding(5, 12, 15, 0);
            pnlUstBar.Controls.Add(lblX);
            pnlUstBar.Controls.Add(txtX);

            Label lblY = new Label() { Text = "Sütun:", AutoSize = true, Margin = new Padding(5, 15, 0, 0) };
            txtY.Text = "10";
            txtY.Width = 50;
            txtY.Margin = new Padding(5, 12, 15, 0);
            pnlUstBar.Controls.Add(lblY);
            pnlUstBar.Controls.Add(txtY);

            Label lblMayin = new Label() { Text = "Mayın:", AutoSize = true, Margin = new Padding(5, 15, 0, 0) };
            txtMayin.Text = "15";
            txtMayin.Width = 50;
            txtMayin.Margin = new Padding(5, 12, 20, 0);
            pnlUstBar.Controls.Add(lblMayin);
            pnlUstBar.Controls.Add(txtMayin);

            btnBasla.Text = "Başla";
            btnBasla.Size = new Size(80, 30);
            btnBasla.Margin = new Padding(5, 10, 0, 0);
            btnBasla.Click += BtnBasla_Click;
            pnlUstBar.Controls.Add(btnBasla);
        }

        private void Form1_Load(object? sender, EventArgs e) { }

        private void BtnEmoji_Click(object? sender, EventArgs e)
        {
            Temizle();
        }

        private void BtnBasla_Click(object? sender, EventArgs e)
        {
            Temizle();

            if (!int.TryParse(txtX.Text.Trim(), out satir) ||
                !int.TryParse(txtY.Text.Trim(), out sutun) ||
                !int.TryParse(txtMayin.Text.Trim(), out mayin))
            {
                MessageBox.Show("Lütfen satır, sütun ve mayın kutularına sadece geçerli tam sayılar girin!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Güvenlik Kilitleri Başlangıcı ---
            if (satir > 30)
            {
                satir = 30;
                txtX.Text = "30";
                MessageBox.Show("Sistemin çökmemesi için satır sayısı maksimum 30 olarak sınırlandırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (sutun > 30)
            {
                sutun = 30;
                txtY.Text = "30";
                MessageBox.Show("Sistemin çökmemesi için sütun sayısı maksimum 30 olarak sınırlandırıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (satir <= 0) satir = 1;
            if (sutun <= 0) sutun = 1;

            // Mayın sayısı toplam kutu sayısından büyük olamaz
            if (mayin >= (satir * sutun))
            {
                mayin = (satir * sutun) - 1;
                txtMayin.Text = mayin.ToString();
            }
            if (mayin <= 0) mayin = 1;
            // --- Güvenlik Kilitleri Sonu ---

            kutular = new Button[satir, sutun];
            mayinVarMi = new bool[satir, sutun];
            oyunBitti = false;
            btnEmoji.Text = "😊";

            MayinlariDagit();

            pnlOyunAlani.SuspendLayout();
            for (int i = 0; i < satir; i++)
            {
                for (int j = 0; j < sutun; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(30, 30);
                    btn.Location = new Point((j * 30) + 10, (i * 30) + 10);
                    btn.Tag = new Point(i, j);
                    btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    btn.MouseUp += Kutu_MouseUp;
                    kutular[i, j] = btn;
                    pnlOyunAlani.Controls.Add(btn);
                }
            }
            pnlOyunAlani.ResumeLayout();
        }

        private void Temizle()
        {
            pnlOyunAlani.Controls.Clear();
            btnEmoji.Text = "😊";
        }

        private void MayinlariDagit()
        {
            Random rnd = new Random();
            int eklenenMayin = 0;
            while (eklenenMayin < mayin)
            {
                int rX = rnd.Next(0, satir);
                int rY = rnd.Next(0, sutun);
                if (!mayinVarMi[rX, rY])
                {
                    mayinVarMi[rX, rY] = true;
                    eklenenMayin++;
                }
            }
        }

        private void Kutu_MouseUp(object? sender, MouseEventArgs e)
        {
            if (oyunBitti || sender == null) return;

            Button basilan = (Button)sender;
            if (basilan.Tag == null) return;

            Point k = (Point)basilan.Tag;
            int x = k.X;
            int y = k.Y;

            if (e.Button == MouseButtons.Right)
            {
                if (basilan.Text == "")
                {
                    basilan.Text = "🚩";
                    basilan.ForeColor = Color.Red;
                }
                else if (basilan.Text == "🚩")
                {
                    basilan.Text = "";
                }
                return;
            }

            if (basilan.Text == "🚩") return;

            if (mayinVarMi[x, y])
            {
                oyunBitti = true;
                btnEmoji.Text = "😠";
                basilan.BackColor = Color.Red;
                basilan.Text = "💣";
                MessageBox.Show("Oyun Bitti!");
            }
            else
            {
                KutuAc(x, y);
            }
        }

        private void KutuAc(int x, int y)
        {
            if (x < 0 || x >= satir || y < 0 || y >= sutun) return;
            if (!kutular[x, y].Enabled) return;

            int etraftakiMayin = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = x + i;
                    int ny = y + j;
                    if (nx >= 0 && nx < satir && ny >= 0 && ny < sutun)
                    {
                        if (mayinVarMi[nx, ny]) etraftakiMayin++;
                    }
                }
            }

            kutular[x, y].Enabled = false;
            kutular[x, y].BackColor = Color.LightGray;

            if (etraftakiMayin > 0)
            {
                kutular[x, y].Text = etraftakiMayin.ToString();
            }
            else
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        KutuAc(x + i, y + j);
                    }
                }
            }
        }
    }
}