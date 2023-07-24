using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Pokemon
        {
            public string Nombre { get; set; }
            public List<string> Habilidades { get; set; }
            public string UrlImagen { get; set; }
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int numeroPokemon))
            {
                label1.Text = "Debes ingresar solo numeros";
                return;
            }

            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{numeroPokemon}";

            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    HttpResponseMessage respuesta = await cliente.GetAsync(apiUrl);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        string contenido = await respuesta.Content.ReadAsStringAsync();

                        JObject pokemonJson = JObject.Parse(contenido);
                        Pokemon pokemon = new Pokemon
                        {
                            Nombre = (string)pokemonJson["name"],
                            Habilidades = pokemonJson["abilities"].Select(a => (string)a["ability"]["name"]).ToList(), 
                            UrlImagen = (string)pokemonJson["sprites"]["front_default"]
                        };

                        textBox2.Text = $"{pokemon.Nombre}";
                        textBox3.Text = $"Habilidades: {string.Join(", ", pokemon.Habilidades)}";
                        

                        pictureBox1.ImageLocation = pokemon.UrlImagen;
                        label1.Text = "";
                    }
                    else
                    {
                        label1.Text = "No se encontró el Pokémon.";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        
                        pictureBox1.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    label1.Text = "Ocurrió un error al conectar con la API: " + ex.Message;
                    textBox2.Text = "";
                    textBox3.Text = "";
                    
                    pictureBox1.Image = null;
                }
            }
        }

       
    }
}
