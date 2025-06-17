namespace Application.Response
{
    /// <summary>
    /// Representa informações detalhadas sobre a resposta de uma operação.
    /// </summary>
    public record ResponseInfo
    {
        /// <summary>
        /// Título ou resumo da resposta.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Descrição detalhada do erro, caso a operação tenha falhado.
        /// </summary>
        public string? ErrorDescription { get; set; }

        /// <summary>
        /// Código de status HTTP associado à resposta.
        /// </summary>
        public int HTTPStatus { get; set; }
    }
}
