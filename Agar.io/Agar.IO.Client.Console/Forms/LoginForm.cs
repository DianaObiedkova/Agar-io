using Agar.IO.Client.WinForms.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Agar.IO.Client.WinForms.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void bLogin_Click(object sender, EventArgs e)
        {
            infoLabel.Text = "Logging in ...";
            await LoginController.StartGame();
        }
    }
}
