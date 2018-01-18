using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Data;


namespace ClassLibrary.Core
{
    [Serializable()]
    public class DBConnection : IDisposable
    {
        [NonSerialized]
        public OdbcConnection con;
        [NonSerialized]
        public OdbcCommand cmd;
        [NonSerialized]
        public OdbcTransaction tr;
        [NonSerialized]
        public OdbcDataReader dr;

        public DBConnection()
        {
            con = new OdbcConnection(System.Configuration.ConfigurationSettings.AppSettings["dbConString"].ToString());
            cmd = new OdbcCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                this.con.Open();
                this.tr = con.BeginTransaction();
                this.cmd.Transaction = tr;
            }
            catch (Exception ex)
            {
                this.RollBack();
            }
        }
            public DBConnection(string connectionString)
            {
                con = new OdbcConnection(connectionString);
                cmd = new OdbcCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                try
                {
                    this.con.Open();
                    this.tr = con.BeginTransaction();
                    this.cmd.Transaction = tr;
                }
                catch (Exception ex)
                {
                        this.RollBack();
                }
            }
        public void Commit()
        {
            tr.Commit();
            this.cmd.Dispose();
            this.con.Close();
        }
        public void RollBack()
        {
            this.tr.Rollback();
            this.cmd.Dispose();
            this.con.Close();
        }


       

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
