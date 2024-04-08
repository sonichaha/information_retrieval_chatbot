using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    [DataContract]
    public class DialogueCorpusItem
    {
        //
        // This class stores a sentence pair as well as the (normalized) TF-IDF embedding
        // for the query sentence (S_1) (you must compute it first, though; see below).
        //
        private string query; // = S_1 in the assignment (used for computing cosine similarity) (need not be a question, though!)
        private string response; // = S_2 in the assignment
        private List<double> tfIdfVector; // Must be generated - see below.

        // You can add fields here, if you also wish to store the tokenized version of
        // each sentence, or (better) the indices of the words (tokens) from the vocabulary
        private List<int> query_token_index_list;
        private List<string> query_token_list;
        private List<int> distinct_token_index_list;

        // This method returns the sentence pair in the format required for saving the
        // dialogue corpus to file (as specified in the assignment).
        //
        // NOTE! Very important! The query and response sentences may *not* contain any tab (\t) characters,
        // since that character is used for separating the two sentences in the AsString() method.
        public string AsString()
        {
            string itemAsString = query + " \t " + response;
            return itemAsString;
        }

        public DialogueCorpusItem(string query, string response)
        {
            this.query = query;
            this.response = response;
            this.tfIdfVector = new List<double>();
            this.query_token_list = new List<string>();
            this.query_token_index_list = new List<int>();
        }

        public void Tokenize()
        {
            string text = query.ToLower();
            char[] delimiterchars = { ' ', ',', '(', ')', '\"', '\t', '!', '.', '?', '#', '-' };

            string[] tokens = text.Split(delimiterchars);
            tokens = (tokens.Where(s => !string.IsNullOrWhiteSpace(s))).ToArray();
            query_token_list = new List<string>(tokens);
        }

        private void NormalizeTFIDFVector()
        {
            double square_sum = 0.0;
            foreach (double num in tfIdfVector)
            {
                square_sum += Math.Pow(num, 2);
            }
            square_sum = Math.Sqrt(square_sum);

            for (int i = 0; i < tfIdfVector.Count; i++)
            {
                tfIdfVector[i] = tfIdfVector[i] / square_sum;
            }
        }
        public void ComputeTFIDFVector(Vocabulary dictionary)
        {
            // Write this method
            // It should generate the (note!) normalized (to unit length)
            // TF-IDF vector for the query sentence (S_1)

            WordDataComparer wordDataComparer = new WordDataComparer();
            foreach (string token in query_token_list)
            {
                WordData word = new WordData(token);
                int idx = dictionary.Distinct_word_list.BinarySearch(word, wordDataComparer);
                query_token_index_list.Add(idx);
            }

            var token_index_grp = query_token_index_list.GroupBy(i => i);
            int number_of_token = dictionary.Distinct_word_list.Count;
            tfIdfVector = new List<double>();
            distinct_token_index_list = new List<int>();
            foreach (var grp in token_index_grp)
            {
                int idx = grp.Key;
                int tf = grp.Count();
                double idf = dictionary.Distinct_word_list[idx].IDF;
                distinct_token_index_list.Add(idx);
                tfIdfVector.Add(tf * idf);
            }

            NormalizeTFIDFVector();
        }
        public List<double> TFIDFVector
        {
            get { return tfIdfVector; }
            set { tfIdfVector = value; }
        }

        [DataMember]
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        [DataMember]
        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        [DataMember]
        public List<int> Query_token_index_list
        {
            get { return query_token_index_list; }
            set { query_token_index_list = value; }
        }

        [DataMember]
        public List<string> Query_token_list
        {
            get { return query_token_list; }
            set { query_token_list = value; }
        }

        public List<int> Distinct_token_index_list
        {
            get { return distinct_token_index_list; }
            set { distinct_token_index_list = value; }
        }
    }
}
