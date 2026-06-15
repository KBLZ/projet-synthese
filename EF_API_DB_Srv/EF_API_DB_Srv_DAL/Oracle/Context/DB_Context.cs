using EF_API_DB_Srv_DAL.Oracle.DTO;
using Microsoft.EntityFrameworkCore;
using EF_API_DB_SRV_Entities;

namespace EF_API_DB_Srv_DAL.Oracle.Context;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DBContext()
    {
    }

    public virtual DbSet<DTO_Description> Descriptions { get; set; }
    public virtual DbSet<DTO_Historic> Historics { get; set; }
    public virtual DbSet<DTO_Note> Notes { get; set; }
    public virtual DbSet<DTO_Array> Arrays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // NOTE
        modelBuilder.Entity<DTO_Note>(entity =>
        {
            entity.HasKey(e => e.NoteId);

            entity.ToTable("TPF_NOTE");

            entity.Property(e => e.NoteId).HasColumnName("IDNOTE");

            entity.Property(e => e.NoteText)
                .HasMaxLength(150)
                .HasColumnName("TEXTENOTE");
        });


        // TABLEAUX
        modelBuilder.Entity<DTO_Array>(entity =>
        {
            entity.HasKey(e => e.ArrayId);

            entity.ToTable("TPF_TABLEAUX");

            entity.Property(e => e.ArrayId).HasColumnName("IDTABLEAU");

            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("TITRETABLEAU");

            entity.Property(e => e.SubTitle)
                .HasMaxLength(150)
                .HasColumnName("SOUSTITRETABLEAU");
        });


        // DESCRIPTION
        modelBuilder.Entity<DTO_Description>(entity =>
        {
            entity.HasNoKey();

            entity.ToTable("TPF_DESCRIPTIONS");

            entity.Property(e => e.ArrayId).HasColumnName("IDTABLEAU");
            entity.Property(e => e.Position).HasColumnName("POSITION");
            entity.Property(e => e.Level).HasColumnName("NIVEAU");

            entity.Property(e => e.Mnemonic)
                .HasMaxLength(70)
                .HasColumnName("MNEMONIQUE");

            entity.Property(e => e.TextDescription)
                .HasMaxLength(200)
                .HasColumnName("DESCRIPTION");

            entity.Property(e => e.FirstLineArray)
                .HasMaxLength(30)
                .HasColumnName("LIGNE1_TAB");

            entity.Property(e => e.Line3LevelSpec)
                .HasMaxLength(30)
                .HasColumnName("LIGNE3_NIV_SPEC");

            entity.Property(e => e.Line4PchCont)
                .HasMaxLength(30)
                .HasColumnName("LIGNE4_PCH_CONT");

            entity.Property(e => e.Variation).HasColumnName("VARIATION");
            entity.Property(e => e.Decimal).HasColumnName("DECIMALE");
            entity.Property(e => e.Note).HasColumnName("NOTE");
        });


        // HISTORIQUE
        modelBuilder.Entity<DTO_Historic>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PRN_Selection });

            entity.ToTable("TPF_HISTO_UTILISATEUR");

            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .HasColumnName("ID_UTILISATEUR");

            entity.Property(e => e.PRN_Selection)
                .HasMaxLength(10)
                .HasColumnName("CHOIXPRN");

            entity.Property(e => e.UrlPool1)
                .HasMaxLength(300)
                .HasColumnName("URL_Banque1")
                .IsRequired(false);

            entity.Property(e => e.UrlPool2)
                .HasMaxLength(300)
                .HasColumnName("URL_Banque2")
                .IsRequired(false);

            entity.Property(e => e.StartedYear).HasColumnName("AnneeDebut");
            entity.Property(e => e.StartedQuarter).HasColumnName("TrimDebut");
            entity.Property(e => e.IndexTitleTab).HasColumnName("IndexTitreTab");
            entity.Property(e => e.DisplayMode).HasColumnName("ModeAffichage");
        });
    }
}