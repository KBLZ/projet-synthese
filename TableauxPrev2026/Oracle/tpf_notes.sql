--------------------------------------------------------
--  Fichier créé - mardi-mars-24-2026   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Table TPF_NOTE
--------------------------------------------------------

CREATE TABLE "TPF_NOTE" 
   (	"IDNOTE" NUMBER, 
	"TEXTENOTE" VARCHAR2(150 BYTE)
   )  ;
REM INSERTING into TPF_NOTE
SET DEFINE OFF;
Insert into TPF_NOTE (IDNOTE,TEXTENOTE) values ('1','1 : Variation en NIVEAU');
Insert into TPF_NOTE (IDNOTE,TEXTENOTE) values ('2','1 : Avant 1991: T1 taxes de vente aux manufacturiers (TVF). Après taxe sur les produits et services (TPS).');
--------------------------------------------------------
--  DDL for Index SYS_C0026242
--------------------------------------------------------

  CREATE UNIQUE INDEX "SYS_C0026242" ON "TPF_NOTE" ("IDNOTE") 
  ;
--------------------------------------------------------
--  Constraints for Table TPF_NOTE
--------------------------------------------------------

  ALTER TABLE "TPF_NOTE" MODIFY ("IDNOTE" NOT NULL ENABLE);
  ALTER TABLE "TPF_NOTE" MODIFY ("TEXTENOTE" NOT NULL ENABLE);
  ALTER TABLE "TPF_NOTE" ADD PRIMARY KEY ("IDNOTE")
  ;
COMMIT;
SELECT * FROM TPF_NOTE;