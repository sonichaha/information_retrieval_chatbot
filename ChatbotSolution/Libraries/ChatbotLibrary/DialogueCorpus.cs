using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    public class DialogueCorpus
    {
        private List<DialogueCorpusItem> itemList;
        private Vocabulary vocabulary;

        public DialogueCorpus()
        {
            itemList = new List<DialogueCorpusItem>();
            vocabulary = new Vocabulary();
        }

        // To do: Write the Process() method below.

        // The method will be quite complex, so you may (should, probably) implement it
        // in parts, adding more methods as needed, or perhaps more classes.
        // Note, however, that some methods (which you should use) have been added already (see below).
       
        // You may, for example, want to add a class for preprocessing (cleaning) the text, and
        // another class for identifying the text that should be kept (following the
        // three conditions above). For any class that you add, make sure to put it
        // in a *separate* file, named accordingly. Do *not* place multiple classes in
        // the same file.

        // (i)   Preprocess the data, i.e. remove (some) special characters, turn the text to lower-case, and so on.
        // (ii)  Tokenize and generate the vocabulary with the (distinct) tokens from the raw data set(s).
        // (iii) Build sentence pairs (S_1,S_2), making sure that S_2 really follows S_1 in the dialogue.
        //       You can do that by, for example, considering exchanges such as
        //       SpeakerA: ... 
        //       SpeakerB: ...
        //       SpeakerA: ...
        //       In that case, it is very likely that the first two sentences form a pair. If, instead, a third
        //       speaker (SpeakerC) were to give the third utterance (intead of SpeakerA), it is far from certain
        //       that the first two sentences form a valid pair, and so on. In other words: To identify valid
        //       pairs, you must consider exchanges involving *two* speakers, and omitting the final sentence.
        //       Note that you should also discard exchanges involving *long* sentences - see the assignment PDF.
        //
        //       NOTE: An easier way is to use the movie_conversations.txt file that is released along with
        //       the list of sentences (movie_lines.txt) for the Cornell Movie Dialog Corpus.
        //       This file does precisely what is described above! (You just have to figure out how ...)
        // 
        // (iv)  Compute and store (e.g. in the vocabulary) the IDF for each word in the vocabulary, treating 
        //       sentences as documents.
        // (v)   Compute and store normalized TF-IDF vectors for each sentence S_1 of every sentence pair. 

        // If you like, you may also add code for showing the dialogue corpus on-screen, in
        // the dialogueCorpusListBox. However, since the dialogue corpus will be very large,
        // you should only display (say) the first 1000 sentence pairs or so.
        // Here, you can use the AsString() method in the DialogueCorpusItem.

        //  The end result of calling the Process() method should be a list of DialogueCorpusItems added
        //  to the itemList. 
        //
        public void Process(List<string> movie_lines, List<string> movie_conversations)
        {
            // Add (a lot of ...) code here, possibly calling other methods (e.g. the ones below).
            Dictionary<int, string> line_dic = new Dictionary<int, string>();
            foreach(string line in movie_lines)
            {
                string[] split_strs = line.Split('\t');
                if(split_strs.Length != 5)
                {
                    continue;
                }
                int id = Int32.Parse(split_strs[0].Split('L')[1]);
                string sentence = split_strs[4];
                int len_of_sen = sentence.Count(c => c == ' ');
                if(len_of_sen < 15)
                {
                    line_dic.Add(id, sentence);
                }
            }

            foreach(string line in movie_conversations)
            {
                string[] split_strs = line.Split('\t');
                if (split_strs.Length != 4)
                {
                    continue;
                }
                string pairs_str = split_strs[3];
                string[] L_split_strs = pairs_str.Split('L');
                for(int i = 1; i < L_split_strs.Length - 1; i++)
                {
                    int id1 = Int32.Parse(L_split_strs[i].Split('\'')[0]);
                    int id2 = Int32.Parse(L_split_strs[i+1].Split('\'')[0]);
                    if(line_dic.ContainsKey(id1) && line_dic.ContainsKey(id2))
                    {
                        string s1 = line_dic[id1];
                        string s2 = line_dic[id2];
                        DialogueCorpusItem dci = new DialogueCorpusItem(s1, s2);
                        dci.Tokenize();
                        itemList.Add(dci);
                    }
                }

            }

            // generate vocaburary
            GenerateVocabulary();

            // compute idf
            ComputeIDFs();

            // compute tfidfvector
            ComputeTFIDFVectors();
        }

        // Should be called from Process(...)
        public void GenerateVocabulary()
        {
            WordDataComparer wordDataComparer = new WordDataComparer();
            foreach(DialogueCorpusItem dci in itemList)
            {
                var distinct_words = dci.Query_token_list.Distinct();
                foreach (string token in distinct_words)
                {
                    WordData word_item = new WordData(token);

                    int idx = vocabulary.Distinct_word_list.BinarySearch(word_item, wordDataComparer);

                    if(idx < 0)
                    {
                        vocabulary.Distinct_word_list.Insert(~idx, word_item);
                    }
                    else
                    {
                        vocabulary.Distinct_word_list[idx].Number_of_sentence++;
                    }
                }
            }
        }


        // Should also be called from Process(...)
        //
        // This method should compute the IDF for each word in the vocabulary
        // Note that, here, each sentence (S_1) of a pair counts as a *document*.
        // Once you have preprocessed all the data files, you will have a set of
        // sentence pairs (each stored as a DialogueCorpusItem in the dialogue corpus)
        // where the sentence S_1 (query) forms a document, for the purpose of the IDF
        // calculation here.
        //
        // Note: This method assumes that GenerateVocabulary() has been executed first.
        public void ComputeIDFs()
        {
            // Add code here ...
            int number_of_query = vocabulary.Distinct_word_list.Count;
            foreach(WordData word in vocabulary.Distinct_word_list)
            {
                word.IDF = (double)(word.Number_of_sentence) / number_of_query;
            }
        }


        // Note: The IDFs must be computed before calling this method, of course.
        public void ComputeTFIDFVectors()
        {
            foreach (DialogueCorpusItem item in itemList)
            {
                item.ComputeTFIDFVector(vocabulary);
            }
        }

        public List<DialogueCorpusItem> ItemList
        {
            get { return itemList; }
            set { itemList = value; }
        }

        public Vocabulary Vocabulary
        {
            get { return vocabulary; }
            set { vocabulary = value; }
        }
    }
}
