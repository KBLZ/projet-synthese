using Microsoft.EntityFrameworkCore;
using PE_DAL.Oracle;

namespace PE_DAL.Oracle
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<DTO_Tableau> Tableaux { get; set; }
        public DbSet<DTO_Description> Descriptions { get; set; }
        public DbSet<DTO_Note> Notes { get; set; }
        public DbSet<DTO_Historique> Historiques { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DTO_Tableau>(entity =>
            {
                entity.ToTable("TPF_TABLEAUX");
                entity.HasKey(e => e.IdTableau);
                entity.Property(e => e.IdTableau).HasColumnName("IDTABLEAU");
                entity.Property(e => e.TitreTableau).HasColumnName("TITRETABLEAU");
                entity.Property(e => e.SousTitreTableau).HasColumnName("SOUSTITRETABLEAU");
            });

            modelBuilder.Entity<DTO_Description>(entity =>
            {
                entity.ToTable("TPF_DESCRIPTIONS");
                entity.HasKey(e => new { e.IdTableau, e.Position });

                entity.HasOne(d => d.Tableau)
                    .WithMany()
                    .HasForeignKey(d => d.IdTableau);

                entity.HasOne(d => d.NoteNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Note);

                entity.Property(e => e.IdTableau).HasColumnName("IDTABLEAU");
                entity.Property(e => e.Position).HasColumnName("POSITION");
                entity.Property(e => e.Niveau).HasColumnName("NIVEAU");
                entity.Property(e => e.Mnemonic).HasColumnName("MNEMONIQUE");
                entity.Property(e => e.DescriptionTexte).HasColumnName("DESCRIPTION");
                entity.Property(e => e.Ligne1Tableau).HasColumnName("LIGNE1_TAB");
                entity.Property(e => e.Ligne3NiveauSpec).HasColumnName("LIGNE3_NIV_SPEC");
                entity.Property(e => e.Ligne4PchCont).HasColumnName("LIGNE4_PCH_CONT");
                entity.Property(e => e.Variation).HasColumnName("VARIATION");
                entity.Property(e => e.Decimale).HasColumnName("DECIMALE");
                entity.Property(e => e.Note).HasColumnName("NOTE");
            });

            modelBuilder.Entity<DTO_Note>(entity =>
            {
                entity.ToTable("TPF_NOTE");
                entity.HasKey(e => e.IdNote);
                entity.Property(e => e.IdNote).HasColumnName("IDNOTE");
                entity.Property(e => e.TexteNote).HasColumnName("TEXTENOTE");
            });

            modelBuilder.Entity<DTO_Historique>(entity =>
            {
                entity.ToTable("TPF_HISTO_UTILISATEUR");
                entity.HasKey(e => new { e.IdUtilisateur, e.ChoixPRN });
                entity.Property(e => e.IdUtilisateur).HasColumnName("ID_UTILISATEUR");
                entity.Property(e => e.ChoixPRN).HasColumnName("CHOIXPRN");
                entity.Property(e => e.UrlBanque1).HasColumnName("URL_Banque1");
                entity.Property(e => e.UrlBanque2).HasColumnName("URL_Banque2");
                entity.Property(e => e.AnneeDebut).HasColumnName("AnneeDebut");
                entity.Property(e => e.TrimDebut).HasColumnName("TrimDebut");
                entity.Property(e => e.IndexTitreTab).HasColumnName("IndexTitreTab");
                entity.Property(e => e.ModeAffichage).HasColumnName("ModeAffichage");
            });
        }
    }
}
