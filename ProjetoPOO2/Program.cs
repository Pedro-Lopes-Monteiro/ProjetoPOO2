using System;
using System.Windows.Forms;

namespace ProjetoPOO2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormProdutos());  // Aqui é onde a janela principal é carregada
        }
    }
}
