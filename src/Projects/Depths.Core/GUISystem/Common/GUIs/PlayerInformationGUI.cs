using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.InputSystem;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class PlayerInformationGUI : GUI
    {
        private sealed class DInfoField
        {
            internal GUITextElement TextElement { get; private set; }
            internal Action<GUITextElement> OnSyncCallback { get; private set; }

            internal DInfoField(GUITextElement textElement, Action<GUITextElement> onSyncCallback)
            {
                this.TextElement = textElement;
                this.OnSyncCallback = onSyncCallback;
            }
        }

        private sbyte pageIndex = 0;

        private bool panelAnimationState;
        private byte panelAnimationFrameCounter;

        private readonly byte panelAnimationFrameDelay = 10;
        private readonly byte totalPageCount;

        private readonly DPoint infoFieldBasePosition;
        private readonly byte infoFieldVerticalSpacing;

        private readonly Rectangle[] backgroundSourceRectangles = [
            new(new(0, 0), new(ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT)),
            new(new(ScreenConstants.GAME_WIDTH, 0), new(ScreenConstants.GAME_WIDTH, ScreenConstants.GAME_HEIGHT)),
        ];

        private readonly GUIImageElement panelElement;
        private readonly GUITextElement titleTextElement;
        private readonly Dictionary<string, IEnumerable<DInfoField>> pageInfoFields;

        private readonly GameInformation gameInformation;
        private readonly GUIManager guiManager;
        private readonly InputManager inputManager;

        internal PlayerInformationGUI(string identifier, AssetDatabase assetDatabase, GameInformation gameInformation, GUIManager guiManager, InputManager inputManager, TextManager textManager) : base(identifier)
        {
            this.gameInformation = gameInformation;
            this.inputManager = inputManager;
            this.guiManager = guiManager;

            this.panelElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_5"),
            };

            this.pageInfoFields = new()
            {
                ["Stats"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Bag:", this.gameInformation.PlayerEntity.CollectedMinerals.Count, '/', this.gameInformation.PlayerEntity.BackpackSize)); }), // Bag
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Energy:", this.gameInformation.PlayerEntity.Energy)); }), // Energy
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Money:", this.gameInformation.PlayerEntity.Money)); }), // Money
                ],

                ["Attributes"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Damage:", this.gameInformation.PlayerEntity.Damage)); }), // Damage
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Power:", this.gameInformation.PlayerEntity.Power)); }), // Power
                ],

                ["Items"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Stairs:", this.gameInformation.PlayerEntity.StairCount)); }), // Stairs
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Scaff:", this.gameInformation.PlayerEntity.PlataformCount)); }), // Plataforms
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = FontType.Dark }), textElement => { textElement.SetValue(string.Concat("Miners:", this.gameInformation.PlayerEntity.RobotCount)); }), // Miners
                ],
            };

            this.titleTextElement = new(textManager, new() { FontType = FontType.DarkOutline, HorizontalAlignment = TextAlignment.Center })
            {
                Position = new(42, 2),
            };

            this.infoFieldBasePosition = new(5, 16);
            this.infoFieldVerticalSpacing = FontConstants.HEIGHT + 2;

            this.totalPageCount = Convert.ToByte(this.pageInfoFields.Count - 1);

            foreach (IEnumerable<DInfoField> infoFields in this.pageInfoFields.Values)
            {
                DPoint targetPosition = this.infoFieldBasePosition;

                foreach (DInfoField infoField in infoFields)
                {
                    infoField.TextElement.Position = targetPosition;
                    targetPosition.Y += this.infoFieldVerticalSpacing;
                }
            }
        }

        protected override void OnBuild()
        {
            AddElement(this.panelElement);
            AddElement(this.titleTextElement);

            foreach (IEnumerable<DInfoField> infoFields in this.pageInfoFields.Values)
            {
                foreach (DInfoField infoField in infoFields)
                {
                    AddElement(infoField.TextElement);
                }
            }
        }

        internal override void Load()
        {
            this.pageIndex = 0;

            this.gameInformation.IsWorldActive = false;

            SyncTitleTextElement();
            SyncInfoFields();
        }

        internal override void Unload()
        {
            this.gameInformation.IsWorldActive = true;
        }

        internal override void Update()
        {
            HandleUserInputs();
            UpdateInfoFieldsVisibility();
            UpdatePanelAnimation();
        }

        private void HandleUserInputs()
        {
            if (this.inputManager.Started(CommandType.Cancel))
            {
                this.guiManager.Close(this.Identifier);
                return;
            }

            if (this.inputManager.Started(CommandType.Left))
            {
                NextPage();
                SyncTitleTextElement();
                return;
            }

            if (this.inputManager.Started(CommandType.Right))
            {
                PreviousPage();
                SyncTitleTextElement();
                return;
            }
        }

        private void UpdateInfoFieldsVisibility()
        {
            byte sectionIndex = 0;

            foreach (IEnumerable<DInfoField> infoFields in this.pageInfoFields.Values)
            {
                bool visibilityFlag = false;

                if (this.pageIndex == sectionIndex)
                {
                    visibilityFlag = true;
                }

                foreach (DInfoField infoField in infoFields)
                {
                    infoField.TextElement.IsVisible = visibilityFlag;
                }

                sectionIndex++;
            }
        }

        private void UpdatePanelAnimation()
        {
            if (++this.panelAnimationFrameCounter > this.panelAnimationFrameDelay)
            {
                this.panelAnimationFrameCounter = 0;
                this.panelAnimationState = !this.panelAnimationState;

                this.panelElement.TextureClipArea = this.backgroundSourceRectangles[Convert.ToByte(this.panelAnimationState)];
            }
        }

        private void SyncTitleTextElement()
        {
            this.titleTextElement.SetValue(this.pageInfoFields.ElementAt(this.pageIndex).Key);
        }

        private void SyncInfoFields()
        {
            foreach (IEnumerable<DInfoField> infoFields in this.pageInfoFields.Values)
            {
                foreach (DInfoField infoField in infoFields)
                {
                    infoField.OnSyncCallback?.Invoke(infoField.TextElement);
                }
            }
        }

        private void NextPage()
        {
            if (++this.pageIndex > this.totalPageCount)
            {
                this.pageIndex = 0;
            }
        }

        private void PreviousPage()
        {
            if (--this.pageIndex < 0)
            {
                this.pageIndex = (sbyte)this.totalPageCount;
            }
        }
    }
}
