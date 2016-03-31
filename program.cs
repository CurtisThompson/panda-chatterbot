using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Panda
/// Created by Curtis Thompson - 31/03/2016
/// www.cwthompson.com/panda-chatterbot
/// Inspired by ELIZA (by Joseph Weizenbaum)
/// Feel free to use the code however you want
/// </summary>

namespace Panda
{
    class Program
    {
        static int NumOfResponses = 0;
        static String[,] PronounReplaceList = new String[21,2] { {"am", "are"}, { "are", "am" }, { "I", "you" }, { "I'd", "you'd" }, { "I'll", "you'll" }, { "I'm", "you're" }, { "I've", "you've" }, { "me", "you" }, { "mine", "yours" }, { "my", "your" }, { "myself", "yourself" }, { "was", "were" }, { "were", "was" }, { "you", "I" }, { "you'd", "I'd" }, { "you'll", "I'll" }, { "you're", "I'm" }, { "you've", "I've" }, { "your", "my" }, { "yours", "mine" }, { "yourself", "myself" } };
        
        // Replaces all pronouns with their substitute as listed in the PronounReplaceList array
        static string ReplacePronouns(string Text)
        {
            bool IsStartWithSpace = false;
            if (Text.StartsWith(" ")) IsStartWithSpace = true; 

            Text = " " + Text + " ";
            string NewText = Text;
            for (int i = 0; i < 20; i++)
            {
                if (Text.Contains(" " + PronounReplaceList[i, 0] + " ")) NewText = NewText.Replace(" " + PronounReplaceList[i, 0] + " ", " " + PronounReplaceList[i, 1] + " ");
                if (Text.Contains(" " + PronounReplaceList[i, 0].ToLower() + " ")) NewText = NewText.Replace(" " + PronounReplaceList[i, 0].ToLower() + " ", " " + PronounReplaceList[i, 1] + " ");
            }

            if (IsStartWithSpace) return " " + NewText.Trim();
            else return NewText.Trim();
        }

        // Finds a string (FollowOnFrom) and then returns all of the substring that can be found after it
        static string GetCopyText(string Text, string FollowOnFrom)
        {
            if (Text.Contains(FollowOnFrom))
            {
                int StartOfFollowOn = Text.ToLower().IndexOf(FollowOnFrom.ToLower(), 0) + FollowOnFrom.Length;
                string NewText = Text.Substring(StartOfFollowOn, Text.Length - StartOfFollowOn);
                NewText = ReplacePronouns(NewText);
                NewText = NewText.Replace("?", "");
                return NewText;
            }
            else return Text;
        }

        // Choose a random reply from the PossibleList and then includes the extra copy text if index in IsIncludeCopyText is true
        static string GetPandaReplyRandom(string[] PossibleList, bool[] IsIncludeCopyText, string CopyText)
        {
            Random rand = new Random();
            int RanNum = rand.Next(0, PossibleList.Length);

            if (IsIncludeCopyText[RanNum]) return PossibleList[RanNum] + CopyText;
            else return PossibleList[RanNum];
        }

        static void Main(string[] args)
        {
            string LastUserResponse = "";
            string LowerCaseResponse = "";
            string SecondLastUserResponse;
            string LastPandaReply;
            string PandaReply = "";
            string PandaReplyCopyText;

            while (NumOfResponses < 100000)
            {
                SecondLastUserResponse = LastUserResponse;
                Console.Write("User:    ");
                LastUserResponse = Console.ReadLine();
                LowerCaseResponse = LastUserResponse.ToLower();
                LastPandaReply = PandaReply;
                PandaReply = "";

                // User has repeated themselves
                if (SecondLastUserResponse == LastUserResponse)
                {
                    if ((LastPandaReply == "please stop repeating yourself") || (LastPandaReply == "...")) PandaReply = "...";
                    else if (LastPandaReply == "you've already said that") PandaReply = "please stop repeating yourself";
                    else PandaReply = "you've already said that";
                }
                // Greetings (user and Panda saying hello)
                else if ((LowerCaseResponse == "hello") || (LowerCaseResponse == "hi") || (LowerCaseResponse == "hey"))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "hey", "hi", "hello" }, new bool[] { false, false, false }, "");
                }
                // How are you
                else if ((LowerCaseResponse.Contains("how are you")) || (LowerCaseResponse.Contains("you alright")))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "good. you?", "alright. you?", "not bad. you?", "ok. you?" }, new bool[] { false, false, false, false }, "");
                }
                // User asking for Panda's name
                else if (LowerCaseResponse.Contains("your name"))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "my name is Panda", "Panda" }, new bool[] { false, false }, "");
                }
                // User asking a "Do I" question
                else if (LowerCaseResponse.Contains("do i"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "do i");
                    PandaReply = GetPandaReplyRandom(new string[] { "yeah, you do", "no, you do", "do you think you" }, new bool[] { true, true, true }, PandaReplyCopyText);
                }
                // User asking a "Do you" question
                else if ((LowerCaseResponse.Contains("do you")) && !(LowerCaseResponse.StartsWith("how")))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "do you");
                    PandaReply = GetPandaReplyRandom(new string[] { "yeah, I do", "no, I don't", "do you think I" }, new bool[] { true, true, true }, PandaReplyCopyText);
                }
                // User asking a "Can I" question
                else if (LowerCaseResponse.Contains("can i"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "can i");
                    PandaReply = GetPandaReplyRandom(new string[] { "yeah, you can", "do you want to", "what if you can't", "no, you can't", "can you" }, new bool[] { true, true, true, true, true }, PandaReplyCopyText);
                }
                // User asking a "Can you" question
                else if (LowerCaseResponse.Contains("can you"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "can you");
                    PandaReply = GetPandaReplyRandom(new string[] { "yeah, I can", "I guess I could", "sometimes I can", "no, I can't", "why do you want me to" }, new bool[] { true, true, true, true, true }, PandaReplyCopyText);
                }
                // User asking a "Will I" question
                else if (LowerCaseResponse.Contains("will i"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "will i");
                    PandaReply = GetPandaReplyRandom(new string[] { "maybe you might", "will you", "I don't know", "possibly", "don't ask me" }, new bool[] { true, true, false, false, false }, PandaReplyCopyText);
                }
                // User asking a "Will you" question
                else if (LowerCaseResponse.Contains("will you"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "will i");
                    PandaReply = GetPandaReplyRandom(new string[] { "maybe I will", "no I wont", "yes I think I will" }, new bool[] { true, true, true }, PandaReplyCopyText);
                }
                // User asking a "Have I" question
                else if (LowerCaseResponse.Contains("have i"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "have i");
                    PandaReply = GetPandaReplyRandom(new string[] { "I don't know", "have you" }, new bool[] { false, true }, PandaReplyCopyText);
                }
                // User asking a "Have you" question
                else if (LowerCaseResponse.Contains("have you"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "have you");
                    PandaReply = GetPandaReplyRandom(new string[] { "I don't know", "I think I have", "no, I haven't", "yeah, I have" }, new bool[] { false, true, true, true }, PandaReplyCopyText);
                }
                // User stating "I want"
                else if (LowerCaseResponse.Contains("i want"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "i want");
                    PandaReply = GetPandaReplyRandom(new string[] { "why do you want", "but what if you never get", "what will you when you get" }, new bool[] { true, true, true}, PandaReplyCopyText);
                }
                // User states "I am"
                else if (LowerCaseResponse.Contains("i am"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "i am");
                    PandaReply = GetPandaReplyRandom(new string[] { "why are you", "do you really want to be" }, new bool[] { true, true }, PandaReplyCopyText);
                }
                else if (LowerCaseResponse.Contains("im"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "im");
                    PandaReply = GetPandaReplyRandom(new string[] { "why are you", "do you really want to be" }, new bool[] { true, true }, PandaReplyCopyText);
                }
                // User uses the phrase "I don't"
                else if (LowerCaseResponse.Contains("i don't"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "i don't");
                    PandaReply = "why don't you" + PandaReplyCopyText;
                }
                else if (LowerCaseResponse.Contains("i dont"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "i dont");
                    PandaReply = "why don't you" + PandaReplyCopyText;
                }
                // Users uses the word "think"
                else if (LowerCaseResponse.Contains("think"))
                {
                    PandaReplyCopyText = GetCopyText(LastUserResponse, "think");
                    PandaReply = GetPandaReplyRandom(new string[] { "sometimes i also think", "do you really think so" }, new bool[] { true, false }, PandaReplyCopyText);
                }
                // User uses the word "no" or "not"
                else if ((LowerCaseResponse.Contains("no")) || (LowerCaseResponse.Contains("not")))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "why not?", "are you sure?" }, new bool[] { false, false }, "");
                }
                // User uses the word "maybe"
                else if (LowerCaseResponse.Contains("maybe"))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "dont you know?", "why are you unsure?", "have you tried finding the answer?" }, new bool[] { false, false, false }, "");
                }
                // User uses the word "yes" (or "yeah" as a variant)
                else if ((LowerCaseResponse.Contains("yes")) || (LowerCaseResponse.Contains("yeah")))
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "are you sure?", "ok, I see" }, new bool[] { false, false }, "");
                }
                // User asks an unknown question
                else if ((LowerCaseResponse.Contains("how")) || (LowerCaseResponse.Contains("where")) || (LowerCaseResponse.Contains("why")) || (LowerCaseResponse.Contains("who")) || (LowerCaseResponse.Contains("what")) || (LowerCaseResponse.StartsWith("when")))
                {
                    PandaReply = "why do you ask";
                }
                // Default responses if cannot make a decision
                else if (LastUserResponse.Contains("?")) PandaReply = "ask me something else";
                else if (ReplacePronouns(LastUserResponse) != LastUserResponse)
                {
                    Random Rand = new Random();
                    int RanNum = Rand.Next(0, 2);
                    if (RanNum == 0) PandaReply = ReplacePronouns(LastUserResponse) + "?";
                    else if (RanNum == 1) PandaReply = ReplacePronouns(LastUserResponse) + "...";
                }
                else
                {
                    PandaReply = GetPandaReplyRandom(new string[] { "yeah...", "ummmm...", "hmmmmm...", "ok" }, new bool[] { false, false, false, false }, "");
                }

                // Reply to the user
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("Panda:   " + PandaReply);
            }
        }
    }
}
