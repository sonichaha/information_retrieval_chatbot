using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    class PredictedResponse
    {
        private List<int> sentence_pair_index_list;
        private List<double> cos_similarity_list;
        private int max_number_of_response;

        public PredictedResponse(int numberOfResponse)
        {
            sentence_pair_index_list = new List<int>();
            cos_similarity_list = new List<double>();
            max_number_of_response = numberOfResponse;
        }

        public void Add(int sentence_ind, double cos_sim)
        {
            if(sentence_pair_index_list.Count >= max_number_of_response
                && cos_sim <= cos_similarity_list[0])
            {
                return;
            }

            int idx = cos_similarity_list.BinarySearch(cos_sim);
            if(idx >= 0)
            {
                cos_similarity_list.Insert(idx, cos_sim);
                sentence_pair_index_list.Insert(idx, sentence_ind);
            }
            else
            {
                cos_similarity_list.Insert(~idx, cos_sim);
                sentence_pair_index_list.Insert(~idx, sentence_ind);
            }
            
            if(sentence_pair_index_list.Count > max_number_of_response)
            {
                sentence_pair_index_list.RemoveAt(0);
                cos_similarity_list.RemoveAt(0);
            }
        }

        public List<int> Sentence_pair_index_list
        {
            get { return sentence_pair_index_list; }
            set { sentence_pair_index_list = value; }
        }

        public List<double> Cos_similarity_list
        {
            get { return cos_similarity_list; }
            set { cos_similarity_list = value; }
        }
    }
}
