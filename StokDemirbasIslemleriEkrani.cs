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
    public partial class StokDemirbasIslemleriEkrani : Form
    {
        public StokDemirbasIslemleriEkrani()
        {
            InitializeComponent();
        }

        stokTakipdbEntities4 db = new stokTakipdbEntities4();
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

        private void btnStokDemirbasListele_Click(object sender, EventArgs e)
        {
            List<localDemirbas> demirbaslar = new List<localDemirbas>();

            foreach (Stok s in db.Stok.ToList())
            {
                localDemirbas ld = new localDemirbas();
                ld.demirbasAdi = s.Demirbas.demirbasAdi;
                ld.marka = s.Demirbas.marka;
                ld.model = s.Demirbas.model;
                ld.demirbasTuru = s.Demirbas.DemirbasTur.demirbasTuruAdi;
                ld.demirbasAdeti = s.stokAdet;
                demirbaslar.Add(ld);
            }
            dgvStokDemirbasListele.DataSource = demirbaslar;
            dgvStokDemirbasListele.Columns[0].Visible = false;
            dgvStokDemirbasListele.Columns[1].Visible = false;
            dgvStokDemirbasListele.Columns[7].Visible = false;
            dgvStokDemirbasListele.Columns[8].Visible = false;
            dgvStokDemirbasListele.Columns[9].Visible = false;

            this.dgvStokDemirbasListele.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void btnStokAra_Click(object sender, EventArgs e)
        {
            List<localDemirbas> demirbaslar = new List<localDemirbas>();

            if (txtStokDemirbasAraMin.Text != "" && txtStokDemirbasAraMax.Text != "")
            {
                if(IsNumeric(txtStokDemirbasAraMin.Text) && IsNumeric(txtStokDemirbasAraMax.Text))
                {
                    int minAdet = Convert.ToInt32(txtStokDemirbasAraMin.Text);
                    int maxAdet = Convert.ToInt32(txtStokDemirbasAraMax.Text);
                    if (maxAdet > minAdet)
                    {
                        foreach (Stok s in db.Stok.Where(x => x.stokAdet <= maxAdet && x.stokAdet >= minAdet))
                        {
                            localDemirbas ld = new localDemirbas();
                            ld.demirbasAdi = s.Demirbas.demirbasAdi;
                            ld.marka = s.Demirbas.marka;
                            ld.model = s.Demirbas.model;
                            ld.demirbasTuru = s.Demirbas.DemirbasTur.demirbasTuruAdi;
                            ld.demirbasAdeti = s.stokAdet;
                            demirbaslar.Add(ld);

                        }
                        dgvStokDemirbasListesi.DataSource = demirbaslar;
                        dgvStokDemirbasListesi.Columns[0].Visible = false;
                        dgvStokDemirbasListesi.Columns[1].Visible = false;
                        dgvStokDemirbasListesi.Columns[7].Visible = false;
                        dgvStokDemirbasListesi.Columns[8].Visible = false;
                        dgvStokDemirbasListesi.Columns[9].Visible = false;
                        this.dgvStokDemirbasListesi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    }
                    else if (maxAdet == minAdet)
                    {
                        MessageBox.Show("Max ve Min adet eşit olamaz!");
                    }
                    else
                    {
                        MessageBox.Show("Minumum adet maximum adetten büyük olamaz!");
                    }

                }
                MessageBox.Show("Adet alanları sayısal olmalıdır.");
             
            }
            else
                MessageBox.Show("Lütfen adet alanlarını doldurunuz.");
        }
    }
}
