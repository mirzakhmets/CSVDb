
using CSVdb.Db;
using System.Collections.Generic;

namespace CSVdb.Query
{
  public class Result
  {
    public Database database;
    public Dictionary<int, Row> rowids = new Dictionary<int, Row>();

    public Result(Database database) {
    	this.database = database;
    }

    public void imply() {
    	this.imply(this.database);
    }

    public void imply(Database database)
    {
      foreach (Table table in database.tables)
        this.imply(table);
    }

    public void imply(Table table)
    {
      foreach (Row row in table.rows)
        this.rowids.Add(row.rowid, row);
    }
  }
}
