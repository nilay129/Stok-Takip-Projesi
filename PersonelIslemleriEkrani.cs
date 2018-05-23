using stokTakip.Model;
using stokTakip.Properties;
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
    public partial class PersonelIslemleriEkrani : Form
    {
        public PersonelIslemleriEkrani()
        {
            InitializeComponent();
        }
        stokTakipdbEntities4 db = new stokTakipdbEntities4();
        OpenFileDialog dosya;

        public bool IsNumeric(string text)
        {
            bool sayiMi = true;
            foreach (char chr in text)
            {
                if (!Char.IsNumber(chr))
                    sayiMi = false;
            }
            return sayiMi;
        }
        public void departmanBul(ComboBox departman, ComboBox fakulte)//fakülte seçimi her değiştiğinde departman bilgilerinin tekrar gelmesi
        {
            departman.SelectedItem = null; //fakülte her değiştiğinde tekrar departmanlar listeleneceği için seçili departman null olur.
            departman.SelectedText = string.Empty;//seçili eleman null olduğu için text de empty olur.
            departman.Items.Clear();//departman combobox'ı temizle
            if(fakulte.SelectedItem!=null)
            {
                Fakulte f = db.Fakulte.FirstOrDefault(x => x.fakulteAdi == fakulte.SelectedItem.ToString());
                foreach (Departman d in db.Departman.Where(x => x.fakulteId == f.fakulteId))
                {
                    departman.Items.Add(d.departmanAdi);
                }
            }          

        }
        private void PersonelIslemleriEkrani_Load(object sender, EventArgs e)
        {
            foreach (Fakulte f in db.Fakulte.ToList())
            {
                cbPersonelEkleFakulte.Items.Add(f.fakulteAdi);
           
            }

            foreach (Personel p in db.Personel.ToList())
            {
                cbDemirbasListelenecekPersonel.Items.Add(p.personelAdi + " " + p.personelSoyadi+ " "+ p.personelSicilNo);
            }
        }

        //--PERSONEL EKLE EKRANI--
        private void cbPersonelEkleFakulte_SelectedIndexChanged(object sender, EventArgs e)
        {
            departmanBul(cbPersonelEkleDepartman, cbPersonelEkleFakulte);
        }

        private void btnFotografEkle_Click(object sender, EventArgs e)//fotoğraf ekle butonuna basınca open file dialog açılması
        {
            dosya = new OpenFileDialog();
            dosya.InitialDirectory = "C:\\";
            dosya.Title = "Resim Seç";
            dosya.FilterIndex = 1;
            dosya.Filter = ("Jpeg Resim Dosyası (*.jpg)|*.jpg|Gif Resim Dosyası (*.gif)|*.gif|Bmp Resim Dosyası(*.bmp)|*.bmp|Tüm Dosyalar(*.*)|*.*");
            if (dosya.ShowDialog() == DialogResult.OK)
            {
                string dosyaadi = "";
                dosyaadi = dosya.FileName;
                picboxFotograf.Image = Image.FromFile(dosyaadi);
            }
            personelEkleFotograf.Text = dosya.FileName;

        }

        private void btnPersonelEkle_Click(object sender, EventArgs e)
        {
            
            if(txtPersonelEkleAdi.Text!="" && txtPersonelEkleSoyadi.Text!="" && cbPersonelEkleFakulte.SelectedItem!=null && cbPersonelEkleDepartman.SelectedItem!=null && personelEkleFotograf.Text!="" && txtKullaniciAdi.Text!="" && txtSifre.Text!="" && cbPersonelYetki.SelectedItem!=null)//boş alan olup olmadığının kontrolü
            {
                if (IsNumeric(txtPersonelEkleAdi.Text) || IsNumeric(txtPersonelEkleSoyadi.Text) || IsNumeric(txtKullaniciAdi.Text))//adı soyadı kullanıcı adı bilgileri sayısalsa
                    MessageBox.Show("Ad,soyad,kullanıcı adı sayısal olamaz.");
                else
                {
                    Fakulte f = db.Fakulte.FirstOrDefault(x => x.fakulteAdi == cbPersonelEkleFakulte.SelectedItem.ToString());
                    Departman dep = db.Departman.FirstOrDefault(x => x.departmanAdi == cbPersonelEkleDepartman.SelectedItem.ToString());
                    Personel p = new Personel();
                    p.personelAdi = txtPersonelEkleAdi.Text;
                    p.personelSoyadi = txtPersonelEkleSoyadi.Text;
                    p.Departman = dep;
                    p.fotograf = dosya.SafeFileName;
                    db.Personel.Add(p);
                    db.SaveChanges();
                    picboxFotograf.Image.Save(Application.StartupPath + "\\resimler\\" + dosya.SafeFileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                    string fakulteId = f.fakulteId.ToString(), departmanId = dep.departmanId.ToString(), personelId = p.personelId.ToString();//personel sicin numarasını oluşturmak için fakulteId departmanId ve personelId bilgileri kullanıldı.Tek basamaklı id'lerin önüne sıfır eklendi.
                    if (f.fakulteId.ToString().Length == 1)
                        fakulteId = "0" + fakulteId;
                    if (dep.departmanId.ToString().Length == 1)
                        departmanId = "0" + departmanId;
                    if (p.personelId.ToString().Length == 1)
                        personelId = "0" + personelId;
                    p.personelSicilNo = fakulteId + departmanId + personelId;

                    Kullanici k = new Kullanici();//personel eklenirken aynı zamanda kullanıcı bilgileride kaydedilir.
                    if (db.Kullanici.Any(x => x.kullaniciAdi == txtKullaniciAdi.Text))//girilen kullanıcı adı daha önceden kullanıcı tablosunda kayıtlıysa 
                    {
                        MessageBox.Show("Kullanıcı adı daha önceden alınmış lütfen yeni bir kullanıcı adı giriniz.");
                    }
                    else//kullanıcı adı daha önceden alınmamışsa
                    {
                        k.kullaniciAdi = txtKullaniciAdi.Text;
                        k.sifre = txtSifre.Text;
                        if (cbPersonelYetki.SelectedItem.ToString() == "Admin")//kullanıcının yetkisi belirlenir.
                            k.Yetki = 1;
                        else if (cbPersonelYetki.SelectedItem.ToString() == "Normal")
                            k.Yetki = 0;
                        k.personelId = p.personelId;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Personel başarıyla eklendi.");

                    txtPersonelEkleAdi.Text = "";
                    txtPersonelEkleSoyadi.Text = "";
                    cbPersonelEkleFakulte.SelectedItem = null;
                    cbPersonelEkleDepartman.SelectedItem = null;
                    personelEkleFotograf.Text = "";
                    txtKullaniciAdi.Text = "";
                    txtSifre.Text = "";
                    cbPersonelYetki.SelectedItem = null;
                }              
            }
            else
                MessageBox.Show("Lütfen boş alan bırakmayınız.");
        }
        //--PERSONEL EKLE EKRANI--

        private void cbDemirbasListelenecekPersonel_SelectedIndexChanged(object sender, EventArgs e)//personel seçildiği anda o personel hangi odalardan sorumlu ise o odalar getirilir.
        {
            cbPersonelDemirbasListesiOdaNumarasi.SelectedItem = null;
            cbPersonelDemirbasListesiOdaNumarasi.SelectedText = string.Empty;
            cbPersonelDemirbasListesiOdaNumarasi.Items.Clear();
            foreach (Oda o in db.Oda.Where(x=>x.Personel.personelAdi + " " + x.Personel.personelSoyadi + " " + x.Personel.personelSicilNo == cbDemirbasListelenecekPersonel.SelectedItem.ToString()))
            {
                cbPersonelDemirbasListesiOdaNumarasi.Items.Add(o.odaNumarasi);//seçilen personelin sorumlu olduğu odalar
            }
            if (cbPersonelDemirbasListesiOdaNumarasi.Items.Count==0 )//Seçilen personelin sorumlu olduğu oda yoksa
                MessageBox.Show("Seçilen personel henüz hiçbir odadan sorumlu değildir.");

        }

        private void btnPersonelDemirbasListele_Click(object sender, EventArgs e)
        {
            if(cbDemirbasListelenecekPersonel.SelectedItem!=null && cbPersonelDemirbasListesiOdaNumarasi.SelectedItem!=null)
            {
                List<localDemirbas> demirbaslar = new List<localDemirbas>();
                foreach (OdaDemirbasAtama ata in db.OdaDemirbasAtama.Where(x => x.Oda.odaNumarasi == cbPersonelDemirbasListesiOdaNumarasi.SelectedItem.ToString()))//seçilen personelin sorumlu olduğu odalardan hangisi seçilirse o demirbaşlar listelenir.
                {
                    localDemirbas ld = new localDemirbas();
                    if (ata.adet > 0)//o odada hangi demirbaşların olduğunun belirlenmesi
                    {
                        ld.demirbasKodu = ata.demirbasKodu;
                        ld.demirbasAdi = ata.Demirbas.demirbasAdi;
                        ld.marka = ata.Demirbas.marka;
                        ld.model = ata.Demirbas.model;
                        ld.demirbasAdeti = ata.adet;
                        demirbaslar.Add(ld);
                    }
                }
                dgvPersonelDemirbasListesi.DataSource = demirbaslar;
                dgvPersonelDemirbasListesi.Columns[0].Visible = false;
                dgvPersonelDemirbasListesi.Columns[5].Visible = false;
                dgvPersonelDemirbasListesi.Columns[7].Visible = false;
                dgvPersonelDemirbasListesi.Columns[8].Visible = false;
                dgvPersonelDemirbasListesi.Columns[9].Visible = false;

                this.dgvPersonelDemirbasListesi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
                MessageBox.Show("Lütfen boş alan bırakmayınız.");
        }
    }
}
