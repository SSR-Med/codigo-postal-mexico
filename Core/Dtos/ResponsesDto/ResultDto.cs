namespace Core.Dtos.ResponsesDto
{
	public class ResultDto
	{
		public string? Message { get; set; }

		public ResultDto() : base() { }

		protected ResultDto(string message)
		{
			Message = message;
		}

		public static ResultDto Success(string message) => new(message);
	}

	public class ResultDto<T> : ResultDto
	{
		public T? Results { get; set; }

		public ResultDto() : base() { }

		protected ResultDto(T results) : base(string.Empty)
		{
			Results = results;
		}

		protected ResultDto(string message) : base(message) { }

		public static ResultDto<T> Success(T Result) => new(Result);
	}

	public class PaginatedResultDto<T> : ResultDto<T>
	{
		public int Total { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public bool HasNext { get; set; }
		public bool HasPrev { get; set; }

		public PaginatedResultDto() : base() { }

		private PaginatedResultDto(int total, int page, int pageSize, bool hasNext, bool hasPrev, T Result) : base(Result)
		{
			Total = total;
			Page = page;
			PageSize = pageSize;
			HasNext = hasNext;
			HasPrev = hasPrev;
		}

		private PaginatedResultDto(string message) : base(message) { }

		public static PaginatedResultDto<T> Success(int total, int page, int pageSize, bool hasNext, bool hasPrev, T result) => new(total, page, pageSize, hasNext, hasPrev, result);
	}
}