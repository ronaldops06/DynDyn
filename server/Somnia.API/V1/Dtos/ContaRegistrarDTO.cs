namespace Somnia.API.V1.Dtos
{
    public class ContaRegistrarDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Status { get; set; }
        public CategoriaRegistrarDTO Categoria { get; set; }
        public ContaRegistrarDTO ContaPai { get; set; }
    }
}
