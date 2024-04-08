using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    class WordDataComparer : IComparer<WordData>
    {
        public int Compare(WordData word1, WordData word2)
        {
            return word1.Word.CompareTo(word2.Word);
        }
    }
}
