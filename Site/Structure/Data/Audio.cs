using System;
using System.IO;
using System.Linq;
using Structure.Entities.System;
using Structure.Enums;

namespace Structure.Data
{
    class Audio
    {
        public Audio(String folderPath, String seasonID, String episodeID, Block block)
        {
            this.folderPath = folderPath;
            this.seasonID = seasonID;
            this.episodeID = episodeID;
            this.block = block;
        }

        private readonly String folderPath;
        private readonly String seasonID;
        private readonly String episodeID;
        private readonly Block block;

        internal void CopySongs()
        {
            var talkPieces = block.TalkList
                .SelectMany(t => t.Pieces)
                .Where(p => !String.IsNullOrEmpty(p.Audio));

            var tellerPieces = block.TalkList
                .SelectMany(t => t.Pieces)
                .Where(p => !String.IsNullOrEmpty(p.Audio));

            talkPieces
                .Union(tellerPieces)
                .ToList()
                .ForEach(getAudio);
        }

        private void getAudio<T>(Piece<T> piece)
            where T : struct
        {
            var audio = piece.Audio;

            var sourcePath = Paths.AudioPath(
                folderPath, seasonID, episodeID, audio
            );

            var destinyPath = Paths.AudioLocalPath(
                seasonID, episodeID, audio
            );

            var sourceInfo = new FileInfo(sourcePath);
            var destinyInfo = new FileInfo(destinyPath);

            var copiedOnce = destinyInfo.Exists;

            var sourceNotFound = !sourceInfo.Exists;
            var copiedIsFresh =
                destinyInfo.CreationTime >= sourceInfo.CreationTime;
            var noCopy = sourceNotFound || copiedIsFresh;

            if (copiedOnce && noCopy)
            {
                return;
            }

            if (sourceNotFound)
            {
                piece.Audio = null;
                return;
            }

            var createDir = 
                destinyInfo.Directory != null
                    && !destinyInfo.Directory.Exists;

            if (createDir)
                destinyInfo.Directory.Create();

            File.Copy(sourcePath, destinyPath, true);
        }
    }
}
