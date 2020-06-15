using BattleshipForms.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipForms
{
    public partial class Form1 : Form
    {
        int[,] mapa = new int[6, 6];
        int puntuacion = 100;
        int BarcosGolpeados = 0;
        public Form1()
        {
            InitializeComponent();
            IniciarPartida();
        }

        void IniciarPartida()
        {
            mapa = new int[6, 6];
            Random random = new Random();
            Metodos m = new Metodos();
            puntuacion = 100;
            label2.Text = "100";
            BarcosGolpeados = 0;
            while (m.CantidadBarcos != 3 || m.CantidadIslas != 2 || m.CantidadFosas != 1)
            {
                int x, y, orientation;
                bool LugarDisponible = false;
                while (!LugarDisponible)
                {
                    int aux = m.ValorParaMapa();
                    if (aux == 30)
                    {
                        x = random.Next(1, 6);
                        y = random.Next(1, 6);
                        orientation = random.Next(0, 2);
                        if (mapa[x, y] == 0)
                        {
                            try
                            {
                                if (
                                mapa[x - 1, y + 1] <= 0 ||
                                mapa[x, y + 1] <= 0 ||
                                mapa[x + 1, y + 1] <= 0 ||
                                mapa[x - 1, y] <= 0 ||
                                mapa[x, y] <= 0 ||
                                mapa[x + 1, y] <= 0 ||
                                mapa[x - 1, y - 1] <= 0 ||
                                mapa[x, y - 1] <= 0 ||
                                mapa[x + 1, y - 1] <= 0
                                )
                                {
                                    if (orientation == 0)
                                    {
                                        mapa[x, y] = 30;
                                        mapa[x + 1, y] = 30;
                                    }
                                    else
                                    {
                                        mapa[x, y] = 30;
                                        mapa[x, y - 1] = 30;
                                    }
                                    LugarDisponible = true;
                                }
                            }
                            catch (Exception)
                            {
                                m.CantidadBarcos--;
                            }
                        }

                    }
                    else if (aux >= -1)
                    {
                        x = random.Next(1, 6);
                        y = random.Next(1, 6);
                        if (mapa[x, y] == 0)
                        {
                            try
                            {
                                if (
                                mapa[x - 1, y + 1] != 30 ||
                                mapa[x, y + 1] != 30 ||
                                mapa[x + 1, y + 1] != 30 ||
                                mapa[x - 1, y] != 30 ||
                                mapa[x, y] != 30 ||
                                mapa[x + 1, y] != 30 ||
                                mapa[x - 1, y - 1] != 30 ||
                                mapa[x, y - 1] != 30 ||
                                mapa[x + 1, y - 1] != 30
                                )
                                {
                                    mapa[x, y] = aux;
                                    LugarDisponible = true;
                                }
                            }
                            catch (Exception)
                            {
                                if (aux == -1) m.CantidadFosas--;
                                if (aux == 50) m.CantidadIslas--;
                            }

                        }
                    }
                }
            }
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (mapa[x, y] == 0) mapa[x, y] = -20;
                    var button = (Button)this.Controls[$"btn{x}{y}"];
                    button.Image = Resources.question;
                }
            }

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string xy = b.Name.Substring(3);
            int x = int.Parse(xy) / 10;
            int y = int.Parse(xy) - (x * 10);
            int aux = RevisaCelda(x, y);
            switch (aux)
            {
                case 30:
                    BarcosGolpeados++;
                    b.Image = Resources.battleship;
                    break;
                case 50:
                    b.Image = Resources.island;
                    break;
                case -1:
                    b.Image = Resources.mystery;
                    break;
                case -20:
                    b.Image = Resources.sea;
                    break;
                default:
                    break;
            }
            puntuacion += aux == -1 ? -1000 : aux;
            label2.Text = puntuacion.ToString();
            if (puntuacion <= 0)
            {
                MessageBox.Show("Perdiste", "Se acabo el juego");
                MostrarMapa();
            }
            if (BarcosGolpeados == 6)
            {
                MessageBox.Show("Ganaste", "Se acabo el juego");
                MostrarMapa();
            }
            b.Enabled = false;
        }

        void MostrarMapa()
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    var b = (Button)this.Controls[$"btn{x}{y}"];
                    switch (mapa[x, y])
                    {
                        case 30:
                            b.Image = Resources.battleship;
                            break;
                        case 50:
                            b.Image = Resources.island;
                            break;
                        case -1:
                            b.Image = Resources.mystery;
                            break;
                        case -20:
                            b.Image = Resources.sea;
                            break;
                        default:
                            break;
                    }
                    b.Enabled = false;
                }
            }
        }

        int RevisaCelda(int x, int y)
        {
            return mapa[x, y];
        }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IniciarPartida();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
