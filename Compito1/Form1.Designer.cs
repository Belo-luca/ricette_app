using System.Drawing;
using System.Windows.Forms;

namespace Compito1
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Configurazione Form Principale
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Text = "Gestione Ricette Elettrodomestici";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Pulsante Salva Globale
            this.btnSalvaTutto = new Button { Text = "Salva su file", Location = new Point(10, 10), Width = 110 };
            this.Controls.Add(this.btnSalvaTutto);

            // TabControl Principale
            this.tabs = new TabControl { Location = new Point(10, 45), Size = new Size(965, 585) };
            this.tabElettro = new TabPage("Elettrodomestici");
            this.tabRicette = new TabPage("Ricette");
            this.tabFiltri = new TabPage("Filtri e Statistiche");

            this.tabs.TabPages.Add(this.tabElettro);
            this.tabs.TabPages.Add(this.tabRicette);
            this.tabs.TabPages.Add(this.tabFiltri);
            this.Controls.Add(this.tabs);

            // Costruzione Schede
            this.CostruisciTabElettrodomestici();
            this.CostruisciTabRicette();
            this.CostruisciTabFiltri();
        }

        private void CostruisciTabElettrodomestici()
        {
            this.dgvElettro = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(560, 520),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.dgvElettro.Columns.Add("Codice", "Codice");
            this.dgvElettro.Columns.Add("Nome", "Nome modello");
            this.dgvElettro.Columns.Add("Descrizione", "Descrizione");
            this.dgvElettro.Columns.Add("Prezzo", "Prezzo (EUR)");
            this.tabElettro.Controls.Add(this.dgvElettro);

            int x = 590, y = 10;
            this.txtEleCodice = AggiungiCampoTesto(this.tabElettro, "Codice:", x, ref y);
            this.txtEleNome = AggiungiCampoTesto(this.tabElettro, "Nome modello:", x, ref y);
            this.txtEleDescr = AggiungiCampoTesto(this.tabElettro, "Descrizione:", x, ref y);
            this.numElePrezzo = AggiungiCampoDecimale(this.tabElettro, "Prezzo (EUR):", x, ref y);

            y += 15;
            this.btnEleNuovo = new Button { Text = "Nuovo", Location = new Point(x, y), Width = 85 };
            this.btnEleSalva = new Button { Text = "Salva", Location = new Point(x + 95, y), Width = 85 };
            this.btnEleElimina = new Button { Text = "Elimina", Location = new Point(x + 190, y), Width = 85 };

            this.tabElettro.Controls.Add(this.btnEleNuovo);
            this.tabElettro.Controls.Add(this.btnEleSalva);
            this.tabElettro.Controls.Add(this.btnEleElimina);
        }

        private void CostruisciTabRicette()
        {
            this.dgvRicette = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(430, 520),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.dgvRicette.Columns.Add("Nome", "Nome");
            this.dgvRicette.Columns.Add("Tipo", "Tipo");
            this.dgvRicette.Columns.Add("Tempo", "Tempo (min)");
            this.dgvRicette.Columns.Add("Costo", "Costo tot.");
            this.tabRicette.Controls.Add(this.dgvRicette);

            int x = 460, y = 10;
            this.txtRicNome = AggiungiCampoTesto(this.tabRicette, "Nome:", x, ref y);
            this.txtRicDescr = AggiungiCampoTesto(this.tabRicette, "Descrizione:", x, ref y);
            this.numRicTempo = AggiungiCampoIntero(this.tabRicette, "Tempo cottura (min):", x, ref y);
            this.numRicCalorie = AggiungiCampoIntero(this.tabRicette, "Calorie/porzione:", x, ref y);
            this.numRicCosto = AggiungiCampoDecimale(this.tabRicette, "Costo ingredienti (EUR):", x, ref y);

            this.tabRicette.Controls.Add(new Label { Text = "Elettrodomestico:", Location = new Point(x, y), Width = 150 });
            this.cmbRicElettro1 = new ComboBox { Location = new Point(x + 155, y), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            this.tabRicette.Controls.Add(this.cmbRicElettro1);
            y += 30;

            this.chkSpeciale = new CheckBox { Text = "Ricetta speciale", Location = new Point(x, y), Width = 150 };
            this.tabRicette.Controls.Add(this.chkSpeciale);
            y += 30;

            this.tabRicette.Controls.Add(new Label { Text = "2° Elettrodomestico:", Location = new Point(x, y), Width = 150 });
            this.cmbRicElettro2 = new ComboBox { Location = new Point(x + 155, y), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
            this.tabRicette.Controls.Add(this.cmbRicElettro2);
            y += 30;

            this.tabRicette.Controls.Add(new Label { Text = "Vino abbinato:", Location = new Point(x, y), Width = 150 });
            this.txtRicVino = new TextBox { Location = new Point(x + 155, y), Width = 220, Enabled = false };
            this.tabRicette.Controls.Add(this.txtRicVino);
            y += 35;

            this.tabRicette.Controls.Add(new Label { Text = "Immagini:", Location = new Point(x, y), Width = 150 });
            y += 20;
            this.lstImmagini = new ListBox { Location = new Point(x, y), Width = 375, Height = 70 };
            this.tabRicette.Controls.Add(this.lstImmagini);
            y += 80;

            this.btnRicAggiungiImg = new Button { Text = "Aggiungi immagine...", Location = new Point(x, y), Width = 150 };
            this.btnRicRimuoviImg = new Button { Text = "Rimuovi selezionata", Location = new Point(x + 160, y), Width = 150 };
            this.tabRicette.Controls.Add(this.btnRicAggiungiImg);
            this.tabRicette.Controls.Add(this.btnRicRimuoviImg);

            y += 40;
            this.btnRicNuovo = new Button { Text = "Nuovo", Location = new Point(x, y), Width = 90 };
            this.btnRicSalva = new Button { Text = "Salva", Location = new Point(x + 100, y), Width = 90 };
            this.btnRicElimina = new Button { Text = "Elimina", Location = new Point(x + 200, y), Width = 90 };
            this.tabRicette.Controls.Add(this.btnRicNuovo);
            this.tabRicette.Controls.Add(this.btnRicSalva);
            this.tabRicette.Controls.Add(this.btnRicElimina);
        }

        private void CostruisciTabFiltri()
        {
            var grp1 = new GroupBox { Text = "Ricerca per intervallo tempo di cottura", Location = new Point(10, 10), Size = new Size(930, 260) };
            this.tabFiltri.Controls.Add(grp1);

            grp1.Controls.Add(new Label { Text = "Da (min):", Location = new Point(15, 30), Width = 60 });
            this.numTempoMin = new NumericUpDown { Location = new Point(80, 28), Width = 70, Maximum = 1000 };
            grp1.Controls.Add(this.numTempoMin);

            grp1.Controls.Add(new Label { Text = "A (min):", Location = new Point(165, 30), Width = 60 });
            this.numTempoMax = new NumericUpDown { Location = new Point(225, 28), Width = 70, Maximum = 1000, Value = 60 };
            grp1.Controls.Add(this.numTempoMax);

            this.radOrdNome = new RadioButton { Text = "Ordina per nome", Location = new Point(320, 28), Width = 150, Checked = true };
            grp1.Controls.Add(this.radOrdNome);
            this.radOrdCosto = new RadioButton { Text = "Ordina per costo", Location = new Point(320, 52), Width = 150 };
            grp1.Controls.Add(this.radOrdCosto);

            this.btnFiltroCerca = new Button { Text = "Cerca", Location = new Point(480, 28), Width = 90 };
            grp1.Controls.Add(this.btnFiltroCerca);

            this.dgvRisultati = new DataGridView
            {
                Location = new Point(15, 90),
                Size = new Size(895, 155),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.dgvRisultati.Columns.Add("Nome", "Nome ricetta");
            this.dgvRisultati.Columns.Add("Descrizione", "Descrizione");
            this.dgvRisultati.Columns.Add("Costo", "Costo complessivo");
            this.dgvRisultati.Columns.Add("Elettro1", "Elettrodomestico 1");
            this.dgvRisultati.Columns.Add("Elettro2", "Elettrodomestico 2");
            grp1.Controls.Add(this.dgvRisultati);

            var grp2 = new GroupBox { Text = "Eliminazione per soglia calorie", Location = new Point(10, 280), Size = new Size(450, 100) };
            this.tabFiltri.Controls.Add(grp2);
            grp2.Controls.Add(new Label { Text = "Elimina ricette con calorie >", Location = new Point(15, 30), Width = 170 });
            this.numSogliaCalorie = new NumericUpDown { Location = new Point(190, 28), Width = 90, Maximum = 10000 };
            grp2.Controls.Add(this.numSogliaCalorie);
            this.btnFiltroEliminaCalorie = new Button { Text = "Elimina", Location = new Point(300, 26), Width = 90 };
            grp2.Controls.Add(this.btnFiltroEliminaCalorie);

            var grp3 = new GroupBox { Text = "Costo medio ricette per vino", Location = new Point(480, 280), Size = new Size(460, 100) };
            this.tabFiltri.Controls.Add(grp3);
            grp3.Controls.Add(new Label { Text = "Nome vino:", Location = new Point(15, 30), Width = 80 });
            this.txtVinoRicerca = new TextBox { Location = new Point(100, 28), Width = 150 };
            grp3.Controls.Add(this.txtVinoRicerca);
            this.btnFiltroCalcolaVino = new Button { Text = "Calcola", Location = new Point(260, 26), Width = 90 };
            grp3.Controls.Add(this.btnFiltroCalcolaVino);
            this.lblCostoMedio = new Label { Location = new Point(15, 65), Width = 420, Text = "" };
            grp3.Controls.Add(this.lblCostoMedio);
        }

        private TextBox AggiungiCampoTesto(TabPage tab, string etichetta, int x, ref int y)
        {
            tab.Controls.Add(new Label { Text = etichetta, Location = new Point(x, y), Width = 150 });
            var txt = new TextBox { Location = new Point(x + 155, y), Width = 220 };
            tab.Controls.Add(txt);
            y += 30;
            return txt;
        }

        private NumericUpDown AggiungiCampoIntero(TabPage tab, string etichetta, int x, ref int y)
        {
            tab.Controls.Add(new Label { Text = etichetta, Location = new Point(x, y), Width = 150 });
            var num = new NumericUpDown { Location = new Point(x + 155, y), Width = 100, Maximum = 10000 };
            tab.Controls.Add(num);
            y += 30;
            return num;
        }

        private NumericUpDown AggiungiCampoDecimale(TabPage tab, string etichetta, int x, ref int y)
        {
            var num = AggiungiCampoIntero(tab, etichetta, x, ref y);
            num.Maximum = 100000;
            num.DecimalPlaces = 2;
            return num;
        }

        #endregion

        #region Dichiarazione dei Componenti

        private Button btnSalvaTutto;
        private TabControl tabs;

        // Tab Elettrodomestici
        private TabPage tabElettro;
        private DataGridView dgvElettro;
        private TextBox txtEleCodice, txtEleNome, txtEleDescr;
        private NumericUpDown numElePrezzo;
        private Button btnEleNuovo, btnEleSalva, btnEleElimina;

        // Tab Ricette
        private TabPage tabRicette;
        private DataGridView dgvRicette;
        private TextBox txtRicNome, txtRicDescr, txtRicVino;
        private NumericUpDown numRicTempo, numRicCalorie, numRicCosto;
        private ComboBox cmbRicElettro1, cmbRicElettro2;
        private CheckBox chkSpeciale;
        private ListBox lstImmagini;
        private Button btnRicAggiungiImg, btnRicRimuoviImg, btnRicNuovo, btnRicSalva, btnRicElimina;

        // Tab Filtri e Statistiche
        private TabPage tabFiltri;
        private NumericUpDown numTempoMin, numTempoMax, numSogliaCalorie;
        private RadioButton radOrdNome, radOrdCosto;
        private DataGridView dgvRisultati;
        private TextBox txtVinoRicerca;
        private Label lblCostoMedio;
        private Button btnFiltroCerca, btnFiltroEliminaCalorie, btnFiltroCalcolaVino;

        #endregion
    }
}