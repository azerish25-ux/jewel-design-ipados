using System;
using System.Data;
using System.Data.SqlClient;

namespace GV.CounterSketch.Shared.DataAccess;

public abstract class DataAccess : IDisposable
{
	protected readonly SqlConnection _connection;

	protected DataAccess(string connString)
	{
		_connection = new SqlConnection(connString);
		_connection.Open();
	}

	public void Dispose()
	{
		if (_connection.State == ConnectionState.Open)
		{
			_connection.Close();
		}
		_connection.Dispose();
	}

	public void ExecuteNonQuery(string query, params object[] args)
	{
		using SqlCommand sqlCommand = new SqlCommand(query, _connection);
		for (int i = 0; i < args.Length; i += 2)
		{
			sqlCommand.Parameters.Add(new SqlParameter(args[i].ToString(), args[i + 1]));
		}
		sqlCommand.ExecuteNonQuery();
	}

	public DataTable ExecuteQuery(string query, params object[] args)
	{
		using SqlCommand sqlCommand = new SqlCommand(query, _connection);
		for (int i = 0; i < args.Length; i += 2)
		{
			sqlCommand.Parameters.Add(new SqlParameter(args[i].ToString(), args[i + 1]));
		}
		using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
		DataTable dataTable = new DataTable("Table");
		DataTable schemaTable = sqlDataReader.GetSchemaTable();
		if (schemaTable != null)
		{
			dataTable.TableName = schemaTable.TableName;
		}
		dataTable.Load(sqlDataReader);
		return dataTable;
	}

	public double? ExecuteScalarDoubleN(string query, params object[] args)
	{
		string text = ExecuteScalarString(query, args);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return double.Parse(text);
	}

	public int ExecuteScalarInt(string query, params object[] args)
	{
		string text = ExecuteScalarString(query, args);
		if (string.IsNullOrEmpty(text))
		{
			return 0;
		}
		return int.Parse(text);
	}

	public int? ExecuteScalarIntN(string query, params object[] args)
	{
		string text = ExecuteScalarString(query, args);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return int.Parse(text);
	}

	protected DataRow ExecuteQueryFirstOrDefault(string query, params object[] args)
	{
		DataTable dataTable = ExecuteQuery(query, args);
		if (dataTable.Rows.Count < 1)
		{
			return null;
		}
		return dataTable.Rows[0];
	}

	protected string ExecuteScalarString(string query, params object[] args)
	{
		using SqlCommand sqlCommand = new SqlCommand(query, _connection);
		for (int i = 0; i < args.Length; i += 2)
		{
			sqlCommand.Parameters.Add(new SqlParameter(args[i].ToString(), args[i + 1]));
		}
		object obj = sqlCommand.ExecuteScalar();
		if (obj != null && obj != DBNull.Value)
		{
			return obj.ToString();
		}
		return null;
	}
}
