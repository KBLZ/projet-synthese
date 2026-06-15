using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Client_App_Entity
{
    public class SerieDTO
    {
        private string m_ID;
        private string m_mnemonic;
        private string m_description;
        private string m_source;
        private string m_lastUpdate;
        private string m_firstPeriod;
        private string m_lastPeriod;
        private char m_frequency;
        private string m_bank;
        private string m_unity;
        private SortedDictionary<string, decimal> _mObservationses;

        public SerieDTO()
        {
            m_ID = "";
            m_mnemonic = "";
            m_description = "";
            m_source = "";
            m_lastUpdate = "";
            m_firstPeriod = "2100-01-01";
            m_lastPeriod = "1900-01-01";
            m_frequency = ' ';
            m_unity = "";
            m_bank = "";
            _mObservationses = new SortedDictionary<string, decimal>();
        }

        public string ID
        {
            get => m_ID;
            set => m_ID = value;
        }

        public string Mnemonic
        {
            get => m_mnemonic;
            set => m_mnemonic = value;
        }

        public string Description
        {
            get => m_description;
            set => m_description = value;
        }

        public string Source
        {
            get => m_source;
            set => m_source = value;
        }

        public string LastUpdate
        {
            get => m_lastUpdate;
            set => m_lastUpdate = value;
        }

        public string FirstPeriod
        {
            get => m_firstPeriod;
            set => m_firstPeriod = value;
        }

        public string LastPeriod
        {
            get => m_lastPeriod;
            set => m_lastUpdate = value;
        }

        public char Frequency
        {
            get => m_frequency;
            set => m_frequency = value;
        }

        public string Bank
        {
            get => m_bank;
            set => m_bank = value;
        }

        public string Unity
        {
            get => m_unity;
            set => m_unity = value;
        }

        public SortedDictionary<string, decimal> Observations
        {
            get => _mObservationses;
            set => _mObservationses = value;
        }

        /// <summary>
        /// Ajoute une observation et met à jour première/dernière période automatiquement.
        /// </summary>
        public void AddObservation(string period, decimal valeur)
        {
            _mObservationses[period] = valeur;

            if (string.Compare(period, m_firstPeriod, StringComparison.Ordinal) < 0)
                m_firstPeriod = period;
            if (string.Compare(period, m_lastPeriod, StringComparison.Ordinal) > 0)
                m_lastPeriod = period;
        }

        public Serie ToEntity()
        {
            return new Serie(
                this.ID,
                this.Mnemonic,
                this.Description,
                this.Source,
                this.LastUpdate,
                this.FirstPeriod,
                this.LastPeriod,
                this.Frequency,
                this.Bank,
                this.Unity,
                this.Observations
            );
        }

        public override string ToString() =>
            $"[{m_ID}] {m_mnemonic} | {m_description} | Fréq: {m_frequency} " +
            $"| {m_firstPeriod} → {m_lastPeriod}bs: {_mObservationses.Count}";
    }
}