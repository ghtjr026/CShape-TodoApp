namespace TodoApp.Shared.Result
{
    /// <summary>
    /// UseCase 실행 결과를 성공/실패로 감싸서 반환하는 래퍼 모델.
    /// 예외를 throw하지 않고 결과값으로 전달하여 ViewModel에서 깔끔하게 분기 처리.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        protected Result(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data, true, null);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }
    }

    /// <summary>
    /// 성공 시 데이터를 함께 반환하는 제네릭 Result.
    /// </summary>
    public class Result<T> : Result
    {
        public T Data { get; }

        internal Result(T data, bool isSuccess, string errorMessage)
            : base(isSuccess, errorMessage)
        {
            Data = data;
        }
    }
}
