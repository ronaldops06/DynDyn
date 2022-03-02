namespace Somnia.API.V1.Dtos
{
    public class ContaDTO
    {
        /// <summary>
        /// Identificador e chave do banco de dados
        /// </summary>
        public int ID { get; set; }
        public string Nome { get; set; }
        public int Status { get; set; }
        public CategoriaDTO Categoria { get; set; }
        public ContaDTO ContaPai { get; set; }
    }
}
