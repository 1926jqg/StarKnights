using GeonBit.UI;
using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.UtilityObjects;
using System.Collections.Generic;

namespace StarKnightsLibrary.Transmissions
{
    public interface ITransmissionManager
    {
        int Width { get; }
        int Height { get; }
        int ImageWidth { get; }

        ITransmission ActiveTransmission { get; }
        IEnumerable<string> GetMessageLines(int fontWidth, int fontHeight);

        void AddTransmission(ITransmission transmission, ulong duration);
        void Update(ISpaceScene enviroment);
        void Draw(UserInterface ui, IContentContainer contentContainer);
    }
}
