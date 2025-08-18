namespace Application.Utils
{
    /// <summary>
    /// Classe utilitária para realizar a paginação de listas de objetos.
    /// Calcula índices, total de páginas e retorna apenas os itens da página solicitada.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens a serem paginados.</typeparam>
    public record PaginatedList<T>
    {
        /// <summary>
        /// Total de itens na coleção original.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Página atual da paginação.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Quantidade de itens por página.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Total de páginas disponíveis.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Índice inicial dos itens da página atual.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Índice final dos itens da página atual.
        /// </summary>
        public int EndIndex { get; private set; }

        /// <summary>
        /// Lista de itens da página atual.
        /// </summary>
        public List<T> Items { get; private set; }

        /// <summary>
        /// Construtor que realiza o cálculo da paginação e seleciona os itens da página informada.
        /// </summary>
        /// <param name="items">Coleção de itens a serem paginados.</param>
        /// <param name="page">Número da página desejada.</param>
        /// <param name="pagesize">Quantidade de itens por página.</param>
        public PaginatedList(IEnumerable<T> items, int page, int pagesize)
        {
            TotalItems = items.Count();
            CurrentPage = page;
            PageSize = pagesize;
            TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;
            CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
            StartIndex = (CurrentPage - 1) * PageSize;
            EndIndex = Math.Min(StartIndex + PageSize - 1, TotalItems - 1);
            Items = items.Skip(StartIndex).Take(PageSize).ToList();
        }
    }
}
