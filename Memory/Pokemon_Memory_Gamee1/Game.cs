using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Resources;
using System.IO;
using System.Globalization;
using System.Collections;

namespace Pokemon_Memory_Gamee
{
    public partial class Game : Form
    {
        const int amountCards = 20, intervalCards = 860;
        string player, text = "movements: ";
        int clicks, foundedCards, tag;
        bool checkPair, canPlay, turn;
        Image verse = Properties.Resources.verse;
        Random rdn = new Random();

        int[] movs = new int[3];
        int[] tagCards = new int[2];
        Image[] imgs = new Image[amountCards];
        SoundPlayer[] sounds = new SoundPlayer[amountCards];

        List<int> XLocation = new List<int>();
        List<int> YLocation = new List<int>();
        List<string> LocationRegister = new List<string>();

        public Game()
        {
            turn = true;
            InitializeComponent();
            GetSounds();
            
        }

        private void StartGame() {

            movs[0] = 0;
            player = turn ? Form1.player1 : Form1.player2;
            lblText.Text = player + text;
            clicks = 0;
            foundedCards = 0;
            checkPair = false;
            canPlay = false;
            LocationRegister.Clear();
            Timer.Enabled = false;
            Timer.Interval = intervalCards;
            TurnCards.Enabled = false;
            Random();
        
        }

        private void GetSounds() {

            var valueIndex = 0;

            ResourceSet rec = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, false, true);

            foreach (DictionaryEntry get in rec)
            {
                object type = get.Value;

                if (type is UnmanagedMemoryStream)
                {

                    sounds[valueIndex] = new SoundPlayer((Stream)type);
                    valueIndex++;

                }
            }

            StartCards();
            StartGame();
            
        }

        private void StartCards() {

            foreach (var pic in this.Controls.OfType<PictureBox>()) {

                tag = int.Parse(String.Format("{0}", pic.Tag));
                imgs[tag] = pic.Image;
                

                var XP = pic.Location.X;
                var YP = pic.Location.Y;

                if (!XLocation.Contains(XP)) { XLocation.Add(XP); }
                if (!YLocation.Contains(YP)) { YLocation.Add(YP); }
            }
        
        }

        private void Random() {

            foreach (var pic in this.Controls.OfType<PictureBox>()) {

                var foundedPlace = false;

                while (!foundedPlace) {

                    var X = XLocation[rdn.Next(0, XLocation.Count)];
                    var Y = YLocation[rdn.Next(0, YLocation.Count)];
                    var currentLocation = X.ToString() + Y.ToString();

                    if (!LocationRegister.Contains(currentLocation))
                    {

                        pic.Location = new Point(X, Y);
                        LocationRegister.Add(currentLocation);
                        foundedPlace = true;
                    }
                
                }
            
            }

            TurnCards.Enabled = true;

        }

        private void TurnDownCards() {

            TurnCards.Enabled = false;

            foreach (var pic in this.Controls.OfType<PictureBox>()) {

                pic.Image = verse;
                pic.Enabled = true;
            
            }

            canPlay = true;

        }

        private void Click(object sender, EventArgs e) {
        
            var pic = (PictureBox)sender;

            if (canPlay) {

                tag = int.Parse(String.Format("{0}", pic.Tag));
                pic.Image = imgs[tag];
                sounds[tag].Play();
                pic.Enabled = false;
                clicks++;

                if (clicks == 1) { tagCards[0] = tag; } else if (clicks == 2) {

                    canPlay = false;
                    tagCards[1] = tag;
                    movs[0]++;
                    lblText.Text = player + text + movs[0].ToString();
                    checkPair = tagCards[0] == tagCards[1];
                    Timer.Enabled = true;

                }
            
            }
        }

        private void VerseCard() {

            foreach (var pic in this.Controls.OfType<PictureBox>()) {

            tag = int.Parse(String.Format("{0}", pic.Tag));

            if (tag == tagCards[0] || tag == tagCards[1]) {

                if (checkPair) { foundedCards++; }
                else
                {

                    pic.Image = verse;
                    pic.Enabled = true;
                }
            
            }
            
            }

            clicks = 0;
            Timer.Enabled = false;
            canPlay = true;
            EndGame();
        
        }

        private void EndGame() {
        
            if (foundedCards == (amountCards*2)){

                MessageBox.Show("Congratulations " + player + " You've got " + movs[0].ToString() + " movements!!");

                if (turn) {

                    movs[1] = movs[0];
                    turn = !turn;
                    StartGame();
                
                } else {


                    movs[2] = movs[0];

                    var winner = movs[1] < movs[2] ? Form1.player1 : Form1.player2;
                    MessageBox.Show("The winner is " + winner);
                    var msg = Form1.player1 + movs[1].ToString() + " movements \n " + Form1.player2 + movs[2].ToString() + " movements";
                    MessageBox.Show(msg);
                    Application.Exit();

                }
            }

        
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            VerseCard();
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void TurnCards_Tick(object sender, EventArgs e)
        {
            TurnDownCards();
        }
    }
}
