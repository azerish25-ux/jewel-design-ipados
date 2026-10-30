using System;
using System.Data;
using System.Data.SqlClient;

namespace DesignLibrary;

public static class LibraryDBBuilder
{
	public static int MakeLibrary(int keyProfile, string description)
	{
		using SqlConnection sqlConnection = new SqlConnection("REDACTED_EMBEDDED_CREDENTIAL");
		sqlConnection.Open();
		SqlCommand sqlCommand = new SqlCommand(" INSERT INTO VersionLibrary (VersionLibraryId,LibraryDescription,PublicRelease,KeyProfileId)\r\n                                VALUES ( (SELECT MAX(VersionLibraryId)+1 FROM VersionLibrary),@LibraryDescription,1,@KeyProfileId); \r\n                       SELECT MAX(VersionLibraryId) as newVersion FROM VersionLibrary;", sqlConnection);
		sqlCommand.Parameters.Add(new SqlParameter("LibraryDescription", SqlDbType.Text));
		sqlCommand.Parameters.Add(new SqlParameter("KeyProfileId", SqlDbType.Int));
		sqlCommand.Parameters["LibraryDescription"].Value = description;
		sqlCommand.Parameters["KeyProfileId"].Value = keyProfile;
		int num = Convert.ToInt32(sqlCommand.ExecuteScalar());
		new SqlCommand("\r\n                    INSERT INTO DesignVersionHistory\r\n                    SELECT\r\n                        (SELECT TOP(1) DesignVersionId FROM designversions WHERE DesignVersions.designid=designs.designid ORDER BY Version DESC) AS DesignVersionId,\r\n\t                    8 as DesignStatusId,\r\n\t                    'added to library " + num + "' as Comment,\r\n\t                    getdate() as date,\r\n\t                    'library builder' as Username\r\n                        FROM \r\n                            designs\r\n                        WHERE \r\n                        (SELECT TOP(1) DesignStatusId FROM designversions WHERE DesignVersions.designid=designs.designid ORDER BY Version DESC ) = 11\r\n                    ", sqlConnection).ExecuteNonQuery();
		SqlCommand sqlCommand2 = new SqlCommand("                    \r\n                    INSERT INTO LibraryDesigns\r\n\r\n                    SELECT \r\n\t                    @LibraryId as LibraryId,\r\n\t                    NULL AS LibraryFilesId,\r\n\t                    designid,\r\n\t                    NULL AS sha1,\r\n\t                    (SELECT TOP(1) DesignVersionId FROM designversions WHERE DesignVersions.designid=designs.designid AND designStatusId IN (8,9,14,16) ORDER BY Version DESC) AS DesignVersionId\r\n                    FROM \r\n\t                    designs\r\n                    WHERE \r\n\t                    (SELECT COUNT(1) FROM DesignKeyProfileLink WHERE DesignKeyProfileLink.DesignId=Designs.DesignId AND KeyProfileId=@KeyProfileId)>0   AND\r\n\t\r\n\t                    (SELECT TOP(1) DesignStatusId FROM designversions WHERE DesignVersions.designid=designs.designid AND designStatusId IN (8,9,14,16) ORDER BY Version DESC ) = 8\r\n\r\n                    ORDER BY designid", sqlConnection);
		sqlCommand2.Parameters.Add(new SqlParameter("LibraryId", SqlDbType.Int));
		sqlCommand2.Parameters.Add(new SqlParameter("KeyProfileId", SqlDbType.Int));
		sqlCommand2.Parameters["LibraryId"].Value = num;
		sqlCommand2.Parameters["KeyProfileId"].Value = keyProfile;
		sqlCommand2.ExecuteNonQuery();
		new SqlCommand("update designs set DesignVersionInNewLibrary=\r\n                        (SELECT TOP(1) DesignVersionID \r\n                          FROM LibraryDesigns\r\n                          where LibraryDesigns.designid=Designs.Designid order by libraryid desc)", sqlConnection).ExecuteNonQuery();
		new SqlCommand("update designs set FirstReleaseLibraryId=\r\n                \t  (select min(libraryid) from LibraryDesigns where libraryDesigns.designid=designs.designid and \r\n\t                    libraryid in (select versionlibraryid from versionlibrary where keyprofileid=" + keyProfile + " and publicrelease=1)\r\n\t                        )", sqlConnection).ExecuteNonQuery();
		LibraryPacker.MakeZBinFile(num, keyProfile);
		SqlCommand sqlCommand3 = new SqlCommand("select name from KeyProfile where keyprofileid=@keyProfileId", sqlConnection);
		sqlCommand3.Parameters.Add(new SqlParameter("KeyProfileId", SqlDbType.Int));
		sqlCommand3.Parameters["KeyProfileId"].Value = keyProfile;
		string keyProfile2 = sqlCommand3.ExecuteScalar().ToString();
		PostLibraryUpdate(num, keyProfile2, description);
		return num;
	}

	private static void PostLibraryUpdate(int libraryId, string keyProfile, string description)
	{
		new SlackClient("REDACTED_EMBEDDED_CREDENTIAL").PostMessage("Library Id " + libraryId + ", Created for keyprofile '" + keyProfile + "' : Description = " + description, "CounterSketch Library Built", "#development");
	}
}
