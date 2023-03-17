using System;
using System.IO;
using System.Net;
using System.Threading;

namespace SimpleHTTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Thread(() => 
            {
                WelcomeUser();
            }).Start();

            new Thread(() =>
            {
                GetMessage();
            }).Start();
        }

        #region METHODS


        /// <summary>
        /// Say hello to the user and fill the body with the message
        /// </summary>
        static void WelcomeUser()
        {
            try
            {
                #region Create and start the listener
                // Create listener
                HttpListener listener = new HttpListener();

                // Set address prefix (User has to enter to call the message)
                listener.Prefixes.Add("http://localhost/welcome/");

                // Start the listener
                listener.Start();

                // Log
                Console.WriteLine("Welcome started...");
                #endregion Create and start the listener

                while (true) // Execute only when listener is active
                {
                    // Call GetContext on listener starts waiting for the request
                    HttpListenerContext context = listener.GetContext();

                    Console.WriteLine("++ Welcome requested...");

                    #region Write a message to the request body
                    // Refer to the response property output and get stream from it
                    using (Stream stream = context.Response.OutputStream)
                    {
                        // Use stream and connect it to write object
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            // When writer is filled print message to client (Browser)
                            writer.Write("Welcome to my application \n\n" +
                                "This is a dream!");
                        }
                    }
                    #endregion Write a message to the request body
                }
            }

            #region ON ERROR
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            #endregion ON ERROR
        }


        /// <summary>
        /// Receive a message from the user
        /// </summary>
        static void GetMessage()
        {
            #region Create and start the listener
            // Create listener
            HttpListener GetMessage = new HttpListener();

            // Set prefix
            string prefix = "http://localhost/message/";

            // Add prefixx
            GetMessage.Prefixes.Add(prefix);

            // Start listener
            GetMessage.Start();

            Console.WriteLine("Get message started...");
            #endregion Create and start the listener

            while (true)
            {
                // Getcontext call will block a thread
                HttpListenerContext context = GetMessage.GetContext();

                try
                {
                    // Log the request on the server side
                    Console.WriteLine("++ Get message requested...");

                    // Set the query string from request
                    var qry = context.Request.QueryString;

                    /*
                     * Those keys where send by the POST request
                     * 1.Key=message
                     * */

                    // Build userdata
                    string message = qry[0];

                    // write to console
                    Console.WriteLine("\n" + message);

                    // When finished close response
                    context.Response.Close();
                }

                #region ON ERROR
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    // Also close when error else application will be blocked
                    context.Response.Close();
                }
                #endregion ON ERROR
            }
        }


        #endregion METHODS
    }
}
