using ClassLibrary.TimBrowser;

namespace TimEditorBrowser
{
    public partial class BrowserPage : ContentPage
    {
        List<LoginCredentialsModel> loginCredentials = new();
        List<Errors_SuccessesModel> errors_SuccessesModels = new();

        private readonly string htmlPath = Path.Combine(Environment.ProcessPath.Replace(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", ""), @"sidetab.html");

        LinkCommitter linkCommitter = new();
        string displayMessage = "";
        public BrowserPage()
        {
            InitializeComponent();

            ImageButtons_Click();
            //MainBrowser.IsVisible = MainBrowser.IsVisible;
            MainBrowser.Source = "https://forums.onlinebookclub.org/review-team/editor-select.php";

            GrammarlyBrowser.Source = "https://app.grammarly.com/ddocs/1660546288";

            setCredentials();
        }

        private void ImageButtons_Click()
        {
            TapGestureRecognizer tapEvent = new TapGestureRecognizer();
            tapEvent.Tapped += MenuButton_Clicked;
            SideBarMenu.GestureRecognizers.Add(tapEvent);
            SideBarMenu.IsEnabled = false;

            TapGestureRecognizer tapEvent2 = new();
            tapEvent2.Tapped += RefreshButton_Clicked;
            RefreshPage.GestureRecognizers.Add(tapEvent2);
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            if (MainBrowser.IsVisible)
            {
                GreenLight.BackgroundColor = Colors.Red;
                GreenLight.Text = "Loading";
                MainBrowser.Reload();
            }
            if (GrammarlyBrowser.IsVisible) GrammarlyBrowser.Reload();
        }

        private void MenuButton_Clicked(object sender, EventArgs e)
        {
            if (SideTab.IsVisible)
            {
                SideTab.IsVisible = false;
                //SuccessMessages.IsVisible = false;
                MainBrowser.WidthRequest = 1100;
                GrammarlyBrowser.WidthRequest = 1100;
            }
            else
            {
                SideTab.IsVisible = true;
                //SuccessMessages.IsVisible = true;
                MainBrowser.WidthRequest = 900;
                GrammarlyBrowser.WidthRequest = 900;
            }
        }

        private void EditorPageBtn_Clicked(object sender, EventArgs e)
        {
            MainBrowser.IsVisible = true;
            GrammarlyBrowser.IsVisible = false;

            EditorPageBtn.IsEnabled = false;
            GrammarlyPage.IsEnabled = true;
        }

        private void GrammarlyPage_Clicked(object sender, EventArgs e)
        {
            MainBrowser.IsVisible = false;
            GrammarlyBrowser.IsVisible = true;

            EditorPageBtn.IsEnabled = true;
            GrammarlyPage.IsEnabled = false;
        }

        private void CheckBalance_Clicked(object sender, EventArgs e)
        {

        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            CredentialsList.IsVisible = !CredentialsList.IsVisible;
            CredentialsTab.IsVisible = !CredentialsTab.IsVisible;
        }

        private async void CredentialsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != "Add New Credentials")
            {
                LoginCredentialsModel model = new();
                foreach (var item in loginCredentials)
                {
                    if (e.SelectedItem == item.username) model = item;
                }

                await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('username').setAttribute('value', '{model.username}')");
                await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('password').setAttribute('value', '{model.password}')");
            }
            else
            {
                SaveCredentialsTab.IsVisible = true;
            }
        }

        string credKey = "password";
        private async void SaveCredentials_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(Password.Text))
            {
                LoginCredentialsModel loginCred = new LoginCredentialsModel
                {
                    username = Username.Text,
                    password = Password.Text
                };
                string usersCred = await SecureStorage.Default.GetAsync(credKey);
                bool isSuccess = SecureStorage.Default.Remove(credKey);
                usersCred = String.Concat(usersCred, "|**|", loginCred.username, "|$$|", loginCred.password);
                await SecureStorage.Default.SetAsync(credKey, usersCred);

                CredentialsList.ItemsSource = new List<string>();
                setCredentials();
            }
        }

        private async void setCredentials()
        {
            string usersCred = await SecureStorage.Default.GetAsync(credKey) ?? "";
            string[] userCred = usersCred.Split("|**|");
            foreach (string cred in userCred)
            {
                string[] credSplit = cred.Split("|$$|");

                if(credSplit.Length > 1)
                loginCredentials.Add(new()
                {
                    username = credSplit[0],
                    password = credSplit[1]
                });
            }

            List<string> usernames = new();
            foreach (var item in loginCredentials)
            {
                usernames.Add(item.username);
            }
            usernames.Add("Add New Credentials");
            CredentialsList.ItemsSource = usernames;
        }

        private void MainBrowser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            string ttt = e.Url;
        }

        private void MainBrowser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            EnablePageButtons(null, e);
        }

        private async void EnablePageButtons(WebNavigatingEventArgs e, WebNavigatedEventArgs element)
        {
            string url = "";
            if (e != null) url = e.Url;
            else if (element != null) url = element.Url;

            if (url.Contains("https://forums.onlinebookclub.org/ucp.php?mode=login"))
            {
                EditorPageBtn.IsEnabled = true;
                GrammarlyPage.IsEnabled = true;
                CheckBalance.IsEnabled = true;

                SaveCredentialsTab.IsVisible = false;
                LayerMenu.IsVisible = true;
                Login.IsVisible = true;
                SearchTab.IsVisible = false;
                CredentialsList.IsVisible = false;
                CredentialsTab.IsVisible = false;
            }
            else if (url.Contains("https://forums.onlinebookclub.org/review-team/editor-submit.php"))
            {
                EditorPageBtn.IsEnabled = false;
                GrammarlyPage.IsEnabled = true;
                CheckBalance.IsEnabled = true;

                SaveCredentialsTab.IsVisible = false;
                LayerMenu.IsVisible = false;
                Login.IsVisible = false;
                SearchTab.IsVisible = false;
                CredentialsList.IsVisible = false;
                CredentialsTab.IsVisible = false;
                string goBack = await MainBrowser.EvaluateJavaScriptAsync("var reviewBody = document.body.innerText;" +
               "var goBack = '';" +
               "if (document.body.innerText.includes('A scorecard has already been submitted for this review. Please select a different review to edit.'))" +
               "{ " +
               "   goBack = 'true'" +
               "}" +
               "else" +
               "{" +
               "goBack = 'false'" +
               "}");
                if (goBack.Contains("false"))
                {
                    try
                    {
                        await EditScoreCard();

                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        
                    }
                    GreenLight.Text = "Proceed";
                    GreenLight.BackgroundColor = Colors.LightSeaGreen;
                }
                else if (goBack.Contains("true")) MainBrowser.Source = "https://forums.onlinebookclub.org/review-team/editor-select.php";
            }
            else if (url.Contains("https://forums.onlinebookclub.org/review-team/editor-view-payments.php"))
            {
                GrammarlyPage.IsEnabled = true;
                EditorPageBtn.IsEnabled = true;
                CheckBalance.IsEnabled = false;

                SaveCredentialsTab.IsVisible = false;
                SearchTab.IsVisible = false;
                LayerMenu.IsVisible = false;
                Login.IsVisible = false;
                CredentialsList.IsVisible = false;
                CredentialsTab.IsVisible = false;
            }
            else if (url.Contains("https://forums.onlinebookclub.org/review-team/editor-select"))
            {
                string process = await MainBrowser.EvaluateJavaScriptAsync("var reviewButton = document.getElementsByClassName('default-submit-action')[0].click();");
                string link = "";
            }

        }

        string bookTitle = "";
        string authorName = "";
        private async Task EditScoreCard()
        {
            await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('er_is_plagiarized').value = 0");

            errors_SuccessesModels = new();
            await GetAuthorandBookName();
            await GetReview();
            GetReviewParagraphandSpace();
            CheckAuthorAndBookName();
            CheckRating();
            HighlightKeyWords();

            await SetGuidelineDropdowns();
            CheckError_Grammarly();

            foreach (var item in errors_SuccessesModels)
            {
                displayMessage = displayMessage + Environment.NewLine + item.Message;
            }

            displayError();
        }

        void displayError()
        {
            string file = File.ReadAllText(htmlPath);
            string temp = "";

            if (errors_SuccessesModels.Count > 0)
            {
                foreach (var item in errors_SuccessesModels)
                {
                    if (item.IsError)
                    {
                        temp = temp + "<br><div class=\"error\">" +
                            $"{item.Message}" +
                            $"</div>";
                    }
                    else
                    {
                        temp = temp + "<br><div class=\"success\">" +
                            $"{item.Message}" +
                            $"</div>";
                    }
                }
            }
            temp = "<br><br>" + temp;
            file = file.Replace("</html>", temp + "</html>");
            file = file.Replace("<style>\r\n           body {\r\n               background-color: powderblue;\r\n           }\r\n           .keywordsOBC{\r\n               background-color: purple;\r\n               color: red;\r\n           }\r\n       </style>", " <style>\r\n           body {\r\n               background-color: powderblue;\r\n           }\r\n           .keywordsOBC{\r\n               background-color: purple;\r\n               color: red;\r\n           }\r\n           .success{\r\n               background-color: greenyellow;\r\n               color: blueviolet;\r\n           }\r\n           .error {\r\n               background-color: #C8C874;\r\n               color: red;\r\n           }\r\n       </style>");

            File.WriteAllText(htmlPath, file);
            SideTab.Source = new Uri(htmlPath);
        }

        private async void CheckError_Grammarly()
        {
            //_0c5aea4c-editor-editorContent

            GrammarlyBrowser.Focus();
            await Clipboard.SetTextAsync(review.Replace("\\\\n", Environment.NewLine));
            Thread.Sleep(3000);

            await GrammarlyBrowser.EvaluateJavaScriptAsync($"var content = document.getElementsByClassName('_9c5f1d66-denali-editor-editor')[0].innerHTML = '';");
            
            //review = review.Replace("looking", "look");
            var reviewTemp = review.Split("\\\\n");

            foreach (var item in reviewTemp)
            {
                await GrammarlyBrowser.EvaluateJavaScriptAsync("var tag = document.createElement(\"p\"); " +
                    $"var text = document.createTextNode(\"{item}\");" +
                    "tag.appendChild(text);" +
                    "var element = document.getElementsByClassName('_9c5f1d66-denali-editor-editor')[0];" +
                    "element.appendChild(tag);");
            }

            GetErrors();
        }

        List<string> errorSpotted = new();
        private async Task GetErrors()
        {
            var editorContent = await GrammarlyBrowser.EvaluateJavaScriptAsync($"document.getElementsByClassName('_9c5f1d66-denali-editor-editor')[0].innerHTML");

            string correctness = "";

            string comparer = await GrammarlyBrowser.EvaluateJavaScriptAsync($"document.getElementsByClassName('_9c5f1d66-denali-editor-editor')[0].innerHTML");
            comparer = comparer.Replace("\\u003C", "<");
            comparer = comparer.Replace("\\\\\\" + "\"", "'");

            comparer = comparer.Replace("\\", "");
            comparer = comparer.TrimStart('"');

            List<string> Temp = comparer.Replace(".", ".||_||_").Split("||_||_").ToList();

            bool brac = false;

            List<string> errors = new();
            foreach (var item in Temp)
            {
                if (item.Contains("alerts-correctness1") || item.Contains("alerts-correctness"))
                {
                    errors.Add(item);
                }
            }

            for (int i = 0; i < errors.Count; i++)
            {
                while (errors[i].Contains("<") && errors[i].Contains(">"))
                {
                    string sp = errors[i].Substring(errors[i].IndexOf("<"), errors[i].IndexOf(">") - errors[i].IndexOf("<") + 1);

                    if (sp.Contains("alerts-correctness1") || sp.Contains("alerts-correctness"))
                    {
                        if (!sp.Contains("/"))
                        {
                            errors[i] = ReplaceFirst(errors[i], sp, "[");
                            brac = true;
                        }
                    }
                    else
                    {
                        if (!sp.Contains("/"))
                        {
                            errors[i] = ReplaceFirst(errors[i], sp, "");
                            brac = false;
                        }
                    }
                    if (sp.Contains("/") && brac)
                    {
                        errors[i] = ReplaceFirst(errors[i], sp, "]");
                    }
                    else if (sp.Contains("/") && !brac)
                    {
                        errors[i] = ReplaceFirst(errors[i], sp, "");
                    }

                }
            }

            errorSpotted = errors;

            errorSpotted = errorSpotted.Distinct().ToList();


            for (int i = 0; i < errorSpotted.Count; i++)
            {
                if (errorSpotted[i].Contains("[[")) errorSpotted[i] = errorSpotted[i].Replace("[[", "[");
                if (errorSpotted[i].Contains("]]")) errorSpotted[i] = errorSpotted[i].Replace("]]", "]");
            }

            int errorNumber = errorSpotted.Count;
            string errorsDisplayed = "";

            if (errorNumber > 0)
            {
                foreach (var item in errorSpotted)
                {
                    errorsDisplayed = errorsDisplayed + " " + item;
                }

            }

            if (errorNumber > 4) errorNumber = 4;
            await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('er_number_of_errors').value = {errorNumber};");//er_number_of_errors
            await MainBrowser.EvaluateJavaScriptAsync($"var ele = document.getElementById('er_number_of_errors_explain').value = \"{errorsDisplayed}\";");

        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private async Task SetGuidelineDropdowns()
        {
            //er_follows_guidelines
            Errors_SuccessesModel model = new();
            List<Errors_SuccessesModel> errorModels = new();
            errorModels = errors_SuccessesModels.Where(x => x.IsError == true).ToList();
            MainBrowser.Eval($"document.getElementById('er_follows_guidelines').value = 1");

            string errorMessage = "";
            if (errorModels.Count > 0) errorMessage = errorModels[0].Message;
            if (errors_SuccessesModels.Any(x => x.IsError == true))
            {
                MainBrowser.Eval($"document.getElementById('er_follows_guidelines').value = 0");

            }
            if (errors_SuccessesModels.Where(x => x.IsError == true).ToList().Count > 1)
            {
                errorMessage = errorModels[0].Message + " " + errorModels[1].Message;
                await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('er_follows_guidelines_explain').value =\"{errorMessage}\"");
            }
            else await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('er_follows_guidelines_explain').value =\"{errorMessage}\"");
        }

        private void HighlightKeyWords()
        {
            string path = @"C:\Users\User\Documents\DOnaldduck\HighlightedWord.txt";
            string keywords = File.ReadAllText(path);
            List<string> keywordList = keywords.Split(',').ToList();
            keywordList.ForEach(x => x.Replace("\"", ""));
            keywordList.ForEach(x => x.Trim());
            keywordList = keywordList.Concat(new List<string>()
            {
                authorName,
                bookTitle,
                rating
            }).ToList();

            string reviewHtmlTemp = reviewHTML;
            string contentTemp = "";
            foreach (string keyword in keywordList)
            {
                if (!string.IsNullOrEmpty(keyword) && review.ToLower().Contains(keyword.ToLower()))
                {
                    reviewHtmlTemp = reviewHtmlTemp.Replace(keyword, "\\u003Cspan class=" + "\\\\\\" + '"' + "keywordsOBC" + "\\\\\\" + '"' + '>' + keyword + "\u003C/span>");
                }
            }

            contentTemp = contentDiv.Replace(reviewHTML, reviewHtmlTemp);

            reviewHtmlTemp = reviewHtmlTemp.Replace("\\u003C", "<");

            reviewHtmlTemp = reviewHtmlTemp.Replace("\\\\\\", "");

            reviewHtmlTemp = reviewHtmlTemp.Replace("\\\\n", "");
            RenewHtml();
            string file = File.ReadAllText(htmlPath);

            file = file.Replace("<div id=\"fullContent\"></div>", $"<div id=\"fullContent\">{reviewHtmlTemp}</div>");

            File.WriteAllText(htmlPath, file);
            SideTab.Source = new Uri(htmlPath);

            SideBarMenu.IsEnabled = true;
        }

        private void RenewHtml()
        {
            string html = "<!DOCTYPE html> \r\n   <html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\"> \r\n   <head>\r\n       <meta charset=\"utf-8\" />\r\n       <title>Test Title</title>\r\n\r\n       <style>\r\n           body {\r\n               background-color: powderblue;\r\n           }\r\n           .keywordsOBC{\r\n               background-color: purple;\r\n               color: red;\r\n           }\r\n       </style>\r\n   </head>\r\n   <body>\r\n       <div id=\"fullContent\"></div>\r\n   </body>\r\n   </html>";
            File.WriteAllText(htmlPath, html);
        }

        string rating = "";
        private void CheckRating()
        {
            string ttt = reviewHTML;

            string bold = "\\u003Cstrong class=\\\\\\" + '"' + "text-strong\\\\\\" + '"' + ">";

            List<string> posibleRating = new();

            string[] ratingNumber = { "1", "2", "3", "4" };
            string[] ratingWords = { "one", "two", "three", "four" };


            string mid = " out of ";
            foreach (var item in ratingNumber)
            {
                foreach (var i in ratingNumber)
                {
                    if (reviewHTML.ToLower().Contains((item + mid + i)))
                    {
                        errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsSuccess = true,
                            Message = "Review contains a rating."
                        });
                        rating = item + mid + i;
                        break;
                    }

                    if (reviewHTML.ToLower().Contains(ratingWords[Array.IndexOf(ratingNumber, item)] + mid + ratingWords[Array.IndexOf(ratingNumber, i)]))
                    {
                        errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsSuccess = true,
                            Message = "Review contains a rating."
                        });
                        rating = ratingWords[Array.IndexOf(ratingNumber, item)] + mid + ratingWords[Array.IndexOf(ratingNumber, i)];
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(rating)) errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "Review does not contain a rating."
            });
            if (reviewHTML.Contains(bold + rating)
                || reviewHTML.Contains(bold + " " + rating)
                || reviewHTML.Contains(bold + "  " + rating)) errors_SuccessesModels.Add(new Errors_SuccessesModel
                {
                    IsSuccess = true,
                    Message = "Review rating was properly emboldened."
                });
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "Review rating was not properly emboldened."
            });
        }

        private void CheckAuthorAndBookName()
        {
            if (review.ToLower().Contains(authorName.ToLower())) errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsSuccess = true,
                Message = "Review contains author's name."
            });
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "Review does not contain author's name."
            });

            if (review.ToLower().Contains(bookTitle.ToLower()))
            {
                errors_SuccessesModels.Add(new Errors_SuccessesModel
                {
                    IsSuccess = true,
                    Message = "Review contains book title."
                });

                string ttt = reviewHTML;

                string italics = "\\u003Cem class=\\\\\\" + '"' + "text-italics\\\\\\" + '"' + ">" + bookTitle;
                string italics1 = "\\u003Cem class=\\\\\\" + '"' + "text-italics\\\\\\" + '"' + ">" + " " + bookTitle;
                string italics2 = "\\u003Cem class=\\\\\\" + '"' + "text-italics\\\\\\" + '"' + ">" + "  " + bookTitle;


                if (reviewHTML.ToLower().Contains(italics.ToLower())
                    || reviewHTML.ToLower().Contains(italics1.ToLower())
                    || reviewHTML.ToLower().Contains(italics2.ToLower()))
                {
                    errors_SuccessesModels.Add(new Errors_SuccessesModel
                    {
                        IsSuccess = true,
                        Message = "Book title is in italics."
                    });
                }
                else errors_SuccessesModels.Add(new Errors_SuccessesModel
                {
                    IsError = true,
                    Message = "The book title is not in italics."
                });
            }
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "Review does not contain book title."
            });
        }

        int doubleLineCount = 0;
        int singleLineParagraphCount = 0;
        private void GetReviewParagraphandSpace()
        {
            string reviewHTMLTemp = "";

            string temp = reviewHTML.Replace("\\\\n", "");
            temp = temp.Replace("\\\\\\", "");
            temp = temp.Replace("\\u003C", "<");

            if (temp.Contains("<br><br>"))
            {
                doubleLineCount = temp.Split("<br><br>").Length - 1;
                reviewHTMLTemp = temp;

                reviewHTMLTemp = reviewHTMLTemp.Replace("<br><br>", "<br>");
                singleLineParagraphCount = reviewHTMLTemp.Split("<br>").Length - 1;
                string[] ttt = temp.Split("<br><br>");
            }
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "The paragraphs are not properly spaced."
            });


            if (singleLineParagraphCount > 0 && singleLineParagraphCount == doubleLineCount) errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsSuccess = true,
                Message = "The paragraphs are properly spaced."
            });
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "The paragraphs are not properly spaced."
            });

            if (singleLineParagraphCount < 5) errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsError = true,
                Message = "Your Paragraphs are less than 5."
            });
            else errors_SuccessesModels.Add(new Errors_SuccessesModel
            {
                IsSuccess = true,
                Message = $"The paragraphs are equal or more than five: {singleLineParagraphCount}."
            });
        }

        string review = "";
        int reviewLength = 0;
        string reviewHTML = "";
        string contentDiv = "";
        private async Task GetReview()
        {
            string divs = "";

            string reviewHTMLTemp = await MainBrowser.EvaluateJavaScriptAsync("document.getElementById('content').innerHTML");
            contentDiv = reviewHTMLTemp;
            divs = await MainBrowser.EvaluateJavaScriptAsync($"document.getElementById('content').innerText");

            string temp = "";
            if (!string.IsNullOrEmpty(divs) && divs != "null")
            {
                temp = divs.Substring(divs.IndexOf("Share to Pinterest") + "Share to Pinterest".Length + ".]".Length + 1);

                if (temp.Contains("Share This Review")) temp = temp.Substring(temp.IndexOf("Share This Review") + 17);
                review = temp.Substring(0, temp.IndexOf("******")).Trim();

                string[] words = review.Split(" ");
                reviewLength = words.Length + 5;

                switch (reviewLength)
                {
                    case > 800:
                        errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsError = true,
                            Message = $"The review is longer than required: {reviewLength}"
                        });
                        break;
                    case < 300:
                        errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsError = true,
                            Message = $"The review is shorter than required: {reviewLength}"
                        });
                        break;
                    case > 399:
                        errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsSuccess = true,
                            Message = $"The review is of appropriate length: {reviewLength}"
                        });
                        break;
                    case > 299:
                        if (await DisplayAlert("Question?", "Is this a children's book", "Yes", "No")) errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsSuccess = true,
                            Message = $"The review is of appropriate length: {reviewLength}"
                        });
                        else errors_SuccessesModels.Add(new Errors_SuccessesModel
                        {
                            IsError = true,
                            Message = $"The review is shorter than required: {reviewLength}"
                        });
                        break;


                }
            }


            if (reviewHTMLTemp.Count() > 5)
            {
                reviewHTML = reviewHTMLTemp.Substring(0, reviewHTMLTemp.IndexOf("******")).Trim();
                reviewHTML = (reviewHTML.Substring(reviewHTML.IndexOf("Share to Pinterest") + "Share to Pinterest".Length + ".]".Length + 1));
                reviewHTML = reviewHTML.Substring(reviewHTML.IndexOf("evenodd") + 7);
                reviewHTML = reviewHTML.Substring(reviewHTML.IndexOf("br>") + 3);
            }
        }

        private async Task GetAuthorandBookName()
        {
            string ems = "";
            int i = 0;
            while (!ems.Contains("[Following is a volunteer review of") && !ems.Contains("Following is an official OnlineBookClub.org review of"))
            {
                ems = await MainBrowser.EvaluateJavaScriptAsync($"document.getElementsByTagName('em')[{i}].innerHTML");
                i += 1;
            }
            i = 0;

            var temp = ems.Replace("\\", "").Split("review of")[1].Split(" by ");
            bookTitle = temp[0].Substring(temp[0].IndexOf('"') + 1, temp[0].LastIndexOf('"') - 2);

            authorName = temp[1].Replace(".]", "");
        }

        private async void WriteErrors_Clicked(object sender, EventArgs e)
        {
            await GetErrors();
            displayError();
            GrammarlyBrowser.Eval("document.getElementsByClassName('_0485f5a1-document_actions-plagiarismBtn')[0].click();");
        }

        private void LeaveComment_Clicked(object sender, EventArgs e)
        {
            EditorCommenter editorCommenter = new EditorCommenter(MainBrowser);

            GreenLight.BackgroundColor = Colors.Red;
            GreenLight.Text = "Loading";
            GrammarlyBrowser.Eval("document.getElementsByClassName('navigation-correctness_f1b9s0d7')[0].click();");
        }

        private void CheckGuidelines_Clicked(object sender, EventArgs e)
        {
            EditScoreCard();
        }
    }
}