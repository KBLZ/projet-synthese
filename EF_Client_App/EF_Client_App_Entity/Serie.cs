namespace EF_Client_App_Entity;

public class Serie
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
    private SortedDictionary<string, decimal> m_observations;
    
    public Serie(){}

    public Serie(
        string id,
        string mnemonic,
        string description,
        string source,
        string lastUpdate,
        string firstPeriod,
        string lastPeriod,
        char frequency,
        string bank,
        string unity,
        SortedDictionary<string, decimal> observations
    )
    {
        this.m_ID = id;
        this.m_mnemonic = mnemonic;
        this.m_description = description;
        this.m_source = source;
        this.m_lastUpdate = lastUpdate;
        this.m_firstPeriod = firstPeriod;
        this.m_lastPeriod = lastPeriod;
        this.m_frequency = frequency;
        this.m_bank = bank;
        this.m_unity = unity;
        this.m_observations = observations;
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
        get => m_observations;
        set => m_observations = value;
    }
}