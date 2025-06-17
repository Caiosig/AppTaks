namespace Application.Response
{
    /// <summary>
    /// Classe base genérica para respostas de operações na aplicação.
    /// Utiliza o tipo genérico <typeparamref name="T"/> para permitir o retorno de qualquer tipo de valor.
    /// Inclui informações sobre a resposta (status, mensagens de erro, etc.) através da propriedade <see cref="ResponseInfo"/>.
    /// </summary>
    /// <typeparam name="T">Tipo do valor retornado na resposta.</typeparam>
    public record ResponseBase<T>
    {
        /// <summary>
        /// Informações detalhadas sobre a resposta, como título, descrição de erro e status HTTP.
        /// </summary>
        public ResponseInfo? ResponseInfo { get; set; }

        /// <summary>
        /// Valor retornado pela operação, do tipo especificado por <typeparamref name="T"/>.
        /// </summary>
        public T? Value { get; set; }
    }
}
