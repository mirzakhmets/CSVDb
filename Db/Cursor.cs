
using CSVdb.CSV;
using System.Collections.Generic;

namespace CSVdb.Db
{
  public class Cursor
  {
    public Database database;
    public Table[] tables;
    public int[] index;
    public Dictionary<string, int> names = new Dictionary<string, int>();

    public Cursor(Database database)
    {
      this.database = database;
      this.tables = new Table[this.database.files.Count];
      this.index = new int[this.database.files.Count];
      int index = 0;
      foreach (CSVFile file in this.database.files)
      {
        this.index[index] = 0;
        this.tables[index] = file.toTable();
        checked { ++index; }
      }
    }

    public bool atEnd()
    {
      int index = 0;
      while (index < this.index.Length)
      {
        if (this.index[index] != 0)
          return false;
        checked { ++index; }
      }
      return true;
    }

    public void increment()
    {
      int index = checked (this.index.Length - 1);
      while (index >= 0 && checked (++this.index[index]) >= this.tables[index].rows.Count)
      {
        this.index[index] = 0;
        checked { --index; }
      }
    }

    public Table run(CSVdb.Query.Node node)
    {
      Table table1 = new Table();
      int num1 = 0;
      int index1 = 0;
      while (index1 < this.index.Length)
      {
        Table table2 = (Table) this.database.tables[index1];
        foreach (string name in table2.names)
          this.names.Add(table2.name + "." + name, checked (num1++));
        checked { ++index1; }
      }
      int num2 = 0;
      foreach (string key in this.names.Keys)
      {
        table1.names.Add((object) key);
        table1.namesIndex.Add(key, checked (num2++));
      }
      do
      {
        if (node.accepts(this))
        {
          Row row = new Row();
          int index2 = 0;
          while (index2 < this.index.Length)
          {
            foreach (Column column in ((Row) this.tables[index2].rows[this.index[index2]]).columns)
              row.columns.Add((object) column);
            checked { ++index2; }
          }
          table1.rows.Add((object) row);
        }
        this.increment();
      }
      while (!this.atEnd());
      return table1;
    }
  }
}
