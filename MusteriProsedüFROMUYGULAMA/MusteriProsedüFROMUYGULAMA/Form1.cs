using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.SqlClient;

namespace MusteriProsedüFROMUYGULAMA
{
    public partial class Form1 : Form
    {

        SqlConnection sqlCon = new SqlConnection("Data Source=DESKTOP-ICU6LR7;Initial Catalog=Musteriler;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }
        //id degişkeninin veri kayıtlarındaki id sunutu için kullanılır
        int id = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void Clear()
        {
            //throw new NotImplementedException();
        }

        private void Kaydetbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    //MessageBox.Show("Bağlandı");
                }
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", id, DbType.Int32, ParameterDirection.Input, ((short)id));
                param.Add("@Adı", Adıtxt.Text.Trim());
                param.Add("@soyadı", Soyadıtxt.Text.Trim());
                param.Add("@sil", false);
                param.Add("@Adres", Adrestxt.Text.Trim());
                sqlCon.Execute
                    ("MusteriekleveDuzenle", param, commandType: CommandType.StoredProcedure);
                if (id == 0)
                {
                    toolStripStatusLabel1.Text = "Kayıt oldu";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Güncelleme oldu";
                }
                FillDataGridView();
                clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                sqlCon.Close();
            }

           



        }


        void FillDataGridView()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Aramametni", aramaAdıtxt.Text.Trim());
            List<MusteriBigileri> list = sqlCon.Query<MusteriBigileri>
                ("MusteriArama", param, commandType:CommandType.StoredProcedure).ToList<MusteriBigileri>();
            dataGridView1.DataSource = list;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[3].Visible = false;


        }
        class MusteriBigileri
        {
            public int id { get; set; }

            public string Adı { get; set; }
            public string soyadı { get; set; }

            public bool sil { get; set; }

            public string Adres { get; set; }


        }

        private void aramabtn_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        //Vazgeç
        private void Vazgeçbtn_Click(object sender, EventArgs e)
        {
            clear();
        }

         void clear()
        {
            Adıtxt.Text = "";
            Soyadıtxt.Text = "";
            Adrestxt.Text = "";
            id = 0;
            Kaydetbtn.Text = "Kaydet";
            Silbtn.Enabled = false;

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow.Index != -1)
                {
                    id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    Adıtxt.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    Soyadıtxt.Text =  dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    Adrestxt.Text =  dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    Silbtn.Enabled = true;
                    Kaydetbtn.Text = "Düzenle";


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Silbtn_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id",id);
                sqlCon.Execute("MüşteriSilYeni",param,commandType:
                    CommandType.StoredProcedure);
                Clear();
                FillDataGridView();
                toolStripStatusLabel1.Text = "Silme başarılı";
                 


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }
    }
}
