namespace ClassLibrary.TimBrowser
{
    public class EditorCommenter
    {
        WebView _browser;
        public EditorCommenter(WebView browser)
        {
            _browser = browser;
            LeaveComment();
        }
        void LeaveComment()
        {

            if (_browser != null)
            {
                ReviewCommentModel commentModel = new();
                Random random = new Random();

                _browser.Eval("var scorecard_rating = document.getElementById(\"er_subjective_rating\");\r\n " +
                    " var scorecard_rating_comment = document.getElementsByName(\"er_subjective_rating_explain\")[0];\r\n " +
                    "var plagiarism = document.getElementById(\"er_is_plagiarized\");\r\n  " +
                    "var error_number = document.getElementById(\"er_number_of_errors\");\r\n  " +
                    "var guideline_rating = document.getElementById(\"er_follows_guidelines\");" +
                    "if (plagiarism.value == 'X') {\r\n    alert(\"Select a plagiarism rating\");\r\n  }" +
                    "else if (plagiarism.value == 1) {\r\n    scorecard_rating.value = 1;\r\n    scorecard_rating_comment.value = \"Your review contains plagiarism. Plagiarism sorce: .\"\r\n  }" +
                    "else{" +
                        "if (error_number.value == 'X') {" +
                        "       alert(\"Select the appropriate number of errors please\");" +
                        "}" +
                        "else if (error_number.value == 0)" +
                        "{" +
                            "if (guideline_rating.value == 'X')" +
                            "{" +
                            "   alert(\"Select the appropriate guideline rating please\");" +
                            "}" +
                            "else if (guideline_rating.value == 1)" +
                            "{" +
                                $" var comment = \"{commentModel.zeroerrors_guideline[0]}\";" +
                                " scorecard_rating.value = 10;" +
                                " scorecard_rating_comment.value = comment" +
                            "}" +
                            "else if (guideline_rating.value == 0)" +
                            "{" +
                            $"     var comment = \"{commentModel.zeroerrors_noguideline[0]}\";" +
                            "      scorecard_rating.value = 6;" +
                            "      scorecard_rating_comment.value = comment;" +
                            "}" +
                         "}" +
                         "else if (error_number.value == 1)" +
                         "{" +
                         "        if (guideline_rating.value == 'X')" +
                         "           {" +
                         "                alert(\"Select the appropriate guideline rating please\");" +
                         "            }" +
                         "        else if (guideline_rating.value == 1)" +
                         "            {" +
                         $"                var comment = \"{commentModel.oneerror_guideline[0]}\";" +
                         "                scorecard_rating.value = 9;" +
                         "                scorecard_rating_comment.value = comment;" +
                         "            }" +
                         "        else if (guideline_rating.value == 0)" +
                         "            {" +
                         $"                var comment = \"{commentModel.oneerror_noguideline[0]}\";" +
                         "                scorecard_rating.value = 5;" +
                         "                scorecard_rating_comment.value = comment;" +
                         "            }" +
                         "}" +
                        " else if (error_number.value == 2)" +
                        " {" +
                        "          if (guideline_rating.value == 'X')" +
                        "          {" +
                        "              alert(\"Select the appropriate guideline rating please\");" +
                        "          }" +
                        "          else if (guideline_rating.value == 1)" +
                        "            {" +
                        $"                var comment = \"{commentModel.twoerrors_guideline[0]}\";" +
                        "                scorecard_rating.value = 8;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                        "          else if (guideline_rating.value == 0)" +
                        "            {" +
                        $"                var comment = \"{commentModel.twoerrors_noguideline[0]}\";" +
                        "                scorecard_rating.value = 5;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                         "}" +
                        "else if (error_number.value == 3)" +
                        "{" +
                        "            if (guideline_rating.value == 'X')" +
                        "            {" +
                        "                alert(\"Select the appropriate guideline rating please\");" +
                        "            }" +
                        "            else if (guideline_rating.value == 1)" +
                        "            {" +
                        $"                var comment = \"{commentModel.threeerrors_guideline[0]}\";" +
                        "                scorecard_rating.value = 7;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                        "            else if (guideline_rating.value == 0)" +
                        "            {" +
                        $"                var comment = \"{commentModel.threeerrors_noguideline[0]}\";" +
                        "                scorecard_rating.value = 4;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                        "}" +
                        "else if (error_number.value == 4)" +
                        "{" +
                        "            if (guideline_rating.value == 'X')" +
                        "            {" +
                        "                alert(\"Select the appropriate guideline rating please\");" +
                        "            }" +
                        "            else if (guideline_rating.value == 1)" +
                        "            {" +
                        $"                var comment = \"{commentModel.fourerrors_guideline[0]}\";" +
                        "                scorecard_rating.value = 6;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                        "            else if (guideline_rating.value == 0)" +
                        "            {" +
                        $"                var comment = \"{commentModel.fourerrors_noguideline[0]}\";" +
                        "                scorecard_rating.value = 3;" +
                        "                scorecard_rating_comment.value = comment;" +
                        "            }" +
                        "}" +
                    "}");


            }





            //}
        }
    }
}
