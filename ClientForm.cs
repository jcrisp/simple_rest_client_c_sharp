using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Net;

namespace RestClient
{
	public partial class ClientForm : Form
	{
		void MakeRequest()
		{
			ResponseTextBox.Text = "Please wait...";

			var url = UrlTextBox.Text;
			var method = VerbComboBox.Text;
			var requestBody = RequestBodyTextBox.Text;
			string reponseAsString = "";

			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = method;
				SetBody(request, requestBody);

				var response = (HttpWebResponse)request.GetResponse();

				reponseAsString = ConvertResponseToString(response);
			}
			catch (Exception ex)
			{
				reponseAsString += "ERROR: " + ex.Message;
			}

			ResponseTextBox.Text = reponseAsString;
		}

		void SetBody(HttpWebRequest request, string requestBody)
		{
			if (requestBody.Length > 0)
			{
				using (Stream requestStream = request.GetRequestStream())
				using (StreamWriter writer = new StreamWriter(requestStream))
				{
					writer.Write(requestBody);
				}
			}
		}

		string ConvertResponseToString(HttpWebResponse response)
		{
			string result = "Status code: " + (int)response.StatusCode + " " + response.StatusCode + "\r\n";

			foreach (string key in response.Headers.Keys)
			{
				result += string.Format("{0}: {1} \r\n", key, response.Headers[key]);
			}

			result += "\r\n";
			result += new StreamReader(response.GetResponseStream()).ReadToEnd();

			return result;
		}

		public ClientForm()
		{
			InitializeComponent();
		}

		void ClientForm_Load(object sender, EventArgs e)
		{
			VerbComboBox.SelectedIndex = 0;
			UrlTextBox.Text = "http://localhost/";
		}

		void ExecuteButton_Click(object sender, EventArgs e)
		{
			MakeRequest();
		}

		void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				MakeRequest();
				e.Handled = true;
			}
		}
	}
}
