using System;
using Structure.Entities;
using Structure.Enums;

namespace Presentation.Helpers
{
    public class EpisodeEditionHelper
    {
        public static void CutCharacter(Episode episode)
        {
            foreach (var scene in episode.SceneList)
            {
                CutCharacter(scene);
            }
        }

        public static void CutCharacter(Scene scene)
        {
            foreach (var talk in scene.TalkList)
            {
                CutCharacter(talk);
            }
        }

        public static void CutCharacter(Talk talk)
        {
            foreach (var piece in talk.Pieces)
            {
                CutCharacter(piece);
            }
        }

        public static void CutCharacter(Piece<TalkStyle> piece)
        {
            var pieceHasCharacter = !String.IsNullOrEmpty(piece.Text)
                                    && piece.Text.Contains("(");

            if (pieceHasCharacter)
                piece.Text = piece.Text
                    .Substring(
                        0, piece.Text.IndexOf("(")
                    )
                    .Trim();
        }





        public static void PutCharacter(Episode episode)
        {
            foreach (var scene in episode.SceneList)
            {
                PutCharacter(scene);
            }
        }

        public static void PutCharacter(Scene scene)
        {
            foreach (var talk in scene.TalkList)
            {
                PutCharacter(talk);
            }
        }

        public static void PutCharacter(Talk talk)
        {
            foreach (var piece in talk.Pieces)
            {
                PutCharacter(talk, piece);
            }
        }

        public static void PutCharacter(Talk talk, Piece<TalkStyle> piece)
        {
            piece.Text += String.Format(" ({0})", talk.Character);
        }



    }
}

