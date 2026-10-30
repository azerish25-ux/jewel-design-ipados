using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Database;

public class CSApplicationDAL : IDisposable
{
	public string CurrentAppDBVersion = "16";

	public string ConnString;

	public SQLiteConnection Connection;

	private bool _disposed;

	public CSApplicationDAL()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		ConnString = $"Data Source={UIHelper.CSApplicationDatabasePath};BinaryGUID=No";
		Connection = new SQLiteConnection(ConnString);
		((DbConnection)(object)Connection).Open();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~CSApplicationDAL()
	{
		Dispose(disposing: false);
	}

	public bool DatabaseUpToDate()
	{
		if (int.Parse(GetDatabaseVersion()) >= int.Parse(CurrentAppDBVersion))
		{
			return true;
		}
		return false;
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (((DbConnection)(object)Connection).State == ConnectionState.Open)
			{
				((DbConnection)(object)Connection).Close();
			}
			Connection.Dispose();
			Connection = null;
			GC.Collect();
			_disposed = true;
		}
	}

	public string GetDatabaseVersion()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand("SELECT dbversion from version LIMIT 1", Connection);
		try
		{
			return ((DbCommand)(object)val).ExecuteScalar().ToString();
		}
		catch (Exception)
		{
			return "0";
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void ExecuteNonQuery(string query)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0017: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		((DbCommand)val).CommandText = query;
		((DbCommand)val).ExecuteNonQuery();
	}

	public ObservableCollection<CSOrder> GetOrders()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		List<CSOrder> list = new List<CSOrder>();
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "SELECT * FROM Quote;";
			SQLiteDataReader val2 = val.ExecuteReader();
			if (((DbDataReader)(object)val2).HasRows)
			{
				while (((DbDataReader)(object)val2).Read())
				{
					CSOrder csQuote = new CSOrder();
					csQuote.QuoteId = int.Parse(((DbDataReader)(object)val2)["QuoteId"].ToString());
					csQuote.CustomerFirstName = ((DbDataReader)(object)val2)["CustomerFirstName"].ToString();
					csQuote.CustomerLastName = ((DbDataReader)(object)val2)["CustomerLastName"].ToString();
					csQuote.CustomerAddress1 = ((DbDataReader)(object)val2)["CustomerAddress1"].ToString();
					csQuote.CustomerAddress2 = ((DbDataReader)(object)val2)["CustomerAddress2"].ToString();
					csQuote.CustomerCity = ((DbDataReader)(object)val2)["CustomerCity"].ToString();
					csQuote.CustomerState = ((DbDataReader)(object)val2)["CustomerState"].ToString();
					csQuote.CustomerZip = ((DbDataReader)(object)val2)["CustomerZip"].ToString();
					csQuote.FirstName = ((DbDataReader)(object)val2)["ContactFirstname"].ToString();
					csQuote.LastName = ((DbDataReader)(object)val2)["ContactLastname"].ToString();
					csQuote.ContactEMail = ((DbDataReader)(object)val2)["ContactEMail"].ToString();
					csQuote.ContactPhone = ((DbDataReader)(object)val2)["ContactPhone"].ToString();
					csQuote.DateSubmitted = DateTime.Parse(((DbDataReader)(object)val2)["DateSubmitted"].ToString());
					csQuote.Notes = ((DbDataReader)(object)val2)["Notes"].ToString();
					csQuote.QuotedDesignPath = ((DbDataReader)(object)val2)["QuotedDesignPath"].ToString();
					if (((DbDataReader)(object)val2)["CAMRequestId"] != DBNull.Value)
					{
						csQuote.CamRequestId = int.Parse(((DbDataReader)(object)val2)["CAMRequestId"].ToString());
					}
					csQuote.ReferenceNumber = ((DbDataReader)(object)val2)["ReferenceNumber"].ToString();
					csQuote.OrderNumber = ((DbDataReader)(object)val2)["OrderNumber"].ToString();
					string[] array = ((DbDataReader)(object)val2)["QuoteStatus"].ToString().Split('|');
					if (array.Length != 0)
					{
						csQuote.QuoteStatus = array[0];
					}
					if (array.Length > 1)
					{
						csQuote.QuoteShipDate = array[1];
					}
					if (csQuote.QuotedDesignPath != null && File.Exists(csQuote.QuotedDesignPath))
					{
						UIHelper.CoreDataService.GetThumbnailFromFile(csQuote.QuotedDesignPath, delegate(BitmapSourceByteArrayInfo bitmapSourceByteArrayInfo)
						{
							csQuote.DesignImageSource = bitmapSourceByteArrayInfo.ItemImageSource;
						});
					}
					list.Add(csQuote);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return new ObservableCollection<CSOrder>(list.OrderByDescending((CSOrder _) => _.DateSubmitted));
	}

	public void CreateQuote(string contactFirstname, string contactLastname, string contactEmail, string contactPhone, string Notes, string designFilePath, DateTime dateSubmitted, int requestId, string referenceNumber, string customerFirstName, string customerLastName, string customerAddress1, string customerAddress2, string customerCity, string customerState, string customerZip, string customerPhone, string customerNotes)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "INSERT INTO Quote ([UserId], [ContactFirstname], [ContactLastname], [ContactEMail], [ContactPhone], [Notes], [QuotedDesignPath], [DateSubmitted], [CAMRequestId], [ReferenceNumber], [CustomerFirstName], [CustomerLastName], [CustomerAddress1], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZip], [CustomerPhone], [CustomerNotes]) VALUES (0,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = contactFirstname;
			SQLiteParameter val3 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val3).Value = contactLastname;
			SQLiteParameter val4 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val4).Value = contactEmail;
			SQLiteParameter val5 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val5).Value = contactPhone;
			SQLiteParameter val6 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val6).Value = Notes;
			SQLiteParameter val7 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val7).Value = designFilePath;
			SQLiteParameter val8 = new SQLiteParameter(DbType.DateTime);
			((DbParameter)(object)val8).Value = dateSubmitted;
			SQLiteParameter val9 = new SQLiteParameter(DbType.Int32);
			((DbParameter)(object)val9).Value = requestId;
			SQLiteParameter val10 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val10).Value = referenceNumber;
			SQLiteParameter val11 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val11).Value = customerFirstName;
			SQLiteParameter val12 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val12).Value = customerLastName;
			SQLiteParameter val13 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val13).Value = customerAddress1;
			SQLiteParameter val14 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val14).Value = customerAddress2;
			SQLiteParameter val15 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val15).Value = customerCity;
			SQLiteParameter val16 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val16).Value = customerState;
			SQLiteParameter val17 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val17).Value = customerZip;
			SQLiteParameter val18 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val18).Value = customerPhone;
			SQLiteParameter val19 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val19).Value = customerNotes;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[18]
			{
				val2, val3, val4, val5, val6, val7, val8, val9, val10, val11,
				val12, val13, val14, val15, val16, val17, val18, val19
			});
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public ObservableCollection<CSUser> GetUsers()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		ObservableCollection<CSUser> observableCollection = new ObservableCollection<CSUser>();
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin, EMail, Phone FROM User WHERE ActiveFlag=1;";
			SQLiteDataReader val2 = val.ExecuteReader();
			if (((DbDataReader)(object)val2).HasRows)
			{
				while (((DbDataReader)(object)val2).Read())
				{
					CSUser cSUser = new CSUser();
					cSUser.UserId = int.Parse(((DbDataReader)(object)val2)["UserId"].ToString());
					cSUser.FirstName = ((DbDataReader)(object)val2)["FirstName"].ToString();
					cSUser.LastName = ((DbDataReader)(object)val2)["LastName"].ToString();
					cSUser.Username = ((DbDataReader)(object)val2)["Username"].ToString();
					cSUser.Password = ((DbDataReader)(object)val2)["Password"].ToString();
					if (Convert.ToInt32(((DbDataReader)(object)val2)["IsAdmin"]) == 1)
					{
						cSUser.IsAdmin = true;
					}
					else
					{
						cSUser.IsAdmin = false;
					}
					cSUser.EMail = ((DbDataReader)(object)val2)["EMail"].ToString();
					cSUser.Phone = ((DbDataReader)(object)val2)["Phone"].ToString();
					observableCollection.Add(cSUser);
				}
			}
			return observableCollection;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public CSUser GetUserByUsername(string username)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin FROM User WHERE Username LIKE ? LIMIT 1;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = username;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[1] { val2 });
			SQLiteDataReader val3 = val.ExecuteReader();
			if (((DbDataReader)(object)val3).HasRows && ((DbDataReader)(object)val3).Read())
			{
				return new CSUser
				{
					UserId = int.Parse(((DbDataReader)(object)val3)["UserId"].ToString()),
					FirstName = ((DbDataReader)(object)val3)["FirstName"].ToString(),
					LastName = ((DbDataReader)(object)val3)["LastName"].ToString(),
					Username = ((DbDataReader)(object)val3)["Username"].ToString(),
					Password = ((DbDataReader)(object)val3)["Password"].ToString(),
					IsAdmin = bool.Parse(((DbDataReader)(object)val3)["IsAdmin"].ToString())
				};
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public CSUser GetUserByUserId(int userId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin FROM User WHERE UserId = ?;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.Int32);
			((DbParameter)(object)val2).Value = userId;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[1] { val2 });
			SQLiteDataReader val3 = val.ExecuteReader();
			if (((DbDataReader)(object)val3).HasRows && ((DbDataReader)(object)val3).Read())
			{
				return new CSUser
				{
					UserId = int.Parse(((DbDataReader)(object)val3)["UserId"].ToString()),
					FirstName = ((DbDataReader)(object)val3)["FirstName"].ToString(),
					LastName = ((DbDataReader)(object)val3)["LastName"].ToString(),
					Username = ((DbDataReader)(object)val3)["Username"].ToString(),
					Password = ((DbDataReader)(object)val3)["Password"].ToString(),
					IsAdmin = bool.Parse(((DbDataReader)(object)val3)["IsAdmin"].ToString())
				};
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "INSERT INTO User ([Username], [Password], [FirstName], [LastName], [IsAdmin]) VALUES (?,?,?,?,?);";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = username;
			SQLiteParameter val3 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val3).Value = password;
			SQLiteParameter val4 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val4).Value = firstName;
			SQLiteParameter val5 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val5).Value = lastName;
			SQLiteParameter val6 = new SQLiteParameter(DbType.Int32);
			if (isAdmin)
			{
				((DbParameter)(object)val5).Value = 1;
			}
			else
			{
				((DbParameter)(object)val5).Value = 0;
			}
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[5] { val2, val3, val4, val5, val6 });
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void CreateUser(CSUser user)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "INSERT INTO User ([Username], [Password], [FirstName], [LastName], [IsAdmin], [EMail], [Phone]) VALUES (?,?,?,?,?,?,?);";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = user.Username;
			SQLiteParameter val3 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val3).Value = user.Password;
			SQLiteParameter val4 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val4).Value = user.FirstName;
			SQLiteParameter val5 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val5).Value = user.LastName;
			SQLiteParameter val6 = new SQLiteParameter(DbType.Int32);
			if (user.IsAdmin)
			{
				((DbParameter)(object)val6).Value = 1;
			}
			else
			{
				((DbParameter)(object)val6).Value = 0;
			}
			SQLiteParameter val7 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val7).Value = user.EMail;
			SQLiteParameter val8 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val8).Value = user.Phone;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[7] { val2, val3, val4, val5, val6, val7, val8 });
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void UpdateUser(int userId, string username, string password, string firstName, string lastName, bool isAdmin, string eMail, string phone)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "UPDATE User SET [Username]=?, [Password]=?, [FirstName]=?, [LastName]=?, [IsAdmin]=? WHERE UserId = ?;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = username;
			SQLiteParameter val3 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val3).Value = password;
			SQLiteParameter val4 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val4).Value = firstName;
			SQLiteParameter val5 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val5).Value = lastName;
			SQLiteParameter val6 = new SQLiteParameter(DbType.Int32);
			if (isAdmin)
			{
				((DbParameter)(object)val5).Value = 1;
			}
			else
			{
				((DbParameter)(object)val5).Value = 0;
			}
			SQLiteParameter val7 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val7).Value = eMail;
			SQLiteParameter val8 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val8).Value = phone;
			SQLiteParameter val9 = new SQLiteParameter(DbType.Int32);
			((DbParameter)(object)val9).Value = userId;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[8] { val2, val3, val4, val5, val6, val7, val8, val9 });
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void UpdateUser(CSUser user)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "UPDATE User SET [Username]=?, [Password]=?, [FirstName]=?, [LastName]=?, [IsAdmin]=?, [EMail]=?, [Phone]=? WHERE UserId = ?;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val2).Value = user.Username;
			SQLiteParameter val3 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val3).Value = user.Password;
			SQLiteParameter val4 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val4).Value = user.FirstName;
			SQLiteParameter val5 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val5).Value = user.LastName;
			SQLiteParameter val6 = new SQLiteParameter(DbType.Int32);
			if (user.IsAdmin)
			{
				((DbParameter)(object)val6).Value = 1;
			}
			else
			{
				((DbParameter)(object)val6).Value = 0;
			}
			SQLiteParameter val7 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val7).Value = user.EMail;
			SQLiteParameter val8 = new SQLiteParameter(DbType.String);
			((DbParameter)(object)val8).Value = user.Phone;
			SQLiteParameter val9 = new SQLiteParameter(DbType.Int32);
			((DbParameter)(object)val9).Value = user.UserId;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[8] { val2, val3, val4, val5, val6, val7, val8, val9 });
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public void DeleteUser(int userId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "UPDATE User SET ActiveFlag=0 WHERE UserId = ?;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.Int32);
			((DbParameter)(object)val2).Value = userId;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[1] { val2 });
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public List<int> GetHiddenDesigns(int userId)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		List<int> list = new List<int>();
		SQLiteCommand val = new SQLiteCommand(Connection);
		try
		{
			((DbCommand)(object)val).CommandText = "SELECT DesignId FROM UserDesignAttributeAssignment WHERE UserId = ? AND IsHidden = 1;";
			SQLiteParameter val2 = new SQLiteParameter(DbType.Int64);
			((DbParameter)(object)val2).Value = userId;
			val.Parameters.AddRange((SQLiteParameter[])(object)new SQLiteParameter[1] { val2 });
			SQLiteDataReader val3 = val.ExecuteReader();
			if (((DbDataReader)(object)val3).HasRows)
			{
				while (((DbDataReader)(object)val3).Read())
				{
					list.Add(int.Parse(((DbDataReader)(object)val3)["DesignId"].ToString()));
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return list;
	}

	public bool SetHiddenDesigns(int userId, List<int> designsToHide, bool hidden)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0077: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00ca: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00a3: Expected O, but got Unknown
		bool flag = false;
		foreach (int item in designsToHide)
		{
			if (item == 0)
			{
				continue;
			}
			SQLiteCommand val = new SQLiteCommand(Connection);
			try
			{
				((DbCommand)(object)val).CommandText = "INSERT OR REPLACE INTO UserDesignAttributeAssignment ([DesignId], [UserId], [IsHidden]) VALUES (?,?,?);";
				SQLiteParameterCollection parameters = val.Parameters;
				SQLiteParameter[] array = new SQLiteParameter[2];
				SQLiteParameter val2 = new SQLiteParameter();
				((DbParameter)val2).Value = item;
				((DbParameter)val2).DbType = DbType.Int32;
				array[0] = val2;
				SQLiteParameter val3 = new SQLiteParameter();
				((DbParameter)val3).Value = userId;
				((DbParameter)val3).DbType = DbType.Int32;
				array[1] = val3;
				parameters.AddRange((SQLiteParameter[])(object)array);
				if (hidden)
				{
					SQLiteParameterCollection parameters2 = val.Parameters;
					SQLiteParameter val4 = new SQLiteParameter();
					((DbParameter)val4).Value = 1;
					((DbParameter)val4).DbType = DbType.Int32;
					parameters2.Add(val4);
				}
				else
				{
					SQLiteParameterCollection parameters3 = val.Parameters;
					SQLiteParameter val5 = new SQLiteParameter();
					((DbParameter)val5).Value = 0;
					((DbParameter)val5).DbType = DbType.Int32;
					parameters3.Add(val5);
				}
				flag = ((DbCommand)(object)val).ExecuteNonQuery() == 1;
				if (!flag)
				{
					break;
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return flag;
	}

	public bool SetHiddenDesigns(List<int> designsToHide, bool hidden)
	{
		return SetHiddenDesigns(0, designsToHide, hidden);
	}

	public List<int> GetHiddenDesigns()
	{
		return GetHiddenDesigns(0);
	}
}
