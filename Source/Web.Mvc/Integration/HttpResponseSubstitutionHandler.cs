using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.System;
using ReusableLibrary.Web.Integration;

namespace ReusableLibrary.Web.Mvc.Integration
{
    public delegate string ParameterizedHttpResponseSubstitutionCallback(HttpContextBase context, object state);

    public sealed class HttpResponseSubstitutionHandler : IHttpHandler, IHttpFilterIgnore
    {
        private static readonly string g_prefix = Guid.NewGuid().Shrink();

        private static readonly List<Pair<string, ParameterizedHttpResponseSubstitutionCallback>> g_callbacks
            = new List<Pair<string, ParameterizedHttpResponseSubstitutionCallback>>();

        private static readonly IDictionary<string, ParameterizedHttpResponseSubstitutionCallback> g_callbackMap
            = new Dictionary<string, ParameterizedHttpResponseSubstitutionCallback>();

        private readonly RequestContext m_requestContext;

        public HttpResponseSubstitutionHandler(RequestContext requestContext)
        {
            m_requestContext = requestContext;
        }

        public object State { get; set; }

        public ParameterizedHttpResponseSubstitutionCallback StartCallback { get; set; }

        public ParameterizedHttpResponseSubstitutionCallback EndCallback { get; set; }

        public static void Add(string name, ParameterizedHttpResponseSubstitutionCallback callback)
        {
            g_callbacks.Add(
                new Pair<string, ParameterizedHttpResponseSubstitutionCallback>(
                    g_prefix + name, callback));
            g_callbackMap.Add(g_prefix + name, callback);
        }

        public static string Token(string name)
        {
            return g_prefix + name;
        }

        public static string Execute(HttpContextBase context, string name, object state)
        {
            return g_callbackMap[g_prefix + name](context, state);
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            if (StartCallback != null)
            {
                AddSubstitution(response, StartCallback);
            }

            SplitCallbacks(response, RenderAction(context), g_callbacks.Count - 1);

            if (EndCallback != null)
            {
                AddSubstitution(response, EndCallback);
            }
        }

        #endregion

        private void AddSubstitution(HttpResponse response, ParameterizedHttpResponseSubstitutionCallback callback)
        {
            response.WriteSubstitution(c => callback(new HttpContextWrapper(c), State));
        }

        private void SplitCallbacks(HttpResponse response, string text, int index)
        {
            if (index < 0)
            {
                response.Write(text);
                return;
            }

            var pair = g_callbacks[index];
            var chunks = text.Split(new[] { pair.First }, StringSplitOptions.None);
            SplitCallbacks(response, chunks[0], index - 1);
            for (int i = 1; i < chunks.Length; i++)
            {
                AddSubstitution(response, pair.Second);
                SplitCallbacks(response, chunks[i], index - 1);
            }
        }

        private string RenderAction(HttpContext context)
        {
            var textWriter = new StringWriter(CultureInfo.CurrentCulture);
            try
            {
                using (var wrapper = new ServerExecuteHttpHandlerWrapper(new MvcHandler(m_requestContext)))
                {
                    context.Server.Execute(wrapper, textWriter, true);
                }
            }
            catch (HttpException hex)
            {
                // The HttpException here is wrapped twice. 
                // 1. ServerExecuteHttpHandlerWrapper
                // 2. HttpServerUtility
                // Unfortunately HttpServerUtility doesn't supply inner exception in case http error
                // code is other than 500, this is why here is a workaround (see 
                // ServerExecuteHttpHandlerWrapper as well).
                Exception ex = hex;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                hex = ex as HttpException;
                var httpCode = (hex != null) ? hex.GetHttpCode() : 500;
                throw new HttpException(httpCode, Properties.Resources.ServerExecuteHttpHandlerWrapper_ProcessRequestError, ex);
            }

            return textWriter.ToString();
        }

        internal class ServerExecuteHttpHandlerWrapper : Page
        {
            private readonly IHttpHandler m_httpHandler;

            public ServerExecuteHttpHandlerWrapper(IHttpHandler httpHandler)
            {
                m_httpHandler = httpHandler;
            }

            public override void ProcessRequest(HttpContext context)
            {
                try
                {
                    m_httpHandler.ProcessRequest(context);
                }
                catch (HttpException hex)
                {
                    if (hex.GetHttpCode() == 500)
                    {
                        throw;
                    }

                    throw new HttpException(500, Properties.Resources.ServerExecuteHttpHandlerWrapper_ProcessRequestError, hex);
                }
            }
        }
    }
}
