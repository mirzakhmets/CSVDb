
namespace CSVdb.Query
{
  public class Parser
  {
    public ParsingStream stream;

    public Parser(ParsingStream stream) {
    	this.stream = stream;
    }

    public void parseBlanks()
    {
      while (!this.stream.atEnd() && " \r\n\t\v".IndexOf(checked ((char) this.stream.current)) >= 0)
        this.stream.Read();
    }

    public Node parseNode()
    {
      this.parseBlanks();
      if (this.stream.atEnd())
        return (Node) null;
      return this.stream.current != 40 ? (this.stream.current != 34 && this.stream.current != 39 ? ("0123456789".IndexOf(checked ((char) this.stream.current)) < 0 ? (this.stream.current != 126 ? this.parseField() : this.parseNot()) : this.parseConstNumber()) : this.parseConstString()) : this.parseGroup();
    }

    public Node parseNot()
    {
      Node not = (Node) null;
      if (this.stream.current == 126)
      {
        this.stream.Read();
        Node node = this.parseNode();
        not = new Node(NodeType.NOT);
        not.left = node;
      }
      return not;
    }

    public Node parseField()
    {
      Node field = (Node) null;
      string str = "";
      while (!this.stream.atEnd() && "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_.".IndexOf(checked ((char) this.stream.current)) >= 0)
      {
        str += checked ((char) this.stream.current);
        this.stream.Read();
      }
      if (str.Length > 0)
      {
        field = new Node(NodeType.FIELD);
        field.field = str;
      }
      return field;
    }

    public Node parseConstNumber()
    {
      Node constNumber = (Node) null;
      string s = "";
      while (!this.stream.atEnd() && "0123456789.e+-E".IndexOf(checked ((char) this.stream.current)) >= 0)
      {
        s += checked ((char) this.stream.current);
        this.stream.Read();
      }
      double result;
      if (s.Length > 0 && double.TryParse(s, out result))
      {
        constNumber = new Node(NodeType.CONST_NUMBER);
        constNumber.realValue = result;
      }
      return constNumber;
    }

    public Node parseConstString()
    {
      Node constString = (Node) null;
      if (this.stream.current == 34 || this.stream.current == 39)
      {
        int current = (int) checked ((ushort) this.stream.current);
        this.stream.Read();
        string str = "";
        while (!this.stream.atEnd() && this.stream.current != current)
        {
          str += checked ((char) this.stream.current);
          this.stream.Read();
        }
        if (this.stream.current == current)
          this.stream.Read();
        constString = new Node(NodeType.CONST_STRING);
        constString.stringValue = str;
      }
      return constString;
    }

    public Node parseGroup()
    {
      if (this.stream.current != 40)
        return (Node) null;
      this.stream.Read();
      this.parseBlanks();
      Node group = this.parse();
      if (this.stream.current == 41)
        this.stream.Read();
      if (group == null)
        return group;
      return new Node(NodeType.GROUP) { left = group };
    }

    public Node parseEq()
    {
      Node node = this.parseNode();
      Node eq = (Node) null;
      NodeType type = NodeType.EQ;
      this.parseBlanks();
      if (this.stream.current == 61)
      {
        this.stream.Read();
        eq = this.parseNode();
      }
      else if (this.stream.current == 60)
      {
        this.stream.Read();
        eq = this.parseNode();
        type = NodeType.LESS;
      }
      else if (this.stream.current == 62)
      {
        this.stream.Read();
        eq = this.parseNode();
        type = NodeType.GREATER;
      }
      if (node == null)
        return eq;
      if (eq == null)
        return node;
      return new Node(type) { left = node, right = eq };
    }

    public bool isMeta()
    {
      return !this.stream.atEnd() && ")|&.~=\"'0123456789+-eE".IndexOf(checked ((char) this.stream.current)) >= 0;
    }

    public Node parseOr()
    {
      Node eq = this.parseEq();
      this.parseBlanks();
      if (eq != null && (!this.isMeta() || this.stream.current == 124))
      {
        this.stream.Read();
        Node or = this.parseOr();
        if (or != null)
          return new Node(NodeType.OR)
          {
            left = eq,
            right = or
          };
      }
      return eq;
    }

    public Node parseAnd()
    {
      Node or = this.parseOr();
      this.parseBlanks();
      if (or != null && (!this.isMeta() || this.stream.current == 38))
      {
        this.stream.Read();
        Node and = this.parseAnd();
        if (and != null)
          return new Node(NodeType.AND)
          {
            left = or,
            right = and
          };
      }
      return or;
    }

    public Node parse() {
    	return this.parseAnd();
    }
  }
}
