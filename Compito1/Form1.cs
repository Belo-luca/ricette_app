using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Compito1
{
    public partial class Form1 : Form
    {
        private readonly Archivio archivio = Archivio.Carica();

        public Form1()
        {
            InitializeComponent();
            AssociaEventi();

            AggiornaGrigliaElettrodomestici();
            AggiornaComboElettrodomestici();
            AggiornaGrigliaRicette();
        }

        private void AssociaEventi()
        {
            // Eventi Generali
            btnSalvaTutto.Click += (s, e) => { archivio.Salva(); MessageBox.Show("Salvato."); };
            FormClosing += (s, e) => archivio.Salva();

            // Eventi Tab Elettrodomestici
            dgvElettro.SelectionChanged += (s, e) => SelezionaElettrodomestico();
            btnEleNuovo.Click += (s, e) => PuliziaCampiElettro();
            btnEleSalva.Click += (s, e) => SalvaElettrodomestico();
            btnEleElimina.Click += (s, e) => EliminaElettrodomestico();

            // Eventi Tab Ricette
            dgvRicette.SelectionChanged += (s, e) => SelezionaRicetta();
            chkSpeciale.CheckedChanged += (s, e) =>
            {
                cmbRicElettro2.Enabled = chkSpeciale.Checked;
                txtRicVino.Enabled = chkSpeciale.Checked;
            };
            btnRicAggiungiImg.Click += (s, e) => AggiungiImmagine();
            btnRicRimuoviImg.Click += (s, e) =>
            {
                if (lstImmagini.SelectedItem != null)
                    lstImmagini.Items.Remove(lstImmagini.SelectedItem);
            };
            btnRicNuovo.Click += (s, e) => PuliziaCampiRicetta();
            btnRicSalva.Click += (s, e) => SalvaRicetta();
            btnRicElimina.Click += (s, e) => EliminaRicetta();

            // Eventi Tab Filtri
            btnFiltroCerca.Click += (s, e) => CercaPerTempoCottura();
            btnFiltroEliminaCalorie.Click += (s, e) => EliminaPerCalorie();
            btnFiltroCalcolaVino.Click += (s, e) => CalcolaCostoMedioPerVino();
        }

        // =====================================================================
        // LOGICA ELETTRODOMESTICI
        // =====================================================================
        private void AggiornaGrigliaElettrodomestici()
        {
            dgvElettro.Rows.Clear();
            foreach (var el in archivio.Elettrodomestici)
            {
                int idx = dgvElettro.Rows.Add(el.Codice, el.NomeModello, el.Descrizione, el.Prezzo.ToString("0.00"));
                dgvElettro.Rows[idx].Tag = el;
            }
        }

        private void SelezionaElettrodomestico()
        {
            if (dgvElettro.SelectedRows.Count == 0) return;
            if (dgvElettro.SelectedRows[0].Tag is not Elettrodomestico el) return;
            txtEleCodice.Text = el.Codice;
            txtEleNome.Text = el.NomeModello;
            txtEleDescr.Text = el.Descrizione;
            numElePrezzo.Value = el.Prezzo;
        }

        private void PuliziaCampiElettro()
        {
            txtEleCodice.Text = "";
            txtEleNome.Text = "";
            txtEleDescr.Text = "";
            numElePrezzo.Value = 0;
            dgvElettro.ClearSelection();
        }

        private void SalvaElettrodomestico()
        {
            if (string.IsNullOrWhiteSpace(txtEleCodice.Text))
            {
                MessageBox.Show("Il codice è obbligatorio.");
                return;
            }

            var esistente = archivio.TrovaElettrodomestico(txtEleCodice.Text.Trim());
            if (esistente == null)
            {
                esistente = new Elettrodomestico { Codice = txtEleCodice.Text.Trim() };
                archivio.AggiungiElettrodomestico(esistente);
            }
            esistente.NomeModello = txtEleNome.Text.Trim();
            esistente.Descrizione = txtEleDescr.Text.Trim();
            esistente.Prezzo = numElePrezzo.Value;

            AggiornaGrigliaElettrodomestici();
            AggiornaComboElettrodomestici();
            AggiornaGrigliaRicette();
            PuliziaCampiElettro();
        }

        private void EliminaElettrodomestico()
        {
            if (dgvElettro.SelectedRows.Count == 0) return;
            if (dgvElettro.SelectedRows[0].Tag is not Elettrodomestico el) return;

            bool usato = archivio.Ricette.Any(r => r.Elettrodomestico == el ||
                                                   (r is RicettaSpeciale rs && rs.Elettrodomestico2 == el));
            if (usato)
            {
                MessageBox.Show("Impossibile eliminare: è usato in almeno una ricetta.");
                return;
            }
            if (MessageBox.Show("Confermi l'eliminazione?", "Conferma", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            archivio.RimuoviElettrodomestico(el);
            AggiornaGrigliaElettrodomestici();
            AggiornaComboElettrodomestici();
            PuliziaCampiElettro();
        }

        // =====================================================================
        // LOGICA RICETTE
        // =====================================================================
        private void AggiornaComboElettrodomestici()
        {
            cmbRicElettro1.DataSource = null;
            cmbRicElettro1.DataSource = archivio.Elettrodomestici.ToList();
            cmbRicElettro2.DataSource = null;
            cmbRicElettro2.DataSource = archivio.Elettrodomestici.ToList();
        }

        private void AggiornaGrigliaRicette()
        {
            dgvRicette.Rows.Clear();
            foreach (var r in archivio.Ricette)
            {
                int idx = dgvRicette.Rows.Add(r.Nome, r.TipoRicetta, r.TempoCotturaMinuti, r.CostoComplessivo().ToString("0.00"));
                dgvRicette.Rows[idx].Tag = r;
            }
        }

        private void SelezionaRicetta()
        {
            if (dgvRicette.SelectedRows.Count == 0) return;
            if (dgvRicette.SelectedRows[0].Tag is not Ricetta r) return;

            txtRicNome.Text = r.Nome;
            txtRicDescr.Text = r.Descrizione;
            numRicTempo.Value = r.TempoCotturaMinuti;
            numRicCalorie.Value = r.Calorie;
            numRicCosto.Value = r.CostoIngredienti;
            cmbRicElettro1.SelectedItem = archivio.Elettrodomestici.FirstOrDefault(e => e == r.Elettrodomestico);

            lstImmagini.Items.Clear();
            foreach (var img in r.Immagini) lstImmagini.Items.Add(img);

            chkSpeciale.Checked = r is RicettaSpeciale;
            if (r is RicettaSpeciale rs)
            {
                cmbRicElettro2.SelectedItem = archivio.Elettrodomestici.FirstOrDefault(e => e == rs.Elettrodomestico2);
                txtRicVino.Text = rs.VinoAbbinato;
            }
            else
            {
                cmbRicElettro2.SelectedItem = null;
                txtRicVino.Text = "";
            }
        }

        private void PuliziaCampiRicetta()
        {
            txtRicNome.Text = "";
            txtRicDescr.Text = "";
            numRicTempo.Value = 0;
            numRicCalorie.Value = 0;
            numRicCosto.Value = 0;
            cmbRicElettro1.SelectedItem = null;
            cmbRicElettro2.SelectedItem = null;
            chkSpeciale.Checked = false;
            txtRicVino.Text = "";
            lstImmagini.Items.Clear();
            dgvRicette.ClearSelection();
        }

        private void AggiungiImmagine()
        {
            using var ofd = new OpenFileDialog { Filter = "Immagini|*.jpg;*.jpeg;*.png;*.bmp;*.gif" };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string destinazione = Path.Combine(Archivio.CartellaImmagini, Path.GetFileName(ofd.FileName));
            File.Copy(ofd.FileName, destinazione, true);
            lstImmagini.Items.Add(destinazione);
        }

        private void SalvaRicetta()
        {
            if (string.IsNullOrWhiteSpace(txtRicNome.Text))
            {
                MessageBox.Show("Il nome della ricetta è obbligatorio.");
                return;
            }
            if (cmbRicElettro1.SelectedItem is not Elettrodomestico elettro1)
            {
                MessageBox.Show("Selezionare l'elettrodomestico principale.");
                return;
            }
            if (chkSpeciale.Checked && cmbRicElettro2.SelectedItem is not Elettrodomestico)
            {
                MessageBox.Show("Selezionare il secondo elettrodomestico per la ricetta speciale.");
                return;
            }

            string nome = txtRicNome.Text.Trim();
            var esistente = archivio.TrovaRicetta(nome);

            bool ricrea = esistente == null || (chkSpeciale.Checked != (esistente is RicettaSpeciale));

            Ricetta ricetta;
            if (ricrea)
            {
                if (esistente != null) archivio.RimuoviRicetta(esistente);
                ricetta = chkSpeciale.Checked ? new RicettaSpeciale() : new Ricetta();
                archivio.AggiungiRicetta(ricetta);
            }
            else
            {
                ricetta = esistente;
            }

            ricetta.Nome = nome;
            ricetta.Descrizione = txtRicDescr.Text.Trim();
            ricetta.TempoCotturaMinuti = (int)numRicTempo.Value;
            ricetta.Calorie = (int)numRicCalorie.Value;
            ricetta.CostoIngredienti = numRicCosto.Value;
            ricetta.Elettrodomestico = elettro1;
            ricetta.Immagini = lstImmagini.Items.Cast<string>().ToList();

            if (ricetta is RicettaSpeciale spec)
            {
                spec.Elettrodomestico2 = (Elettrodomestico)cmbRicElettro2.SelectedItem;
                spec.VinoAbbinato = txtRicVino.Text.Trim();
            }

            AggiornaGrigliaRicette();
            PuliziaCampiRicetta();
        }

        private void EliminaRicetta()
        {
            if (dgvRicette.SelectedRows.Count == 0) return;
            if (dgvRicette.SelectedRows[0].Tag is not Ricetta r) return;
            if (MessageBox.Show("Confermi l'eliminazione?", "Conferma", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            archivio.RimuoviRicetta(r);
            AggiornaGrigliaRicette();
            PuliziaCampiRicetta();
        }

        // =====================================================================
        // LOGICA FILTRI E STATISTICHE
        // =====================================================================
        private void CercaPerTempoCottura()
        {
            int min = (int)numTempoMin.Value;
            int max = (int)numTempoMax.Value;
            if (min > max)
            {
                MessageBox.Show("Il valore minimo non può essere maggiore del massimo.");
                return;
            }

            var risultati = archivio.CercaPerTempoCottura(min, max, radOrdCosto.Checked);

            dgvRisultati.Rows.Clear();
            foreach (var r in risultati)
            {
                string elettro1 = r.Elettrodomestico?.NomeModello ?? "-";
                string elettro2 = (r is RicettaSpeciale rs && rs.Elettrodomestico2 != null) ? rs.Elettrodomestico2.NomeModello : "-";
                dgvRisultati.Rows.Add(r.Nome, r.Descrizione, r.CostoComplessivo().ToString("0.00"), elettro1, elettro2);
            }

            if (dgvRisultati.Rows.Count == 0)
                MessageBox.Show("Nessuna ricetta trovata nell'intervallo indicato.");
        }

        private void EliminaPerCalorie()
        {
            int soglia = (int)numSogliaCalorie.Value;
            int numeroDaEliminare = archivio.Ricette.Count(r => r.Calorie > soglia);
            if (numeroDaEliminare == 0)
            {
                MessageBox.Show("Nessuna ricetta supera la soglia indicata.");
                return;
            }
            if (MessageBox.Show($"Verranno eliminate {numeroDaEliminare} ricette. Continuare?", "Conferma", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            archivio.EliminaRicetteSopraCalorie(soglia);
            AggiornaGrigliaRicette();
            MessageBox.Show("Eliminazione completata.");
        }

        private void CalcolaCostoMedioPerVino()
        {
            string vino = txtVinoRicerca.Text.Trim();
            if (string.IsNullOrWhiteSpace(vino))
            {
                MessageBox.Show("Inserire il nome del vino.");
                return;
            }

            var media = archivio.CostoMedioPerVino(vino);
            lblCostoMedio.Text = media == null
                ? "Nessuna ricetta trovata con questo vino."
                : $"Costo medio per '{vino}': {media:0.00} EUR";
        }
    }
}