using Microsoft.EntityFrameworkCore;

namespace PE_DAL.Oracle.Context
{
    public class PE_DBContext : DbContext
    {
        public PE_DBContext(DbContextOptions<PE_DBContext> options) : base(options) { }

        public PE_DBContext() { }

        public virtual DbSet<DTO_Description> Descriptions { get; set; }
        public virtual DbSet<DTO_Historique> Historiques { get; set; }
        public virtual DbSet<DTO_Note> Notes { get; set; }
        public virtual DbSet<DTO_Tableau> Tableaux { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NOTE
            modelBuilder.Entity<DTO_Note>(entity =>
            {
                entity.HasKey(e => e.IdNote);

                entity.ToTable("TPF_NOTE");

                entity.Property(e => e.IdNote).HasColumnName("IDNOTE");

                entity.Property(e => e.TexteNote)
                      .HasMaxLength(150)
                      .HasColumnName("TEXTENOTE");
            });


            // TABLEAUX
            modelBuilder.Entity<DTO_Tableau>(entity =>
            {
                entity.HasKey(e => e.IdTableau);

                entity.ToTable("TPF_TABLEAUX");

                entity.Property(e => e.IdTableau).HasColumnName("IDTABLEAU");

                entity.Property(e => e.TitreTableau)
                      .HasMaxLength(150)
                      .HasColumnName("TITRETABLEAU");

                entity.Property(e => e.SousTitreTableau)
                      .HasMaxLength(150)
                      .HasColumnName("SOUSTITRETABLEAU");
            });


            // DESCRIPTION
            modelBuilder.Entity<DTO_Description>(entity =>
            { 
                entity.HasNoKey();

                entity.ToTable("TPF_DESCRIPTIONS");

                entity.Property(e => e.IdTableau).HasColumnName("IDTABLEAU");
                entity.Property(e => e.Position).HasColumnName("POSITION");
                entity.Property(e => e.Niveau).HasColumnName("NIVEAU");

                entity.Property(e => e.Mnemonic)
                      .HasMaxLength(70)
                      .HasColumnName("MNEMONIQUE");

                entity.Property(e => e.DescriptionTexte)
                      .HasMaxLength(200)
                      .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Ligne1Tableau)
                      .HasMaxLength(30)
                      .HasColumnName("LIGNE1_TAB");

                entity.Property(e => e.Ligne3NiveauSpec)
                      .HasMaxLength(30)
                      .HasColumnName("LIGNE3_NIV_SPEC");

                entity.Property(e => e.Ligne4PchCont)
                      .HasMaxLength(30)
                      .HasColumnName("LIGNE4_PCH_CONT");

                entity.Property(e => e.Variation).HasColumnName("VARIATION");
                entity.Property(e => e.Decimale).HasColumnName("DECIMALE");
                entity.Property(e => e.Note).HasColumnName("NOTE");
            });


            // HISTORIQUE
            modelBuilder.Entity<DTO_Historique>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TPF_HISTO_UTILISATEUR");

                entity.Property(e => e.IdUtilisateur)
                      .HasMaxLength(100)
                      .HasColumnName("ID_UTILISATEUR");

                entity.Property(e => e.ChoixPRN)
                      .HasMaxLength(10)
                      .HasColumnName("CHOIXPRN");

                entity.Property(e => e.UrlBanque1)
                      .HasMaxLength(300)
                      .HasColumnName("URL_Banque1");

                entity.Property(e => e.UrlBanque2)
                      .HasMaxLength(300)
                      .HasColumnName("URL_Banque2");

                entity.Property(e => e.AnneeDebut).HasColumnName("AnneeDebut");
                entity.Property(e => e.TrimDebut).HasColumnName("TrimDebut");
                entity.Property(e => e.IndexTitreTab).HasColumnName("IndexTitreTab");
                entity.Property(e => e.ModeAffichage).HasColumnName("ModeAffichage");
            });

                    
        }
    }
}