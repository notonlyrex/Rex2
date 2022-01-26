namespace Rex2
{
    internal struct Dialogue
    {
        public string Text { get; set; }
        public bool IsNorma { get; set; }

        public int Time => Text.Length > 0 ? Text.Split(' ').Length * 1 : 5;
    }

    internal class DialogueManager
    {
        private List<List<Dialogue>> randomSequences = new List<List<Dialogue>>();
        private List<Dialogue> currentSequence;

        private int dialogueShowTime = 0;
        private int dialogueIndex;

        private Dialogue displayedDialogue;
        private Random r = new Random();
        private Dialogue empty = new Dialogue { Text = string.Empty, IsNorma = true };

        public DialogueManager()
        {
            //var introductionSequence = new List<Dialogue>();
            //introductionSequence.Add(new Dialogue { Text = "Hello, Remiligius.", IsNorma = true });
            //introductionSequence.Add(new Dialogue { Text = "I'm Rex.", IsNorma = false });
            //introductionSequence.Add(new Dialogue { Text = "It was a name of your dog, wasn't it?", IsNorma = true });
            //introductionSequence.Add(new Dialogue { Text = "Nevermind. We have a problem.", IsNorma = true });

            //var randomSingSequence1 = new List<Dialogue>();
            //randomSingSequence1.Add(new Dialogue { Text = "...🎵infecting my mind🎶...", IsNorma = true });
            //randomSingSequence1.Add(new Dialogue { Text = "...🎵there is no exception🎶...", IsNorma = true });
            //randomSingSequence1.Add(new Dialogue { Text = "...🎵in this library🎶...", IsNorma = true });
            //randomSingSequence1.Add(new Dialogue { Text = "Could you stop singing?", IsNorma = false });
            //randomSingSequence1.Add(empty);
            //sequences.Add(randomSingSequence1);

            //var randomSingSequence2 = new List<Dialogue>();
            //randomSingSequence2.Add(new Dialogue { Text = "...🎵it was a lie🎶...", IsNorma = true });
            //randomSingSequence2.Add(new Dialogue { Text = "...🎵great success🎶...", IsNorma = true });
            //randomSingSequence2.Add(new Dialogue { Text = "Stop singing, back to work.", IsNorma = false });
            //randomSingSequence2.Add(empty);
            //sequences.Add(randomSingSequence2);

            var randomSequence1 = new List<Dialogue>();
            randomSequence1.Add(new Dialogue { Text = "Wiec, jak to jest byc renegatem, dobrze?", IsNorma = true });
            randomSequence1.Add(new Dialogue { Text = "Nie ma tak, ze dobrze, albo ze nie dobrze.", IsNorma = false });
            randomSequence1.Add(new Dialogue { Text = "Jakbym mial powiedziec, co cenie w zyciu...", IsNorma = false });
            randomSequence1.Add(new Dialogue { Text = "...powiedzialbym, ze ludzi.", IsNorma = false });
            randomSequence1.Add(empty);
            randomSequences.Add(randomSequence1);

            /*
             * Can I call you 'Rex-onii-chan'?              /Fucking weeb.
             * ...🎵zaaaaaankoku na tenshi no mina wa🎶....
             *
            */

            /*
             * Rex, we're running out of time!   Probability of winning dropped to {0:F3}%
             * Rex, jump off this platform NOW!
             * It's dangerous to go alone, take this.  /[Rex received QUANTUM APPARATUS]
             * USE THE APPARATUS NOW!
             * Take that shiny thing! I'm out of energy!
             * Don't mess with me, human.
             * /Be serious, Norma.      Trust me, you don't want it.
             * /What the hell is this?  Probably a boss.  It looks we need the whole energy of Japan to kill him.   /What are you talking about?
             * I've told you I hate Grobons?
             * /This is it!     Don't be so happy, human.
             * /I am your boss, right?      Hm, pathethic.
             * It's not I like you or something...  s-s-stupid! (jakiś mega BUFF)
             * It reminds me of a movie... Tron?
             * When you are here, I'd like to remind you...     ...the ship's technical review is past due.
             */

            //currentSequence = introductionSequence;
            currentSequence = randomSequence1;
            displayedDialogue = currentSequence[0];
            dialogueIndex = 0;
        }

        private void EmptyDialogue(int elapsedTime)
        {
            DisplayedDialogue = empty;
            dialogueShowTime = elapsedTime;
        }

        public void UpdateDialogueOnTime(int elapsedTime)
        {
            if (elapsedTime - dialogueShowTime >= DisplayedDialogue.Time)
            {
                if (dialogueIndex + 1 < currentSequence.Count)
                {
                    dialogueIndex++;
                }
                else
                {
                    if (randomSequences.Count > 0)
                    {
                        currentSequence = randomSequences[r.Next(randomSequences.Count)];
                        dialogueIndex = 0;
                    }
                    else
                    {
                        EmptyDialogue(elapsedTime);
                        return;
                    }
                }

                DisplayedDialogue = currentSequence[dialogueIndex];
                dialogueShowTime = elapsedTime;
            }
        }

        public void UpdateDialogueOnSituation(int health)
        {
        }

        public Dialogue DisplayedDialogue
        {
            get { return displayedDialogue; }
            private set { displayedDialogue = value; }
        }
    }
}