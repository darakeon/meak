using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Presentation.Models;
using Structure.Entities;
using Structure.Enums;

namespace Presentation.Helpers
{
    public class Adder
    {
        public SeasonEditEpisodeModel Model;
        public String View;

        public Adder()
        {
            Model = new SeasonEditEpisodeModel();
        }

        public void SetScene(Int32 scene)
        {
            Model.Story.SceneList = new List<Scene>();

            for (var s = 0; s <= scene; s++)
            {
                Model.Story.SceneList.Add(new Scene(s));
            }

            Model.SceneCounter = scene;
        }

        public void SetPieceTeller(Int32 scene, Int32 piece, Int32 teller, Int32 talk)
        {
            Model.Story.SceneList[scene].TellerList = 
                setPieceParagraphTypeList<Teller, TellerStyle>(teller, piece);

            setCountersAndView(piece, teller, talk, "Teller");
        }

        public void SetPieceTalk(Int32 scene, Int32 piece, Int32 talk, Int32 teller)
        {
            Model.Story.SceneList[scene].TalkList = 
                setPieceParagraphTypeList<Talk, TalkStyle>(talk, piece);

            setCountersAndView(piece, teller, talk, "Talk");
        }

        private static IList<TParagraph> setPieceParagraphTypeList<TParagraph, TPiece>(Int32 paragraph, Int32 piece)
            where TParagraph : Paragraph<TPiece>, new()
            where TPiece : struct
        {
            var paragraphList = new List<TParagraph>();

            for (var p = 0; p <= paragraph; p++)
            {
                paragraphList.Add(new TParagraph());
            }

            for (var p = 0; p <= piece; p++)
            {
                var tPiece = (TPiece)Enum.Parse(typeof(TPiece), "Default");
                paragraphList.Last().Pieces.Add(new Piece<TPiece>(tPiece));
            }

            return paragraphList;
        }

        private void setCountersAndView(Int32 piece, Int32 teller, Int32 talk, String type)
        {
            Model.PieceCounter = piece;
            setCountersAndView(teller, talk, type + "Piece");
        }


        public void SetParagraphTeller(Int32 scene, Int32 paragraph, Int32 teller, Int32 talk)
        {
            Model.Story.SceneList[scene].TellerList =
                setParagraph<Teller, TellerStyle>(scene, teller, paragraph, ParagraphType.Teller);

            setCountersAndView(paragraph, teller, talk);
        }

        public void SetParagraphTalk(Int32 scene, Int32 paragraph, Int32 talk, Int32 teller)
        {
            Model.Story.SceneList[scene].TalkList =
                setParagraph<Talk, TalkStyle>(scene, talk, paragraph, ParagraphType.Talk);

            setCountersAndView(paragraph, teller, talk);
        }

        private IList<TParagraph> setParagraph<TParagraph, TPiece>(Int32 scene, Int32 teller, Int32 paragraph, ParagraphType paragraphType)
            where TParagraph : Paragraph<TPiece>, new()
            where TPiece : struct
        {
            var paragraphList = new List<TParagraph>();

            for (var t = 0; t <= teller; t++)
            {
                paragraphList.Add(new TParagraph());
                
                var tPiece = (TPiece)Enum.Parse(typeof(TPiece), "Default");
                paragraphList.Last().Pieces.Add(new Piece<TPiece>(tPiece));
            }

            for (var p = 0; p <= paragraph; p++)
            {
                Model.Story.SceneList[scene].ParagraphTypeList.Add(paragraphType);
            }

            return paragraphList;
        }

        private void setCountersAndView(Int32 paragraph, Int32 teller, Int32 talk)
        {
            Model.ParagraphCounter = paragraph;

            setCountersAndView(teller, talk, "Paragraph");
        }


        private void setCountersAndView(Int32 teller, Int32 talk, String type)
        {
            Model.TellerCounter = teller;
            Model.TalkCounter = talk;

            View = "Author/" + type;
        }

    }
}