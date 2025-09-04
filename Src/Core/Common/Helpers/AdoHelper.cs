using Barin.Framework.Common.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Barin.Framework.Common.Helpers;

public class AdoHelper
{
    public AdoHelper(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public AdoHelper Instance(string connectionString)
    {
        return new AdoHelper(connectionString);
    }

    public string ConnectionString { private get; set; }

    public int ExecuteQuery(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        var cnn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand { Connection = cnn, CommandType = commandType, CommandText = commandText };
        cmd.Parameters.AddRange(parameters);

        try
        {
            cnn.Open();
            return cmd.ExecuteNonQuery();
        }
        catch (AdoException ex)
        {
            throw new AdoException(ex.Message, ex);
        }
        finally
        {
            cnn.Close();
        }
    }

    public int ExecuteQuery(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteQuery(commandText, parameters.ToArray(), commandType);
    }

    public int ExecuteQuery(string commandText, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteQuery(commandText, new List<SqlParameter>().ToArray(), commandType);
    }

    public object ExecuteScaler(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        using (var cnn = new SqlConnection(ConnectionString))
        {
            var cmd = new SqlCommand { Connection = cnn, CommandType = commandType, CommandText = commandText };
            cmd.Parameters.AddRange(parameters);
            cnn.Open();

            object retVal = cmd.ExecuteScalar();

            cnn.Close();

            return retVal;
        }
    }

    public object ExecuteScaler(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteScaler(commandText, parameters.ToArray(), commandType);
    }

    public object ExecuteScaler(string commandText, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteScaler(commandText, new List<SqlParameter>().ToArray(), commandType);
    }

    public SqlDataReader ExecuteReader(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        var cnn = new SqlConnection(ConnectionString);
        SqlDataReader dataReader = null;
        var cmd = new SqlCommand { Connection = cnn, CommandType = commandType, CommandText = commandText };
        cmd.Parameters.AddRange(parameters);

        try
        {
            cnn.Open();
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (AdoException ex)
        {
            throw new AdoException(ex.Message, ex);
        }

        return dataReader;
    }

    public SqlDataReader ExecuteReader(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteReader(commandText, parameters.ToArray(), commandType);
    }

    public SqlDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure)
    {
        return ExecuteReader(commandText, new List<SqlParameter>().ToArray(), commandType);
    }

    public DataSet GetDataSet(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        var cnn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand { Connection = cnn, CommandType = commandType, CommandText = commandText };
        cmd.Parameters.AddRange(parameters);
        var ds = new DataSet();
        var da = new SqlDataAdapter(cmd);

        try
        {
            cnn.Open();
            da.Fill(ds);
        }
        catch (AdoException ex)
        {
            throw new AdoException(ex.Message, ex);
        }
        finally
        {
            cnn.Close();
        }

        return ds;
    }

    public DataSet GetDataSet(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        return GetDataSet(commandText, parameters.ToArray(), commandType);
    }

    public DataSet GetDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure)
    {
        return GetDataSet(commandText, new List<SqlParameter>().ToArray(), commandType);
    }

    public DataTable GetDataTable(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        var cnn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand { Connection = cnn, CommandType = commandType, CommandText = commandText };
        cmd.Parameters.AddRange(parameters);
        var dt = new DataTable();
        var da = new SqlDataAdapter(cmd);

        try
        {
            cnn.Open();
            da.Fill(dt);
        }
        catch (AdoException ex)
        {
            throw new AdoException(ex.Message, ex);
        }
        finally
        {
            cnn.Close();
        }

        return dt;
    }

    public DataTable GetDataTable(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        return GetDataTable(commandText, parameters.ToArray(), commandType);
    }

    public DataTable GetDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure)
    {
        return GetDataTable(commandText, new List<SqlParameter>().ToArray(), commandType);
    }

    public List<T> GetList<T>(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure) where T : new()
    {
        var dataTable = GetDataTable(commandText, parameters, commandType);
        return ConvertDataTableToList<T>(dataTable);
    }

    public List<T> GetList<T>(string commandText, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure) where T : new()
    {
        var dataTable = GetDataTable(commandText, parameters, commandType);
        return ConvertDataTableToList<T>(dataTable);
    }

    public List<T> GetList<T>(string commandText, CommandType commandType = CommandType.StoredProcedure) where T : new()
    {
        var dataTable = GetDataTable(commandText, commandType);
        return ConvertDataTableToList<T>(dataTable);
    }

    private List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }

    private T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                    pro.SetValue(obj, dr[column.ColumnName], null);
                else
                    continue;
            }
        }

        return obj;
    }
}