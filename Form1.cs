using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace IDCardGenerator
{
    public partial class Form1 : Form
    {
        private Image? userPhoto = null;
        private TextBox txtName = null!;
        private TextBox txtID = null!;
        private TextBox txtAddress = null!;
        private PictureBox picPreview = null!;
        private PictureBox picResult = null!;
        private Button btnUpload = null!;
        private Button btnGenerate = null!;
        private Button btnSave = null!;

        public Form1()
        {
            InitializeComponent();
            SetupManualUI();
        }

        // THIS FIXES THE ERROR CS0103
        private void Form1_Load(object sender, EventArgs e)
        {
            // Keep this empty
        }

        private void SetupManualUI()
        {
            this.Text = "ID Card Generator 2026";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbl1 = new Label() { Text = "Full Name:", Location = new Point(20, 20), AutoSize = true };
            txtName = new TextBox() { Location = new Point(20, 45), Width = 200 };

            Label lbl2 = new Label() { Text = "ID Number:", Location = new Point(20, 80), AutoSize = true };
            txtID = new TextBox() { Location = new Point(20, 105), Width = 200 };

            Label lbl3 = new Label() { Text = "Address:", Location = new Point(20, 140), AutoSize = true };
            txtAddress = new TextBox() { Location = new Point(20, 165), Width = 200, Multiline = true, Height = 60 };

            btnUpload = new Button() { Text = "Upload Photo", Location = new Point(20, 240), Width = 200 };
            btnUpload.Click += HandleUpload;

            picPreview = new PictureBox() { Location = new Point(230, 45), Size = new Size(120, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

            btnGenerate = new Button() { Text = "Generate ID Card", Location = new Point(20, 320), Width = 330, Height = 40, BackColor = Color.AliceBlue };
            btnGenerate.Click += HandleGenerate;

            picResult = new PictureBox() { Location = new Point(380, 45), Size = new Size(580, 380), BorderStyle = BorderStyle.Fixed3D, SizeMode = PictureBoxSizeMode.Zoom };

            btnSave = new Button() { Text = "Save ID Card", Location = new Point(380, 440), Width = 580, Height = 40, BackColor = Color.LightGreen };
            btnSave.Click += HandleSave;

            this.Controls.AddRange(new Control[] { lbl1, txtName, lbl2, txtID, lbl3, txtAddress, btnUpload, picPreview, btnGenerate, picResult, btnSave });
        }

        private void HandleUpload(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    userPhoto = Image.FromFile(ofd.FileName);
                    picPreview.Image = userPhoto;
                }
            }
        }

        private void HandleGenerate(object? sender, EventArgs e)
        {
            if (userPhoto == null)
            {
                MessageBox.Show("Please upload a photo first!");
                return;
            }

            Bitmap bitmap = new Bitmap(600, 380);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                g.FillRectangle(new SolidBrush(Color.MidnightBlue), 0, 0, 600, 70);
                g.DrawString("IDENTITY CARD", new Font("Arial", 20, FontStyle.Bold), Brushes.White, 200, 15);

                g.DrawImage(userPhoto, 30, 90, 140, 160);
                g.DrawRectangle(Pens.Black, 30, 90, 140, 160);

                Font fBody = new Font("Arial", 12, FontStyle.Regular);
                Font fLabel = new Font("Arial", 12, FontStyle.Bold);
                int x = 200, y = 100;

                g.DrawString("Name:", fLabel, Brushes.Black, x, y);
                g.DrawString(txtName.Text, fBody, Brushes.Black, x + 80, y);

                g.DrawString("ID:", fLabel, Brushes.Black, x, y + 40);
                g.DrawString(txtID.Text, fBody, Brushes.Black, x + 80, y + 40);

                g.DrawString("Address:", fLabel, Brushes.Black, x, y + 80);
                g.DrawString(txtAddress.Text, fBody, Brushes.Black, new RectangleF(x + 80, y + 80, 300, 100));

                g.FillRectangle(new SolidBrush(Color.MidnightBlue), 0, 340, 600, 40);
            }
            picResult.Image = bitmap;
        }

        private void HandleSave(object? sender, EventArgs e)
        {
            if (picResult.Image == null) return;
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PNG|*.png" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    picResult.Image.Save(sfd.FileName, ImageFormat.Png);
                    MessageBox.Show("ID Card Saved Successfully!");
                }
            }
        }
    }
}