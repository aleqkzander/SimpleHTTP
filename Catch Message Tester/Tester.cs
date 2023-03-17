using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Catch_Message_Tester
{
    public partial class Tester : Form
    {
        public Tester()
        {
            InitializeComponent();
        }

        private void Tester_Load(object sender, EventArgs e)
        {

        }


        private void btn_Execute_Click(object sender, EventArgs e)
        {
            txt_Answer.Clear();

            GetWelcome(txt_Request.Text);
        }


        private void GetWelcome(string domain)
        {
            #region Create the uri for the get request
            string welcomeDomain = $"http://{domain}";
            Uri welcomeURI = new Uri(welcomeDomain);
            #endregion Create the uri for the get request

            try
            {
                // Initialize a string
                string obtainedMessage = ReceiveMessage(welcomeURI);

                // set the answer text box
                txt_Answer.Text = obtainedMessage;
            }

            #region ON ERROR
            catch (Exception ex)
            {
                txt_Answer.Text += ex.Message;
            }
            #endregion ON ERROR
        }


        /// <summary>
        /// A method that will receive a message body from a request
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private string ReceiveMessage(Uri uri)
        {
            // send the get request to given URI
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // register a response from the server
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Initialize a string
            string obtainedMessage = string.Empty;

            #region Recieve the message from the response body
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                obtainedMessage = reader.ReadToEnd();
            }
            #endregion Recieve the message from the response body

            return obtainedMessage;
        }
    }
}
