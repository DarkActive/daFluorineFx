/*
	FluorineFx open source library 
	Copyright (C) 2007 Zoltan Csibi, zoltan@TheSilentGroup.com, FluorineFx.com 
	
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	Lesser General Public License for more details.
	
	You should have received a copy of the GNU Lesser General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace FluorineFx.Management.Data.Database.MSSql
{
	/// <summary>
	/// Summary description for MSSqlDriver.
	/// </summary>
	public class MSSqlDriver : Driver
	{
		public MSSqlDriver(DomainUrl domainUrl) : base(domainUrl)
		{
		}

		public override IDbConnection CreateConnection()
		{
            StringBuilder sb = new StringBuilder();
            /*
            string database;
            if (this.DomainUrl.Host.ToLower() == "localhost")
                database = Path.Combine(HttpRuntime.AppDomainAppPath, this.DomainUrl.Database);
            else
                database = this.DomainUrl.Database;
            sb.AppendFormat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", database);

            if (this.DomainUrl.Password != null && this.DomainUrl.Password != string.Empty)
                sb.AppendFormat("Jet OLEDB:Database Password={0}", this.DomainUrl.Password);
            else
                sb.AppendFormat("User Id={0}; Password=", this.DomainUrl.User);
             */
            return new SqlConnection(sb.ToString());
        }

		public override void ConfigureConnection(IDbConnection connection)
		{
			//NA
		}

        public override IDbDataAdapter GetDbDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public override IDbCommand GetDbCommand(string cmdText, IDbConnection connection)
        {
            SqlCommand command = new SqlCommand(cmdText, connection as SqlConnection);
            return command;
        }

        public override string ConnectionClass
        {
            get
            {
                return typeof(SqlConnection).FullName;
            }
        }

        public override string CommandClass
        {
            get
            {
                return typeof(SqlCommand).FullName;
            }
        }

        public override string DataReaderClass
        {
            get
            {
                return typeof(SqlDataReader).FullName;
            }
        }

        public override string GetDataReaderAccessor(Column column)
        {
            return string.Empty;
        }

        public override string IdentityQuery
        {
            get
            {
                return "select @@identity";
            }
        }

        public override string GetCommand(Column column, string parameterName, System.Data.ParameterDirection parameterDirection, DataRowVersion dataRowVersion, bool isNullable)
        {
            SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), column.OriginalSQLType);
            return string.Format("System.Data.SqlClient.SqlParameter(\"{0}\", System.Data.SqlDbType.{1}, {2}, System.Data.ParameterDirection.{3}, {4}, (System.Byte)({5}), (System.Byte)({6}), \"{7}\", System.Data.DataRowVersion.{8}, null)",
                parameterName, /*The name of the parameter*/
                sqlDbType.ToString(), /*One of the OleDbType values*/
                column.Length.ToString(), /*The length of the parameter*/
                parameterDirection.ToString(), /*One of the ParameterDirection values*/
                isNullable.ToString(),
                column.Precision.ToString(), /*The total number of digits to the left and right of the decimal point to which Value is resolved*/
                column.Scale.ToString(), /*The total number of decimal places to which Value is resolved*/
                column.Name, /*The name of the source column*/
                dataRowVersion.ToString()
                );
        }
	}
}
