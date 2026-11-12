using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Integrador_Final
{
    internal class mazo
    {
        private List<Personaje> personajes;

        public mazo(List<Personaje> personajes) 
        {
            this.personajes = new List<Personaje>(personajes);
        }

        public void Mezclar()
        {
            int n = this.personajes.Count;
            while (n > 1)
            {
                n--;
                // Genera un índice aleatorio entre 0 y n (inclusive)
                int k = Random.Shared.Next(n + 1);

                // Intercambia los elementos usando tuplas de C#
                (this.personajes[k], this.personajes[n]) = (this.personajes[n], this.personajes[k]);
            }
        }

        public Personaje[] Repartir(int cantidad) 
        { 
            return this.personajes
                .Take(cantidad)
                .ToArray();
        }

        public void Devolver(Personaje[] personajes)
        {
            this.personajes.AddRange(personajes);
        }

        public int GetCantidad() => personajes.Count;
    }
}
