using System;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace Zinals.ErrorHandling.WPF
{
    public class ErrorHandler : IDisposable
    {

        public ErrorHandlerConfiguration Configuration { get; private set; }

        private Application BoundApp = null;
        private AppDomain BoundDomain = null;

        public ErrorHandler() : this(new ErrorHandlerConfiguration())
        {

        }

        public ErrorHandler(ErrorHandlerConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        /// <summary>
        /// Bind this Error Handler to an <see cref="Application"/> to detect Unhandled Exceptions
        /// </summary>
        /// <param name="App"></param>
        /// <returns></returns>
        public bool BindApplication(Application App)
        {
            if (this.BoundApp != null)
                return false;

            this.BoundApp = App;
            App.DispatcherUnhandledException += DispatcherUnhandledException;
            App.Dispatcher.UnhandledException += DispatcherUnhandledException;
            return true;
        }

        /// <summary>
        /// Unbind the previously set <see cref="Application"/>
        /// </summary>
        public void UnBindApplication()
        {
            if(this.BoundApp != null)
            {
                this.BoundApp.DispatcherUnhandledException -= DispatcherUnhandledException;
                this.BoundApp.Dispatcher.UnhandledException -= DispatcherUnhandledException;
                this.BoundApp = null;
            }
        }

        /// <summary>
        /// Bind this Error Handler to an <see cref="AppDomain"/> to detect Unhandled Exceptions
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public bool BindDomain(AppDomain Domain)
        {
            if (this.BoundDomain != null)
                return false;

            this.BoundDomain = Domain;
            Domain.FirstChanceException += FirstChanceException;
            Domain.UnhandledException += UnhandledException;
            return true;
        }

        /// <summary>
        /// Unbind the previously set <see cref="AppDomain"/>
        /// </summary>
        public void UnBindDomain()
        {
            if(this.BoundDomain != null)
            {
                this.BoundDomain.FirstChanceException -= FirstChanceException;
                this.BoundDomain.UnhandledException -= UnhandledException;
                this.BoundDomain = null;
            }
        }

        /// <summary>
        /// Log an exception to the console and/or a Web server
        /// </summary>
        /// <param name="ex">The exception that occurred</param>
        public void LogError(Exception ex)
        {
            bool? Handled;
            this.LogError(ex, out Handled);
        }

        /// <summary>
        /// Log an exception to the console and/or a Web server
        /// </summary>
        /// <param name="ex">The exception that occurred</param>
        /// <param name="handled"></param>
        public void LogError(Exception ex, out bool? handled)
        {
            handled = null;

            if (this.Configuration.LogToConsole)
                Console.Error.WriteLine(ex.ToString());

            if(this.Configuration.LogToWebServer)
            {
                try
                {
                    HttpWebRequest Req = (HttpWebRequest)HttpWebRequest.Create(this.Configuration.HTTPAddress);

                    if (this.Configuration.HTTPMethod != null)
                        Req.Method = this.Configuration.HTTPMethod;

                    if (this.Configuration.Credentials != null)
                        Req.Credentials = this.Configuration.Credentials;

                    String JSON = JsonConvert.SerializeObject(ex);

                    using (StreamWriter sw = new StreamWriter(Req.GetRequestStream()))
                    {
                        sw.Write(JSON);
                    }

                    HttpWebResponse Res = (HttpWebResponse)Req.GetResponse();

                    if (Res.StatusCode != HttpStatusCode.OK && this.Configuration.LogToConsole)
                    {
                        ProtocolViolationException pvex = new ProtocolViolationException("ErrorHandler WebServer returned ivalid StatusCode");
                        pvex.Data.Add("Status Code", Res.StatusCode);
                        Console.Error.WriteLine(pvex.ToString());
                    }
                    Res.Close();
                }
                catch (Exception iex)
                {
                    if (this.Configuration.LogToConsole)
                        Console.Error.WriteLine(iex.ToString());
                }
            }

            if (this.Configuration.SetErrorHandled.HasValue)
                handled = this.Configuration.SetErrorHandled;
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            bool? Handled;
            LogError(e.ExceptionObject as Exception, out Handled);
        }

        private void FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            bool? Handled;
            LogError(e.Exception, out Handled);
        }

        private void DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            bool? Handled;
            LogError(e.Exception, out Handled);
            if (Handled.HasValue)
                e.Handled = Handled.Value;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool x)
        {
            this.UnBindApplication();
            this.UnBindDomain();

            if (x)
                GC.SuppressFinalize(this);
        }
    }
}
