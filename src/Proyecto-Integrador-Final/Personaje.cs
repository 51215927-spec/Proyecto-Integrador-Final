using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBases;

namespace Proyecto_Integrador_Final
{
    internal enum TipoAtributo
    {
        Fuerza,
        Velocidad,
        Resistencia,
        Inteligencia,
        Energia,
        Habilidad
    }

    internal class Personaje : IEntidad<Personaje>
    {
        private string nombre;
        private string ascii;

        private int fuerza;
        private int velocidad;
        private int resistencia;
        private int inteligencia;
        private int energia;
        private int habilidad;

        public Personaje(
            string nombre,
            string ascii,
            int fuerza,
            int velocidad,
            int resistencia,
            int inteligencia,
            int energia,
            int habilidad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (ascii == null)
                throw new ArgumentException("El ASCII no puede ser nulo.");

            this.nombre = nombre;
            this.ascii = ascii;

            this.fuerza = fuerza;
            this.velocidad = velocidad;
            this.resistencia = resistencia;
            this.inteligencia = inteligencia;
            this.energia = energia;
            this.habilidad = habilidad;

            Validar();
        }

        private void Validar()
        {
            int suma = fuerza + velocidad + resistencia
                     + inteligencia + energia + habilidad;

            if (fuerza < 250 || fuerza > 750 ||
                velocidad < 250 || velocidad > 750 ||
                resistencia < 250 || resistencia > 750 ||
                inteligencia < 250 || inteligencia > 750 ||
                energia < 250 || energia > 750 ||
                habilidad < 250 || habilidad > 750)
            {
                throw new ArgumentException(
                    "Todos los atributos deben estar entre 250 y 750.");
            }

            if (suma != 3000)
            {
                throw new ArgumentException(
                    "La suma de los atributos debe ser exactamente 3000.");
            }

            if (fuerza < 650 && velocidad < 650 &&
                resistencia < 650 && inteligencia < 650 &&
                energia < 650 && habilidad < 650)
            {
                throw new ArgumentException(
                    "Al menos un atributo debe ser mayor o igual a 650.");
            }

            if (fuerza > 350 && velocidad > 350 &&
                resistencia > 350 && inteligencia > 350 &&
                energia > 350 && habilidad > 350)
            {
                throw new ArgumentException(
                    "Al menos un atributo debe ser menor o igual a 350.");
            }
        }

        public string GetNombre() => nombre;

        public string GetAscii() => ascii;

        public int GetAtributo(TipoAtributo tipo)
        {
            return tipo switch
            {
                TipoAtributo.Fuerza => fuerza,
                TipoAtributo.Velocidad => velocidad,
                TipoAtributo.Resistencia => resistencia,
                TipoAtributo.Inteligencia => inteligencia,
                TipoAtributo.Energia => energia,
                TipoAtributo.Habilidad => habilidad,

                _ => throw new ArgumentOutOfRangeException(nameof(tipo))
            };
        }

        public static void Configurar(ConstructorEntidad<Personaje> entidad)
        {
            // Pendiente: configuración mediante la API de DataBases.
        }
    }
}

