﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    [DataContract]
    public class WordData
    {
        private string word;
        private double idf;
        private int number_of_sentence;
        // For each word in the vocabulary, this class stores the word itself (a string)
        // as well as the IDF (must be computed, of course).

        public WordData(string word)
        {
            this.word = word;
            idf = 0.0;
            number_of_sentence = 1;
        }
        public string AsString(string formatString)
        {
            string wordDataAsString = word.PadRight(20) + " (" + idf.ToString(formatString) + ")" + " (" + number_of_sentence.ToString(formatString) + ")"; 
            return wordDataAsString;
        }

        [DataMember]
        public string Word
        {
            get { return word; }
            set { word = value; }
        }

        [DataMember]
        public double IDF
        {
            get { return idf; }
            set { idf = value; }
        }

        [DataMember]
        public int Number_of_sentence
        {
            get { return number_of_sentence; }
            set { number_of_sentence = value; }
        }
    }
}
