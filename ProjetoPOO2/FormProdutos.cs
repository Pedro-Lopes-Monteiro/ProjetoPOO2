using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace ProjetoPOO2
{
    public partial class FormProdutos : Form
    {
        public FormProdutos()
        {
            InitializeComponent();
        }

        private void FormProdutos_Load(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            try
            {
                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                string query = "SELECT * FROM Produtos";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dgvProdutos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os produtos: " + ex.Message);
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = txtNome.Text;
                int quantidade = int.Parse(txtQuantidade.Text);
                decimal preco = decimal.Parse(txtPreco.Text);

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection; // Obtendo a conexão do Singleton
                conn.Open(); // Garantindo que a conexão esteja aberta

                string query = "INSERT INTO Produtos (Nome, Quantidade, Preco) VALUES (@Nome, @Quantidade, @Preco)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                cmd.Parameters.AddWithValue("@Preco", preco);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Produto adicionado com sucesso!");

                // Fechar a conexão após a operação
                conn.Close();

                CarregarProdutos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar o produto: " + ex.Message);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int idProduto = int.Parse(dgvProdutos.SelectedRows[0].Cells[0].Value.ToString());
                string nome = txtNome.Text;
                int quantidade = int.Parse(txtQuantidade.Text);
                decimal preco = decimal.Parse(txtPreco.Text);

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                string query = "UPDATE Produtos SET Nome = @Nome, Quantidade = @Quantidade, Preco = @Preco WHERE IdProduto = @IdProduto";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                cmd.Parameters.AddWithValue("@Preco", preco);
                cmd.Parameters.AddWithValue("@IdProduto", idProduto);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Produto atualizado com sucesso!");
                CarregarProdutos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o produto: " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                int idProduto = int.Parse(dgvProdutos.SelectedRows[0].Cells[0].Value.ToString());

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                string query = "DELETE FROM Produtos WHERE IdProduto = @IdProduto";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdProduto", idProduto);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Produto excluído com sucesso!");
                CarregarProdutos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir o produto: " + ex.Message);
            }
        }

        private void btnLer_Click(object sender, EventArgs e)
        {
            CarregarProdutos();
        }
    }
}
