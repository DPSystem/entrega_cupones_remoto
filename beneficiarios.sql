/*
   miércoles, 3 de enero de 201812:23:20
   User: 
   Server: DROP\DPS_17
   Database: sindicato
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_beneficiarios
	(
	Familiar nvarchar(255) NULL,
	Tipo_Doc nvarchar(255) NULL,
	Nro_Doc_Fliar float(53) NULL,
	Parentesco nvarchar(255) NULL,
	CUIL_Titul nvarchar(255) NULL,
	dni_titular nvarchar(10) NULL,
	Titular nvarchar(255) NULL,
	Domicilio_Titul nvarchar(255) NULL,
	Tel_Titular nvarchar(255) NULL,
	Empresa nvarchar(255) NULL,
	Prov_Loc_Titul nvarchar(255) NULL,
	Edad_Titul float(53) NULL,
	Edad_Fliar float(53) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_beneficiarios SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.beneficiarios)
	 EXEC('INSERT INTO dbo.Tmp_beneficiarios (Familiar, Tipo_Doc, Nro_Doc_Fliar, Parentesco, CUIL_Titul, Titular, Domicilio_Titul, Tel_Titular, Empresa, Prov_Loc_Titul, Edad_Titul, Edad_Fliar)
		SELECT Familiar, Tipo_Doc, Nro_Doc_Fliar, Parentesco, CUIL_Titul, Titular, Domicilio_Titul, Tel_Titular, Empresa, Prov_Loc_Titul, Edad_Titul, Edad_Fliar FROM dbo.beneficiarios WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.beneficiarios
GO
EXECUTE sp_rename N'dbo.Tmp_beneficiarios', N'beneficiarios', 'OBJECT' 
GO
COMMIT
