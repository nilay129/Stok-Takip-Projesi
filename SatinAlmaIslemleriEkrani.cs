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
    public partial class SatinAlmaIslemleriEkrani : Form
    {
        public SatinAlmaIslemleriEkrani()
        {
            InitializeComponent();
        }

        stokTakipdbEntities4 db = new stokTakipdbEntities4();

        public bool IsNumeric(string text)//inputların numeric olup olmadığını kontrol eder.
        {
            bool sayiMi = true;
            foreach (char chr in text)
            {
                if (!Char.IsNumber(chr))
                    sayiMi = false;
            }
            return sayiMi;
        }


        private void btnDemirbasEkle_Click(object sender, EventArgs e)//kullanıcıdan demirbaş bilgileri alınır hem stoğa hemde demirbas tablosuna kaydedilir.
        {
            Demirbas d;
            if(cbDemirbasTuru.SelectedItem!=null && txtDemirbasAdi.Text!="" && txtDemirbasEkleMarka.Text!="" && txtDemirbasModel.Text!="" && txtDemirbasEkleFiyat.Text!="" && txtDemirbasEkleAdet.Text!="")// boş alan olup olmadığının kontrolü
            {
                if(IsNumeric(txtDemirbasEkleFiyat.Text) && IsNumeric(txtDemirbasEkleAdet.Text))//Fiyat ve adet numeric olmalı
                    {
                    if (IsNumeric(txtDemirbasAdi.Text) == false && IsNumeric(txtDemirbasEkleMarka.Text) == false && IsNumeric(txtDemirbasEkleMarka.Text) == false)//demirbaş adı marka ve modeli numeric olamaz.
                    {
                        d = db.Demirbas.FirstOrDefault(x => x.demirbasAdi == txtDemirbasAdi.Text && x.marka == txtDemirbasEkleMarka.Text && x.model == txtDemirbasModel.Text);//eklenecek demirbaş daha önceden demirbaş tablosuna kaydedildiyse
                        if (d != null)
                        {
                            MessageBox.Show("Demirbaş daha önceden eklenmiş demirbaşı güncellemek için demirbaş işlemleri ekranına gidiniz.");
                        }
                        else
                        {
                            //daha önceden eklenmediyse yeni bir demirbaş kaydı oluşturulur.
                            d = new Demirbas();
                            d.demirbasAdi = txtDemirbasAdi.Text.ToLower().Trim();
                            d.DemirbasTur= db.DemirbasTur.FirstOrDefault(x => x.demirbasTuruAdi == cbDemirbasTuru.SelectedItem.ToString());
                            d.fiyat = Convert.ToDecimal(txtDemirbasEkleFiyat.Text);
                            d.adet = Convert.ToInt32(txtDemirbasEkleAdet.Text);
                            d.alımTarihi = Convert.ToDateTime(dtpDemirbasAlimTarihi.Value);
                            d.model = txtDemirbasModel.Text.ToLower().Trim();
                            d.marka = txtDemirbasEkleMarka.Text.ToLower().Trim();
                            db.Demirbas.Add(d);
                            db.SaveChanges();

                            Stok s = new Stok();//eklenen demirbaş aynı zamanda stok tablosunada kaydedilir.
                            s.Demirbas = d;
                            s.stokAdet = d.adet;
                            db.Stok.Add(s);
                            db.SaveChanges();
                            MessageBox.Show("Demirbaş başarıyla eklendi.");

                        }

                        txtDemirbasAdi.Text = "";
                        cbDemirbasTuru.SelectedItem = null;
                        cbDemirbasTuru.SelectedText = string.Empty;
                        txtDemirbasEkleFiyat.Text = " ";
                        txtDemirbasEkleAdet.Text = " ";
                        txtDemirbasModel.Text = " ";
                        txtDemirbasEkleMarka.Text = " ";
                    }
                    else////demirbaş adı marka ve modeli sayıysa
                        MessageBox.Show("Adı, marka ve model bilgileri sayısal olamaz.");
                }
                  
                else//fiyat ve adet numeric değilse
                {
                    MessageBox.Show("Fiyat ve adet bilgileri sayı olmalıdır.");
                }
               
            }
            else//boş alan kaldıysa
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız.");
            }
          
        }

        private void SatinAlmaIslemleriEkrani_Load(object sender, EventArgs e)//Sayfa yüklendiğinde demirbaş türleri veri tabanından çekilir.
        {
            foreach (DemirbasTur dt in db.DemirbasTur.ToList())
            {
                cbDemirbasTuru.Items.Add(dt.demirbasTuruAdi);
            }

          //pictureBox2.Image = Image.FromFile("C://Users//X556//Desktop/telefon//DSC_0796.JPG");

        }

    
    }
    }

