using BattleshipForms.Properties;
using System;
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
        bool ValidarEspacio(int x, int y)
        {
            bool Valido = true;
            if (mapa[x, y] != 0) Valido = false;
            if (mapa[(x == 0 ? 1 : x) - 1, (y == 5 ? 4 : y) + 1] != 0) Valido = false;
            if (mapa[x, (y == 5 ? 4 : y) + 1] != 0) Valido = false;
            if (mapa[(x == 5 ? 4 : x) + 1, (y == 5 ? 4 : y) + 1] != 0) Valido = false;
            if (mapa[(x == 0 ? 1 : x) - 1, y] != 0) Valido = false;
            if (mapa[(x == 5 ? 4 : x) + 1, y] != 0) Valido = false;
            if (mapa[(x == 0 ? 1 : x) - 1, (y == 0 ? 1 : y) - 1] != 0) Valido = false;
            if (mapa[x, (y == 0 ? 1 : y) - 1] != 0) Valido = false;
            if (mapa[(x == 5 ? 4 : x) + 1, (y == 0 ? 1 : y) - 1] != 0) Valido = false;
            return Valido;
        }
        bool ValidarEspacioBarco(int x, int y)
        {
            bool Valido = ValidarEspacio(x, y);
            if (x == 5) x--;
            else x++;
            if (y == 5) y--;
            else y++;
            return Valido && ValidarEspacio(x, y);
        }
        void IniciarPartida()
        {
            mapa = new int[6, 6];
            Random random = new Random();
            puntuacion = 100;
            label2.Text = puntuacion.ToString();
            BarcosGolpeados = 0;
            int x, y, orientation;
            bool LugarDisponible;
            for (int i = 0; i < 3; i++)
            {
                LugarDisponible = false;
                while (!LugarDisponible)
                {
                    x = random.Next(0, 6);
                    y = random.Next(0, 6);
                    orientation = random.Next(0, 2);
                    if (ValidarEspacioBarco(x, y))
                    {
                        LugarDisponible = true;
                        if (orientation == 0)
                        {
                            mapa[x, y] = 30;
                            if (x == 5) mapa[x - 1, y] = 30;
                            else mapa[x + 1, y] = 30;

                        }
                        else
                        {
                            mapa[x, y] = 30;
                            if (y == 5) mapa[x, y - 1] = 30;
                            else mapa[x, y + 1] = 30;
                        }
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                LugarDisponible = false;
                while (!LugarDisponible)
                {
                    x = random.Next(0, 6);
                    y = random.Next(0, 6);
                    if (ValidarEspacio(x, y))
                    {
                        LugarDisponible = true;
                        mapa[x, y] = 50;
                    }
                }
            }
            LugarDisponible = false;
            while (!LugarDisponible)
            {
                x = random.Next(0, 6);
                y = random.Next(0, 6);
                if (ValidarEspacio(x, y))
                {
                    LugarDisponible = true;
                    mapa[x, y] = -1;
                }
            }
            for (x = 0; x < 6; x++)
            {
                for (y = 0; y < 6; y++)
                {
                    if (mapa[x, y] == 0) mapa[x, y] = -20;
                    var button = (Button)this.Controls[$"btn{x}{y}"];
                    button.Image = Resources.question;
                    button.Enabled = true;
                }
            }

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            b.Enabled = false;
            string xy = b.Name.Substring(3);
            int x = int.Parse(xy) / 10;
            int y = int.Parse(xy) - (x * 10);
            int aux = mapa[x, y];
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
