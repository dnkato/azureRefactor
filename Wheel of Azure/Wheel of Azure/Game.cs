using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel_of_Azure
{
    public class Game
    {
        public Wheel wheel = new Wheel();
        public PhraseBoard phraseBoard;
        public List<Player> players = new List<Player>();

        ICategorizedPhrases catphrase;
        private GameUI ui = new GameUI();


        public Game(ICategorizedPhrases catphrase)
        {
            this.catphrase = catphrase;
            phraseBoard = new PhraseBoard(catphrase.GetPhrase(catphrase.category));
            
        }

        /// <summary>
        /// Starts the executions of the Wheel of Fortune console app game
        /// </summary>
        public void Start()
        {

            foreach (string playerName in ui.GetPlayerNames())
            {
                players.Add(new Player(playerName));
            }
            int currentPlayer = 0;
            int roundWinner = 0;

            ui.DisplayWelcomeMessage(players);
            ui.DisplayCat(catphrase);

            while (!phraseBoard.IsGameOver())
            {
                
                if (players.Count > 1)
                {
                    ui.SetPlayerTextColor(currentPlayer);
                    ui.DisplayPlayerTurn(players[currentPlayer]);
                }

                TakeTurn(players[currentPlayer], phraseBoard);

                // check to see if the current player won the game
                if (phraseBoard.IsGameOver()) roundWinner = currentPlayer;

                // advance to the next player
                currentPlayer = (currentPlayer + 1) % players.Count;

            }

            ui.ResetTextColorToWhite();

            ui.DisplayBoard(phraseBoard);
            ui.DisplayWinner(players, roundWinner);

            ui.GetExit();

        }

        /// <summary>
        /// The current player takes their turn until they guess incorrectly, solve the puzzle, por spin Lose-A-Turn
        /// </summary>
        /// <param name="playerOne"></param>
        /// <param name="phraseBoard"></param>
        private void TakeTurn(Player playerOne, PhraseBoard phraseBoard)
        {
            bool turnOver;

            do
            {

                ui.DisplayBoard(phraseBoard);

                if (ui.GetUserChoice() == 1)
                {
                    Spin(playerOne, out turnOver);
                }
                else
                {
                    Solve(playerOne, out turnOver);
                }

                ui.DisplayPlayerScore(playerOne);

            } while (!(turnOver || phraseBoard.IsGameOver()));

        }

        /// <summary>
        /// Spins the wheel and asks the user to make a guess.
        /// </summary>
        private void Spin(Player playerOne, out bool turnOver)
        {
            int wheelAmount = wheel.WheelSpin();
            ui.DisplayWheelAmount(wheelAmount);

            // If the player spins a value of LOSE A TURN or BANKRUPTCY, there turn is over...
            switch (wheelAmount)
            {
                case Wheel.LoseATurn:
                    turnOver = true;
                    return;
                case Wheel.Bankruptcy:
                    turnOver = true;
                    playerOne.DeductCurrentScore(playerOne.TurnScore);
                    return;
                default:
                    break;
            }

            // Spin was good, so Ask the user to guess a letter
            char spinGuessLetter = ui.GetSpinGuessLetter(phraseBoard, playerOne);

            //if the phrase contains the character, the program tells the user of this and tell them how much they won. 
            //It then goes back to the spin or solve function.
            int pointsEarned = phraseBoard.MakeGuess(wheelAmount, spinGuessLetter);
            if (pointsEarned > 0)
            {
                ui.DisplaySpinGuessLetterSuccess(spinGuessLetter, pointsEarned);
                playerOne.AddCurrentScore(pointsEarned);
                turnOver = false;
                return;
            }
            //if it does not contain the character, it tells the user it has not have it and to guess again.
            //It then goes back to the spin or solve function.
            else
            {
                ui.DisplaySpinGuessetterFailure(spinGuessLetter);
                turnOver = true;
                return;
            }
        }

        /// <summary>
        /// Asks the user to solve the phrase.
        /// </summary>
        private void Solve(Player playerOne, out bool turnOver)
        {
            //Prompts the user to solve the phrase. If they get it right, they win. 
            //If not, then it goes back to the spin or solve function.
            string solveGuess = ui.GetSolveGuess();
            int pointsEarned = phraseBoard.MakeGuess(solveGuess);
            if (pointsEarned <= 0)
            {
                ui.DisplaySolveGuessFailure(solveGuess);
                turnOver = true;
            }
            else
            {
                ui.DisplaySolveGuessSuccess(solveGuess);
                playerOne.AddCurrentScore(pointsEarned);
                turnOver = false;
            }
        }


    }
}
