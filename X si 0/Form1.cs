using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace X_si_0
{
    public partial class Form1 : Form
    {
        int mutare = -1;
        int[,] mat = new int[11, 11];
        int[] line = new int[10];

        const int CMAX = 100000;

        static int[] C = new int[CMAX];
        static int[] pow3 = new int[11];
        static int[,] A = new int[4, 4];
        static bool[] viz = new bool[CMAX];

        public Form1()
        {
            InitializeComponent();
            pow3[0] = 1;
            for (int i = 1; i <= 10; i++)
                pow3[i] = pow3[i - 1] * 3;
            Back(1, 0);
        }

        static private int END_GAME()
        {
            if(A[0,0] == 1 && A[1,1] == 1 && A[2,2] == 1) return 1;
            if(A[2,0] == 1 && A[1,1] == 1 && A[0,2] == 1) return 1;
            if(A[0,0] == 2 && A[1,1] == 2 && A[2,2] == 2) return 1;
            if(A[2,0] == 2 && A[1,1] == 2 && A[0,2] == 2) return 1;
 
            for(int i = 0; i < 3; i++)
            {
                if(A[i,0] == 1 && A[i,1] == 1 && A[i,2] == 1) return 1;
                if(A[0,i] == 1 && A[1,i] == 1 && A[2,i] == 1) return 1;
                if(A[i,0] == 2 && A[i,1] == 2 && A[i,2] == 2) return 1;
                if(A[0,i] == 2 && A[1,i] == 2 && A[2,i] == 2) return 1;
            }
            return 0;
        }

        static private int min(int a, int b)
        {
            if (a < b) return a;
            return b;
        }

        static private int max(int a, int b)
        {
            if (a > b) return a;
            return b;
        }

        static private void Back(int mov, int conf)
        {
            viz[conf] = true;
 
            if(mov % 2 != 0)
                C[conf] = 1; // X pierde
            else
                C[conf] = 3; // X castiga
            if(END_GAME() != 0) return;
 
            if(mov == 10)
            {
                C[conf] = 2; // egal
                return;
            }
 
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    if(A[i,j] == 0)
                        if(mov % 2 != 0)
                        {
                            A[i,j] = 1;
                            int newconf = conf + pow3[3 * i + j];
                            if(!viz[newconf])
                                Back(mov + 1, newconf);
                            C[conf] = max(C[conf], C[newconf]);
                            A[i,j] = 0;
                        }
                        else
                        {
                            A[i,j] = 2;
                            int newconf = conf + 2 * pow3[3 * i + j];
                            if(!viz[newconf])
                                Back(mov + 1, newconf);
                            C[conf] = min(C[conf], C[newconf]);
                            A[i,j] = 0;
                        }
        }

        private static int Jucator(int mut)
        {
            if (mut % 2 == 1) return 1;
            return 2;
        }

        private void Restart()
        {
            mutare = -1;
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= 3; j++)
                    mat[i, j] = 0;
            foreach(Button b in this.Controls)
            {
                b.Text = "";
            }
        }

        private void Computer_Move()
        {
            if (mutare == 10) return;
            int configuratie = 0;
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= 3; j++)
                    if (mat[i, j] == 1)
                        configuratie += pow3[3 * (i - 1) + j - 1];
                    else if(mat[i, j] == 2)
                        configuratie += 2 * pow3[3 * (i - 1) + j - 1];

            int ii = 0, jj =  0, bestconf = 99999999;
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= 3; j++)
                    if (mat[i, j] == 0)
                    {
                        int newconf = configuratie + 2 * pow3[3 * (i - 1) + j - 1];
                        if(C[newconf] < bestconf)
                        {
                            bestconf = C[newconf];
                            ii = i; jj = j;
                        }
                    }
            mat[ii, jj] = 2;

            if (ii == 1 && jj == 1) button1.Text = "0"; else
            if (ii == 1 && jj == 2) button2.Text = "0"; else
            if (ii == 1 && jj == 3) button3.Text = "0"; else
            if (ii == 2 && jj == 1) button4.Text = "0"; else
            if (ii == 2 && jj == 2) button5.Text = "0"; else
            if (ii == 2 && jj == 3) button6.Text = "0"; else
            if (ii == 3 && jj == 1) button7.Text = "0"; else
            if (ii == 3 && jj == 2) button8.Text = "0"; else
            if (ii == 3 && jj == 3) button9.Text = "0";
        }

        private void checkGame()
        {
            Computer_Move();

            line[1] = mat[1, 1] * mat[1, 2] * mat[1, 3];
            line[2] = mat[2, 1] * mat[2, 2] * mat[2, 3];
            line[3] = mat[3, 1] * mat[3, 2] * mat[3, 3];
            line[4] = mat[1, 1] * mat[2, 1] * mat[3, 1];
            line[5] = mat[1, 2] * mat[2, 2] * mat[3, 2];
            line[6] = mat[1, 3] * mat[2, 3] * mat[3, 3];
            line[7] = mat[1, 1] * mat[2, 2] * mat[3, 3];
            line[8] = mat[3, 1] * mat[2, 2] * mat[1, 3];

            for (int i = 1; i <= 8; i++)
                if (line[i] == 1)
                { MessageBox.Show("X castiga!"); Restart();  return; }
                else if (line[i] == 8)
                { MessageBox.Show("0 castiga!"); Restart(); return; }
                else if (mutare == 9)
                { MessageBox.Show("Egalitate!"); Restart(); return; }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (mat[1, 1] == 0)
            {
                mutare += 2;
                mat[1, 1] = Jucator(mutare);
                if (mat[1, 1] == 1)
                    button1.Text = "X";
                else
                    button1.Text = "0";
                checkGame();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (mat[1, 2] == 0)
            {
                mutare += 2;
                mat[1, 2] = Jucator(mutare);
                if (mat[1, 2] == 1)
                    button2.Text = "X";
                else
                    button2.Text = "0";
                checkGame();
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (mat[1, 3] == 0)
            {
                mutare += 2;
                mat[1, 3] = Jucator(mutare);
                if (mat[1, 3] == 1)
                    button3.Text = "X";
                else
                    button3.Text = "0";

                checkGame();
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (mat[2, 1] == 0)
            {
                mutare += 2;
                mat[2, 1] = Jucator(mutare);
                if (mat[2, 1] == 1)
                    button4.Text = "X";
                else
                    button4.Text = "0";
                checkGame();
            }

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (mat[2, 2] == 0)
            {
                mutare += 2;
                mat[2, 2] = Jucator(mutare);
                if (mat[2, 2] == 1)
                    button5.Text = "X";
                else
                    button5.Text = "0";
                checkGame();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (mat[2, 3] == 0)
            {
                mutare += 2;
                mat[2, 3] = Jucator(mutare);
                if (mat[2, 3] == 1)
                    button6.Text = "X";
                else
                    button6.Text = "0";
                checkGame();
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (mat[3, 1] == 0)
            {
                mutare += 2;
                mat[3, 1] = Jucator(mutare);
                if (mat[3, 1] == 1)
                    button7.Text = "X";
                else
                    button7.Text = "0";
                checkGame();
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (mat[3, 2] == 0)
            {
                mutare += 2;
                mat[3, 2] = Jucator(mutare);
                if (mat[3, 2] == 1)
                    button8.Text = "X";
                else
                    button8.Text = "0";
                checkGame();
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            if (mat[3, 3] == 0)
            {
                mutare += 2;
                mat[3, 3] = Jucator(mutare);
                if (mat[3, 3] == 1)
                    button9.Text = "X";
                else
                    button9.Text = "0";
                checkGame();
            }
        }
    }
}
