using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotLibrary
{
    public class Chatbot
    {
        protected const int DEFAULT_NUMBER_OF_MATCHES = 5;

        protected DialogueCorpus dialogueCorpus;
        protected Random randomNumberGenerator;
        protected int numberOfMatches = DEFAULT_NUMBER_OF_MATCHES;

        public virtual void Initialize()
        {
            randomNumberGenerator = new Random();
        }

        public void SetDialogueCorpus(DialogueCorpus dialogueCorpus)
        {
            this.dialogueCorpus = dialogueCorpus;
        }

        //
        // ToDo: Write the method below
        //
        // Given an input sentence, the method should 
        // compute the cosine similarity between the normalized
        // TF-IDF vector of the test sentence and the normalized
        // TF-IDF vectors of every sentence S_1 (i.e. the Query sentence
        // in each DialogueCorpusItem). The results should then be
        // sorted (you may add a class for this, perhaps with two fields,
        // one field for the index of the sentence pair (in the dialogue corpus), and
        // one field for the cosine similarity. Finally, a sentence pair should
        // be selected (as described below) and the corresponding Response should be returned.
        //
        // Then 
        // (i) sort the results in falling order of cosine similarity,
        // (ii) pick a random sentence pair (index) from among the top five (numberOfMatches, see above).
        // (iii) return sentence S_2 (Response) from the selected pair (DialogueCorpusItem).
        //
        //
        public virtual string GenerateResponse(string inputSentence)
        {
            // Add code here ...
            PredictedResponse predictedResponse = new PredictedResponse(numberOfMatches);
            DialogueCorpusItem inputQuery = new DialogueCorpusItem(inputSentence, "");
            inputQuery.Tokenize();
            inputQuery.ComputeTFIDFVector(dialogueCorpus.Vocabulary);

            for (int pair_ind = 0;pair_ind < dialogueCorpus.ItemList.Count; pair_ind++)
            {
                DialogueCorpusItem pair = dialogueCorpus.ItemList[pair_ind];
                List<int> ind_vec = pair.Distinct_token_index_list.AsQueryable().Intersect(
                    inputQuery.Distinct_token_index_list).ToList();
                double sum_of_product = 0.0;
                foreach(int idx in ind_vec)
                {
                    int idx_of_input = inputQuery.Distinct_token_index_list.IndexOf(idx);
                    int idx_of_pair = pair.Distinct_token_index_list.IndexOf(idx);
                    sum_of_product += inputQuery.TFIDFVector[idx_of_input] * pair.TFIDFVector[idx_of_pair];
                }

                predictedResponse.Add(pair_ind, sum_of_product);
            }

            Random random_instance = new Random();
            int random_index = random_instance.Next(0, predictedResponse.Sentence_pair_index_list.Count);
            int sentence_id = predictedResponse.Sentence_pair_index_list[random_index];

            return dialogueCorpus.ItemList[sentence_id].Response; // REMOVE this line - it's just here in order to make the code compile
        }

    }
}
