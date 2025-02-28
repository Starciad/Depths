using Depths.Core.Constants;
using Depths.Core.Databases;
using Depths.Core.Entities.Common;
using Depths.Core.Enums.Fonts;
using Depths.Core.Enums.Text;
using Depths.Core.GUISystem.Common.Elements;
using Depths.Core.Managers;
using Depths.Core.Mathematics.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DPlayerInformationGUI : DGUI
    {
        private sealed class DInfoField
        {
            internal DGUITextElement TextElement { get; private set; }
            internal Action<DGUITextElement> OnSyncCallback { get; private set; }

            internal DInfoField(DGUITextElement textElement, Action<DGUITextElement> onSyncCallback)
            {
                this.TextElement = textElement;
                this.OnSyncCallback = onSyncCallback;
            }
        }

        private sbyte pageIndex = 0;

        private readonly byte totalPageCount;

        private readonly DPoint infoFieldBasePosition;
        private readonly byte infoFieldVerticalSpacing;

        private readonly DGUIImageElement panelElement;
        private readonly DGUITextElement titleTextElement;
        private readonly Dictionary<string, IEnumerable<DInfoField>> pageInfoFields;

        private readonly DGameInformation gameInformation;
        private readonly DGUIManager guiManager;
        private readonly DInputManager inputManager;
        private readonly DPlayerEntity playerEntity;

        internal DPlayerInformationGUI(string identifier, DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DTextManager textManager) : base(identifier)
        {
            this.gameInformation = gameInformation;
            this.inputManager = inputManager;
            this.guiManager = guiManager;
            this.playerEntity = gameInformation.PlayerEntity;

            this.panelElement = new()
            {
                Texture = assetDatabase.GetTexture("texture_gui_5"),
            };

            this.pageInfoFields = new()
            {
                ["Stats"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Bag:", this.playerEntity.CollectedMinerals.Count, '/', this.playerEntity.BackpackSize)); }), // Bag
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Energy:", this.playerEntity.Energy)); }), // Energy
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Money:", this.playerEntity.Money)); }), // Money
                ],

                ["Attributes"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Damage:", this.playerEntity.Damage)); }), // Damage
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Power:", this.playerEntity.Power)); }), // Power
                ],

                ["Items"] = [
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Stairs:", this.playerEntity.StairCount)); }), // Stairs
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Scaff:", this.playerEntity.PlataformCount)); }), // Plataforms
                    new(new(textManager, new() { CharacterSpacing = -1, FontType = DFontType.Dark }), (DGUITextElement textElement) => { textElement.SetValue(string.Concat("Miners:", this.playerEntity.RobotCount)); }), // Miners
                ],
            };

            this.titleTextElement = new(textManager, new() { FontType = DFontType.DarkOutline, HorizontalAlignment = DTextAlignment.Center })
            {
                Position = new(42, 2),
            };

            this.infoFieldBasePosition = new(5, 16);
            this.infoFieldVerticalSpacing = DFontConstants.HEIGHT + 2;

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
        }

        private void HandleUserInputs()
        {
            if (this.inputManager.Started(DKeyMappingConstant.Cancel))
            {
                this.guiManager.Close(this.Identifier);
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Left))
            {
                NextPage();
                SyncTitleTextElement();
                return;
            }

            if (this.inputManager.Started(DKeyMappingConstant.Right))
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
