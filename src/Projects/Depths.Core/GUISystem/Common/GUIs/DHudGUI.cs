using Depths.Core.Constants;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.World.Ores;

using Microsoft.Xna.Framework;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DHudGUI : DGUI
    {
        private byte notifyTextMovementFrameCounter = 0;
        private byte notifyTextVisibilityFrameCounter = 0;

        private readonly byte notifyTextSpeed = 1;
        private readonly byte notifyTextMovementDelayFrames = 5;
        private readonly byte notifyTextVisibilityTotalFrames = 32;

        private readonly DGUITextElement notifyTextElement;

        private readonly DPlayerEntity playerEntity;
        private readonly DTextManager textManager;

        internal DHudGUI(DTextManager textManager, DPlayerEntity playerEntity) : base()
        {
            this.playerEntity = playerEntity;
            this.textManager = textManager;

            this.notifyTextElement = new(this.textManager)
            {
                IsVisible = false,
                FontType = DFontType.DarkOutline,
                TextAlignment = DTextAlignment.Center,
                Spacing = 1
            };

        }

        protected override void OnBuild()
        {
            AddElement(this.notifyTextElement);
        }

        internal override void Load()
        {
            this.playerEntity.OnCollectedOre += Player_OnCollectedOre;
            this.playerEntity.OnFullBackpack += Player_OnFullBackpack;
        }

        internal override void Unload()
        {
            this.playerEntity.OnCollectedOre -= Player_OnCollectedOre;
            this.playerEntity.OnFullBackpack -= Player_OnFullBackpack;
        }

        internal override void Update()
        {
            base.Update();

            UpdateNotifyTextElement();
        }

        private void UpdateNotifyTextElement()
        {
            if (!this.notifyTextElement.IsVisible)
            {
                return;
            }

            this.notifyTextVisibilityFrameCounter++;

            if (this.notifyTextVisibilityFrameCounter >= this.notifyTextVisibilityTotalFrames)
            {
                this.notifyTextElement.IsVisible = false;
                return;
            }

            this.notifyTextMovementFrameCounter++;

            if (this.notifyTextMovementFrameCounter < this.notifyTextMovementDelayFrames)
            {
                return;
            }

            this.notifyTextMovementFrameCounter = 0;

            this.notifyTextElement.Position += new Point(0, -this.notifyTextSpeed);
        }

        private void ResetNotifyTextElement()
        {
            this.notifyTextVisibilityFrameCounter = 0;
            this.notifyTextElement.Position = new(DScreenConstants.GAME_WIDTH / 2, DScreenConstants.GAME_HEIGHT / 2);
            this.notifyTextElement.IsVisible = true;
        }

        private void Player_OnCollectedOre(DOre ore)
        {
            ResetNotifyTextElement();
            this.notifyTextElement.SetValue(ore.DisplayName);
        }

        private void Player_OnFullBackpack()
        {
            ResetNotifyTextElement();
            this.notifyTextElement.SetValue("Full Bag!");
        }
    }
}
