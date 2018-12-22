using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Memory_Gamee
{
    public partial class Form1 : Form
    {
        public static string player1, player2;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            player1 = txtPlayer1.Text + " ";
            player2 = txtPlayer2.Text + " ";

            if (txtPlayer1.Text == "" || txtPlayer2.Text == "")
            {

                MessageBox.Show("Insert all player's names!");
            }
            else {

                Game game = new Game();
                game.Show();
                this.Enabled = false;
                this.Visible = false;
            
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
