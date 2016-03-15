using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Zinals.ErrorHandling.WPF
{
    /// <summary>
    /// Settings for an ErrorHandler Class
    /// </summary>
    public class ErrorHandlerConfiguration
    {
        /// <summary>
        /// Should the Error Handler log to a webserver
        /// </summary>
        public bool LogToWebServer { get; private set; }

        /// <summary>
        /// Should the Error Handler log to the consoles error stream
        /// </summary>
        public bool LogToConsole { get; private set; }

        /// <summary>
        /// The type of HTTP Method to use when logging
        /// <para>Default: "POST"</para>
        /// <para>Only usable if <see cref="ErrorHandlerConfiguration.LogToWebServer"/> is true</para>
        /// </summary>
        public String HTTPMethod { get; private set; }

        /// <summary>
        /// The URI of the WebServer
        /// <para>Default: null</para>
        /// <para>Only usable if <see cref="ErrorHandlerConfiguration.LogToWebServer"/> is true</para>
        /// </summary>
        public Uri HTTPAddress { get; private set; }

        /// <summary>
        /// Credentials to the WebServer, if any is needed
        /// <para>Default: null</para>
        /// <para>Only usable if <see cref="ErrorHandlerConfiguration.LogToWebServer"/> is true</para>
        /// </summary>
        public NetworkCredential Credentials { get; private set; }

        /// <summary>
        /// If the exception can be marked as handled, should the Error Handler do it
        /// <para>Default: null</para>
        /// </summary>
        public bool? SetErrorHandled { get; private set; }

        /// <summary>
        /// Creates a configuration that only logs to the console's error stream
        /// </summary>
        public ErrorHandlerConfiguration() : this(true, false, null)
        {

        }

        /// <summary>
        /// Creates a configuration that that can log to both the console's error stream and a web server
        /// </summary>
        /// <param name="LogToConsole"></param>
        /// <param name="LogToWebServer"></param>
        /// <param name="HTTPAddress">The URI of the WebServer</param>
        public ErrorHandlerConfiguration(bool LogToConsole, bool LogToWebServer, Uri HTTPAddress) : this(LogToConsole, LogToWebServer, HTTPAddress, "POST", null)
        {

        }

        /// <summary>
        /// Creates a configuration that that can log to both the console's error stream and a web server
        /// </summary>
        /// <param name="LogToConsole"></param>
        /// <param name="LogToWebServer"></param>
        /// <param name="HTTPAddress">The URI of the WebServer</param>
        /// <param name="HTTPMethod">The type of HTTP Method to use when logging</param>
        public ErrorHandlerConfiguration(bool LogToConsole, bool LogToWebServer, Uri HTTPAddress, String HTTPMethod) : this(LogToConsole, LogToWebServer, HTTPAddress, HTTPMethod, null)
        {

        }

        /// <summary>
        /// Creates a configuration that that can log to both the console's error stream and a web server
        /// </summary>
        /// <param name="LogToConsole"></param>
        /// <param name="LogToWebServer"></param>
        /// <param name="HTTPAddress">The URI of the WebServer</param>
        /// <param name="HTTPMethod">The type of HTTP Method to use when logging</param>
        /// <param name="Credentials">Credentials to the WebServer</param>
        public ErrorHandlerConfiguration(bool LogToConsole, bool LogToWebServer, Uri HTTPAddress, String HTTPMethod, NetworkCredential Credentials) : this(LogToConsole, LogToWebServer, HTTPAddress, HTTPMethod, Credentials, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogToConsole"></param>
        /// <param name="LogToWebServer"></param>
        /// <param name="HTTPAddress">The URI of the WebServer</param>
        /// <param name="HTTPMethod">The type of HTTP Method to use when logging</param>
        /// <param name="Credentials">Credentials to the WebServer</param>
        /// <param name="SetErrorHandled">If the exception can be marked as handled, should the Error Handler do it</param>
        public ErrorHandlerConfiguration(bool LogToConsole, bool LogToWebServer, Uri HTTPAddress, String HTTPMethod, NetworkCredential Credentials, bool? SetErrorHandled)
        {
            this.LogToConsole = LogToConsole;
            this.LogToWebServer = LogToWebServer;
            this.HTTPAddress = HTTPAddress;
            this.HTTPMethod = HTTPMethod;
            this.Credentials = Credentials;
            this.SetErrorHandled = SetErrorHandled;
        }
    }
}
