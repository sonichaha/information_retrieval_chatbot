using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    [DataContract]
    public class Vocabulary
    {
        private List<WordData> distinct_word_list;
        // Write this class - it should hold the vocabulary, i.e. the list of all distinct words (tokens) sorted
        // in alphabetical order, along with the IDFs, computed from the raw data set.

        // You may use a list of WordData instances if you wish (where each WordData instance would hold information
        // about one word).
        public Vocabulary()
        {
            distinct_word_list = new List<WordData>();
        }
        public List<WordData> Distinct_word_list
        {
            get { return distinct_word_list; }
            set { distinct_word_list = value; }
        }
    }
}
