using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.UtilityObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarKnightsLibrary.Transmissions
{
    public class TransmissionManager : ITransmissionManager
    {
        private class TransmissionManagerQueueBucket
        {
            public ITransmission Transmission { get; set; }
            public ulong DurationLeft { get; set; }
            public ulong TotalDuration { get; set; }
        }

        private const int LEFT_BORDER = 20;
        private const int BOTTOM_BORDER = 10;

        private readonly Queue<TransmissionManagerQueueBucket> _transmissionQueue;
        private Panel _transmissionPanel;
        private readonly Paragraph _paragraph;
        private readonly Image _image;

        public ITransmission ActiveTransmission => _transmissionQueue.Count > 0 ? _transmissionQueue.Peek()?.Transmission : null;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ImageWidth { get; private set; }

        public TransmissionManager(int width, int height, int imageWidth)
        {
            Width = width;
            Height = height;
            ImageWidth = imageWidth;

            _paragraph = new RichParagraph("", scale: 1f, offset: new Vector2(-10, -10));
            _image = new Image();
            _transmissionQueue = new Queue<TransmissionManagerQueueBucket>();
        }

        public void AddTransmission(ITransmission transmission, ulong duration)
        {
            _transmissionQueue.Enqueue(new TransmissionManagerQueueBucket
            {
                Transmission = transmission,
                DurationLeft = duration,
                TotalDuration = duration
            });
        }

        public void Update(ISpaceScene enviroment)
        {
            if (ActiveTransmission != null && --_transmissionQueue.Peek().DurationLeft <= 0)
            {
                _transmissionQueue.Dequeue();
            }
        }

        public IEnumerable<string> GetMessageLines(int fontWidth, int fontHeight)
        {
            if (ActiveTransmission != null)
            {
                var width = Width - ImageWidth - 45;
                var charactersPerLine = width / fontWidth;
                int characters = 0;
                var lineBuilder = new StringBuilder();

                var words = new Queue<string>(ActiveTransmission.Message.Split(' '));

                bool first = true;
                while (words.Any())
                {
                    var word = $"{words.Dequeue()} ";
                    characters += word.Count();
                    if (!first && characters >= charactersPerLine)
                    {
                        yield return lineBuilder.ToString();
                        characters = 0;
                        lineBuilder = new StringBuilder();
                    }
                    lineBuilder.Append(word);
                    first = false;
                }
                yield return lineBuilder.ToString().TrimEnd();
            }
        }

        public IEnumerable<string> GetVisibleMessageLines(int fontWidth, int fontHeight)
        {
            var transmission = _transmissionQueue.Peek();
            if (transmission != null)
            {
                var messageLines = GetMessageLines(fontWidth, fontHeight).ToList();
                var visibleLines = Height / fontHeight;
                var totalLines = messageLines.Count;
                var progress = (transmission.TotalDuration - transmission.DurationLeft) / (double)transmission.TotalDuration;

                var skip = totalLines > visibleLines ? (int)(totalLines * progress) : 0;

                return GetMessageLines(fontWidth, fontHeight).Skip(skip).Take(visibleLines);
            }
            return new List<string>();
        }

        private void SetupTransmissionPanel(UserInterface ui)
        {
            int xMin = LEFT_BORDER + Height;
            int yMax = ui.ScreenHeight - Height - BOTTOM_BORDER - 6;
            _transmissionPanel = new Panel(new Vector2(Width, Height + 12), offset: new Vector2(xMin, yMax), anchor: Anchor.TopLeft);
            _transmissionPanel.Opacity = 128;
            ui.AddEntity(_transmissionPanel);
            _transmissionPanel.AddChild(_paragraph);

            int imageHeight = Height;
            const int imageWidth = 140;
            const int textOffset = 5;
            int textXMin = imageWidth + 2 * textOffset;

            _paragraph.Offset = new Vector2(textXMin, textOffset);
            _paragraph.Size = new Vector2(Width - imageWidth, _paragraph.Size.Y);

            _image.Anchor = Anchor.TopLeft;
            _image.Offset = new Vector2(5, 5);
            _image.Size = new Vector2(imageWidth, imageHeight);

            _transmissionPanel.AddChild(_image);
            _transmissionPanel.Padding = new Vector2(0, 0);
        }

        public void Draw(UserInterface ui, IContentContainer contentContainer)
        {
            if (_transmissionPanel == null)
            {
                SetupTransmissionPanel(ui);
            }
            var transmission = ActiveTransmission;

            if (transmission != null)
            {
                var font = Resources.Fonts[(int)_paragraph.TextStyle];// contentContainer.LoadFont("myFont");
                var fontSize = font.MeasureString("a");
                var message = string.Join("\n", GetVisibleMessageLines((int)fontSize.X, (int)fontSize.Y));

                var pixel = contentContainer.LoadTexture("Pixel");

                _transmissionPanel.Visible = true;
                _paragraph.Text = message;
                _image.Texture = pixel;
            }
            else if (_transmissionPanel.Visible)
            {
                _transmissionPanel.Visible = false;
            }
        }
    }
}
