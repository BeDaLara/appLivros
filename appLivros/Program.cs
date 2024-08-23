using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace appLivros
{
    public class Program
    {
        private static string connectingString = "Server=localhost;Database=db_livros;Uid=root;Pwd=1234567;SslMode=none;";


        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Adicionar Livro");
                Console.WriteLine("2 - Listar Livros");
                Console.WriteLine("3 - Editar Livro");
                Console.WriteLine("4 - Excluir Livro");
                Console.WriteLine("5 - Sair");
                Console.Write("Escolha uma opção acima: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarLivro();
                        break;
                    case "2":
                        ListarLivros();
                        break;
                    case "3":
                        Editar();
                        break;
                    case "4":
                        Excluir();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
        }
        static void AdicionarLivro()
        {
            Console.Write("Informe o Titulo do Livro: ");
            string titulo = Console.ReadLine();

            Console.Write("Informe o Autor do Livro: ");
            string autor = Console.ReadLine();

            Console.Write("Informe o Ano de Publicação do Livro : ");
            string anoPublicacao = Console.ReadLine();

            Console.Write("Informe o Genero do Livro: ");
            string genero = Console.ReadLine();

            Console.Write("Informe o número de paginas do Livro: ");
            int numeroPaginas = int.Parse(Console.ReadLine());


            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "INSERT INTO livros(titulo,autor,anopublicacao,genero,numeropaginas) VALUES(@titulo,@autor,@anopublicacao,@genero,@numeropaginas)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@autor", autor);
                cmd.Parameters.AddWithValue("@anopublicacao", anoPublicacao);
                cmd.Parameters.AddWithValue("@genero", genero);
                cmd.Parameters.AddWithValue("@numeropaginas", numeroPaginas);

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Livro cadastrado com sucesso");
        }
        static void ListarLivros()
        {
            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "SELECT  id_livro, titulo, autor, anopublicacao, genero, numeropaginas FROM livros";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($" Id: {reader["id_livro"]}, Titulo: {reader["titulo"]}, Autor: {reader["autor"]}, Ano Publicacao: {reader["anopublicacao"]}, Genero: {reader["genero"]}, Numero Paginas: {reader["numeropaginas"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não existe Livro cadastrado ");
                    }
                }
            }
        }
        static void Excluir()
        {
            Console.Write("Informe o Id do Livro que deseja excluir: ");
            int idExclusao = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "DELETE FROM livros WHERE id_livro=@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);

                int linhaAfetada = cmd.ExecuteNonQuery();

                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Livro excluido com sucesso");
                }
                else
                {
                    Console.WriteLine("Livro não encontrado");
                }
            }
        }
        static void Editar()
        {
            Console.Write("Informe o Id do Livro que deseja editar: ");
            int idEditar = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectingString))
            {
                connection.Open();
                string query = "SELECT * FROM livros WHERE id_livro=@Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idEditar);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.Write("Informe o novo titulo: (*Deixe o campo em branco para não alterar): ");
                        string novoTitulo = Console.ReadLine();

                        Console.Write("Informe o novo autor: (*Deixe o campo em branco para não alterar): ");
                        string novoAutor = Console.ReadLine();

                        Console.Write("Informe o novo Ano de Publicação: (*Deixe o campo em branco para não alterar): ");
                        string novoAnoPublicacao = Console.ReadLine();

                        Console.Write("Informe o novo genero: (*Deixe o campo em branco para não alterar): ");
                        string novoGenero = Console.ReadLine();

                        Console.Write("Informe o novo número de paginas: (*Deixe o campo em branco para não alterar): ");
                        string novoNumeroPaginas = Console.ReadLine();


                        reader.Close();

                        string queryUpdate = "UPDATE livros SET titulo = @titulo, autor=@autor, anopublicacao=@anoPublicacao, genero=@genero, numeropaginas=@numeroPaginas WHERE id_livro=@Id";

                        cmd = new MySqlCommand(queryUpdate, connection);

                        cmd.Parameters.AddWithValue("@titulo", string.IsNullOrWhiteSpace(novoTitulo) ? reader["titulo"] : novoTitulo);
                        cmd.Parameters.AddWithValue("@autor", string.IsNullOrWhiteSpace(novoAutor) ? reader["autor"] : novoAutor);
                        cmd.Parameters.AddWithValue("@anopublicacao", string.IsNullOrWhiteSpace(novoAnoPublicacao) ? reader["anopublicacao"] : novoAnoPublicacao);
                        cmd.Parameters.AddWithValue("@genero", string.IsNullOrWhiteSpace(novoGenero) ? reader["genero"] :novoGenero);
                        cmd.Parameters.AddWithValue("@numeropaginas", string.IsNullOrWhiteSpace(novoNumeroPaginas) ? reader["numeropaginas"] : int.Parse(novoNumeroPaginas));

                        cmd.Parameters.AddWithValue("@Id", idEditar);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("O Livro foi atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("O Id do Livro informado não existe!");

                    }
                }
            }

        }

    }
}
