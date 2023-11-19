using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public class OperationResult<T>
    {
        protected OperationResult()
        {
            this.Success = true;
        }
        protected OperationResult(string message, int? failureCode)
        {
            this.Success = false;
            this.FailureMessages.Add(message);
            FailureCode = failureCode;
        }
        protected OperationResult(IEnumerable<string> messages, int? failureCode)
        {
            this.Success = false;
            this.FailureMessages.AddRange(messages);
            FailureCode = failureCode;
        }
        protected OperationResult(Exception ex, int? failureCode)
        {
            this.Success = false;
            this.Exception = ex;
            FailureCode = failureCode;
        }
        public T Result { get; protected set; }
        public bool Success { get; protected set; }
        public int? FailureCode { get; protected set; }
        public List<string> FailureMessages { get; protected set; } = new List<string>();
        public Exception Exception { get; protected set; }
        public static OperationResult<T> SuccessResult(T result)
        {
            return new OperationResult<T>() { Result = result };
        }
        public static OperationResult<T> FailureResult(string message)
        {
            return new OperationResult<T>(message, null);
        }
        public static OperationResult<T> FailureResult(string message, int failureCode)
        {
            return new OperationResult<T>(message, failureCode);
        }
        public static OperationResult<T> FailureResult(IEnumerable<string> messages)
        {
            return new OperationResult<T>(messages, null);
        }
        public static OperationResult<T> FailureResult(IEnumerable<string> messages, int failureCode)
        {
            return new OperationResult<T>(messages, failureCode);
        }
        public static OperationResult<T> ExceptionResult(Exception ex)
        {
            return new OperationResult<T>(ex, null);
        }
        public static OperationResult<T> ExceptionResult(Exception ex, int failureCode)
        {
            return new OperationResult<T>(ex, failureCode);
        }
        public bool IsException()
        {
            return this.Exception != null;
        }
    }
}
