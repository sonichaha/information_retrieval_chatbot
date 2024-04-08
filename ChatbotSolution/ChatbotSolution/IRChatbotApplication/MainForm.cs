using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatbotLibrary;

namespace IRChatbotApplication
{
    public partial class MainForm : Form
    {
        private DialogueCorpus corpus = null; // The dialogue corpus, consisting of sentence pairs.
        private Chatbot chatbot;
        private List<string> movie_lines = null;
        private List<string> movie_conversations = null;

        // Add fields here as needed, e.g. for the raw data.

        public MainForm()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                inputTextBox.InputReceived += new EventHandler<StringEventArgs>(HandleInputReceived);
            }
        }

        private void loadConversationPairsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Write code here for loading and parsing raw data.
                movie_conversations = new List<string>();
                openFileDialog.Filter = "Text files (*.tsv)|*.tsv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader dataReader = new StreamReader(openFileDialog.FileName);
                    {
                        while (!dataReader.EndOfStream)
                        {
                            movie_conversations.Add(dataReader.ReadLine());
                        }
                    }
                    loadMovieLinesToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void loadMovieLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Write code here for loading and parsing raw data.
                movie_lines = new List<string>();
                openFileDialog.Filter = "Text files (*.tsv)|*.tsv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    StreamReader dataReader = new StreamReader(openFileDialog.FileName);
                    {
                        while (!dataReader.EndOfStream)
                        {
                            movie_lines.Add(dataReader.ReadLine());
                        }
                    }
                    generateDialogueCorpusButton.Enabled = true;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //
        // This method you get for free ... :)
        private void generateChatBotButton_Click(object sender, EventArgs e)
        {
            generateChatBotButton.Enabled = false;
            chatbot = new Chatbot();
            chatbot.SetDialogueCorpus(corpus);
            inputTextBox.Enabled = true;
            mainTabControl.SelectedTab = chatTabPage;
        }

        //
        // To do: Write the method below.
        //
        // This method is called whenever the user enters text (= hits return) in
        // the input text box.
        // 
        // The input sentence should be passed to the chatbot, which then generates
        // an output. Both the (user) input and the (chatbot) output should be displayed in the
        // listbox (listbox.Items.Insert(0, ...), so that one can follow the
        // entire dialogue
        // 
        private void HandleInputReceived(object sender, StringEventArgs e)
        {
            string inputSentence = e.Information;

            // Add code here ...
            string response = chatbot.GenerateResponse(inputSentence);
            dialogueListBox.Items.Add("user input sentence: " + inputSentence);
            dialogueListBox.Items.Add("generated response: " + response);
        }

        // To do: Write this method. Hint: Use the StreamWriter class
        // as well as the AsString() method from the DialogueCorpusItem class.
        private void saveDialogueCorpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This method should save the dialogue corpus, with one exchange per
            // row, i.e. S_1, tab-character ("\t"), S_2. As mentioned in the assignment
            // you must hand in this file *in addition* to the raw data.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = "Text files (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter dataWriter = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int ii = 0; ii < corpus.ItemList.Count; ii++)
                        {
                            dataWriter.WriteLine(corpus.ItemList[ii].AsString());
                        }
                        dataWriter.Close();
                    }
                }
            }
        }

        private void generateDialogueCorpusButton_Click(object sender, EventArgs e)
        {
            corpus = new DialogueCorpus();
            // Add methods (in the DialogueCorpus class) for processing the raw
            // data, e.g. corpus.Process(rawData) ...
            corpus.Process(movie_lines, movie_conversations);

            generateChatBotButton.Enabled = true;
            saveDialogueCorpusToolStripMenuItem.Enabled = true;
        }
    }
}
