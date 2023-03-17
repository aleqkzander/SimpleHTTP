using System;
using System.IO;
using System.Net;
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
            GetWelcome();
        }


        private void btn_Execute_Click(object sender, EventArgs e)
        {
            SendMessage();
            txt_Answer.Clear();
        }


        private void GetWelcome()
        {
            #region Create the uri for the get request
            string welcome = $"http://localhost/welcome";
            Uri welcomeURI = new Uri(welcome);
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


        private void SendMessage()
        {
            #region create uri for httpwebrequest
            string message = $"http://localhost/message/?usermessage={txt_Answer.Text}";
            Uri messageURI = new Uri(message);
            #endregion

            try
            {
                PostMessage(messageURI);
            }

            #region ON ERROR
            catch (Exception ex)
            {
                txt_Answer.Text += ex.Message;
            }
            #endregion ON ERROR
        }


        #region METHODS

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


        /// <summary>
        /// A method that will post a message to given uri
        /// </summary>
        /// <param name="uri"></param>
        private void PostMessage(Uri uri)
        {
            // setup post request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // execute the post request
            request.GetResponse();
        }

        #endregion METHODS
    }
}
