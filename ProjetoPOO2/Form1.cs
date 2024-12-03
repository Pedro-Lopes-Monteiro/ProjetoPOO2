using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace ProjetoPOO2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarProdutos();
            CarregarFornecedores();
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
                // Verifica se uma linha foi selecionada na DataGridView
                if (dgvProdutos.SelectedRows.Count > 0)
                {
                    // Obtém os valores do produto selecionado
                    int idProduto = int.Parse(dgvProdutos.SelectedRows[0].Cells[0].Value.ToString()); // ID do produto
                    string nome = txtNome.Text;
                    int quantidade = int.Parse(txtQuantidade.Text);
                    decimal preco = decimal.Parse(txtPreco.Text);

                    // Obter a conexão
                    MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                    conn.Open(); // Abre a conexão

                    string query = "UPDATE Produtos SET Nome = @Nome, Quantidade = @Quantidade, Preco = @Preco WHERE IdProduto = @IdProduto";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", nome);
                    cmd.Parameters.AddWithValue("@Quantidade", quantidade);
                    cmd.Parameters.AddWithValue("@Preco", preco);
                    cmd.Parameters.AddWithValue("@IdProduto", idProduto);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produto atualizado com sucesso!");

                    // Fechar a conexão após a operação
                    conn.Close();

                    CarregarProdutos(); // Atualiza a lista de produtos na DataGridView
                }
                else
                {
                    MessageBox.Show("Selecione um produto para atualizar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o produto: " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Verifica se alguma linha foi selecionada na DataGridView
            if (dgvProdutos.SelectedRows.Count > 0)
            {
                // Obtém o ID do produto a partir da linha selecionada
                int produtoId = Convert.ToInt32(dgvProdutos.SelectedRows[0].Cells[0].Value); // Considerando que o ID está na primeira coluna

                // Confirmação de exclusão
                DialogResult result = MessageBox.Show("Tem certeza de que deseja excluir este produto?", "Excluir", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Abre a conexão com o banco de dados
                        MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                        conn.Open();

                        // Comando SQL para excluir o produto
                        string query = "DELETE FROM Produtos WHERE IdProduto = @IdProduto"; // Alterado para Produtos
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@IdProduto", produtoId);

                        // Executa o comando de exclusão
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Produto excluído com sucesso!");
                            // Atualiza a DataGridView após a exclusão
                            CarregarProdutos();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao excluir produto.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro: " + ex.Message);
                    }
                    finally
                    {
                        // Fecha a conexão
                        DatabaseConnection.GetInstance().CloseConnection();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um produto para excluir.");
            }
        }

        private void btnLer_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter os parâmetros de busca dos campos de texto
                string nomeBusca = txtNome.Text.Trim();
                string quantidadeBusca = txtQuantidade.Text.Trim();
                string precoBusca = txtPreco.Text.Trim();

                // Montar a query de busca com base nos campos preenchidos
                string query = "SELECT * FROM Produtos WHERE 1=1";

                // Se o nome foi preenchido, adicionar à consulta
                if (!string.IsNullOrEmpty(nomeBusca))
                {
                    query += " AND Nome LIKE @Nome";
                }

                // Se a quantidade foi preenchida, adicionar à consulta
                if (!string.IsNullOrEmpty(quantidadeBusca))
                {
                    query += " AND Quantidade = @Quantidade";
                }

                // Se o preço foi preenchido, adicionar à consulta
                if (!string.IsNullOrEmpty(precoBusca))
                {
                    query += " AND Preco = @Preco";
                }

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Adicionar os parâmetros à consulta
                if (!string.IsNullOrEmpty(nomeBusca))
                {
                    cmd.Parameters.AddWithValue("@Nome", "%" + nomeBusca + "%");
                }
                if (!string.IsNullOrEmpty(quantidadeBusca))
                {
                    cmd.Parameters.AddWithValue("@Quantidade", int.Parse(quantidadeBusca));
                }
                if (!string.IsNullOrEmpty(precoBusca))
                {
                    cmd.Parameters.AddWithValue("@Preco", decimal.Parse(precoBusca));
                }

                // Executar a consulta e carregar os resultados
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dgvProdutos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar produtos: " + ex.Message);
            }
        }

        private void CarregarFornecedores()
        {
            try
            {
                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                string query = "SELECT * FROM Fornecedores";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dgvFornecedores.DataSource = dt; // Supondo que exista um DataGridView para fornecedores
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os fornecedores: " + ex.Message);
            }
        }

        // Adicionar um novo fornecedor
        private void btnAdicionarFornecedor_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = txtNomeFornecedor.Text; // Campo de nome do fornecedor
                string telefone = txtTelefoneFornecedor.Text; // Campo de telefone do fornecedor

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                conn.Open(); // Garantindo que a conexão esteja aberta

                string query = "INSERT INTO Fornecedores (Nome, Telefone) VALUES (@Nome, @Telefone)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Telefone", telefone);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Fornecedor adicionado com sucesso!");

                // Fechar a conexão após a operação
                conn.Close();

                CarregarFornecedores(); // Atualiza a lista de fornecedores na DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar fornecedor: " + ex.Message);
            }
        }


        private void btnLerFornecedor_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter os parâmetros de busca dos campos de texto
                string nomeBusca = txtNomeFornecedor.Text.Trim();
                string telefoneBusca = txtTelefoneFornecedor.Text.Trim();

                // Montar a query de busca com base nos campos preenchidos
                string query = "SELECT * FROM Fornecedores WHERE 1=1";

                // Se o nome foi preenchido, adicionar à consulta
                if (!string.IsNullOrEmpty(nomeBusca))
                {
                    query += " AND Nome LIKE @Nome";
                }

                // Se o telefone foi preenchido, adicionar à consulta
                if (!string.IsNullOrEmpty(telefoneBusca))
                {
                    query += " AND Telefone LIKE @Telefone";
                }

                // Obter a conexão
                MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Adicionar os parâmetros à consulta
                if (!string.IsNullOrEmpty(nomeBusca))
                {
                    cmd.Parameters.AddWithValue("@Nome", "%" + nomeBusca + "%");
                }
                if (!string.IsNullOrEmpty(telefoneBusca))
                {
                    cmd.Parameters.AddWithValue("@Telefone", "%" + telefoneBusca + "%");
                }

                // Executar a consulta e carregar os resultados
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvFornecedores.DataSource = dt;

                MessageBox.Show("Busca de fornecedores concluída com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar fornecedores: " + ex.Message);
            }
        }


        // Função de Atualizar Fornecedor
        private void btnAtualizarFornecedor_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se uma linha foi selecionada na DataGridView
                if (dgvFornecedores.SelectedRows.Count > 0)
                {
                    // Obtém os valores do fornecedor selecionado
                    int idFornecedor = int.Parse(dgvFornecedores.SelectedRows[0].Cells[0].Value.ToString()); // ID do fornecedor
                    string nome = txtNomeFornecedor.Text;
                    string telefone = txtTelefoneFornecedor.Text;

                    // Obter a conexão
                    MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                    conn.Open(); // Abre a conexão

                    string query = "UPDATE Fornecedores SET Nome = @Nome, Telefone = @Telefone WHERE IdFornecedor = @IdFornecedor"; // Use o nome correto da coluna
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", nome);
                    cmd.Parameters.AddWithValue("@Telefone", telefone);
                    cmd.Parameters.AddWithValue("@IdFornecedor", idFornecedor);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Fornecedor atualizado com sucesso!");

                    // Fechar a conexão após a operação
                    conn.Close();

                    CarregarFornecedores(); // Atualiza a lista de fornecedores na DataGridView
                }
                else
                {
                    MessageBox.Show("Selecione um fornecedor para atualizar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar fornecedor: " + ex.Message);
            }
        }



        // Função de Excluir Fornecedor
        private void btnExcluirFornecedor_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvFornecedores.SelectedRows.Count > 0)
                {
                    int fornecedorId = Convert.ToInt32(dgvFornecedores.SelectedRows[0].Cells[0].Value); // Obtém o ID do fornecedor selecionado

                    // Obter a conexão
                    MySqlConnection conn = DatabaseConnection.GetInstance().Connection;
                    conn.Open(); // Abre a conexão

                    string query = "DELETE FROM Fornecedores WHERE IdFornecedor = @IdFornecedor"; // Corrigido
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdFornecedor", fornecedorId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Fornecedor excluído com sucesso!");

                    conn.Close(); // Fechar a conexão após a operação

                    CarregarFornecedores(); // Atualiza a lista de fornecedores na DataGridView
                }
                else
                {
                    MessageBox.Show("Selecione um fornecedor para excluir.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir fornecedor: " + ex.Message);
            }
        }


        private void lblTelefone_Click(object sender, EventArgs e)
        {

        }

        private void lblTelefone_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
