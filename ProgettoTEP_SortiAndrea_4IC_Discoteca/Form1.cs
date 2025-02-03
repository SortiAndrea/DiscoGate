using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgettoTEP_SortiAndrea_4IC_Discoteca
{
    public partial class Form1 : Form
    {
        private Discoteca IngressoLista;
        private Discoteca IngressoTavolo;

        public Form1()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            IngressoLista = new Discoteca(200); // Capacità 200 per la lista
            IngressoTavolo = new Discoteca(100); // Capacità 100 per il tavolo

            #region AUDIO BACKGROUND

            string path = "C:/Users/andre/source/repos/19-ZARA_Outside_Club.wav"; // Percorso del file audio
            NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(path); // Legge il file audio
            NAudio.Wave.DirectSoundOut output = new NAudio.Wave.DirectSoundOut(); // Inizializza l'output audio
            output.Init(new NAudio.Wave.WaveChannel32(wave)); // Inizializza il canale audio
            output.Play();

            output.PlaybackStopped += (sender, e) =>
            {
                output.Play();
            };

            #endregion

        }

        //------------------------------------------------------------------------------------------------------------------------

        private void btnAggiungi_Lista_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNome_Lista.Text))
            {
                MessageBox.Show("E' OBBLIGATORIO inserire un nome per la lista!");
                return;
            }

            if (rbLista_Uomo.Checked == false && rbLista_Donna.Checked == false)
            {
                MessageBox.Show("E' OBBLIGATORIO scegliere il sesso della persona che desidera entrare in lista");
                return;
            }

            var Thread_Lista = new Thread(() =>
            {
                string Persona_Lista = tbNome_Lista.Text;
                string SessoPersona_Lista = rbLista_Uomo.Checked ? "Uomo" : "Donna";
                bool Entrato = IngressoLista.TryEnter();

                if (Entrato)
                {
                    Invoke(new Action(() =>
                    {
                        lbIngresso_Lista.Items.Add($"{Persona_Lista} è entrato/a in lista. - " + "Il prezzo è di: " + IngressoLista.CalcolaPrezzoLista(SessoPersona_Lista) + " euro. - " + IngressoLista.GetStatus());
                    }));

                    tbNome_Lista.Clear();
                    rbLista_Uomo.Checked = false; // Deseleziona il RadioButton e pulisce la textbox per un nuovo inserimento
                    rbLista_Donna.Checked = false;

                    //int tempoSoggiorno = 10000;
                    //Task.Delay(tempoSoggiorno).Wait(); // Aspetta 10 secondi

                    Random r = new Random();
                    int tempoMaxLista = r.Next(1000, 10001); // Aspetta tra 1 e 10 secondi
                    Thread.Sleep(tempoMaxLista);

                    IngressoLista.Leave();

                    Invoke(new Action(() =>
                    {
                        lbIngresso_Lista.Items.Add($"Una persona è uscita dalla discoteca. - " + IngressoLista.GetStatus());
                    }));
                }
                else
                {
                    MessageBox.Show("Ingresso in lista pieno, attendere l'uscita di una persona!");
                }
            });

            Thread_Lista.Start();
        }

        //------------------------------------------------------------------------------------------------------------------------

        private void btnAggiungi_Tavolo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbNome_Tavolo.Text))
            {
                MessageBox.Show("E' OBBLIGATORIO inserire un nome per il tavolo!");
                return;
            }

            if (rbTavolo_Uomo.Checked == false && rbTavolo_Donna.Checked == false)
            {
                MessageBox.Show("E' OBBLIGATORIO scegliere il sesso della persona che desidera entrare al tavolo");
                return;
            }

            var Thread_Tavolo = new Thread(() =>
            {
                string Persona_Tavolo = tbNome_Tavolo.Text;
                string SessoPersona_Tavolo = rbTavolo_Uomo.Checked ? "Uomo" : "Donna";
                bool Entrato = IngressoTavolo.TryEnter();

                if (Entrato)
                {

                    Invoke(new Action(() =>
                    {
                        lbIngresso_Tavolo.Items.Add($"{Persona_Tavolo} è entrato/a al tavolo. - " + "Il prezzo è di: " + IngressoTavolo.CalcolaPrezzoTavolo(SessoPersona_Tavolo) + " euro. - " + IngressoTavolo.GetStatus());
                    }));

                    tbNome_Tavolo.Clear();
                    rbTavolo_Uomo.Checked = false; // Deseleziona il RadioButton e pulisce la textbox per un nuovo inserimento
                    rbTavolo_Donna.Checked = false;

                    //int tempoSoggiorno = 20000;
                    //Task.Delay(tempoSoggiorno).Wait(); // Aspetta 20 secondi

                    Random r = new Random();
                    int tempoMaxTavolo = r.Next(10000, 20001); // Aspetta tra 10 e 20 secondi
                    Thread.Sleep(tempoMaxTavolo);

                    IngressoTavolo.Leave();

                    Invoke(new Action(() =>
                    {
                        lbIngresso_Tavolo.Items.Add($"Una persona è uscita dalla discoteca. - " + IngressoTavolo.GetStatus());
                    }));
                }
                else
                {
                    MessageBox.Show("Ingresso ai tavoli pieno, attendere l'uscita di una persona!");
                }
            });

            Thread_Tavolo.Start();
        }

        //------------------------------------------------------------------------------------------------------------------------

        private void btnBloccaAccesso_Click(object sender, EventArgs e)
        {
            IngressoLista.Blocca();
            IngressoTavolo.Blocca();
            MessageBox.Show("L'ingresso in discoteca è stato bloccato. Le persone non possono entrare, ma possono uscire.");
        }

        //------------------------------------------------------------------------------------------------------------------------

        private void btnSbloccaAccesso_Click(object sender, EventArgs e)
        {
            IngressoLista.Sblocca();
            IngressoTavolo.Sblocca();
            MessageBox.Show("L'ingresso in discoteca è stato sbloccato. Le persone possono entrare di nuovo.");
        }
    }
}
