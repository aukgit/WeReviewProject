CREATE PROCEDURE [dbo].[CleanUpWithRoles]
	
AS
	SET NOCOUNT ON;
	DELETE FROM   dbo.AspNetUserClaims;
	DELETE FROM   dbo.AspNetUserLogins;
	DELETE FROM   dbo.AspNetUserRoles;
	DELETE FROM   dbo.AspNetUsers;
	DBCC checkident ('AspNetUsers', reseed, 0);

RETURN 0

PM> Enable-Migrations -ContextTypeName ApplicationDbContext -MigrationsDirectory "Models\Migrations\Indentity"
PM> Enable-Migrations -ContextTypeName DevIdentityDbContext -MigrationsDirectory "Models\Migrations\DevIdentity"

Update-Database -Force -ConfigurationTypeName WereViewApp.Models.Migrations.DevIdentity.Configuration
Update-Database -Force -ConfigurationTypeName WereViewApp.Models.Migrations.Indentity.Configuration


Bootstrap Datetime picker:
https://github.com/Eonasdan/bootstrap-datetimepicker

self refence
http://blogs.microsoft.co.il/gilf/2011/06/03/how-to-configure-a-self-referencing-entity-in-code-first/


List of constraints

List SQL
SELECT * 
FROM sys.indexes 
WHERE type_desc LIKE '%non%'

DROP

ALTER TABLE dbo.App DROP CONSTRAINT URLUnique;


ADdd
ALTER TABLE dbo.App
  ADD CONSTRAINT URLUnique UNIQUE (PlatformID, PlatformVersion, URL,CategoryID);

    $.ajax({
                        type: "POST",
                        dataType: "JSON",
                        url: $.WereViewApp.writeReviewFormUrl,
						data: formData,
                        success: function (response) {

                        },
                        error: function (xhr, status, error) {

                        }
                    }); // ajax end

// refresh rotate
http://jsfiddle.net/chadkuehn/1twpzLz8/

<h4>FontAwesome</h4>

<i class="fa fa-spinner fa-spin"></i>

<i class="fa fa-circle-o-notch fa-spin"></i>

<i class="fa fa-refresh fa-spin"></i>

<i class="fa fa-refresh fa-spin-custom"></i>

<br />
<br />

<h4>Glyphicons</h4>
 <span class="glyphicon glyphicon-refresh glyphicon-spin"></span>
