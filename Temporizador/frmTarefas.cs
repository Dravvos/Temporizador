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
    public partial class frmTarefas : Form
    {
        public frmTarefas()
        {
            InitializeComponent();
        }
        SqlConnection conexao = null;
        //private string cx="Data Source=localhost;" +
        //"Initial Catalog=Temporizador; integrated security=true";
        private string cx = "Data Source=localhost;" +
            "Initial Catalog=Temporizador; user id=your_user_id; password=your_password";
        private string sql = "";
        private SqlCommand CriaParametros()
        {
            SqlCommand comando = new SqlCommand(sql, conexao);
            comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtTarefa.Text;



            return comando;
        }
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            conexao = new SqlConnection(cx);
            try {
                conexao.Open();
                sql = "insert into tarefas(nome) values (@nome)";
            CriaParametros().ExecuteNonQuery();
                txtTarefa.Clear();
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.Message);

            }
            finally
            {
                conexao.Close();
            }
            MessageBox.Show("Tarefa adicionada com sucesso");
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Tarefa.nome = txtTarefa.Text;
            Close();
        }
    }
}
