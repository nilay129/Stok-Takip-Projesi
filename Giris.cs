using stokTakip.Ekranlar;
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

namespace stokTakip
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }
        stokTakipdbEntities4 db = new stokTakipdbEntities4();


        private void btnGiris_Click(object sender, EventArgs e)
        {
            Kullanici k = db.Kullanici.FirstOrDefault(x => x.kullaniciAdi == txtKullaniciAdi.Text && x.sifre == txtParola.Text);

            if (k!=null)
            {
                AnaEkran ae = new AnaEkran();
                ae.Show();
            }
          
        }
    }
}
