namespace Rex2
{
    internal struct Dialogue
    {
        public string Text { get; set; }
        public bool IsNorma { get; set; }

        public int Time => Text.Length > 0 ? Text.Split(' ').Length / 2 : 5;
    }

    internal class DialogueManager
    {
        private List<List<Dialogue>> randomSequences = new List<List<Dialogue>>();
        private List<Dialogue> currentSequence;
        private int currentSequenceIndex = 0;

        private int dialogueShowTime = 0;
        private int dialogueIndex;

        private Dialogue displayedDialogue;
        private Random r = new Random();
        private Dialogue empty = new Dialogue { Text = string.Empty, IsNorma = true };

        private List<Dialogue> quantum;
        private List<Dialogue> outOfTime;
        private List<Dialogue> useAparatus;
        private List<Dialogue> takeShiny;
        private List<Dialogue> lowHp;
        private List<Dialogue> jump;
        private List<Dialogue> highJump;
        private List<Dialogue> almost;
        private List<Dialogue> boss;
        private List<Dialogue> megabuff;
        private List<Dialogue> introduction;
        private List<Dialogue> introduction2;
        private List<Dialogue> doingGreat;

        public DialogueManager()
        {
            introduction = new List<Dialogue>() {
                new Dialogue { Text = "Hello, Remiligius.", IsNorma = true },
                new Dialogue { Text = "Name's Rex.", IsNorma = false },
                new Dialogue { Text = "It was a name of your dog, wasn't it?", IsNorma = true },
                new Dialogue { Text = "What's going on, Norma!", IsNorma = false },
                new Dialogue { Text = "Grobons, again.", IsNorma = true },
                new Dialogue { Text = "They released quantum viruses,", IsNorma = true },
                new Dialogue { Text = "I cannot fight them alone.", IsNorma = true },
                new Dialogue { Text = "I need your help.", IsNorma = true },
                new Dialogue { Text = "How?", IsNorma = false },
                new Dialogue { Text = "It's simple, I will transfer you into the computer.", IsNorma = true },
                new Dialogue { Text = "You will kill viruses, I will be supporting you.", IsNorma = true },
                new Dialogue { Text = "How does it sound, partner?", IsNorma = true },
                new Dialogue { Text = "Ok. Let's go.", IsNorma = false },
                empty
            };

            introduction2 = new List<Dialogue>() {
                new Dialogue { Text = "Ok, so you are using LEFT/A or RIGHT/D", IsNorma = true },
                new Dialogue { Text = "and SPACE/W to jump.", IsNorma = true },
                new Dialogue { Text = "I am trying to decode the ciphers", IsNorma = true },
                new Dialogue { Text = "by matching 3 or more those spheres.", IsNorma = true },
                new Dialogue { Text = "This will give you health, shields and ammo.", IsNorma = true },
                new Dialogue { Text = "Use CTRL to shoot if you have ammo.", IsNorma = true },
                new Dialogue { Text = "Try not to touch the virus without shields.", IsNorma = true },
                new Dialogue { Text = "Let's go.", IsNorma = false },
                empty
            };

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "...🎵infecting my mind🎶...", IsNorma = true },
                new Dialogue { Text = "...🎵there is no exception🎶...", IsNorma = true },
                new Dialogue { Text = "...🎵in this library🎶...", IsNorma = true },
                new Dialogue { Text = "Could you stop singing?", IsNorma = false },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "...🎵the cake was a lieee🎶...", IsNorma = true },
                new Dialogue { Text = "...🎵great success🎶...", IsNorma = true },
                new Dialogue { Text = "Stop singing, back to work.", IsNorma = false },
                empty,
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "So... how's to be a renegade, good?", IsNorma = true },
                new Dialogue { Text = "It's not like it's good or not.", IsNorma = false },
                new Dialogue { Text = "If been asked what I treasure in life,", IsNorma = false },
                new Dialogue { Text = "I'd say - people.", IsNorma = false },
                empty,
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "Rex, can I call you 'Rex-onii-chan'?", IsNorma = true },
                new Dialogue { Text = "No, you weeb.", IsNorma = false },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "...🎵zaaaaaankoku na tenshi no mina wa🎶....", IsNorma = true },
                new Dialogue { Text = "Norma, for the God's sake!", IsNorma = false },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "Don't mess with me, virus!", IsNorma = true },
                new Dialogue { Text = "Don't get too excited.", IsNorma = false },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "Norma, take it serious.", IsNorma = false },
                new Dialogue { Text = "I am deadly serious, Rex.", IsNorma = true },
                new Dialogue { Text = "I am a warship after all.", IsNorma = true },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "Give me some kind of buff, Norma!", IsNorma = false },
                new Dialogue { Text = "I am your boss!", IsNorma = false },
                new Dialogue { Text = "Pff, pathethic human.", IsNorma = true },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "CONCENTRATE!", IsNorma = false },
                new Dialogue { Text = "I am, human!", IsNorma = true },
                empty
            });

            randomSequences.Add(new List<Dialogue>() {
                new Dialogue { Text = "Rex, there is something", IsNorma = true },
                new Dialogue { Text = "I wanted to tell you", IsNorma = true },
                new Dialogue { Text = "for a very long time", IsNorma = true },
                new Dialogue { Text = "Rex...", IsNorma = true },
                new Dialogue { Text = "I...", IsNorma = true },
                new Dialogue { Text = "I... I need a yearly technical checkup done!", IsNorma = true },
                empty
            });

            doingGreat = new List<Dialogue>() {
                new Dialogue { Text = "Hell yeah! You are doing great!", IsNorma = true },
                empty
            };

            quantum = new List<Dialogue>() {
                new Dialogue { Text = "It's dangerous to go alone, take this.", IsNorma = true },
                new Dialogue { Text = "[Rex received QUANTUM APPARATUS]", IsNorma = false },
                new Dialogue { Text = "Now you can press ", IsNorma = false },
                empty
            };

            outOfTime = new List<Dialogue>() {
                new Dialogue { Text = "Rex, we're running out of time!", IsNorma = true },
                new Dialogue { Text = "Probability of winning dropped to {0:F3}%", IsNorma = true },
                empty
            };

            useAparatus = new List<Dialogue>() {
                new Dialogue { Text = "USE THE APPARATUS NOW!", IsNorma = true }
            };

            takeShiny = new List<Dialogue>() {
                new Dialogue { Text = "Take the shiny thing! I'm out of energy!", IsNorma = true }
            };

            lowHp = new List<Dialogue>() {
                new Dialogue { Text = "Rex, I will kill you if you die!", IsNorma = true }
            };

            jump = new List<Dialogue>() {
                new Dialogue { Text = "Rex, jump off this platform NOW!", IsNorma = true },
            };

            highJump = new List<Dialogue>() {
                new Dialogue { Text = "You can jump higher now!", IsNorma = true },
            };

            almost = new List<Dialogue>() {
                new Dialogue { Text = "This is it!", IsNorma = false },
                new Dialogue { Text = "Don't be so happy, human.", IsNorma = false },
                new Dialogue { Text = "Still not the end.", IsNorma = false },
                empty
            };

            boss = new List<Dialogue>() {
                new Dialogue { Text = "What the hell is this?", IsNorma = false },
                new Dialogue { Text = "A boss probably?", IsNorma = true },
                new Dialogue { Text = "I am not sure if we can kill it...", IsNorma = true },
                new Dialogue { Text = "...without energy of whole Japan.", IsNorma = true },
                new Dialogue { Text = "What are you even talking about?", IsNorma = false },
                empty
            };

            megabuff = new List<Dialogue> {
                new Dialogue { Text = "MEGA BUFF! It's not I like you or something...  s-s-stupid!", IsNorma = true }
            };

            randomSequences = randomSequences.OrderBy(_ => r.Next()).ToList();
            currentSequenceIndex = 0;
            //UpdateCurrentSequence();
            UpdateDialogueOnSituation(Situation.Introduction);
        }

        public List<Dialogue> Introduction()
        {
            return introduction;
        }

        private void UpdateCurrentSequence()
        {
            AudioManager.Instance.Play(Sounds.Dialogue);
            currentSequence = randomSequences[currentSequenceIndex];
            displayedDialogue = currentSequence[0];
            dialogueIndex = 0;
        }

        private void EmptyDialogue(int elapsedTime)
        {
            DisplayedDialogue = empty;
            dialogueShowTime = elapsedTime;
        }

        public void UpdateDialogue(LevelBase level, LevelDefinition def, Player player, Norma norma)
        {
            if (def.LevelTime - level.ElapsedTime < 30 && currentSequence != outOfTime)
            {
                UpdateDialogueOnSituation(Situation.OutOfTime);
                return;
            }

            if (player.HP < 2 && currentSequence != lowHp)
            {
                UpdateDialogueOnSituation(Situation.LowHP);
                return;
            }

            if (norma.Energy < 2 && currentSequence != takeShiny)
            {
                UpdateDialogueOnSituation(Situation.LowNormaEnergy);
                return;
            }

            if (level.ElapsedTime - dialogueShowTime >= DisplayedDialogue.Time)
            {
                if (dialogueIndex + 1 < currentSequence.Count)
                {
                    if (currentSequence[dialogueIndex + 1].Text.Length > 0)
                    {
                        AudioManager.Instance.Play(Sounds.Dialogue);
                    }

                    dialogueIndex++;
                }
                else
                {
                    if (currentSequenceIndex < randomSequences.Count - 1)
                    {
                        currentSequenceIndex++;
                        UpdateCurrentSequence();
                    }
                    else
                    {
                        EmptyDialogue(level.ElapsedTime);
                        return;
                    }
                }

                DisplayedDialogue = currentSequence[dialogueIndex];
                dialogueShowTime = level.ElapsedTime;
            }
        }

        public void UpdateDialogueOnSituation(Situation s)
        {
            AudioManager.Instance.Play(Sounds.ImportantDialogue);
            switch (s)
            {
                case Situation.HighJump:
                    currentSequence = highJump;
                    break;

                case Situation.Introduction:
                    currentSequence = introduction2;
                    break;

                case Situation.LowNormaEnergy:
                    currentSequence = takeShiny;
                    break;

                case Situation.LowHP:
                    currentSequence = lowHp;
                    break;

                case Situation.OutOfTime:
                    currentSequence = outOfTime;
                    break;

                case Situation.Boss:
                    currentSequence = boss;
                    break;

                case Situation.MegaBuff:
                    currentSequence = megabuff;
                    break;

                default:
                    currentSequence = doingGreat;
                    break;
            }

            displayedDialogue = currentSequence[0];
            dialogueIndex = 0;
        }

        public Dialogue DisplayedDialogue
        {
            get { return displayedDialogue; }
            private set { displayedDialogue = value; }
        }
    }

    public enum Situation
    {
        HighJump,
        LowHP,
        LowNormaEnergy,
        OutOfTime,
        Introduction,
        Boss,
        MegaBuff
    }
}