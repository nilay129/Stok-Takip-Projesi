using stokTakip.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stokTakip.Ekranlar
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }
        stokTakipdbEntities4 db = new stokTakipdbEntities4();

        private void Anasayfa_Load(object sender, EventArgs e)
        {
           // Personel p = db.Kullanici.FirstOrDefault(x => x.kullaniciAdi == txtKullaniciAdi.Text && x.sifre == txtParola.Text);
            //picboxKullanıcıFotograf.Image = Image.FromFile((Application.StartupPath + "\\resimler\\" + p.fotograf));
        }
    }
}
