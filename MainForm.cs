
#define TRIAL

using CSVdb.CSV;
using CSVdb.Db;
using CSVdb.Query;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#if TRIAL
using Microsoft.Win32;
#endif

namespace CSVdb
{
  public class MainForm : Form
  {
    private IContainer components = (IContainer) null;
    private RichTextBox richTextBoxHelp;
    private SaveFileDialog saveFileDialog;
    private OpenFileDialog openFileDialog;
    private RichTextBox richTextBoxQuery;
    private Label labelQuery;
    private TextBox textBoxResult;
    private Button buttonResult;
    private Label labelResult;
    private ListBox listBoxSource;
    private Button buttonSourceAdd;
    private Button buttonSourceRemove;
    private Label labelSource;
    private Button buttonRun;
    private TabPage tabPageHelp;
    private TabPage tabPageMain;
    private TabControl tabControl;

    public MainForm() {
    	this.InitializeComponent();
    }

    private void ButtonRunClick(object sender, EventArgs e)
    {
    	if (this.textBoxResult.Text.Length == 0) {
    		MessageBox.Show("Please specify the output file");
    		return;
    	}
    	
      Database database = new Database();
      foreach (string path in this.listBoxSource.Items)
      {
        FileStream fileStream = File.Open(path, FileMode.Open);
        string[] strArray = path.Split('.', '\\', '/');
        CSVFile file = new CSVFile(new ParsingStream((Stream) fileStream), strArray[checked (strArray.Length - 2)]);
        fileStream.Close();
        database.addCSVFile(file, strArray[checked (strArray.Length - 2)]);
      }
      Node node = new Parser(new ParsingStream((Stream) new MemoryStream(Encoding.Default.GetBytes(this.richTextBoxQuery.Text)))).parse();
      Table table = new CSVdb.Db.Cursor(database).run(node);
      FileStream output = File.Open(this.textBoxResult.Text, FileMode.OpenOrCreate);
      table.write((Stream) output);
      output.Close();
    }

    private void ButtonSourceAddClick(object sender, EventArgs e)
    {
      if (this.openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.listBoxSource.Items.Add((object) this.openFileDialog.FileName);
    }

    private void ButtonSourceRemoveClick(object sender, EventArgs e)
    {
      this.listBoxSource.Items.Remove(this.listBoxSource.SelectedItem);
    }

    private void ButtonResultClick(object sender, EventArgs e)
    {
      if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.textBoxResult.Text = this.saveFileDialog.FileName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
    	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
    	this.tabControl = new System.Windows.Forms.TabControl();
    	this.tabPageMain = new System.Windows.Forms.TabPage();
    	this.richTextBoxQuery = new System.Windows.Forms.RichTextBox();
    	this.labelQuery = new System.Windows.Forms.Label();
    	this.buttonResult = new System.Windows.Forms.Button();
    	this.textBoxResult = new System.Windows.Forms.TextBox();
    	this.labelResult = new System.Windows.Forms.Label();
    	this.buttonSourceRemove = new System.Windows.Forms.Button();
    	this.buttonSourceAdd = new System.Windows.Forms.Button();
    	this.listBoxSource = new System.Windows.Forms.ListBox();
    	this.labelSource = new System.Windows.Forms.Label();
    	this.buttonRun = new System.Windows.Forms.Button();
    	this.tabPageHelp = new System.Windows.Forms.TabPage();
    	this.richTextBoxHelp = new System.Windows.Forms.RichTextBox();
    	this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
    	this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
    	this.tabControl.SuspendLayout();
    	this.tabPageMain.SuspendLayout();
    	this.tabPageHelp.SuspendLayout();
    	this.SuspendLayout();
    	// 
    	// tabControl
    	// 
    	this.tabControl.Controls.Add(this.tabPageMain);
    	this.tabControl.Controls.Add(this.tabPageHelp);
    	this.tabControl.Location = new System.Drawing.Point(3, 0);
    	this.tabControl.Margin = new System.Windows.Forms.Padding(4);
    	this.tabControl.Name = "tabControl";
    	this.tabControl.SelectedIndex = 0;
    	this.tabControl.Size = new System.Drawing.Size(456, 398);
    	this.tabControl.TabIndex = 0;
    	// 
    	// tabPageMain
    	// 
    	this.tabPageMain.Controls.Add(this.richTextBoxQuery);
    	this.tabPageMain.Controls.Add(this.labelQuery);
    	this.tabPageMain.Controls.Add(this.buttonResult);
    	this.tabPageMain.Controls.Add(this.textBoxResult);
    	this.tabPageMain.Controls.Add(this.labelResult);
    	this.tabPageMain.Controls.Add(this.buttonSourceRemove);
    	this.tabPageMain.Controls.Add(this.buttonSourceAdd);
    	this.tabPageMain.Controls.Add(this.listBoxSource);
    	this.tabPageMain.Controls.Add(this.labelSource);
    	this.tabPageMain.Controls.Add(this.buttonRun);
    	this.tabPageMain.Location = new System.Drawing.Point(4, 25);
    	this.tabPageMain.Margin = new System.Windows.Forms.Padding(4);
    	this.tabPageMain.Name = "tabPageMain";
    	this.tabPageMain.Padding = new System.Windows.Forms.Padding(4);
    	this.tabPageMain.Size = new System.Drawing.Size(448, 369);
    	this.tabPageMain.TabIndex = 0;
    	this.tabPageMain.Text = "Main";
    	this.tabPageMain.UseVisualStyleBackColor = true;
    	// 
    	// richTextBoxQuery
    	// 
    	this.richTextBoxQuery.Location = new System.Drawing.Point(23, 209);
    	this.richTextBoxQuery.Margin = new System.Windows.Forms.Padding(4);
    	this.richTextBoxQuery.Name = "richTextBoxQuery";
    	this.richTextBoxQuery.Size = new System.Drawing.Size(407, 84);
    	this.richTextBoxQuery.TabIndex = 9;
    	this.richTextBoxQuery.Text = "Fruits.Fruit = \'Apple\' & Stores.Distance > 25";
    	// 
    	// labelQuery
    	// 
    	this.labelQuery.Location = new System.Drawing.Point(23, 187);
    	this.labelQuery.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
    	this.labelQuery.Name = "labelQuery";
    	this.labelQuery.Size = new System.Drawing.Size(68, 18);
    	this.labelQuery.TabIndex = 8;
    	this.labelQuery.Text = "Query:";
    	// 
    	// buttonResult
    	// 
    	this.buttonResult.Location = new System.Drawing.Point(385, 146);
    	this.buttonResult.Margin = new System.Windows.Forms.Padding(4);
    	this.buttonResult.Name = "buttonResult";
    	this.buttonResult.Size = new System.Drawing.Size(47, 25);
    	this.buttonResult.TabIndex = 7;
    	this.buttonResult.Text = "...";
    	this.buttonResult.UseVisualStyleBackColor = true;
    	this.buttonResult.Click += new System.EventHandler(this.ButtonResultClick);
    	// 
    	// textBoxResult
    	// 
    	this.textBoxResult.Location = new System.Drawing.Point(23, 146);
    	this.textBoxResult.Margin = new System.Windows.Forms.Padding(4);
    	this.textBoxResult.Name = "textBoxResult";
    	this.textBoxResult.Size = new System.Drawing.Size(353, 22);
    	this.textBoxResult.TabIndex = 6;
    	// 
    	// labelResult
    	// 
    	this.labelResult.Location = new System.Drawing.Point(23, 123);
    	this.labelResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
    	this.labelResult.Name = "labelResult";
    	this.labelResult.Size = new System.Drawing.Size(101, 20);
    	this.labelResult.TabIndex = 5;
    	this.labelResult.Text = "Result:";
    	// 
    	// buttonSourceRemove
    	// 
    	this.buttonSourceRemove.Location = new System.Drawing.Point(385, 78);
    	this.buttonSourceRemove.Margin = new System.Windows.Forms.Padding(4);
    	this.buttonSourceRemove.Name = "buttonSourceRemove";
    	this.buttonSourceRemove.Size = new System.Drawing.Size(47, 30);
    	this.buttonSourceRemove.TabIndex = 4;
    	this.buttonSourceRemove.Text = "-";
    	this.buttonSourceRemove.UseVisualStyleBackColor = true;
    	this.buttonSourceRemove.Click += new System.EventHandler(this.ButtonSourceRemoveClick);
    	// 
    	// buttonSourceAdd
    	// 
    	this.buttonSourceAdd.Location = new System.Drawing.Point(385, 38);
    	this.buttonSourceAdd.Margin = new System.Windows.Forms.Padding(4);
    	this.buttonSourceAdd.Name = "buttonSourceAdd";
    	this.buttonSourceAdd.Size = new System.Drawing.Size(47, 31);
    	this.buttonSourceAdd.TabIndex = 3;
    	this.buttonSourceAdd.Text = "+";
    	this.buttonSourceAdd.UseVisualStyleBackColor = true;
    	this.buttonSourceAdd.Click += new System.EventHandler(this.ButtonSourceAddClick);
    	// 
    	// listBoxSource
    	// 
    	this.listBoxSource.FormattingEnabled = true;
    	this.listBoxSource.ItemHeight = 16;
    	this.listBoxSource.Items.AddRange(new object[] {
			"Fruits.csv",
			"Stores.csv"});
    	this.listBoxSource.Location = new System.Drawing.Point(23, 38);
    	this.listBoxSource.Margin = new System.Windows.Forms.Padding(4);
    	this.listBoxSource.Name = "listBoxSource";
    	this.listBoxSource.Size = new System.Drawing.Size(353, 68);
    	this.listBoxSource.TabIndex = 2;
    	// 
    	// labelSource
    	// 
    	this.labelSource.Location = new System.Drawing.Point(23, 17);
    	this.labelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
    	this.labelSource.Name = "labelSource";
    	this.labelSource.Size = new System.Drawing.Size(76, 17);
    	this.labelSource.TabIndex = 1;
    	this.labelSource.Text = "Source:";
    	// 
    	// buttonRun
    	// 
    	this.buttonRun.Location = new System.Drawing.Point(159, 316);
    	this.buttonRun.Margin = new System.Windows.Forms.Padding(4);
    	this.buttonRun.Name = "buttonRun";
    	this.buttonRun.Size = new System.Drawing.Size(119, 32);
    	this.buttonRun.TabIndex = 0;
    	this.buttonRun.Text = "Run";
    	this.buttonRun.UseVisualStyleBackColor = true;
    	this.buttonRun.Click += new System.EventHandler(this.ButtonRunClick);
    	// 
    	// tabPageHelp
    	// 
    	this.tabPageHelp.Controls.Add(this.richTextBoxHelp);
    	this.tabPageHelp.Location = new System.Drawing.Point(4, 25);
    	this.tabPageHelp.Margin = new System.Windows.Forms.Padding(4);
    	this.tabPageHelp.Name = "tabPageHelp";
    	this.tabPageHelp.Padding = new System.Windows.Forms.Padding(4);
    	this.tabPageHelp.Size = new System.Drawing.Size(448, 369);
    	this.tabPageHelp.TabIndex = 1;
    	this.tabPageHelp.Text = "Help";
    	this.tabPageHelp.UseVisualStyleBackColor = true;
    	// 
    	// richTextBoxHelp
    	// 
    	this.richTextBoxHelp.Location = new System.Drawing.Point(8, 7);
    	this.richTextBoxHelp.Margin = new System.Windows.Forms.Padding(4);
    	this.richTextBoxHelp.Name = "richTextBoxHelp";
    	this.richTextBoxHelp.ReadOnly = true;
    	this.richTextBoxHelp.Size = new System.Drawing.Size(428, 350);
    	this.richTextBoxHelp.TabIndex = 0;
    	this.richTextBoxHelp.Text = "\n\"&\" - logical AND\n\n\"|\" - logical OR\n\n\"~\" - logical NOT\n\n\"( )\" - grouping\n\n\"<\" - " +
	"less\n\n\">\" - greater\n\n\"=\" - equal\n\n\"<table>.<column>\" - field\n\n\" \'...\' \" - string" +
	"\n\n#.##... - number";
    	// 
    	// openFileDialog
    	// 
    	this.openFileDialog.FileName = "openFileDialog1";
    	this.openFileDialog.Filter = "CSV | *.csv";
    	// 
    	// saveFileDialog
    	// 
    	this.saveFileDialog.Filter = "CSV | *.csv";
    	// 
    	// MainForm
    	// 
    	this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
    	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    	this.ClientSize = new System.Drawing.Size(461, 394);
    	this.Controls.Add(this.tabControl);
    	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
    	this.Margin = new System.Windows.Forms.Padding(4);
    	this.MaximizeBox = false;
    	this.MinimizeBox = false;
    	this.Name = "MainForm";
    	this.Text = "CSVdb";
    	this.Shown += new System.EventHandler(this.MainFormShown);
    	this.tabControl.ResumeLayout(false);
    	this.tabPageMain.ResumeLayout(false);
    	this.tabPageMain.PerformLayout();
    	this.tabPageHelp.ResumeLayout(false);
    	this.ResumeLayout(false);

    }
    
    #if TRIAL
    public void CheckRuns() {
		try {
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\OVG-Developers", true);
			
			int runs = -1;
			
			if (key != null && key.GetValue("Runs") != null) {
				runs = (int) key.GetValue("Runs");
			} else {
				key = Registry.CurrentUser.CreateSubKey("Software\\OVG-Developers");
			}
			
			runs = runs + 1;
			
			key.SetValue("Runs", runs);
			
			if (runs > 30) {
				System.Windows.Forms.MessageBox.Show("Number of runs expired.\n"
							+ "Please register the application (visit https://ovg-developers.mystrikingly.com/ for purchase).");
				
				Environment.Exit(0);
			}
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
	}
	
	public bool IsRegistered() {
		try {
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\OVG-Developers");
			
			if (key != null && key.GetValue("Registered") != null) {
				return true;
			}
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
		
		return false;
	}
    #endif
    
		void MainFormShown(object sender, EventArgs e)
		{
			#if TRIAL
			if (!IsRegistered()) {
    			CheckRuns();
    		}
			#endif
		}
  }
}
