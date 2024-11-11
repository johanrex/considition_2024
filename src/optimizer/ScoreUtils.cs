using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;
using NativeScorer;

namespace optimizer
{
    internal class ScoreUtils
    {
        private ServerUtils serverUtils;
        private ConfigService configService;

        private ScoreUtils(){}

        public ScoreUtils(ServerUtils serverUtils, ConfigService configService)
        {
            this.serverUtils = serverUtils;
            this.configService = configService;
        }

        public GameResult SubmitGame(GameInput input)
        {
            var scorer = new NativeScorer.NativeScorer(configService);
            var gameResponse = scorer.RunGame(input);
            var totalScore = gameResponse.Score.TotalScore;

            //TODO remove docker call once verified.
            //var serverGameResponse = serverUtils.SubmitGameAsync(input).Result;
            //var serverTotalScore = serverGameResponse.Score.TotalScore;

            //if (Math.Abs(serverTotalScore - totalScore) > 5)
            //{
            //    throw new Exception();
            //}

            return gameResponse.Score;
        }
    }
}
