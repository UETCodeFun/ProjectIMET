using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordPad_SonPX
{
    public partial class FrmMain : Form
    {
        private string appName = "WordPad_SonPX";
        private bool isFileExist;
        private bool isFileChanged;
        private string currentFileName;
        public FrmMain()
        {
            InitializeComponent();
            LoadFonts();
            LoadSize();
            LoadStyles();
            FormLoadFuntion();
        }
        public string Data
        {
            get { return ((RichTextBox)tabControl.SelectedTab.Controls[0]).Text; }
        }
        public RichTextBox DataTextBox
        {
            get
            {
                return ((RichTextBox)tabControl.SelectedTab.Controls[0]);
            }
        }
        private List<Document> documents = new List<Document>();

        private void SetSettingsForText()
        {
            if (cmbFontCollections.Items.Count >0 && cmbFontSize.Items.Count > 0 && cmbFontStyle.Items.Count > 0 && tabControl.TabCount > 0)
            {
                ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont = new Font((string)cmbFontCollections.SelectedItem, Single.Parse(cmbFontSize.Text), GetFontStyle());
            }else if(cmbFontCollections.Items.Count == 0 && cmbFontSize.Items.Count == 0 && cmbFontStyle.Items.Count == 0 && tabControl.TabCount > 0)
            {
                ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont = new Font((string)cmbFontCollections.SelectedItem, Single.Parse(cmbFontSize.Text), GetFontStyle());
            }
        }
        private FontStyle GetFontStyle()
        {
            FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), cmbFontStyle.SelectedItem.ToString());
            if (btnInDam.Checked)
            {
                fontStyle = fontStyle | FontStyle.Bold;
            }
            if (btnInNghieng.Checked)
            {
                fontStyle = fontStyle | FontStyle.Italic;
            }
            if (btnGachChan.Checked)
            {
                fontStyle = fontStyle | FontStyle.Underline;
            }
            return fontStyle;
        }
        private void SaveAsFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";
            DialogResult result = saveFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt")
                    rtbHienThiText.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                if (Path.GetExtension(saveFileDialog.FileName) == ".rtf")
                    rtbHienThiText.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
                this.Text = Path.GetFileName(saveFileDialog.FileName) + appName;
                isFileExist = true;
                isFileChanged = false;
                currentFileName = saveFileDialog.FileName;
            }
        }
        private void SaveFile()
        {
            if (isFileExist)
            {
                if(Path.GetExtension(currentFileName) == ".txt")
                {
                    rtbHienThiText.SaveFile(currentFileName, RichTextBoxStreamType.PlainText);
                }
                if(Path.GetExtension(currentFileName) == ".rtf"){
                    rtbHienThiText.SaveFile(currentFileName, RichTextBoxStreamType.RichText);
                }
                isFileChanged = false;

            }
            else
            {
                if (isFileChanged)
                {
                    SaveAsFile();
                }
                else
                {
                    ClearEditor();
                }
            }
        }
        private void ClearEditor()
        {
            rtbHienThiText.Clear();
            this.Text = "Untitled" + appName;
            isFileChanged = false;
            isFileExist = false;
            currentFileName = "";
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show("Do you want to save your file", "Thong bao",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveFile();
                        ClearEditor();
                        break;
                    case DialogResult.No:
                        ClearEditor();
                        break;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";
            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                    rtbHienThiText.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                    rtbHienThiText.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
                this.Text = Path.GetFileName(openFileDialog.FileName) + appName;
                isFileExist = true;
                isFileChanged = false;
                currentFileName = openFileDialog.FileName;
                btnUndo.Enabled = false;
                btnRedo.Enabled = false;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //lblShowCap.Text = "";
            //isFileExist = false;
            //isFileChanged = false;
            //currentFileName = "";
            //if (Control.IsKeyLocked(Keys.CapsLock))
            //{
            //    lblShowCap.Text = "CAPSLOCK IS ON";
            //}
            //else
            //{
            //    lblShowCap.Text = "CAPSLOCK IS OFF";
            //}
        }

        private void btnInDam_Click(object sender, EventArgs e)
        {
            rtbHienThiText.SelectionFont = new Font(rtbHienThiText.Font, FontStyle.Bold);
        }

        private void rtbHienThiText_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                btnInDam.Checked = ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Bold;
                btnInNghieng.Checked = ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Italic;
                btnGachChan.Checked = ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Underline;
                if (((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Strikeout)
                {
                    cmbFontStyle.SelectedIndex = 3;
                }
                cmbFontCollections.Text = ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Name;
                cmbFontSize.Text = ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionFont.Size.ToString();

            }
            catch(NullReferenceException exception)
            {
                MessageBox.Show("Null Reference exception", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnInNghieng_Click(object sender, EventArgs e)
        {
            rtbHienThiText.SelectionFont = new Font(rtbHienThiText.Font, FontStyle.Italic);
        }

        private void btnGachChan_Click(object sender, EventArgs e)
        {
            rtbHienThiText.SelectionFont = new Font(rtbHienThiText.Font, FontStyle.Underline);
        }

        private void cmbFontCollections_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSettingsForText();
        }


        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            SetSettingsForText();
        }
        private void LoadFonts()
        {
            var fontsCollection = new InstalledFontCollection();
            var ff = fontsCollection.Families;
            foreach (var item in ff)
            {
                cmbFontCollections.Items.Add(item.Name);
            }
            cmbFontCollections.SelectedIndex = 61;
        }
        private void LoadSize()
        {
            for(int i = 8; i <= 100; i++)
            {
                cmbFontSize.Items.Add(i);
            }
            cmbFontSize.SelectedIndex = 3;
        }
        private void LoadStyles()
        {
            cmbFontStyle.Items.Add(FontStyle.Regular.ToString());
            cmbFontStyle.Items.Add(FontStyle.Bold.ToString());
            cmbFontStyle.Items.Add(FontStyle.Italic.ToString());
            cmbFontStyle.Items.Add(FontStyle.Underline.ToString());
            cmbFontStyle.Items.Add(FontStyle.Strikeout.ToString());
            cmbFontStyle.SelectedIndex = 0;
        }
        private void FormLoadFuntion()
        {
            var newDoc = new Document();
            tabControl.SelectedTab.Tag = newDoc;
            documents.Add(newDoc);
        }
        private void rtbHienThiText_TextChanged_1(object sender, EventArgs e)
        {
            ((Document)tabControl.SelectedTab.Tag).IsEdit = true;
        }

        private void cmbFontStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSettingsForText();
        }

        private void cmbFontSize_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }catch(Exception exception)
            {
                return;
            }
            SetSettingsForText();
        }
        int index = 2;
        private void btnNew_Click(object sender, EventArgs e)
        {
            index++;
            TabPage page = new TabPage("newDoc" + index);
            var richTextBox = new RichTextBox();
            richTextBox.Location = new Point(3, 3);
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.TextChanged += rtbHienThiText_TextChanged_1;
            richTextBox.SelectionChanged += rtbHienThiText_SelectionChanged;
            page.Controls.Add(richTextBox);
            tabControl.Controls.Add(page);
            tabControl.SelectedTab = page;
            var newDoc = new Document();
            tabControl.SelectedTab.Tag = newDoc;
            documents.Add(newDoc);
        }
        private void openFile(String fileName)
        {
            var check = true;
            foreach(TabPage tab in tabControl.TabPages)
            {
                if(fileName == ((Document)tab.Tag).PathDocument)
                {
                    MessageBox.Show("Doc file thanh cong !", "Thong bao", MessageBoxButtons.OK);
                    tabControl.SelectedTab = tab;
                    check = false;
                    break;
                }
            }
            if (check)
            {
                var newDoc = new Document();
                newDoc.PathDocument = fileName;
                TabPage page = new TabPage(newDoc.getNameDoc());
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.Location = new Point(3, 3);
                richTextBox.Dock = DockStyle.Fill;
                richTextBox.TextChanged += rtbHienThiText_TextChanged_1;
                richTextBox.SelectionChanged += rtbHienThiText_SelectionChanged;
                richTextBox.EnableAutoDragDrop = true;
                StreamReader sr = new StreamReader(fileName, Encoding.Default);
                string str = sr.ReadToEnd();
                if(Path.GetExtension(newDoc.PathDocument) == ".txt")
                {
                    richTextBox.Text = str;
                }
                else
                {
                    richTextBox.Rtf = str;
                }
                sr.Close();
                page.Controls.Add(richTextBox);
                tabControl.TabPages.Add(page);
                tabControl.SelectedTab.Tag = newDoc;
                documents.Add(newDoc);
            }
        }
        private void SaveAsDoc()
        {
            saveFileDialog1.FileName = tabControl.SelectedTab.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ((Document)tabControl.SelectedTab.Tag).PathDocument = saveFileDialog1.FileName;
                ((RichTextBox)tabControl.SelectedTab.Controls[0]).SaveFile(
                    ((Document)tabControl.SelectedTab.Tag).PathDocument, RichTextBoxStreamType.RichText);
            }
        }
        private void SaveDoc()
        {
            if (((Document)tabControl.SelectedTab.Tag).PathDocument == null)
            {
                SaveAsDoc();
            }
            else
            {
                ((RichTextBox)tabControl.SelectedTab.Controls[0]).SaveFile(
                    ((Document)tabControl.SelectedTab.Tag).PathDocument, RichTextBoxStreamType.RichText);
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(DialogResult.OK == openFileDialog2.ShowDialog())
            {
                foreach(var fileName in openFileDialog2.FileNames)
                {
                    openFile(fileName);
                }
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionColor = colorDialog1.Color;
            }
        }

        private void btnCanGiua_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionAlignment = HorizontalAlignment.Center;
        }

        private void btnCanLeTrai_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionAlignment = HorizontalAlignment.Left;
        }

        private void btnCanLePhai_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).SelectionAlignment = HorizontalAlignment.Right;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDoc();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).Copy();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).Paste();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).Undo();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            ((RichTextBox)tabControl.SelectedTab.Controls[0]).Redo();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
        }

        private void btnCanDeu2Ben_Click(object sender, EventArgs e)
        {
            Justify(rtbHienThiText.Text, rtbHienThiText.Font, rtbHienThiText.ClientSize.Width);
        }
        private string Justify(string text, Font font, int width)
        {
            char SpaceChar = (char)0x200A;
            List<string> WordsList = text.Split((char)32).ToList();
            if (WordsList.Capacity < 2)
                return text;

            int NumberOfWords = WordsList.Capacity - 1;
            int WordsWidth = TextRenderer.MeasureText(text.Replace(" ", ""), font).Width;
            int SpaceCharWidth = TextRenderer.MeasureText(WordsList[0] + SpaceChar, font).Width
                               - TextRenderer.MeasureText(WordsList[0], font).Width;

            //Calculate the average spacing between each word minus the last one 
            int AverageSpace = ((width - WordsWidth) / NumberOfWords) / SpaceCharWidth;
            float AdjustSpace = (width - (WordsWidth + (AverageSpace * NumberOfWords * SpaceCharWidth)));

            //Add spaces to all words
            return ((Func<string>)(() => {
                string Spaces = "";
                string AdjustedWords = "";

                for (int h = 0; h < AverageSpace; h++)
                    Spaces += SpaceChar;

                foreach (string Word in WordsList)
                {
                    AdjustedWords += Word + Spaces;
                    //Adjust the spacing if there's a reminder
                    if (AdjustSpace > 0)
                    {
                        AdjustedWords += SpaceChar;
                        AdjustSpace -= SpaceCharWidth;
                    }
                }
                return AdjustedWords.TrimEnd();
            }))();
        }


        //private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (((Document)tabControl.SelectedTab.Tag).IsEdit || ((Document)tabControl.SelectedTab.Tag).IsEdit)
        //    {
        //        var res = MessageBox.Show("Do you want to save changes?", "Closing Tab", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //        if (res == DialogResult.Yes)
        //        {
        //            SaveDoc();
        //        }
        //    }
        //    ((RichTextBox)tabControl.SelectedTab.Controls[0]).Dispose();
        //    tabControl.SelectedTab.Dispose();
        //    tabControl.SelectedIndex = tabControl.TabPages.Count - 1;
        //    if (tabControl.TabPages.Count == 0)
        //    {
        //        this.Close();
        //    }
        //}

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Clipboard.SetImage(Image.FromFile(ofd.FileName));
                    rtbHienThiText.Paste();
                }
            }
        }

        private void btnFontMenuTrip_Click(object sender, EventArgs e)
        {

        }

        private void btnHelpMenuTrip_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ban hay lien he voi toi", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
           System.Diagnostics.Process.Start("https://facebook.com/xuanson.uet");

        }
    }
}
