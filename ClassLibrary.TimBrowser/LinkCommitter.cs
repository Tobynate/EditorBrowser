namespace ClassLibrary.TimBrowser
{
    public class LinkCommitter
    {
        string reviewsGetterPath = /*@"C:\Users\User\Documents\ReviewLinkCopier\MyReviewLinks.txt";*/@"D:\ReviewLinkCopier.Ameli\MyReviewLinks.txt";
        string allReviewPath = /*@"C:\Users\User\Documents\ReviewLinkCopier\ReviewLinkCommiter.docs\AllReview.txt";*/@"D:\ReviewLinkCopier.Ameli\ReviewLinkCommiter.docs\AllReview.txt";
        static string doneReviewsPath = /*@"C:\Users\User\Documents\ReviewLinkCopier\ReviewLinkCommiter.docs\DoneReviewID.txt";*/@"D:\ReviewLinkCopier.Ameli\ReviewLinkCommiter.docs\DoneReviewID.txt";
        static string availableReviewsPath = /*@"C:\Users\User\Documents\ReviewLinkCopier\ReviewLinkCommiter.docs\AvailableReviewID.txt";*/@"D:\ReviewLinkCopier.Ameli\ReviewLinkCommiter.docs\AvailableReviewID.txt";

        string doneReviews = File.ReadAllText(doneReviewsPath);
        string availableReviews = File.ReadAllText(availableReviewsPath);
        List<string> tempAvailableID = new();
        List<string> tempDoneID = new();
        public string linkToNavigate()
        {
            string output = "";

            string reviewsString = File.ReadAllText(reviewsGetterPath);

            int reviewIds = 0;

            List<string> reviews = new List<string>();
            if (!string.IsNullOrEmpty(reviewsString))
            {
                reviews = reviewsString.Split("~~||").ToList();

            }

            List<string> allReviewsWithIDretrieved = File.ReadAllLines(allReviewPath).ToList();
            List<string> allReviewsCommit = new List<string>();

            if (allReviewsWithIDretrieved.Count == 0)
            {
                if (reviews.Count == 0)
                {
                    Console.Error.WriteLine("No reviews to edit at this time");
                }
                else
                {
                    foreach (string review in reviews)
                    {
                        reviewIds += 1;
                        allReviewsCommit.Add(reviewIds + "|**|" + review.Trim());
                    }
                    File.WriteAllLines(allReviewPath, allReviewsCommit.ToArray());
                }
            }
            else
            {
                if (reviews.Count > 0)
                {
                    int lastId = 0;
                    int.TryParse(allReviewsWithIDretrieved[allReviewsWithIDretrieved.Count - 1].Substring(0, allReviewsWithIDretrieved[0].IndexOf("|**|")), out lastId);

                    int retainer = 0;
                    int comparer = 0;
                    foreach (string item in allReviewsWithIDretrieved)
                    {
                        string str = item.Substring(0, item.IndexOf("|**|"));

                        int.TryParse(str, out comparer);

                        if (comparer > retainer) retainer = comparer;
                    }
                    lastId = retainer + 500;
                    var temp = new List<string>();
                    foreach (string review in reviews)
                    {
                        lastId = lastId + 1;
                        var test = review.Trim().Replace("\n", "");
                        var test1 = String.Join(" ", allReviewsWithIDretrieved);
                        if (!test1.Contains(test))
                        {
                            temp.Add(lastId + "|**|" + review.Trim());
                        }
                    }
                    temp.Reverse();
                    if (temp.Count > 0)
                    {
                        allReviewsCommit = temp.Concat(allReviewsWithIDretrieved).ToList();
                    }
                    else
                    {
                        allReviewsCommit = allReviewsWithIDretrieved;
                    }
                    File.WriteAllLines(allReviewPath, allReviewsCommit.ToArray());

                }

            }



            FillTempAvailable();
            FillTempDone();

            if (string.IsNullOrEmpty(doneReviews))
            {
                foreach (string review in allReviewsCommit)
                {
                    string id = review.Substring(0, review.IndexOf("|**|"));
                    if (!tempAvailableID.Contains(id))
                    {
                        availableReviews = availableReviews + "||" + id;
                    }
                }
            }
            else
            {
                foreach (string review in allReviewsCommit)
                {
                    string id = review.Substring(0, review.IndexOf("|**|"));

                    if (!tempDoneID.Contains(id))
                    {
                        if (!tempAvailableID.Contains(id))
                        {
                            availableReviews = availableReviews + "||" + id;
                        }
                    }
                }

                //availableReviews.Replace(doneReviews.Replace("||", "$||"), "");
            }


            if (!String.IsNullOrWhiteSpace(availableReviews) && availableReviews[0] == '|') availableReviews = availableReviews.Remove(0, 2);

            FillTempAvailable();
            foreach (string id in tempDoneID) tempAvailableID.Remove(id);

            File.WriteAllText(availableReviewsPath, availableReviews);

            ListenForKeyPress(allReviewsCommit, out output);

            availableReviews = AddandTrim(tempAvailableID, availableReviews, true);
            doneReviews = AddandTrim(tempDoneID, doneReviews, false);


            File.WriteAllText(availableReviewsPath, availableReviews);
            File.WriteAllText(doneReviewsPath, doneReviews);

            //Console.WriteLine("No more REVIEWS to edit");

            return output;

        }
        void ListenForKeyPress(List<string> allReviewsCommit, out string output)
        {
            output = "";

            //ConsoleKeyInfo keyInfo = new ConsoleKeyInfo((char)13, ConsoleKey.Enter, false, false, false);
            //    while (Console.ReadKey() == keyInfo)
            //    {
            if (tempAvailableID.Count > 0)
            {
                Console.WriteLine("You have " + tempAvailableID.Count + " reviews left to edit");
                Random rand = new Random();

                int index = rand.Next(tempAvailableID.Count);
                string copiedReview = allReviewsCommit.Where(x => x.Contains(tempAvailableID[index] + "|**|")).First();
                copiedReview = copiedReview.Substring(copiedReview.IndexOf("|**|") + "|**|".Count());

                if (!string.IsNullOrEmpty(copiedReview))
                {
                    output = copiedReview;
                }

                // Clipboard.SetText(tempAvailableID[index]);
                Console.WriteLine("ID: " + tempAvailableID[index]);
                Console.WriteLine(copiedReview);

                tempDoneID.Add(tempAvailableID[index]);
                tempAvailableID.Remove(tempAvailableID[index]);


            }
            //    else
            //    {
            //        break;
            //    }
            ////}
        }
        string AddandTrim(List<string> tempList, string longString, bool availableID)
        {
            //if (availableID)
            //{
            //}

            longString = "";
            tempList = tempList.Distinct().ToList();
            foreach (var item in tempList)
            {
                longString = longString + "||" + item;
            }
            if (!String.IsNullOrWhiteSpace(longString) && longString[0] == '|') longString = longString.Remove(0, 2);

            return longString;
        }


        void FillTempAvailable()
        {
            if (!string.IsNullOrEmpty(availableReviews))
            {
                tempAvailableID = availableReviews.Split("||").ToList();
                tempAvailableID = tempAvailableID.Distinct().ToList();
            }
        }

        void FillTempDone()
        {
            if (!string.IsNullOrEmpty(doneReviews))
            {
                tempDoneID = doneReviews.Split("||").ToList();
                tempDoneID = tempDoneID.Distinct().ToList();
            }
        }

    }

}
