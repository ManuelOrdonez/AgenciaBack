namespace AgenciaDeEmpleoVirutal.Business.Referentials
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Entities.Referentials;
    using Utils.ResponseMessages;


    /// <summary>
    /// base class to business
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BusinessBase<T> where T : class, new()
    {
        /// <summary>
        /// The response business
        /// </summary>
        protected Response<T> ResponseBusiness;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessBase{T}"/> class.
        /// </summary>
        public BusinessBase()
        {
            ResponseBusiness = new Response<T>
            {
                CodeResponse = 0,
                TransactionMade = false,
                Message = new List<string>(),
                Data = new List<T>()
            };
        }

        /// <summary>
        /// Responses the success.
        /// </summary>
        /// <returns></returns>
        public Response<T> ResponseSuccess(Entities.Responses.UsersDataResponse response)
        {
            ResponseBusiness.TransactionMade = true;
            ResponseBusiness.CodeResponse = (int) ServiceResponseCode.Success;
            ResponseBusiness.Message.Add(ResponseMessageHelper.GetParameter(ServiceResponseCode.Success));
            return ResponseBusiness;
        }

        /// <summary>
        /// Responses the success.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Response<T> ResponseSuccess(ServiceResponseCode code)
        {
            ResponseBusiness.TransactionMade = true;
            ResponseBusiness.CodeResponse = (int) code;
            ResponseBusiness.Message.Add(ResponseMessageHelper.GetParameter(code));
            return ResponseBusiness;
        }

        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <returns></returns>
        public Response<T> ResponseFail()
        {
            ResponseBusiness.TransactionMade = false;
            ResponseBusiness.CodeResponse = (int) ServiceResponseCode.InternalError;
            ResponseBusiness.Message.Add(ResponseMessageHelper.GetParameter(ServiceResponseCode.InternalError));
            return ResponseBusiness;
        }

        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Response<T> ResponseFail(ServiceResponseCode code)
        {
            ResponseBusiness.TransactionMade = false;
            ResponseBusiness.CodeResponse = (int) code;
            ResponseBusiness.Message.Add(ResponseMessageHelper.GetParameter(code));
            return ResponseBusiness;
        }

        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="messages">The message.</param>
        /// <returns></returns>
        public Response<T> ResponseFail(ServiceResponseCode code, IList<string> messages)
        {
            ResponseBusiness.TransactionMade = false;
            ResponseBusiness.CodeResponse = (int) code;
            ResponseBusiness.Message = messages;
            return ResponseBusiness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static Response<TEntity> ResponseSuccess<TEntity>(IList<TEntity> entity) where TEntity : class, new()
        {
            return new Response<TEntity>
            {
                CodeResponse = (int) ServiceResponseCode.Success,
                TransactionMade = true,
                Message = new List<string> {ResponseMessageHelper.GetParameter(ServiceResponseCode.Success)},
                Data = entity
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="list String"></typeparam>
        /// <returns></returns>
        public static Response<List<string>> ResponseSuccessList(List<List<string>> any)
        {
            return new Response<List<string>>
            {
                CodeResponse = (int)ServiceResponseCode.Success,
                TransactionMade = true,
                Message = new List<string> { ResponseMessageHelper.GetParameter(ServiceResponseCode.Success) },
                Data = any
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static Response<TEntity> ResponseFail<TEntity>() where TEntity : class, new()
        {
            return new Response<TEntity>
            {
                CodeResponse = (int) ServiceResponseCode.InternalError,
                TransactionMade = false,
                Message = new List<string>
                {
                    ResponseMessageHelper.GetParameter(ServiceResponseCode.InternalError)
                }
            };
        }

        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static Response<TEntity> ResponseFail<TEntity>(IList<string> messages) where TEntity : class, new()
        {
            var delimiter = Environment.NewLine;
            return new Response<TEntity>
            {
                CodeResponse = (int)ServiceResponseCode.InternalError,
                TransactionMade = false,
                Message = new List<string>
                {
                    messages.Aggregate((i,j) => string.Concat(i,delimiter,j))
                }
            };
        }


        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <param name="messages">The message.</param>
        /// <returns></returns>
        public Response<TEntity> ResponseBadRequest<TEntity>(IList<string> messages) where TEntity : class, new()
        {
            var delimiter = Environment.NewLine;
            return new Response<TEntity>
            {
                CodeResponse = (int)ServiceResponseCode.BadRequest,
                TransactionMade = false,
                Message = new List<string>
                {
                    string.Format(ResponseMessageHelper.GetParameter(ServiceResponseCode.BadRequest), delimiter, messages.Aggregate((i,j) => string.Concat(i,delimiter,j)))
                }
            };
        }

        /// <summary>
        /// Responses the fail.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public Response<TEntity> ResponseFail<TEntity>(ServiceResponseCode code) where TEntity : class, new()
        {
            return new Response<TEntity>
            {
                CodeResponse = (int)code,
                TransactionMade = false,
                Message = new List<string>
                {
                    ResponseMessageHelper.GetParameter(code)
                }
            };
        }
    }
}
