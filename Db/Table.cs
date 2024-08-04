
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVdb.Db
{
  public class Table
  {
    public string name = (string) null;
    public ArrayList rows = new ArrayList();
    public ArrayList names = new ArrayList();
    public Dictionary<string, int> namesIndex = new Dictionary<string, int>();
    public Dictionary<int, Row> index = new Dictionary<int, Row>();
    public int databaseIndex = -1;

    public void write(Stream output)
    {
      int num1 = 0;
      foreach (string name in this.names)
      {
        string str = "";
        if (num1 > 0)
          str += ",";
        byte[] bytes = Encoding.Default.GetBytes(str + "\"" + name + "\"");
        output.Write(bytes, 0, bytes.Length);
        checked { ++num1; }
      }
      foreach (Row row in this.rows)
      {
        int num2 = 0;
        byte[] bytes1 = Encoding.Default.GetBytes("\n");
        output.Write(bytes1, 0, bytes1.Length);
        foreach (Column column in row.columns)
        {
          string str = "";
          if (num2 > 0)
            str += ",";
          byte[] bytes2 = Encoding.Default.GetBytes(str + column.value);
          output.Write(bytes2, 0, bytes2.Length);
          checked { ++num2; }
        }
      }
    }
  }
}
