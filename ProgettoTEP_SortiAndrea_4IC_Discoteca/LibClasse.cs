using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgettoTEP_SortiAndrea_4IC_Discoteca
{
    public class Discoteca
    {
        private int CapacitàMax;
        private int Persone;
        private SemaphoreSlim Semaforo;
        private object lockObj = new object();  // Lock per sincronizzare l'accesso al bagno
        private bool Bloccato; // Stato del blocco


        public Discoteca(int Capacità)
        {
            CapacitàMax = Capacità;
            Persone = 0;
            Semaforo = new SemaphoreSlim(Capacità, Capacità); // Limitare l'ingresso alla capacità
            Bloccato = false; // Gli ingressi non sono bloccati inizialmente
        }


        public bool TryEnter()
        {
            if (Semaforo.Wait(0)) // Non blocca se non c'è posto
            {
                lock (lockObj)
                {
                    if (Bloccato)
                    {
                        Semaforo.Release(); // Se è bloccato, restituisce il posto e non entra
                        return false;
                    }
                    Persone++;
                }
                return true;
            }
            return false; // Se non c'è posto, non entra
        }


        public void Leave()
        {
            lock (lockObj)
            {
                if (Persone > 0)
                {
                    Persone--;
                    Semaforo.Release(); // Libera un posto
                }
                else
                {
                    Console.WriteLine("ERRORE! La discoteca è vuota.");
                }
            }
        }


        //Ottengo lo stato, ovvero quante persone ci sono e quante ne possono entrare
        public string GetStatus()
        {
            return $"Persone attuali: {Persone}/{CapacitàMax}";
        }


        public int CalcolaPrezzoLista(string SessoPersona_Lista)
        {
            if (SessoPersona_Lista == "Uomo")
            {
                return 20; // Prezzo per gli uomini
            }
            if (SessoPersona_Lista == "Donna")
            {
                return 15; // Prezzo per le donne
            }
            else
            {
                return 0; // Prezzo non valido se il sesso è diverso
            }
        }


        public int CalcolaPrezzoTavolo(string SessoPersona_Tavolo)
        {
            if (SessoPersona_Tavolo == "Uomo")
            {
                return 100; // Prezzo per gli uomini
            }
            if (SessoPersona_Tavolo == "Donna")
            {
                return 80; // Prezzo per le donne
            }
            else
            {
                return 0; // Prezzo non valido se il sesso è diverso
            }
        }


        public void Blocca()
        {
            lock (lockObj)
            {
                Bloccato = true;
            }
        }


        public void Sblocca()
        {
            lock (lockObj)
            {
                Bloccato = false;
            }
        }
    }
}
