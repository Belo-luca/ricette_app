using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Compito1
{
    // ---------------------------------------------------------------------
    // Elettrodomestico
    // ---------------------------------------------------------------------
    public class Elettrodomestico
    {
        public string Codice { get; set; } = "";
        public string NomeModello { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public decimal Prezzo { get; set; }

        public override string ToString() => $"{Codice} - {NomeModello}";
    }

    // ---------------------------------------------------------------------
    // Ricetta (base) e RicettaSpeciale
    // Gli attributi Json* servono solo a far capire a System.Text.Json come
    // distinguere le due classi quando salva/carica su file.
    // ---------------------------------------------------------------------
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Tipo")]
    [JsonDerivedType(typeof(Ricetta), "Base")]
    [JsonDerivedType(typeof(RicettaSpeciale), "Speciale")]
    public class Ricetta
    {
        public string Nome { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public int TempoCotturaMinuti { get; set; }
        public int Calorie { get; set; }
        public decimal CostoIngredienti { get; set; }
        public Elettrodomestico Elettrodomestico { get; set; }
        public List<string> Immagini { get; set; } = new List<string>();

        public virtual decimal CostoComplessivo() =>
            (Elettrodomestico?.Prezzo ?? 0) + CostoIngredienti;

        public virtual string TipoRicetta => "Base";
    }

    public class RicettaSpeciale : Ricetta
    {
        public Elettrodomestico Elettrodomestico2 { get; set; }
        public string VinoAbbinato { get; set; } = "";

        public override decimal CostoComplessivo() =>
            base.CostoComplessivo() + (Elettrodomestico2?.Prezzo ?? 0);

        public override string TipoRicetta => "Speciale";
    }

    // ---------------------------------------------------------------------
    // Archivio: classe contenitore, con le liste e tutto il CRUD
    // ---------------------------------------------------------------------
    public class Archivio
    {
        public List<Elettrodomestico> Elettrodomestici { get; set; } = new List<Elettrodomestico>();
        public List<Ricetta> Ricette { get; set; } = new List<Ricetta>();

        // ---- CRUD Elettrodomestici ----
        public void AggiungiElettrodomestico(Elettrodomestico e) => Elettrodomestici.Add(e);

        public void RimuoviElettrodomestico(Elettrodomestico e) => Elettrodomestici.Remove(e);

        public Elettrodomestico TrovaElettrodomestico(string codice) =>
            Elettrodomestici.FirstOrDefault(x => x.Codice == codice);

        // ---- CRUD Ricette ----
        public void AggiungiRicetta(Ricetta r) => Ricette.Add(r);

        public void RimuoviRicetta(Ricetta r) => Ricette.Remove(r);

        public Ricetta TrovaRicetta(string nome) =>
            Ricette.FirstOrDefault(r => r.Nome == nome);

        // ---- Query richieste ----
        public List<Ricetta> CercaPerTempoCottura(int min, int max, bool ordinaPerCosto) =>
            ordinaPerCosto
                ? Ricette.Where(r => r.TempoCotturaMinuti >= min && r.TempoCotturaMinuti <= max)
                         .OrderByDescending(r => r.CostoComplessivo()).ToList()
                : Ricette.Where(r => r.TempoCotturaMinuti >= min && r.TempoCotturaMinuti <= max)
                         .OrderByDescending(r => r.Nome, StringComparer.OrdinalIgnoreCase).ToList();

        public int EliminaRicetteSopraCalorie(int soglia)
        {
            var daEliminare = Ricette.Where(r => r.Calorie > soglia).ToList();
            foreach (var r in daEliminare) Ricette.Remove(r);
            return daEliminare.Count;
        }

        public decimal? CostoMedioPerVino(string vino)
        {
            var lista = Ricette.OfType<RicettaSpeciale>()
                                .Where(r => string.Equals(r.VinoAbbinato, vino, StringComparison.OrdinalIgnoreCase))
                                .ToList();
            return lista.Count == 0 ? null : lista.Average(r => r.CostoComplessivo());
        }

        // ---- Persistenza su file (unico file JSON) ----
        private static readonly string PercorsoFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "archivio.json");

        public void Salva()
        {
            var opzioni = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(PercorsoFile, JsonSerializer.Serialize(this, opzioni));
        }

        public static Archivio Carica()
        {
            if (!File.Exists(PercorsoFile)) return new Archivio();
            string json = File.ReadAllText(PercorsoFile);
            return JsonSerializer.Deserialize<Archivio>(json) ?? new Archivio();
        }

        public static string CartellaImmagini
        {
            get
            {
                string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Immagini");
                Directory.CreateDirectory(p);
                return p;
            }
        }
    }
}

