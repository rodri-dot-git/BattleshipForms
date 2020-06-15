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
            // Funcion para crear el mapa y restablecer los valores
            IniciarPartida();
        }
        // Funcion para validar que el espacio este vacio y que no haya
        // ningun valor en los cuadros adyacentes
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

        // Los barcos usan dos espacios asi que hay que revisar ambos valores
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
            // reestablecemos el mapa en 0
            mapa = new int[6, 6];
            Random random = new Random();
            // puntuacion base
            puntuacion = 100;
            label2.Text = puntuacion.ToString();
            // reestablecemos
            BarcosGolpeados = 0;
            int x, y, orientation;
            bool LugarDisponible;
            // Asignamos aleatoriamente los barcos
            for (int i = 0; i < 3; i++)
            {
                LugarDisponible = false;
                while (!LugarDisponible)
                {
                    // va a buscar un ligar aleatorio que sea valido
                    x = random.Next(0, 6);
                    y = random.Next(0, 6);
                    orientation = random.Next(0, 2);
                    if (ValidarEspacioBarco(x, y))
                    {
                        LugarDisponible = true;
                        // Para ponerlos en verticalo u horizontal
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

            // Asignar las islas, igual que los barcos
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

            // Asignar la fosa marina
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

            // Poner las imagenes de "?" para "ocultar" los valores
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

        // Funcion que revisa el valor del arreglo y asigna la imagen correspondiente
        void SustituirImagen(int valor, Button b)
        {
            switch (valor)
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
        }

        // Accion que al hacer click revisa donde hiciste click
        // y asigna los valores correspondientes
        private void ButtonClick(object sender, EventArgs e)
        {
            Button b = sender as Button;
            b.Enabled = false;
            string xy = b.Name.Substring(3);
            int x = int.Parse(xy) / 10;
            int y = int.Parse(xy) - (x * 10);
            int aux = mapa[x, y];
            SustituirImagen(aux, b);
            // Si es la fosa pon un valor negativo para acabar el jeugo
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

        // Funcion para mostrar los valores del mapa
        void MostrarMapa()
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    var b = (Button)this.Controls[$"btn{x}{y}"];
                    SustituirImagen(mapa[x, y], b);
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
