using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temporizador
{
    public partial class frmListaGeral : Form
    {
        public frmListaGeral()
        {
            InitializeComponent();
            lvlLista.View = View.Details;
            lvlLista.GridLines = true;
            lvlLista.Columns.Add("Tarefa", 200, HorizontalAlignment.Left);
            lvlLista.Columns.Add("Dia", 110, HorizontalAlignment.Left);
            lvlLista.Columns.Add("Tempo gasto", 130, HorizontalAlignment.Left);
        }
        SqlConnection conexao = null;
        //private string cx="Data Source=localhost;" +
        //"Initial Catalog=Temporizador; integrated security=true;
        private string cx = "Data Source=localhost;" +
            "Initial Catalog=Temporizador; user id=your_user_id; password=your_password";
        private string sql = "";
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lvlLista.Items.Clear();
            PreencheListView();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            lvlLista.Items.Clear();
            var dia = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            PreencheListView(dia);  
        }

        
        private void btnReset_Click(object sender, EventArgs e)
        {
            lvlLista.Items.Clear();
            PreencheListView();
        }

        public void PreencheListView()
        {
            sql = "select t.nome as tarefa,l.Dia,l.Tempo " +
                "from tbLog l left join tarefas t " +
                "on t.id = l.cod_tarefa";
            conexao = new SqlConnection(cx);
            SqlCommand comando = new SqlCommand(sql, conexao);
            try
            {
                conexao.Open();
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                        reader.GetTimeSpan(2).ToString(@"hh\:mm\:ss"),
                    };
                    var linha_listview = new ListViewItem(row);
                    lvlLista.Items.Add(linha_listview);
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
        public void PreencheListView(string dia)
        {
            sql = "select t.nome as tarefa,l.Dia,l.Tempo " +
                "from tbLog l left join tarefas t " +
                "on t.id = l.cod_tarefa where dia=" + $"'{dia}'";
            conexao = new SqlConnection(cx);
            SqlCommand comando = new SqlCommand(sql, conexao);
            try
            {
                conexao.Open();
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                        reader.GetTimeSpan(2).ToString(@"hh\:mm\:ss"),
                    };
                    var linha_listview = new ListViewItem(row);
                    lvlLista.Items.Add(linha_listview);
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
            finally
            {
                conexao.Close();
            }
            btnReset.Visible = true;
        }
    }
}
