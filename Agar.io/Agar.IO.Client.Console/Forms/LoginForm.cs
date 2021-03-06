using Agar.IO.Client.WinForms.Controllers;
using System;
using System.Windows.Forms;

namespace Agar.IO.Client.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        LoginController LoginController { get; set; }
        public LoginForm()
        {
            InitializeComponent();
            LoginController = new LoginController(this);
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            infoLabel.Text = "Logging in ...";
            LoginController.StartGame();
        }
    }
}
