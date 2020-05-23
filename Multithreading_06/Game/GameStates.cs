namespace Multithreading_06
{
    public enum GameState
    {
        GameIdle,
        GameWaiting,
        GameActive,
        GameOver,
        GameWin
    }

    internal class GameStates
    {
        private Game myGame;
        private GameState myGameState;

        public GameState GameState => myGameState;

        public GameStates(Game game)
        {
            this.myGame = game;
        }

        public void SetState(GameState gameState)
        {
            myGameState = gameState;
            UpdateStateText();
        }

        public void UpdateStateText()
        {
            switch (myGameState)
            {
                case GameState.GameIdle:
                    MainForm.Form.UpdateGameStateText("Game Idle");
                    break;

                case GameState.GameWaiting:
                    MainForm.Form.UpdateGameStateText("Game Waiting");
                    break;

                case GameState.GameActive:
                    MainForm.Form.UpdateGameStateText("Game Active");
                    break;

                case GameState.GameOver:
                    MainForm.Form.UpdateGameStateText("Game Over...");
                    break;

                case GameState.GameWin:
                    MainForm.Form.UpdateGameStateText("Game Win...");
                    break;
            }
        }
    }
}