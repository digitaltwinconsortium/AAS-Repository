namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc.Formatters;
    using System;
    using Microsoft.Net.Http.Headers;
    using System.Threading.Tasks;

    public class RawRequestBodyFormatter : InputFormatter
    {
        public RawRequestBodyFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }

        public override Boolean CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var contentType = context.HttpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType) || contentType == "application/octet-stream")
            {
                return true;
            }

            return false;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var contentType = context.HttpContext.Request.ContentType;

            if (string.IsNullOrEmpty(contentType) || contentType == "application/octet-stream")
            {
                return await InputFormatterResult.SuccessAsync(request.Body);
            }

            return await InputFormatterResult.FailureAsync();
        }
    }
}
