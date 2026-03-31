--------------------------------------------------------
--  Fichier créé - mardi-mars-24-2026   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Table TPF_HISTO_UTILISATEUR
--------------------------------------------------------

  CREATE TABLE "TPF_HISTO_UTILISATEUR" 
   (	"ID_UTILISATEUR" VARCHAR2(100 BYTE), 
	"CHOIXPRN" VARCHAR2(10 BYTE), 
	"URL_Banque1" VARCHAR2(300 BYTE), 
	"URL_Banque2" VARCHAR2(300 BYTE), 
	"AnneeDebut" NUMBER(4,0), 
	"TrimDebut" NUMBER(2,0), 
	"IndexTitreTab" NUMBER(3,0), 
	"ModeAffichage" NUMBER(1,0)
   ) ;
REM INSERTING into TPF_HISTO_UTILISATEUR
SET DEFINE OFF;
Insert into TPF_HISTO_UTILISATEUR (ID_UTILISATEUR,CHOIXPRN,"URL_Banque1","URL_Banque2","AnneeDebut","TrimDebut","IndexTitreTab","ModeAffichage") values ('WFUSER123','PREVQUE','S:\PREV\seq.wf1',null,'2022','2','19','0');
Insert into TPF_HISTO_UTILISATEUR (ID_UTILISATEUR,CHOIXPRN,"URL_Banque1","URL_Banque2","AnneeDebut","TrimDebut","IndexTitreTab","ModeAffichage") values ('WFUSER0002','COMPCAN','S:\Prev\jancf3.wf1','S:\Prev\sc5f.wf1','2025','0','0','1');
Insert into TPF_HISTO_UTILISATEUR (ID_UTILISATEUR,CHOIXPRN,"URL_Banque1","URL_Banque2","AnneeDebut","TrimDebut","IndexTitreTab","ModeAffichage") values ('WFUSER0002','PREVQUE','S:\Prev\jancf3.wf1',null,'2013','3','0','2');
--------------------------------------------------------

  CREATE UNIQUE INDEX "PK_HISTO_UTILISATEUR" ON "TPF_HISTO_UTILISATEUR" ("ID_UTILISATEUR", "CHOIXPRN") 
   ;
--------------------------------------------------------
--  Constraints for Table TPF_HISTO_UTILISATEUR
--------------------------------------------------------

  ALTER TABLE "TPF_HISTO_UTILISATEUR" MODIFY ("ID_UTILISATEUR" NOT NULL ENABLE);
  ALTER TABLE "TPF_HISTO_UTILISATEUR" MODIFY ("CHOIXPRN" NOT NULL ENABLE);
  ALTER TABLE "TPF_HISTO_UTILISATEUR" ADD CONSTRAINT "PK_HISTO_UTILISATEUR" PRIMARY KEY ("ID_UTILISATEUR", "CHOIXPRN")
  USING INDEX "PK_HISTO_UTILISATEUR"  ENABLE;

--------------------------------------------------------
--  End of DDL for Table TPF_HISTO_UTILISATEUR
--------------------------------------------------------

COMMIT;
SELECT * FROM TPF_HISTO_UTILISATEUR;