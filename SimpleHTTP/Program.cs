using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeUser();
        }

        #region DEFINITION OF METHODS

        /// <summary>
        /// Say hello to the user
        /// </summary>
        static void WelcomeUser()
        {
            HttpListener listener = null;
            try
            {
                // Create listener
                listener = new HttpListener();

                // Set address prefix (User has to enter to call the message)
                listener.Prefixes.Add("http://localhost:1300/welcome/");

                // Start the listener
                listener.Start();

                while (true) // Execute only when listener is active
                {
                    Console.Write("- Service started and waiting for connection...\n");

                    // Call GetContext on listener starts waiting for the request
                    HttpListenerContext context = listener.GetContext();

                    // Send message when requested
                    string welcomeMsg = "Welcome to my application!";

                    // (Optional) Calculate the lenght of the replay
                    context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(welcomeMsg);

                    // (Optional) Get response status
                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    // Refer to the response property output and get stream from it
                    using (Stream stream = context.Response.OutputStream)
                    {
                        // Use stream and connect it to write object
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            // When writer is filled print message to client (Browser)
                            writer.Write(welcomeMsg);
                        }
                    }

                    // Information for server
                    Console.WriteLine("- Welcome message was sent!\n\n");
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }


        #endregion
    }
}
