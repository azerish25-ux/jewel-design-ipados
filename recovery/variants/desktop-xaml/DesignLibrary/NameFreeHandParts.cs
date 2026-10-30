using System;
using System.Data;
using System.Data.SqlClient;

namespace DesignLibrary;

public static class NameFreeHandParts
{
	private enum FHAttrib
	{
		FHChain = 393,
		FHWireRope = 394,
		FHSymbolic = 395,
		FHSports = 396,
		FHReligious = 397,
		FHShapes = 398,
		FHShanks = 399,
		FHScrolls = 400,
		FHMixMatch = 401,
		FHMisc = 402,
		FHAnimals = 403,
		FHSettings = 404,
		FHCollarsBeads = 405,
		FHFloral = 406,
		FHElements = 407,
		FHBails = 408,
		FHRepeatingElements = 409,
		FHText = 410,
		FHNecklaceCenters = 422
	}

	private static int AttribIdtoInt(int attribId)
	{
		return (FHAttrib)attribId switch
		{
			FHAttrib.FHChain => 0, 
			FHAttrib.FHWireRope => 1, 
			FHAttrib.FHSymbolic => 2, 
			FHAttrib.FHSports => 3, 
			FHAttrib.FHReligious => 4, 
			FHAttrib.FHShapes => 5, 
			FHAttrib.FHShanks => 6, 
			FHAttrib.FHScrolls => 7, 
			FHAttrib.FHMixMatch => 8, 
			FHAttrib.FHMisc => 9, 
			FHAttrib.FHAnimals => 10, 
			FHAttrib.FHSettings => 11, 
			FHAttrib.FHCollarsBeads => 12, 
			FHAttrib.FHFloral => 13, 
			FHAttrib.FHElements => 14, 
			FHAttrib.FHBails => 15, 
			FHAttrib.FHRepeatingElements => 16, 
			FHAttrib.FHText => 17, 
			FHAttrib.FHNecklaceCenters => 18, 
			_ => -1, 
		};
	}

	public static void NameFreeHand()
	{
		string[] array = new string[19]
		{
			"CH", "WI", "SY", "SP", "RE", "SP", "SH", "SC", "MI", "MC",
			"AN", "SE", "CO", "FL", "EL", "BA", "NE", "RP", "TE"
		};
		using SqlConnection sqlConnection = new SqlConnection("Network Library=DBMSSOCN;Data Source=172.22.10.28;database=CounterSketchLibrary;User id=sa;Password=Welcome1;MultipleActiveResultSets=True;");
		sqlConnection.Open();
		SqlCommand sqlCommand = new SqlCommand("\r\n              SELECT \r\n                    designid,\r\n                    CSNo,\r\n                    (SELECT DesignVersionAttribute.AttributeValue FROM DesignVersionAttribute WHERE DesignVersionAttribute.DesignVersionId=designs.DesignVersionId AND attributeid=428) AS fhtype,\r\n                    (SELECT attributeid FROM DesignAttribute WHERE DesignAttribute.DesignId=designs.DesignId AND attributeid IN (393,394,395,396,397,398,399,400,401,402,403,404,405,406,407,408,409,410,422)) AS Attribid\r\n                FROM \r\n                    designs \r\n                WHERE\r\n                    (SELECT COUNT(1) FROM DesignVersionAttribute WHERE DesignVersionAttribute.DesignVersionId=designs.DesignVersionId AND attributeid=428)=1\r\n                AND\r\n                    (SELECT COUNT(1) FROM DesignAttribute WHERE DesignAttribute.DesignId=designs.DesignId AND attributeid IN (393,394,395,396,397,398,399,400,401,402,403,404,405,406,407,408,409,410,422))=1\r\n            ", sqlConnection);
		SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
		int[] array2 = new int[19];
		while (sqlDataReader.Read())
		{
			string[] array3 = sqlDataReader["CSNo"].ToString().Split('-');
			if (array3.Length != 3 || (array3[0] != "CP" && array3[0] != "SP"))
			{
				continue;
			}
			int num = Convert.ToInt32(array3[2]);
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array3[1] == array[i])
				{
					num2 = i;
				}
			}
			if (num > array2[num2])
			{
				array2[num2] = num;
			}
		}
		sqlDataReader.Close();
		sqlDataReader = sqlCommand.ExecuteReader();
		while (sqlDataReader.Read())
		{
			string[] array4 = sqlDataReader["CSNo"].ToString().Split('-');
			bool flag = false;
			string text = "";
			string text2 = "";
			if (array4.Length == 3)
			{
				text = array4[0];
				text2 = array4[1];
			}
			string text3 = sqlDataReader["fhtype"].ToString();
			text3 = text3.Substring(0, 2);
			if (text3 != text)
			{
				flag = true;
			}
			int num3 = AttribIdtoInt(Convert.ToInt32(sqlDataReader["Attribid"]));
			if (array[num3] != text2)
			{
				flag = true;
			}
			if (flag)
			{
				int num4 = ++array2[num3];
				int num5 = Convert.ToInt32(sqlDataReader["DesignID"]);
				string value = text3 + "-" + array[num3] + "-" + $"{num4:D3}";
				SqlCommand sqlCommand2 = new SqlCommand("UPDATE Designs SET CSNo=@CSNo WHERE DesignId=@DesignID", sqlConnection);
				sqlCommand2.Parameters.Add(new SqlParameter("CSNo", SqlDbType.NVarChar));
				sqlCommand2.Parameters.Add(new SqlParameter("DesignId", SqlDbType.Int));
				sqlCommand2.Parameters["CSNo"].Value = value;
				sqlCommand2.Parameters["DesignId"].Value = num5;
				sqlCommand2.ExecuteNonQuery();
			}
		}
	}
}
