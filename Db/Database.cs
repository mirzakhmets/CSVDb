
using CSVdb.CSV;
using System.Collections;
using System.Collections.Generic;

namespace CSVdb.Db
{
  public class Database
  {
    public ArrayList files = new ArrayList();
    public ArrayList tables = new ArrayList();
    public Dictionary<string, Table> tablesIndex = new Dictionary<string, Table>();

    public Table getTable(string name)
    {
      if (!this.tablesIndex.ContainsKey(name))
      {
        CSVFile csvFile = (CSVFile) null;
        foreach (CSVFile file in this.files)
        {
          if (file.name.Equals(name))
          {
            csvFile = file;
            break;
          }
        }
        if (csvFile != null)
        {
          Table table = csvFile.toTable();
          this.tables.Add((object) table);
          this.tablesIndex.Add(name, table);
          table.databaseIndex = checked (this.tables.Count - 1);
        }
      }
      return this.tablesIndex[name];
    }

    public void addCSVFile(CSVFile file, string name)
    {
      this.files.Add((object) file);
      Table table = file.toTable();
      this.tables.Add((object) table);
      this.tablesIndex.Add(name, table);
      table.databaseIndex = checked (this.tables.Count - 1);
    }
  }
}
