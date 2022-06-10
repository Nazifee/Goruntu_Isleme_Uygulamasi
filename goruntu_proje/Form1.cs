using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace goruntu_proje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Simge_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Cikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Bitmap resim = new Bitmap(220, 220);
        Bitmap resim_grayScale = new Bitmap(220, 220);
        Bitmap resim_bitMap = new Bitmap(220, 220);
        Bitmap resim_erosion = new Bitmap(220, 220);
        Bitmap resim_dilasion = new Bitmap(220, 220);
        Bitmap resim_opening = new Bitmap(220, 220);
        Bitmap resim_closing = new Bitmap(220, 220);
        Bitmap resim_convulation = new Bitmap(220, 220);
        Bitmap resim_gradient = new Bitmap(220, 220);
        Bitmap resim_std = new Bitmap(220, 220);

        Bitmap CikisResmi;
        int ResimGenisligi, ResimYuksekligi;
        int R = 0, G = 0, B = 0;
        Color OkunanRenk, DonusenRenk;

        private void btn_gri_Click(object sender, EventArgs e)
        {
            int GriDeger = 0;
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = resim.GetPixel(x, y);
                    double R = OkunanRenk.R;
                    double G = OkunanRenk.G;
                    double B = OkunanRenk.B;
                    //GriDeger = Convert.ToInt16((R + G + B) / 3);

                    GriDeger = Convert.ToInt16(R * 0.3 + G * 0.6 + B * 0.1);
                    DonusenRenk = Color.FromArgb(GriDeger, GriDeger, GriDeger);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_parlaklik_Click(object sender, EventArgs e)
        {
            Color c;
            int r, g, b;
            int anti_renk;
            if (textBox1.Text == "")
                MessageBox.Show("Lütfen değer giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                anti_renk = Convert.ToInt32(textBox1.Text);
                for (int i = 0; i < 220; i++)
                {
                    for (int j = 0; j < 220; j++)
                    {
                        c = resim_grayScale.GetPixel(i, j);
                        r = c.R + anti_renk;
                        if (r > 255) r = 255;
                        g = c.R + anti_renk;
                        if (g > 255) g = 255;
                        b = c.R + anti_renk;
                        if (b > 255) b = 255;
                        resim_grayScale.SetPixel(i, j, Color.FromArgb(r, g, b));

                    }
                }
                pictureBox2.Image = resim_grayScale;
                this.Refresh();
            }
        }

        private void btn_esikleme_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Lütfen değer giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int EsiklemeDegeri = Convert.ToInt32(textBox1.Text); //Eşikleme değeri parantez içine yazılır

                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = resim.GetPixel(x, y);
                        if (OkunanRenk.R >= EsiklemeDegeri)
                            R = 255;
                        else
                            R = 0;
                        if (OkunanRenk.G >= EsiklemeDegeri)
                            G = 255;
                        else
                            G = 0;
                        if (OkunanRenk.B >= EsiklemeDegeri)
                            B = 255;
                        else
                            B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void btn_histogram_Click(object sender, EventArgs e)
        {
            ArrayList DiziPiksel = new ArrayList();
            int OrtalamaRenk = 0;
            Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            for (int x = 0; x < GirisResmi.Width; x++)
            {
                for (int y = 0; y < GirisResmi.Height; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.
                    DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor.
                }
            }
            int[] DiziPikselSayilari = new int[256];
            for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
            {
                int PikselSayisi = 0;
                for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek.
                {
                    if (r == Convert.ToInt16(DiziPiksel[s]))
                        PikselSayisi++;
                }
                DiziPikselSayilari[r] = PikselSayisi;
            }
            //Değerleri listbox'a ekliyor.
            int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak.
            for (int k = 0; k <= 255; k++)
            {
                //listBox1.Items.Add("Renk:" + k + "=" + DiziPikselSayilari[k]);
                //Maksimum piksel sayısını bulmaya çalışıyor.
                if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                {
                    RenkMaksPikselSayisi = DiziPikselSayilari[k];
                }
            }

            pictureBox2.BackColor = Color.White;

            //Grafiği çiziyor.
            Graphics CizimAlani;
            Pen Kalem1 = new Pen(System.Drawing.Color.Blue, 1);
            Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
            CizimAlani = pictureBox2.CreateGraphics();
            pictureBox2.Refresh();
            int GrafikYuksekligi = 300;
            double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
            double OlcekX = 1.5;
            int X_kaydirma = 10;
            for (int x = 0; x <= 255; x++)
            {
                if (x % 50 == 0)
                    CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                   GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
               (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                //Dikey kırmızı çizgiler.

            }
            textBox1.Text = "Maks.Piks=" + RenkMaksPikselSayisi.ToString();
        }

        private void btn_kontrast_Click(object sender, EventArgs e)
        {
            double C_KontrastSeviyesi = Convert.ToInt32(textBox1.Text);
            double F_KontrastFaktoru = (259 * (C_KontrastSeviyesi + 255)) / (255 * (259 -
            C_KontrastSeviyesi));
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = resim.GetPixel(x, y);
                    R = OkunanRenk.R;
                    G = OkunanRenk.G;
                    B = OkunanRenk.B;
                    R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                    G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                    B = (int)((F_KontrastFaktoru * (B - 128)) + 128);
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_tasima_Click(object sender, EventArgs e)
        {
            double x2 = 0, y2 = 0;
            //Taşıma mesafelerini atıyor.
            int Tx = 100;
            int Ty = 50;
            for (int x1 = 0; x1 < (ResimGenisligi); x1++)
            {
                for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                {
                    OkunanRenk = resim.GetPixel(x1, y1);
                    x2 = x1 + Tx;
                    y2 = y1 + Ty;
                    if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                        CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_mean_Click(object sender, EventArgs e)
        {
            int SablonBoyutu = 5; //şablon boyutu 3 den büyük tek rakam olmalıdır(3, 5, 7 gibi).
            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = resim.GetPixel(x + i, y + j);
                            toplamR = toplamR + OkunanRenk.R;
                            toplamG = toplamG + OkunanRenk.G;
                            toplamB = toplamB + OkunanRenk.B;
                        }
                    }
                    ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                    ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                    ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);
                    CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_gauss_Click(object sender, EventArgs e)
        {
            int SablonBoyutu = 5; //Çekirdek matrisin boyutu
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
            int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
            int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 + 33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.

            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;
                    //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                    int k = 0; //matris içindeki elemanları sırayla okurken kullanılacak.
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = resim.GetPixel(x + i, y + j);
                            toplamR = toplamR + OkunanRenk.R * Matris[k];
                            toplamG = toplamG + OkunanRenk.G * Matris[k];
                            toplamB = toplamB + OkunanRenk.B * Matris[k];
                            k++;
                        }
                        ortalamaR = toplamR / MatrisToplami;
                        ortalamaG = toplamG / MatrisToplami;
                        ortalamaB = toplamB / MatrisToplami;
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_median_Click(object sender, EventArgs e)
        {
            int SablonBoyutu = 7; //şablon boyutu 3 den büyük tek rakam olmalıdır(3, 5, 7 gibi).
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int[] R = new int[ElemanSayisi];
            int[] G = new int[ElemanSayisi];
            int[] B = new int[ElemanSayisi];
            int[] Gri = new int[ElemanSayisi];
            int x, y, i, j;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                    int k = 0;
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = resim.GetPixel(x + i, y + j);
                            R[k] = OkunanRenk.R;
                            G[k] = OkunanRenk.G;
                            B[k] = OkunanRenk.B;
                            Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); //Gri ton formülü
                            k++;
                        }
                    }
                    //Gri tona göre sıralama yapıyor. Aynı anda üç rengide değiştiriyor.
                    int GeciciSayi = 0;
                    for (i = 0; i < ElemanSayisi; i++)
                    {
                        for (j = i + 1; j < ElemanSayisi; j++)
                        {
                            if (Gri[j] < Gri[i])
                            {
                                GeciciSayi = Gri[i];
                                Gri[i] = Gri[j];
                                Gri[j] = GeciciSayi;
                                GeciciSayi = R[i];
                                R[i] = R[j];
                                R[j] = GeciciSayi;
                                GeciciSayi = G[i];
                                G[i] = G[j];
                                G[j] = GeciciSayi;
                                GeciciSayi = B[i];
                                B[i] = B[j];
                                B[j] = GeciciSayi;
                            }
                        }
                    }
                    //Sıralama sonrası ortadaki değeri çıkış resminin piksel değeri olarak atıyor.
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) / 2], B[(ElemanSayisi - 1) / 2]));
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_sobel_Click(object sender, EventArgs e)
        {
            Bitmap GirisResmi, CikisResmiXY;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);

            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y;
            Color Renk;
            int P1, P2, P3, P4, P5, P6, P7, P8, P9;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    Renk = GirisResmi.GetPixel(x - 1, y - 1);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y - 1);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y - 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y);
                    P5 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P6 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y + 1);
                    P7 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P8 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P9 = (Renk.R + Renk.G + Renk.B) / 3;


                    int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9);
                    int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9);
                    int Gxy = Gx + Gy;
                    if (Gx > 255) Gx = 255;
                    if (Gy > 255) Gy = 255;
                    if (Gxy > 255) Gxy = 255;
                    CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                }
            }
            pictureBox2.Image = CikisResmiXY;
        }

        private void btn_dondurme_Click(object sender, EventArgs e)
        {
            int Aci = Convert.ToInt16(textBox1.Text);
            if (textBox1.Text == "")
                MessageBox.Show("Lütfen değer giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                double RadyanAci = Aci * 2 * Math.PI / 360;
                double x2 = 0, y2 = 0;
                //Resim merkezini buluyor. Resim merkezi etrafında döndürecek.
                int x0 = ResimGenisligi / 2;
                int y0 = ResimYuksekligi / 2;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = resim.GetPixel(x1, y1);
                        //Döndürme Formülleri
                        x2 = Math.Cos(RadyanAci) * (x1 - x0) - Math.Sin(RadyanAci) * (y1 - y0) + x0;
                        y2 = Math.Sin(RadyanAci) * (x1 - x0) + Math.Cos(RadyanAci) * (y1 - y0) + y0;
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void btn_cevirme_Click(object sender, EventArgs e)
        {
            resim.RotateFlip(RotateFlipType.Rotate180FlipNone);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = resim;
        }

        private void btn_aynalama_Click(object sender, EventArgs e)
        {
            Bitmap mimg = new Bitmap(ResimGenisligi * 2, ResimYuksekligi);

            for (int y = 0; y < ResimGenisligi; y++)
            {
                for (int lx = 0, rx = ResimYuksekligi * 2 - 1; lx < ResimYuksekligi; lx++, rx--)
                {
                    //get source pixel value
                    Color p = resim.GetPixel(lx, y);

                    //set mirror pixel value
                    mimg.SetPixel(lx, y, p);
                    mimg.SetPixel(rx, y, p);
                }
            }
            //load mirror image in picturebox2;
            pictureBox2.Width = 400;
            pictureBox2.Height = 300;
            pictureBox2.Image = mimg;
        }

        private void btn_oteleme_Click(object sender, EventArgs e)
        {
            double x2 = 0, y2 = 0;
            //Taşıma mesafelerini atıyor.
            int Tx = 100;
            int Ty = 50;
            for (int x1 = 0; x1 < (ResimGenisligi); x1++)
            {
                for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                {
                    OkunanRenk = resim.GetPixel(x1, y1);
                    x2 = x1 + Tx;
                    y2 = y1 + Ty;
                    if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                        CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_yakin_Click(object sender, EventArgs e)
        {
            Size size = new Size(5, 10);
            Bitmap btm = new Bitmap(resim, resim.Width + (resim.Width * size.Width / 100), resim.Height + (resim.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(btm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pictureBox2.Image = btm;
        }


        private void btn_dilation_Click(object sender, EventArgs e)
        {
            int x1 = 255, x2 = 255, x3 = 255;
            Color c;
            int b1, b2, b3;
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_bitMap.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_bitMap.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_bitMap.GetPixel(x + 1, y);
                    b3 = c.R;

                    if (b1 == x1 & b2 == x2 & b3 == x3)
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                }
            }
            pictureBox2.Image = resim_dilasion;
            this.Refresh();
        }

        private void btn_erosion_Click(object sender, EventArgs e)
        {
            int x1 = 255, x2 = 255, x3 = 255;
            Color c;
            int b1, b2, b3;
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_bitMap.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_bitMap.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_bitMap.GetPixel(x + 1, y);
                    b3 = c.R;

                    if (b1 == x1 & b2 == x2 & b3 == x3)
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            pictureBox2.Image = resim_erosion;
            this.Refresh();
        }

        private void btn_kenar_Click(object sender, EventArgs e)
        {

        }


        double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length));
            //matris boyutu içindeki eleman sayısı olduğu için kare matrisde
            //karekökü matris boyutu olur.
            double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın
                                                                             //tersi alındığında bu matris içinde tutulacak
                                                                             //--I Birim matrisin içeriğini dolduruyor
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }
            //--Matris Tersini alma işlemi---------
            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu.
                    }
                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }
                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }

        private void btn_perspektif_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(txt_x1.Text);
            double y1 = Convert.ToDouble(txt_y1.Text);
            double x2 = Convert.ToDouble(txt_x2.Text);
            double y2 = Convert.ToDouble(txt_y2.Text);
            double x3 = Convert.ToDouble(txt_x3.Text);
            double y3 = Convert.ToDouble(txt_y3.Text);
            double x4 = Convert.ToDouble(txt_x4.Text);
            double y4 = Convert.ToDouble(txt_y4.Text);
            double X1 = Convert.ToDouble(txt_XX1.Text);
            double Y1 = Convert.ToDouble(txt_YY1.Text);
            double X2 = Convert.ToDouble(txt_XX2.Text);
            double Y2 = Convert.ToDouble(txt_YY2.Text);
            double X3 = Convert.ToDouble(txt_XX3.Text);
            double Y3 = Convert.ToDouble(txt_YY3.Text);
            double X4 = Convert.ToDouble(txt_XX4.Text);
            double Y4 = Convert.ToDouble(txt_YY4.Text);
            double[,] GirisMatrisi = new double[8, 8];
            // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }
            GirisMatrisi[0, 0] = x1;
            GirisMatrisi[0, 1] = y1;
            GirisMatrisi[0, 2] = 1;
            GirisMatrisi[0, 3] = 0;
            GirisMatrisi[0, 4] = 0;
            GirisMatrisi[0, 5] = 0;
            GirisMatrisi[0, 6] = -x1 * X1;
            GirisMatrisi[0, 7] = -y1 * X1;
            //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
            GirisMatrisi[1, 0] = 0;
            GirisMatrisi[1, 1] = 0;
            GirisMatrisi[1, 2] = 0;
            GirisMatrisi[1, 3] = x1;
            GirisMatrisi[1, 4] = y1;
            GirisMatrisi[1, 5] = 1;
            GirisMatrisi[1, 6] = -x1 * Y1;
            GirisMatrisi[1, 7] = -y1 * Y1;
            //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 }
            GirisMatrisi[2, 0] = x2;
            GirisMatrisi[2, 1] = y2;
            GirisMatrisi[2, 2] = 1;
            GirisMatrisi[2, 3] = 0;
            GirisMatrisi[2, 4] = 0;
            GirisMatrisi[2, 5] = 0;
            GirisMatrisi[2, 6] = -x2 * X2;
            GirisMatrisi[2, 7] = -y2 * X2;
            //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
            GirisMatrisi[3, 0] = 0;
            GirisMatrisi[3, 1] = 0;
            GirisMatrisi[3, 2] = 0;
            GirisMatrisi[3, 3] = x2;
            GirisMatrisi[3, 4] = y2;
            GirisMatrisi[3, 5] = 1;
            GirisMatrisi[3, 6] = -x2 * Y2;
            GirisMatrisi[3, 7] = -y2 * Y2;
            //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
            GirisMatrisi[4, 0] = x3;
            GirisMatrisi[4, 1] = y3;
            GirisMatrisi[4, 2] = 1;
            GirisMatrisi[4, 3] = 0;
            GirisMatrisi[4, 4] = 0;
            GirisMatrisi[4, 5] = 0;
            GirisMatrisi[4, 6] = -x3 * X3;
            GirisMatrisi[4, 7] = -y3 * X3;
            //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
            GirisMatrisi[5, 0] = 0;
            GirisMatrisi[5, 1] = 0;
            GirisMatrisi[5, 2] = 0;
            GirisMatrisi[5, 3] = x3;
            GirisMatrisi[5, 4] = y3;
            GirisMatrisi[5, 5] = 1;
            GirisMatrisi[5, 6] = -x3 * Y3;
            GirisMatrisi[5, 7] = -y3 * Y3;
            //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
            GirisMatrisi[6, 0] = x4;
            GirisMatrisi[6, 1] = y4;
            GirisMatrisi[6, 2] = 1;
            GirisMatrisi[6, 3] = 0;
            GirisMatrisi[6, 4] = 0;
            GirisMatrisi[6, 5] = 0;
            GirisMatrisi[6, 6] = -x4 * X4;
            GirisMatrisi[6, 7] = -y4 * X4;
            //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 }
            GirisMatrisi[7, 0] = 0;
            GirisMatrisi[7, 1] = 0;
            GirisMatrisi[7, 2] = 0;
            GirisMatrisi[7, 3] = x4;
            GirisMatrisi[7, 4] = y4;
            GirisMatrisi[7, 5] = 1;
            GirisMatrisi[7, 6] = -x4 * Y4;
            GirisMatrisi[7, 7] = -y4 * Y4;
            //------------------------------------------------------------------
            double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);
            //-------------------- A Dönüşüm Matrisi (3x3) -----------------
            double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0, a21 = 0, a22 = 0;
            a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 + matrisBTersi[0, 2] *
            X2 + matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 + matrisBTersi[0, 5] * Y3 +
            matrisBTersi[0, 6] * X4 + matrisBTersi[0, 7] * Y4;
            a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 + matrisBTersi[1, 2] *
            X2 + matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 + matrisBTersi[1, 5] * Y3 +
            matrisBTersi[1, 6] * X4 + matrisBTersi[1, 7] * Y4;
            a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 + matrisBTersi[2, 2] *
            X2 + matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 + matrisBTersi[2, 5] * Y3 +
            matrisBTersi[2, 6] * X4 + matrisBTersi[2, 7] * Y4;
            a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 + matrisBTersi[3, 2] *
            X2 + matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 + matrisBTersi[3, 5] * Y3 +
            matrisBTersi[3, 6] * X4 + matrisBTersi[3, 7] * Y4;
            a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 + matrisBTersi[4, 2] *
            X2 + matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 + matrisBTersi[4, 5] * Y3 +
            matrisBTersi[4, 6] * X4 + matrisBTersi[4, 7] * Y4;
            a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 + matrisBTersi[5, 2] *
            X2 + matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 + matrisBTersi[5, 5] * Y3 +
            matrisBTersi[5, 6] * X4 + matrisBTersi[5, 7] * Y4;
            a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 + matrisBTersi[6, 2] *
            X2 + matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 + matrisBTersi[6, 5] * Y3 +
            matrisBTersi[6, 6] * X4 + matrisBTersi[6, 7] * Y4;
            a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 + matrisBTersi[7, 2] * X2 + matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 + matrisBTersi[7, 5] * Y3 + matrisBTersi[7, 6] * X4 + matrisBTersi[7, 7] * Y4;
            a22 = 1;
            //------------------------- Perspektif düzeltme işlemi ------------------------
            PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22);
        }
        //================== Perspektif düzeltme işlemi =================
        void PerspektifDuzelt(double a00, double a01, double a02,
        double a10, double a11, double a12, double a20,
        double a21, double a22)
        {
            Bitmap GirisResmi, CikisResmi;
            Color OkunanRenk;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            double X, Y, z;
            for (int x = 0; x < (ResimGenisligi); x++)
            {
                for (int y = 0; y < (ResimYuksekligi); y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    z = a20 * x + a21 * y + 1;
                    X = (a00 * x + a01 * y + a02) / z;
                    Y = (a10 * x + a11 * y + a12) / z;
                    if (X > 0 && X < ResimGenisligi && Y > 0 && Y < ResimYuksekligi)
                        //Picturebox ın dışına çıkan kısımlar oluşturulmayacak.
                        CikisResmi.SetPixel((int)X, (int)Y, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_dengeleme_Click(object sender, EventArgs e)
        {
            // BAKCAZ
        }

        private void btn_laplacian_Click(object sender, EventArgs e)
        {
            int width = resim.Width - 1, height = resim.Height - 1;
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    Color color2, color4, color5, color6, color8;
                    color2 = resim.GetPixel(x, y - 1);
                    color4 = resim.GetPixel(x - 1, y);
                    color5 = resim.GetPixel(x, y);
                    color6 = resim.GetPixel(x + 1, y);
                    color8 = resim.GetPixel(x, y + 1);
                    int r = (color2.R + color4.R + color5.R * (-4)) + color6.R + color8.R;
                    int g = (color2.G + color4.G + color5.G * (-4)) + color6.G + color8.G;
                    int b = (color2.B + color4.B + color5.B * (-4)) + color6.B + color8.B;

                    int avg = ((r + g + b) / 3);
                    if (avg > 255) avg = 255;
                    if (avg < 0) avg = 0;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_prewitt_Click(object sender, EventArgs e)
        {
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y;
            Color Renk;
            int P1, P2, P3, P4, P5, P6, P7, P8, P9;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    Renk = resim.GetPixel(x - 1, y - 1);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x, y - 1);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x + 1, y - 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x - 1, y);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x, y);
                    P5 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x + 1, y);
                    P6 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x - 1, y + 1);
                    P7 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x, y + 1);
                    P8 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = resim.GetPixel(x + 1, y + 1);
                    P9 = (Renk.R + Renk.G + Renk.B) / 3;
                    int Gx = Math.Abs(-P1 + P3 - P4 + P6 - P7 + P9); //Dikey çizgileri Bulur
                    int Gy = Math.Abs(P1 + P2 + P3 - P7 - P8 - P9); //Yatay Çizgileri Bulur.
                    int PrewittDegeri = 0;
                    PrewittDegeri = Gx;
                    PrewittDegeri = Gy;
                    PrewittDegeri = Gx + Gy; //1. Formül
                                             //PrewittDegeri = Convert.ToInt16(Math.Sqrt(Gx * Gx + Gy * Gy)); //2.Formül
                                             //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (PrewittDegeri > 255) PrewittDegeri = 255;
                    //Eşikleme: Örnek olarak 100 değeri kullanıldı.
                    //if (PrewittDegeri > 100)
                    //PrewittDegeri = 255;
                    //else
                    //PrewittDegeri = 0;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(PrewittDegeri, PrewittDegeri, PrewittDegeri));
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            resim_bitMap = resim_grayScale;
            int x1 = 255, x2 = 255, x3 = 255;
            Color c;
            int b1, b2, b3;
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_bitMap.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_bitMap.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_bitMap.GetPixel(x + 1, y);
                    b3 = c.R;

                    if (b1 == x1 & b2 == x2 & b3 == x3)
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_erosion.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_erosion.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_erosion.GetPixel(x + 1, y);
                    b3 = c.R;

                    if (b1 == x1 & b2 == x2 & b3 == x3)
                    {
                        resim_opening.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        resim_opening.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }

            pictureBox2.Image = resim_opening;
            this.Refresh();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            int x1 = 255, x2 = 255, x3 = 255;
            Color c;
            int b1, b2, b3;
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_bitMap.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_bitMap.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_bitMap.GetPixel(x + 1, y);
                    b3 = c.R;
                    if (b1 == x1 || b2 == x2 || b3 == x3)
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            for (int y = 1; y < 220 - 1; y++)
            {
                for (int x = 1; x < 220 - 1; x++)
                {
                    c = resim_dilasion.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_dilasion.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_dilasion.GetPixel(x + 1, y);
                    b3 = c.R;
                    if (b1 == x1 && b2 == x2 && b3 == x3)
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        resim_erosion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
        }



        private void btn_Gradyent_Click(object sender, EventArgs e)
        {
            Color renk;
            int r, g, b;
            int gray;
            for (int x = 0; x < 240; x++)
            {
                for (int y = 0; y < 240; y++)
                {
                    renk = resim.GetPixel(x, y);
                    r = Convert.ToInt32(renk.R);
                    g = Convert.ToInt32(renk.G);
                    b = Convert.ToInt32(renk.B);

                    gray = (r + g + b) / 3;
                    resim_grayScale.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            //

            int gradient;
            Color cl;
            int x1, x2, x3, x4;
            int b1, b2, b3, b4;
            x1 = -1; x2 = 1; x3 = -1; x4 = 1;
            for (int x = 1; x < 299; x++)
            {
                for (int y = 1; y < 299; y++)
                {
                    cl = resim_grayScale.GetPixel(x, y);
                    b1 = cl.R;
                    cl = resim_grayScale.GetPixel(x + 1, y);
                    b2 = cl.R;
                    cl = resim_grayScale.GetPixel(x, y + 1);
                    b3 = cl.R;
                    cl = resim_grayScale.GetPixel(x + 1, y + 1);
                    b4 = cl.R;
                    gradient = x1 * b1 + x2 * b2 + x3 * b3 + x4 * b4;
                    if (gradient > 255) gradient = 255;
                    if (gradient < 0) gradient = 0;
                    resim_gradient.SetPixel(x, y, Color.FromArgb(gradient, gradient, gradient));
                }
            }
            pictureBox1.Image = resim_gradient;
            this.Refresh();
        }

        private void btn_Morfoloji_Click(object sender, EventArgs e)
        {
            double bit = 0;
            for (int y = 0; y < resim_bitMap.Height; y++)
            {
                for (int x = 0; x < resim_bitMap.Width; x++)
                {
                    bit += resim_bitMap.GetPixel(x, y).GetBrightness();
                }
            }

            bit = bit / (resim_bitMap.Width * resim_bitMap.Height);
            bit = bit < .3 ? .3 : bit;
            bit = bit > .7 ? .7 : bit;

            for (int y = 0; y < resim_bitMap.Height; y++)
            {
                for (int x = 0; x < resim_bitMap.Width; x++)
                {
                    if (resim_bitMap.GetPixel(x, y).GetBrightness() > bit) resim_bitMap.SetPixel(x, y, Color.White);
                    else resim_bitMap.SetPixel(x, y, Color.Black);
                }
            }
            pictureBox1.Image = resim_bitMap;
            this.Refresh();
        }

        private void btn_uzak_Click(object sender, EventArgs e)
        {
            int x2 = 0, y2 = 0; //Çıkış resminin x ve y si olacak.
            int KucultmeKatsayisi = 2;
            for (int x1 = 0; x1 < ResimGenisligi; x1 = x1 + KucultmeKatsayisi)
            {
                y2 = 0;
                for (int y1 = 0; y1 < ResimYuksekligi; y1 = y1 + KucultmeKatsayisi)
                {
                    OkunanRenk = resim.GetPixel(x1, y1);
                    DonusenRenk = OkunanRenk;
                    CikisResmi.SetPixel(x2, y2, DonusenRenk);
                    y2++;
                }
                x2++;
            }
            pictureBox2.Image = CikisResmi;
        }

        private void btn_Dilation_Click(object sender, EventArgs e)
        {
            int x1 = 255, x2 = 255, x3 = 255;
            Color c;
            int b1, b2, b3;
            for (int y = 1; y < 300 - 1; y++)
            {
                for (int x = 1; x < 300 - 1; x++)
                {
                    c = resim_bitMap.GetPixel(x, y);
                    b1 = c.R;
                    c = resim_bitMap.GetPixel(x - 1, y);
                    b2 = c.R;
                    c = resim_bitMap.GetPixel(x + 1, y);
                    b3 = c.R;

                    if (b1 == x1 & b2 == x2 & b3 == x3)
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        resim_dilasion.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                }
            }
            pictureBox2.Image = resim_dilasion;
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Color OkunanRenk, DonusenRenk;
            int R = 0, G = 0, B = 0;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. İçerisine görüntü yüklendi.
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur. Tanımlaması globalde yapıldı.
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                MessageBox.Show("Lütfen değer giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                int X1 = Convert.ToInt16(textBox1.Text);
                int X2 = Convert.ToInt16(textBox2.Text);
                int Y1 = Convert.ToInt16(textBox3.Text);
                int Y2 = Convert.ToInt16(textBox4.Text);
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = OkunanRenk.R;
                        G = OkunanRenk.G;
                        B = OkunanRenk.B;
                        int Gri = (R + G + B) / 3;
                        //*********** Kontras Formülü***************
                        int X = Gri;
                        int Y = ((((X - X1) * Y2 - Y1)) / (X2 - X1)) + Y1;
                        if (Y > 255) Y = 255;
                        if (Y < 0) Y = 0;
                        DonusenRenk = Color.FromArgb(Y, Y, Y);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Refresh();
                pictureBox2.Image = null;
                pictureBox2.Image = CikisResmi;
            }
        }

        private void btn_negatif_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = resim.GetPixel(x, y);
                    R = 255 - OkunanRenk.R;
                    G = 255 - OkunanRenk.G;
                    B = 255 - OkunanRenk.B;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btn_ekle.Select();
            ResimGenisligi = resim.Width;
            ResimYuksekligi = resim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            pictureBox2.Image = CikisResmi;
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
            Graphics.FromImage(resim).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_grayScale).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_bitMap).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_erosion).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_dilasion).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_opening).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_closing).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_convulation).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_gradient).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(resim_std).DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
        }

        private void btn_temizle_Click(object sender, EventArgs e)
        {
            resim = new Bitmap(pictureBox1.Image);
            //pictureBox2.Image = null;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

    }
    }
