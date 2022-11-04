using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temporizador
{
    public partial class frmTemporizador : Form
    {
        //Projeto em WF para gerenciar tempo
        Stopwatch cronometro = new Stopwatch();
        public frmTemporizador()
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
            comando.Parameters.Add("@cod_tarefa", SqlDbType.Int).Value = cbTarefas.SelectedIndex;
            comando.Parameters.Add("@Dia", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy/MM/dd");
            comando.Parameters.Add("@Tempo", SqlDbType.Time).Value = cronometro.Elapsed;


            return comando;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PreencheCombo();
        }
      
        private void PreencheCombo()
        {
            conexao = new SqlConnection(cx);
            sql = "select nome as tarefa from tarefas";
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
                    };
                    var linha_listview = new ListViewItem(row);

                    cbTarefas.Items.Add(linha_listview.Text);
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
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ExisteErro = false;
            if (cbTarefas.SelectedItem is null)
            {
                errorProvider1.SetError(cbTarefas, "Selecione a tarefa que irá fazer!");
                ExisteErro = true;
            }
            if (ExisteErro == false)
            {
                cronometro.Restart();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTempo.Text = cronometro.Elapsed.ToString(@"hh\:mm\:ss");
            if (cronometro.IsRunning)
            {
                FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.FixedDialog;
            }
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            if (cronometro.IsRunning)
            {
                if (cronometro.Elapsed.TotalSeconds < 60)
                {
                    cronometro.Stop();
                    sql = "insert into tbLog (cod_tarefa, Dia, Tempo) " +
                        "values (@cod_tarefa, @Dia, @Tempo)";
                    conexao = new SqlConnection(cx);
                    CriaParametros();
                    try
                    {
                        conexao.Open();
                        CriaParametros().ExecuteNonQuery();

                        MessageBox.Show("Você executou a tarefa: " + cbTarefas.SelectedItem.ToString().ToUpper() + " por "
                            + cronometro.Elapsed.TotalSeconds.ToString("0") + " segundos");
                        cbTarefas.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
                else if (cronometro.Elapsed.Minutes < 60)
                {

                    cronometro.Stop();
                    sql = "insert into tbLog (cod_tarefa, Dia, Tempo) " +
                        "values (@cod_tarefa, @Dia, @Tempo)";
                    conexao = new SqlConnection(cx);
                    CriaParametros();
                    try
                    {
                        conexao.Open();
                        CriaParametros().ExecuteNonQuery();

                        MessageBox.Show("Você executou a tarefa: " + cbTarefas.SelectedItem + " por "
                        + cronometro.Elapsed.TotalMinutes.ToString("0") + " minutos");
                        cbTarefas.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                    }
                    
                }
                else
                {
                    cronometro.Stop();
                    sql = "insert into tbLog (cod_tarefa, Dia, Tempo) " + 
                        "values (@cod_tarefa, @Dia, @Tempo)";
                    conexao = new SqlConnection(cx);
                    CriaParametros();
                    try
                    {
                        conexao.Open();
                        CriaParametros().ExecuteNonQuery();

                        MessageBox.Show("Você executou a tarefa: " + cbTarefas.SelectedItem.ToString().ToUpper() + " por "
                    + cronometro.Elapsed.TotalHours.ToString("00") + " horas " +
                    (cronometro.Elapsed.TotalMinutes / 60).ToString("00") + " minutos");
                        cbTarefas.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conexao.Close();
                    }
                }
            }
            timer1.Stop();
            lblTempo.Text = "00:00:00";
        }
        private void btnListar_Click(object sender, EventArgs e)
        {
            frmListaGeral FrmLista = new frmListaGeral();
            FrmLista.ShowDialog();
        }

        private void btnListarDia_Click(object sender, EventArgs e)
        {
            frmListaDoDia FrmLista = new frmListaDoDia();
            FrmLista.ShowDialog();
        }

        private void btnCriarTarefa_Click(object sender, EventArgs e)
        {
            frmTarefas FrmTarefas = new frmTarefas();
            FrmTarefas.ShowDialog();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(Visible)
            {
                PreencheCombo();
                timer2.Stop();
            }
        }
    }
}
