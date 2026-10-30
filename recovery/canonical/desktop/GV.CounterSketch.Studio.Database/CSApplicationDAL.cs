using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
		ConnString = $"Data Source={UIHelper.CSApplicationDatabasePath};BinaryGUID=No";
		Connection = new SQLiteConnection(ConnString);
		Connection.Open();
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
			if (Connection.State == ConnectionState.Open)
			{
				Connection.Close();
			}
			Connection.Dispose();
			Connection = null;
			GC.Collect();
			_disposed = true;
		}
	}

	public string GetDatabaseVersion()
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT dbversion from version LIMIT 1", Connection);
		try
		{
			return sQLiteCommand.ExecuteScalar().ToString();
		}
		catch (Exception)
		{
			return "0";
		}
	}

	public void ExecuteNonQuery(string query)
	{
		SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = query;
		sQLiteCommand.ExecuteNonQuery();
	}

	public ObservableCollection<CSOrder> GetOrders()
	{
		List<CSOrder> list = new List<CSOrder>();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection))
		{
			sQLiteCommand.CommandText = "SELECT * FROM Quote;";
			SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.HasRows)
			{
				while (sQLiteDataReader.Read())
				{
					CSOrder csQuote = new CSOrder();
					csQuote.QuoteId = int.Parse(sQLiteDataReader["QuoteId"].ToString());
					csQuote.CustomerFirstName = sQLiteDataReader["CustomerFirstName"].ToString();
					csQuote.CustomerLastName = sQLiteDataReader["CustomerLastName"].ToString();
					csQuote.CustomerAddress1 = sQLiteDataReader["CustomerAddress1"].ToString();
					csQuote.CustomerAddress2 = sQLiteDataReader["CustomerAddress2"].ToString();
					csQuote.CustomerCity = sQLiteDataReader["CustomerCity"].ToString();
					csQuote.CustomerState = sQLiteDataReader["CustomerState"].ToString();
					csQuote.CustomerZip = sQLiteDataReader["CustomerZip"].ToString();
					csQuote.FirstName = sQLiteDataReader["ContactFirstname"].ToString();
					csQuote.LastName = sQLiteDataReader["ContactLastname"].ToString();
					csQuote.ContactEMail = sQLiteDataReader["ContactEMail"].ToString();
					csQuote.ContactPhone = sQLiteDataReader["ContactPhone"].ToString();
					csQuote.DateSubmitted = DateTime.Parse(sQLiteDataReader["DateSubmitted"].ToString());
					csQuote.Notes = sQLiteDataReader["Notes"].ToString();
					csQuote.QuotedDesignPath = sQLiteDataReader["QuotedDesignPath"].ToString();
					if (sQLiteDataReader["CAMRequestId"] != DBNull.Value)
					{
						csQuote.CamRequestId = int.Parse(sQLiteDataReader["CAMRequestId"].ToString());
					}
					csQuote.ReferenceNumber = sQLiteDataReader["ReferenceNumber"].ToString();
					csQuote.OrderNumber = sQLiteDataReader["OrderNumber"].ToString();
					string[] array = sQLiteDataReader["QuoteStatus"].ToString().Split('|');
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
		return new ObservableCollection<CSOrder>(list.OrderByDescending((CSOrder _) => _.DateSubmitted));
	}

	public void CreateQuote(string contactFirstname, string contactLastname, string contactEmail, string contactPhone, string Notes, string designFilePath, DateTime dateSubmitted, int requestId, string referenceNumber, string customerFirstName, string customerLastName, string customerAddress1, string customerAddress2, string customerCity, string customerState, string customerZip, string customerPhone, string customerNotes)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "INSERT INTO Quote ([UserId], [ContactFirstname], [ContactLastname], [ContactEMail], [ContactPhone], [Notes], [QuotedDesignPath], [DateSubmitted], [CAMRequestId], [ReferenceNumber], [CustomerFirstName], [CustomerLastName], [CustomerAddress1], [CustomerAddress2], [CustomerCity], [CustomerState], [CustomerZip], [CustomerPhone], [CustomerNotes]) VALUES (0,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = contactFirstname;
		SQLiteParameter sQLiteParameter2 = new SQLiteParameter(DbType.String);
		sQLiteParameter2.Value = contactLastname;
		SQLiteParameter sQLiteParameter3 = new SQLiteParameter(DbType.String);
		sQLiteParameter3.Value = contactEmail;
		SQLiteParameter sQLiteParameter4 = new SQLiteParameter(DbType.String);
		sQLiteParameter4.Value = contactPhone;
		SQLiteParameter sQLiteParameter5 = new SQLiteParameter(DbType.String);
		sQLiteParameter5.Value = Notes;
		SQLiteParameter sQLiteParameter6 = new SQLiteParameter(DbType.String);
		sQLiteParameter6.Value = designFilePath;
		SQLiteParameter sQLiteParameter7 = new SQLiteParameter(DbType.DateTime);
		sQLiteParameter7.Value = dateSubmitted;
		SQLiteParameter sQLiteParameter8 = new SQLiteParameter(DbType.Int32);
		sQLiteParameter8.Value = requestId;
		SQLiteParameter sQLiteParameter9 = new SQLiteParameter(DbType.String);
		sQLiteParameter9.Value = referenceNumber;
		SQLiteParameter sQLiteParameter10 = new SQLiteParameter(DbType.String);
		sQLiteParameter10.Value = customerFirstName;
		SQLiteParameter sQLiteParameter11 = new SQLiteParameter(DbType.String);
		sQLiteParameter11.Value = customerLastName;
		SQLiteParameter sQLiteParameter12 = new SQLiteParameter(DbType.String);
		sQLiteParameter12.Value = customerAddress1;
		SQLiteParameter sQLiteParameter13 = new SQLiteParameter(DbType.String);
		sQLiteParameter13.Value = customerAddress2;
		SQLiteParameter sQLiteParameter14 = new SQLiteParameter(DbType.String);
		sQLiteParameter14.Value = customerCity;
		SQLiteParameter sQLiteParameter15 = new SQLiteParameter(DbType.String);
		sQLiteParameter15.Value = customerState;
		SQLiteParameter sQLiteParameter16 = new SQLiteParameter(DbType.String);
		sQLiteParameter16.Value = customerZip;
		SQLiteParameter sQLiteParameter17 = new SQLiteParameter(DbType.String);
		sQLiteParameter17.Value = customerPhone;
		SQLiteParameter sQLiteParameter18 = new SQLiteParameter(DbType.String);
		sQLiteParameter18.Value = customerNotes;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[18]
		{
			sQLiteParameter, sQLiteParameter2, sQLiteParameter3, sQLiteParameter4, sQLiteParameter5, sQLiteParameter6, sQLiteParameter7, sQLiteParameter8, sQLiteParameter9, sQLiteParameter10,
			sQLiteParameter11, sQLiteParameter12, sQLiteParameter13, sQLiteParameter14, sQLiteParameter15, sQLiteParameter16, sQLiteParameter17, sQLiteParameter18
		});
		sQLiteCommand.ExecuteNonQuery();
	}

	public ObservableCollection<CSUser> GetUsers()
	{
		ObservableCollection<CSUser> observableCollection = new ObservableCollection<CSUser>();
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin, EMail, Phone FROM User WHERE ActiveFlag=1;";
		SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		if (sQLiteDataReader.HasRows)
		{
			while (sQLiteDataReader.Read())
			{
				CSUser cSUser = new CSUser();
				cSUser.UserId = int.Parse(sQLiteDataReader["UserId"].ToString());
				cSUser.FirstName = sQLiteDataReader["FirstName"].ToString();
				cSUser.LastName = sQLiteDataReader["LastName"].ToString();
				cSUser.Username = sQLiteDataReader["Username"].ToString();
				cSUser.Password = sQLiteDataReader["Password"].ToString();
				if (Convert.ToInt32(sQLiteDataReader["IsAdmin"]) == 1)
				{
					cSUser.IsAdmin = true;
				}
				else
				{
					cSUser.IsAdmin = false;
				}
				cSUser.EMail = sQLiteDataReader["EMail"].ToString();
				cSUser.Phone = sQLiteDataReader["Phone"].ToString();
				observableCollection.Add(cSUser);
			}
		}
		return observableCollection;
	}

	public CSUser GetUserByUsername(string username)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin FROM User WHERE Username LIKE ? LIMIT 1;";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = username;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[1] { sQLiteParameter });
		SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		if (sQLiteDataReader.HasRows && sQLiteDataReader.Read())
		{
			return new CSUser
			{
				UserId = int.Parse(sQLiteDataReader["UserId"].ToString()),
				FirstName = sQLiteDataReader["FirstName"].ToString(),
				LastName = sQLiteDataReader["LastName"].ToString(),
				Username = sQLiteDataReader["Username"].ToString(),
				Password = sQLiteDataReader["Password"].ToString(),
				IsAdmin = bool.Parse(sQLiteDataReader["IsAdmin"].ToString())
			};
		}
		return null;
	}

	public CSUser GetUserByUserId(int userId)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "SELECT UserId, Username, Password, FirstName, LastName, IsAdmin FROM User WHERE UserId = ?;";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.Int32);
		sQLiteParameter.Value = userId;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[1] { sQLiteParameter });
		SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
		if (sQLiteDataReader.HasRows && sQLiteDataReader.Read())
		{
			return new CSUser
			{
				UserId = int.Parse(sQLiteDataReader["UserId"].ToString()),
				FirstName = sQLiteDataReader["FirstName"].ToString(),
				LastName = sQLiteDataReader["LastName"].ToString(),
				Username = sQLiteDataReader["Username"].ToString(),
				Password = sQLiteDataReader["Password"].ToString(),
				IsAdmin = bool.Parse(sQLiteDataReader["IsAdmin"].ToString())
			};
		}
		return null;
	}

	public void CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "INSERT INTO User ([Username], [Password], [FirstName], [LastName], [IsAdmin]) VALUES (?,?,?,?,?);";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = username;
		SQLiteParameter sQLiteParameter2 = new SQLiteParameter(DbType.String);
		sQLiteParameter2.Value = password;
		SQLiteParameter sQLiteParameter3 = new SQLiteParameter(DbType.String);
		sQLiteParameter3.Value = firstName;
		SQLiteParameter sQLiteParameter4 = new SQLiteParameter(DbType.String);
		sQLiteParameter4.Value = lastName;
		SQLiteParameter sQLiteParameter5 = new SQLiteParameter(DbType.Int32);
		if (isAdmin)
		{
			sQLiteParameter4.Value = 1;
		}
		else
		{
			sQLiteParameter4.Value = 0;
		}
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[5] { sQLiteParameter, sQLiteParameter2, sQLiteParameter3, sQLiteParameter4, sQLiteParameter5 });
		sQLiteCommand.ExecuteNonQuery();
	}

	public void CreateUser(CSUser user)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "INSERT INTO User ([Username], [Password], [FirstName], [LastName], [IsAdmin], [EMail], [Phone]) VALUES (?,?,?,?,?,?,?);";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = user.Username;
		SQLiteParameter sQLiteParameter2 = new SQLiteParameter(DbType.String);
		sQLiteParameter2.Value = user.Password;
		SQLiteParameter sQLiteParameter3 = new SQLiteParameter(DbType.String);
		sQLiteParameter3.Value = user.FirstName;
		SQLiteParameter sQLiteParameter4 = new SQLiteParameter(DbType.String);
		sQLiteParameter4.Value = user.LastName;
		SQLiteParameter sQLiteParameter5 = new SQLiteParameter(DbType.Int32);
		if (user.IsAdmin)
		{
			sQLiteParameter5.Value = 1;
		}
		else
		{
			sQLiteParameter5.Value = 0;
		}
		SQLiteParameter sQLiteParameter6 = new SQLiteParameter(DbType.String);
		sQLiteParameter6.Value = user.EMail;
		SQLiteParameter sQLiteParameter7 = new SQLiteParameter(DbType.String);
		sQLiteParameter7.Value = user.Phone;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[7] { sQLiteParameter, sQLiteParameter2, sQLiteParameter3, sQLiteParameter4, sQLiteParameter5, sQLiteParameter6, sQLiteParameter7 });
		sQLiteCommand.ExecuteNonQuery();
	}

	public void UpdateUser(int userId, string username, string password, string firstName, string lastName, bool isAdmin, string eMail, string phone)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "UPDATE User SET [Username]=?, [Password]=?, [FirstName]=?, [LastName]=?, [IsAdmin]=? WHERE UserId = ?;";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = username;
		SQLiteParameter sQLiteParameter2 = new SQLiteParameter(DbType.String);
		sQLiteParameter2.Value = password;
		SQLiteParameter sQLiteParameter3 = new SQLiteParameter(DbType.String);
		sQLiteParameter3.Value = firstName;
		SQLiteParameter sQLiteParameter4 = new SQLiteParameter(DbType.String);
		sQLiteParameter4.Value = lastName;
		SQLiteParameter sQLiteParameter5 = new SQLiteParameter(DbType.Int32);
		if (isAdmin)
		{
			sQLiteParameter4.Value = 1;
		}
		else
		{
			sQLiteParameter4.Value = 0;
		}
		SQLiteParameter sQLiteParameter6 = new SQLiteParameter(DbType.String);
		sQLiteParameter6.Value = eMail;
		SQLiteParameter sQLiteParameter7 = new SQLiteParameter(DbType.String);
		sQLiteParameter7.Value = phone;
		SQLiteParameter sQLiteParameter8 = new SQLiteParameter(DbType.Int32);
		sQLiteParameter8.Value = userId;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[8] { sQLiteParameter, sQLiteParameter2, sQLiteParameter3, sQLiteParameter4, sQLiteParameter5, sQLiteParameter6, sQLiteParameter7, sQLiteParameter8 });
		sQLiteCommand.ExecuteNonQuery();
	}

	public void UpdateUser(CSUser user)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "UPDATE User SET [Username]=?, [Password]=?, [FirstName]=?, [LastName]=?, [IsAdmin]=?, [EMail]=?, [Phone]=? WHERE UserId = ?;";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.String);
		sQLiteParameter.Value = user.Username;
		SQLiteParameter sQLiteParameter2 = new SQLiteParameter(DbType.String);
		sQLiteParameter2.Value = user.Password;
		SQLiteParameter sQLiteParameter3 = new SQLiteParameter(DbType.String);
		sQLiteParameter3.Value = user.FirstName;
		SQLiteParameter sQLiteParameter4 = new SQLiteParameter(DbType.String);
		sQLiteParameter4.Value = user.LastName;
		SQLiteParameter sQLiteParameter5 = new SQLiteParameter(DbType.Int32);
		if (user.IsAdmin)
		{
			sQLiteParameter5.Value = 1;
		}
		else
		{
			sQLiteParameter5.Value = 0;
		}
		SQLiteParameter sQLiteParameter6 = new SQLiteParameter(DbType.String);
		sQLiteParameter6.Value = user.EMail;
		SQLiteParameter sQLiteParameter7 = new SQLiteParameter(DbType.String);
		sQLiteParameter7.Value = user.Phone;
		SQLiteParameter sQLiteParameter8 = new SQLiteParameter(DbType.Int32);
		sQLiteParameter8.Value = user.UserId;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[8] { sQLiteParameter, sQLiteParameter2, sQLiteParameter3, sQLiteParameter4, sQLiteParameter5, sQLiteParameter6, sQLiteParameter7, sQLiteParameter8 });
		sQLiteCommand.ExecuteNonQuery();
	}

	public void DeleteUser(int userId)
	{
		using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
		sQLiteCommand.CommandText = "UPDATE User SET ActiveFlag=0 WHERE UserId = ?;";
		SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.Int32);
		sQLiteParameter.Value = userId;
		sQLiteCommand.Parameters.AddRange(new SQLiteParameter[1] { sQLiteParameter });
		sQLiteCommand.ExecuteNonQuery();
	}

	public List<int> GetHiddenDesigns(int userId)
	{
		List<int> list = new List<int>();
		using (SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection))
		{
			sQLiteCommand.CommandText = "SELECT DesignId FROM UserDesignAttributeAssignment WHERE UserId = ? AND IsHidden = 1;";
			SQLiteParameter sQLiteParameter = new SQLiteParameter(DbType.Int64);
			sQLiteParameter.Value = userId;
			sQLiteCommand.Parameters.AddRange(new SQLiteParameter[1] { sQLiteParameter });
			SQLiteDataReader sQLiteDataReader = sQLiteCommand.ExecuteReader();
			if (sQLiteDataReader.HasRows)
			{
				while (sQLiteDataReader.Read())
				{
					list.Add(int.Parse(sQLiteDataReader["DesignId"].ToString()));
				}
			}
		}
		return list;
	}

	public bool SetHiddenDesigns(int userId, List<int> designsToHide, bool hidden)
	{
		bool flag = false;
		foreach (int item in designsToHide)
		{
			if (item == 0)
			{
				continue;
			}
			using SQLiteCommand sQLiteCommand = new SQLiteCommand(Connection);
			sQLiteCommand.CommandText = "INSERT OR REPLACE INTO UserDesignAttributeAssignment ([DesignId], [UserId], [IsHidden]) VALUES (?,?,?);";
			sQLiteCommand.Parameters.AddRange(new SQLiteParameter[2]
			{
				new SQLiteParameter
				{
					Value = item,
					DbType = DbType.Int32
				},
				new SQLiteParameter
				{
					Value = userId,
					DbType = DbType.Int32
				}
			});
			if (hidden)
			{
				sQLiteCommand.Parameters.Add(new SQLiteParameter
				{
					Value = 1,
					DbType = DbType.Int32
				});
			}
			else
			{
				sQLiteCommand.Parameters.Add(new SQLiteParameter
				{
					Value = 0,
					DbType = DbType.Int32
				});
			}
			flag = sQLiteCommand.ExecuteNonQuery() == 1;
			if (!flag)
			{
				break;
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
