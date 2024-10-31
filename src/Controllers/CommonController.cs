using AdminShell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Opc.Ua.Server;
using System;
using UaRestGateway.Server.Model;

namespace UaRestGateway.Server.Controllers
{
    public class CommonController : ControllerBase
    {
        private readonly static object m_lock = new();

        protected ILogger _logger { get; private set; }

        private readonly UANodesetViewer _viewer;

        static protected StandardServer _server;

        public CommonController(ILogger logger, UANodesetViewer viewer)
        {
            _viewer = viewer;
            _logger = logger;
        }

        protected Opc.Ua.Client.Session GetSessionContext(HttpContext context, string accessToken = null)
        {
            lock (m_lock)
            {
                _server = _viewer.StartServer(context.Session.Id);

                var endpoints = _server.GetEndpoints();
                if (endpoints == null || endpoints.Count == 0)
                {
                    var status = _server.GetStatus();
                    throw new ApiResponseException(ErrorCodes.ServerNotRunning, $"{ErrorCodes.ServerNotRunning}. State={status.State}");
                }

                return _viewer.GetSession(context.Session.Id).GetAwaiter().GetResult();
            }
        }
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(Exception e, string defaultCode = null)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            Failed = true;

            if (e is ApiResponseException are)
            {
                ErrorCode = are.ErrorCode;
                ErrorText = are.ErrorText;
            }
            else
            {
                ErrorCode = (String.IsNullOrEmpty(defaultCode)) ? ErrorCodes.UnexpectedError : defaultCode;
                ErrorText = $"[{e.GetType().Name}] {e.Message}";
            }
        }

        public ApiResponse(T result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            Result = result;
        }


        public bool Failed { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorText { get; set; }

        public T Result { get; set; }

        public override string ToString()
        {
            if (Failed)
            {
                return $"[{ErrorCode}] '{ErrorText}'";
            }

            return $"{Result}";
        }
    }

    public class ApiResponseException : ApplicationException
    {
        public ApiResponseException(string code, string text) : base(text)
        {
            ErrorCode = code;
        }

        public ApiResponseException(string code, string text, Exception e) : base(text, e)
        {
            ErrorCode = code;
        }

        public string ErrorCode { get; }

        public string ErrorText { get { return base.Message; } }
    }
}