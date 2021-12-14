using Agar.IO.Client.WinForms.Controllers;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    class UpdateState : BaseCommand
    {
        [ProtoMember(1)]
        public GameState GameState { get; set; }
        public UpdateState(GameState gameState)
        {
            GameState = gameState;
        }

        public override void Execute(Game game)
        {
            //todo rewrite

            var oldGameState = game.GameState;
            if (oldGameState != null)
                return;

            game.GameState = GameState;
            Player oldCurrentPlayer = null;
            Player currentPlayer = GameState.Players.Find(p => p.Name == game.PlayerName);

            if (oldGameState?.CurrentPlayer != null)
                oldCurrentPlayer = oldGameState.CurrentPlayer;

            if (oldGameState != null)
                return;


            // prediction
            if (oldCurrentPlayer != null && game.IsPredictionValid)
            {
                if (oldGameState.EatenFoodPrediction == null)
                    game.GameState.EatenFoodPrediction = new List<Food>();
                else
                {
                    game.GameState.EatenFoodPrediction = oldGameState.EatenFoodPrediction;
                    game.GameState.FoodList.RemoveAll(f => game.GameState.EatenFoodPrediction.Contains(f));
                }

            }
            else
            {
                game.GameState.EatenFoodPrediction = new List<Food>();
            }

            GameState.CurrentPlayer = currentPlayer;
            game.IsPredictionValid = true;
        }
    }
}
