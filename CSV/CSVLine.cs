
using System.Collections;

namespace CSVdb.CSV
{
  public class CSVLine
  {
    public static int ROW_ID = -1;
    public ArrayList values = new ArrayList();
    public int rowid;

    public CSVLine(ParsingStream stream)
    {
      while (!stream.atEnd())
      {
        string str = this.parseValue(stream);
        if (str.Length > 0)
          this.values.Add((object) str);
        if (stream.current == 10)
        {
          stream.Read();
          break;
        }
      }
      this.rowid = checked (++CSVLine.ROW_ID);
    }

    public string parseValue(ParsingStream stream)
    {
      string str = "";
      stream.parseBlanks();
      int num = -1;
      if (stream.current == 34 || stream.current == 39)
      {
        num = stream.current;
        stream.Read();
      }
      if (num == -1)
      {
        while (!stream.atEnd() && ",;".IndexOf(checked ((char) stream.current)) == -1 && stream.current != 10)
        {
          if (stream.current == 92)
          {
            stream.Read();
            str += checked ((char) stream.current);
          }
          else
            str += checked ((char) stream.current);
          stream.Read();
        }
      }
      else
      {
        while (!stream.atEnd() && stream.current != 10 && stream.current != num && ",;".IndexOf(checked ((char) stream.current)) == -1)
        {
          if (stream.current == 92)
          {
            stream.Read();
            str += checked ((char) stream.current);
          }
          else
            str += checked ((char) stream.current);
          stream.Read();
        }
        if (stream.current == num)
          stream.Read();
      }
      stream.parseBlanks();
      if (",; \t".IndexOf(checked ((char) stream.current)) >= 0)
        stream.Read();
      stream.parseBlanks();
      return str.Trim();
    }
  }
}
