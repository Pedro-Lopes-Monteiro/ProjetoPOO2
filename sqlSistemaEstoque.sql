CREATE DATABASE SistemaEstoque;

USE SistemaEstoque;

-- Tabela de Produtos
CREATE TABLE Produtos (
    IdProduto INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Quantidade INT NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL
);

-- Tabela de Fornecedores
CREATE TABLE Fornecedores (
    IdFornecedor INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Telefone VARCHAR(15)
);

-- Tabela de Pedidos
CREATE TABLE Pedidos (
    IdPedido INT AUTO_INCREMENT PRIMARY KEY,
    IdProduto INT NOT NULL,
    IdFornecedor INT NOT NULL,
    Quantidade INT NOT NULL,
    DataPedido DATE NOT NULL,
    FOREIGN KEY (IdProduto) REFERENCES Produtos(IdProduto),
    FOREIGN KEY (IdFornecedor) REFERENCES Fornecedores(IdFornecedor)
);

-- Inserção de Dados Iniciais
INSERT INTO Produtos (Nome, Quantidade, Preco) VALUES 
('Caneta', 50, 1.50),
('Caderno', 20, 15.00),
('Apontador', 30, 3.00);

INSERT INTO Fornecedores (Nome, Telefone) VALUES 
('Fornecedor A', '123456789'),
('Fornecedor B', '987654321');

INSERT INTO Pedidos (IdProduto, IdFornecedor, Quantidade, DataPedido) VALUES 
(1, 1, 100, '2024-11-20'),
(2, 2, 50, '2024-11-21');
