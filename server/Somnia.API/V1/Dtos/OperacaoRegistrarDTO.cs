namespace Somnia.API.V1.Dtos
{
    public class OperacaoRegistrarDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Recorrente { get; set; }
        public int Tipo { get; set; }
        public int Status { get; set; }
        public int CategoriaID { get; set; }
        public CategoriaRegistrarDTO Categoria { get; set; }
    }
}
