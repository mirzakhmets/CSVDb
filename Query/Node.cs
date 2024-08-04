
using CSVdb.Db;

namespace CSVdb.Query
{
  public class Node
  {
    public NodeType type;
    public Node left;
    public Node right;
    public string stringValue;
    public double realValue;
    public string field;

    public Node(NodeType type) {
    	this.type = type;
    }

    public string getValue(Node node, Cursor cursor)
    {
      if (node.type == NodeType.CONST_NUMBER)
        return string.Concat((object) node.realValue);
      if (node.type == NodeType.CONST_STRING)
        return node.stringValue;
      if (node.type != NodeType.FIELD)
        return (string) null;
      string[] strArray = node.field.Split('.');
      Table table = cursor.database.getTable(strArray[0]);
      return ((Column) ((Row) table.rows[cursor.index[table.databaseIndex]]).columns[table.namesIndex[strArray[1]]]).value;
    }

    public static int compare(string a, string b)
    {
      double result1;
      double result2;
      if (!double.TryParse(a, out result1) || !double.TryParse(b, out result2))
        return a.CompareTo(b);
      return result1 == result2 ? 0 : (result1 < result2 ? -1 : 1);
    }

    public bool accepts(Cursor cursor)
    {
      switch (this.type)
      {
        case NodeType.AND:
          return this.left.accepts(cursor) && this.right.accepts(cursor);
        case NodeType.OR:
          return this.left.accepts(cursor) || this.right.accepts(cursor);
        case NodeType.NOT:
          return !this.left.accepts(cursor);
        case NodeType.GROUP:
          return this.left.accepts(cursor);
        case NodeType.LESS:
          return Node.compare(this.left.getValue(this.left, cursor), this.right.getValue(this.right, cursor)) < 0;
        case NodeType.GREATER:
          return Node.compare(this.left.getValue(this.left, cursor), this.right.getValue(this.right, cursor)) > 0;
        case NodeType.EQ:
          return Node.compare(this.left.getValue(this.left, cursor), this.right.getValue(this.right, cursor)) == 0;
        default:
          return true;
      }
    }
  }
}
