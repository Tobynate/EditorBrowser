namespace ClassLibrary.TimBrowser
{
    public class ReviewCommentModel
    {
        public List<string> zeroerrors_noguideline { get; set; } = new();
        public List<string> zeroerrors_guideline { get; set; } = new();
        public List<string> oneerror_noguideline { get; set; } = new();
        public List<string> oneerror_guideline { get; set; } = new();
        public List<string> twoerrors_noguideline { get; set; } = new();
        public List<string> twoerrors_guideline { get; set; } = new();
        public List<string> threeerrors_noguideline { get; set; } = new();
        public List<string> threeerrors_guideline { get; set; } = new();
        public List<string> fourerrors_noguideline { get; set; } = new();
        public List<string> fourerrors_guideline { get; set; } = new();

        public ReviewCommentModel()
        {
            ZeroErrors();
            OneError();
            TwoErrors();
            ThreeErrors();
            FourErrors();
        }
        void ZeroErrors()
        {
            string[] tempzeroerrors_noguideline = { "I enjoyed reading your review. There were no errors spotted. However, there was a guideline breach that dented your review. I would suggest you go through the guidelines again. Good luck." };

            zeroerrors_noguideline = tempzeroerrors_noguideline.ToList();

            string[] tempzeroerrors_guideline = { "I enjoyed reading your review. All the guidelines were adhered to and there were no errors. Looking forward to more reviews from you. Kudos" };

            zeroerrors_guideline = tempzeroerrors_guideline.ToList();
        }
        void OneError()
        {
            string[] temponeerror_noguideline = { "I enjoyed reading your review. However, there was an error and a guideline breach. I would suggest you go through the guidelines again. Good luck." };
            oneerror_noguideline = temponeerror_noguideline.ToList();

            string[] temponeerror_guideline = { "I enjoyed reading your review. All the guidelines were adhered to. However, there was an error that dented your review. Proper proofreading would have helped with that." };
            oneerror_guideline = temponeerror_guideline.ToList();
        }
        void TwoErrors()
        {
            string[] temptwoerrors_noguideline = { "I enjoyed reading your review. However, there were errors and a guideline breach. I would suggest you go through the guidelines again. Good luck." };
            twoerrors_noguideline = temptwoerrors_noguideline.ToList();

            string[] temptwoerrors_guideline = { "I enjoyed reading your review. All the guidelines were adhered to. However, there were errors that dented your review. Proper proofreading would have helped with that." };
            twoerrors_guideline = temptwoerrors_guideline.ToList();
        }
        void ThreeErrors()
        {
            string[] tempthreeerrors_noguideline = { "I enjoyed reading your review. However, there were errors and a guideline breach. I would suggest you go through the guidelines again. Good luck." };
            threeerrors_noguideline = tempthreeerrors_noguideline.ToList();

            string[] tempthreeerrors_guideline = { "I enjoyed reading your review. All the guidelines were adhered to. However, there were errors that dented your review. Proper proofreading would have helped with that." };
            threeerrors_guideline = tempthreeerrors_guideline.ToList();
        }

        void FourErrors()
        {
            string[] tempfourerrors_noguideline = { "I enjoyed reading your review. However, there were errors and a guideline breach. I would suggest you go through the guidelines again. Good luck." };
            fourerrors_noguideline = tempfourerrors_noguideline.ToList();

            string[] tempfourerrors_guideline = { "I enjoyed reading your review. All the guidelines were adhered to. However, there were errors that dented your review. Proper proofreading would have helped with that." };
            fourerrors_guideline = tempfourerrors_guideline.ToList();
        }

    }
}
