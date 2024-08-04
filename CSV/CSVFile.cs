
using CSVdb.Db;
using System.Collections;
using System.Collections.Generic;

namespace CSVdb.CSV
{
  public class CSVFile
  {
    public string name = (string) null;
    public ArrayList names = new ArrayList();
    public Dictionary<string, int> namesIndex = new Dictionary<string, int>();
    public ArrayList lines = new ArrayList();

    public CSVFile(ParsingStream stream, string name)
    {
      this.name = name;
      CSVLine csvLine1 = new CSVLine(stream);
      int num = 0;
      foreach (string key in csvLine1.values)
      {
        this.names.Add((object) key);
        this.namesIndex.Add(key, checked (num++));
      }
      while (!stream.atEnd())
      {
        CSVLine csvLine2 = new CSVLine(stream);
        if (csvLine2.values.Count > 0)
          this.lines.Add((object) csvLine2);
      }
    }

    public Table toTable()
    {
      Table table = new Table();
      table.name = this.name;
      int num = 0;
      foreach (string name in this.names)
      {
        table.names.Add((object) name);
        table.namesIndex.Add(name, checked (num++));
      }
      foreach (CSVLine line in this.lines)
      {
        Row row = new Row();
        foreach (string str in line.values)
          row.columns.Add((object) new Column(str));
        table.rows.Add((object) row);
        row.rowid = line.rowid;
        table.index.Add(row.rowid, row);
        row.table = table;
      }
      return table;
    }
  }
}
