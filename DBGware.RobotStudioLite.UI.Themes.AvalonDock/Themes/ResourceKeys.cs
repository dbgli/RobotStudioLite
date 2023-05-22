namespace DBGware.RobotStudioLite.UI.Themes.AvalonDock.Themes
{
    using System.Windows;

    /// <summary>
    /// Resource key management class to keep track of all resources
    /// that can be re-styled in applications that make use of the implemented controls.
    /// </summary>
    public static class ResourceKeys
    {
        #region Accent Keys
        /// <summary>
        /// Accent Color Key - This Color key is used to accent elements in the UI
        /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
        /// </summary>
        public static readonly ComponentResourceKey ControlAccentColorKey = new(typeof(ResourceKeys), "ControlAccentColorKey");

        /// <summary>
        /// Accent Brush Key - This Brush key is used to accent elements in the UI
        /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
        /// </summary>
        public static readonly ComponentResourceKey ControlAccentBrushKey = new(typeof(ResourceKeys), "ControlAccentBrushKey");
        #endregion Accent Keys

        #region Brush Keys
        // General
        public static readonly ComponentResourceKey Background = new(typeof(ResourceKeys), "Background");
        public static readonly ComponentResourceKey PanelBorderBrush = new(typeof(ResourceKeys), "PanelBorderBrush");
        public static readonly ComponentResourceKey TabBackground = new(typeof(ResourceKeys), "TabBackground");

        // Auto Hide : Tab
        public static readonly ComponentResourceKey AutoHideTabDefaultBackground = new(typeof(ResourceKeys), "AutoHideTabDefaultBackground");
        public static readonly ComponentResourceKey AutoHideTabDefaultBorder = new(typeof(ResourceKeys), "AutoHideTabDefaultBorder");
        public static readonly ComponentResourceKey AutoHideTabDefaultText = new(typeof(ResourceKeys), "AutoHideTabDefaultText");

        // Mouse Over Auto Hide Button for (collapsed) Auto Hidden Elements
        public static readonly ComponentResourceKey AutoHideTabHoveredBackground = new(typeof(ResourceKeys), "AutoHideTabHoveredBackground");
        // AccentColor
        public static readonly ComponentResourceKey AutoHideTabHoveredBorder = new(typeof(ResourceKeys), "AutoHideTabHoveredBorder");
        // AccentColor
        public static readonly ComponentResourceKey AutoHideTabHoveredText = new(typeof(ResourceKeys), "AutoHideTabHoveredText");

        // Document Well : Overflow Button
        public static readonly ComponentResourceKey DocumentWellOverflowButtonDefaultGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonDefaultGlyph");
        public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBackground = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredBackground");
        public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredBorder = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredBorder");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellOverflowButtonHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonHoveredGlyph");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBackground = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedBackground");
        public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedBorder = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedBorder");
        public static readonly ComponentResourceKey DocumentWellOverflowButtonPressedGlyph = new(typeof(ResourceKeys), "DocumentWellOverflowButtonPressedGlyph");

        // Document Well : Tab
        // Selected Document Highlight Header Top color (AccentColor)
        public static readonly ComponentResourceKey DocumentWellTabSelectedActiveBackground = new(typeof(ResourceKeys), "DocumentWellTabSelectedActiveBackground");

        public static readonly ComponentResourceKey DocumentWellTabSelectedActiveText = new(typeof(ResourceKeys), "DocumentWellTabSelectedActiveText");
        public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveBackground = new(typeof(ResourceKeys), "DocumentWellTabSelectedInactiveBackground");
        public static readonly ComponentResourceKey DocumentWellTabSelectedInactiveText = new(typeof(ResourceKeys), "DocumentWellTabSelectedInactiveText");
        public static readonly ComponentResourceKey DocumentWellTabUnselectedBackground = new(typeof(ResourceKeys), "DocumentWellTabUnselectedBackground");
        public static readonly ComponentResourceKey DocumentWellTabUnselectedText = new(typeof(ResourceKeys), "DocumentWellTabUnselectedText");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabUnselectedHoveredBackground");
        public static readonly ComponentResourceKey DocumentWellTabUnselectedHoveredText = new(typeof(ResourceKeys), "DocumentWellTabUnselectedHoveredText");

        // Document Well : Tab : Button
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveGlyph");

        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredBackground");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActiveHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActiveHoveredGlyph");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedBackground");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedActivePressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedActivePressedGlyph");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveGlyph");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredBackground");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactiveHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactiveHoveredGlyph");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedBackground");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonSelectedInactivePressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonSelectedInactivePressedGlyph");
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredGlyph");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBackground");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonHoveredGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonHoveredGlyph");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBackground = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedBackground");
        // AccentColor
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedBorder = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedBorder");
        public static readonly ComponentResourceKey DocumentWellTabButtonUnselectedTabHoveredButtonPressedGlyph = new(typeof(ResourceKeys), "DocumentWellTabButtonUnselectedTabHoveredButtonPressedGlyph");

        // Tool Window : Caption
        // Background of selected toolwindow (AccentColor)
        public static readonly ComponentResourceKey ToolWindowCaptionActiveBackground = new(typeof(ResourceKeys), "ToolWindowCaptionActiveBackground");
        public static readonly ComponentResourceKey ToolWindowCaptionActiveGrip = new(typeof(ResourceKeys), "ToolWindowCaptionActiveGrip");
        public static readonly ComponentResourceKey ToolWindowCaptionActiveText = new(typeof(ResourceKeys), "ToolWindowCaptionActiveText");
        public static readonly ComponentResourceKey ToolWindowCaptionInactiveBackground = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveBackground");
        public static readonly ComponentResourceKey ToolWindowCaptionInactiveGrip = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveGrip");
        public static readonly ComponentResourceKey ToolWindowCaptionInactiveText = new(typeof(ResourceKeys), "ToolWindowCaptionInactiveText");

        // Tool Window : Caption : Button
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveGlyph");
        // AccentColor
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredBackground");
        // AccentColor
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredBorder");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActiveHoveredGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActiveHoveredGlyph");
        // AccentColor
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedBackground");
        // AccentColor
        public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedBorder");

        public static readonly ComponentResourceKey ToolWindowCaptionButtonActivePressedGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonActivePressedGlyph");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveGlyph");

        // AccentColor
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredBackground");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredBorder");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactiveHoveredGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactiveHoveredGlyph");

        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBackground = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedBackground");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedBorder = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedBorder");
        public static readonly ComponentResourceKey ToolWindowCaptionButtonInactivePressedGlyph = new(typeof(ResourceKeys), "ToolWindowCaptionButtonInactivePressedGlyph");

        // Floating Document Window
        public static readonly ComponentResourceKey FloatingDocumentWindowBackground = new(typeof(ResourceKeys), "FloatingDocumentWindowBackground");
        public static readonly ComponentResourceKey FloatingDocumentWindowBorder = new(typeof(ResourceKeys), "FloatingDocumentWindowBorder");
        public static readonly ComponentResourceKey FloatingDocumentWindowBorderInactive = new(typeof(ResourceKeys), "FloatingDocumentWindowBorderInactive");

        // Floating Tool Window
        public static readonly ComponentResourceKey FloatingToolWindowBackground = new(typeof(ResourceKeys), "FloatingToolWindowBackground");
        public static readonly ComponentResourceKey FloatingToolWindowBorder = new(typeof(ResourceKeys), "FloatingToolWindowBorder");
        public static readonly ComponentResourceKey FloatingToolWindowBorderInactive = new(typeof(ResourceKeys), "FloatingToolWindowBorderInactive");

        // Navigator Window
        public static readonly ComponentResourceKey NavigatorWindowBackground = new(typeof(ResourceKeys), "NavigatorWindowBackground");
        public static readonly ComponentResourceKey NavigatorWindowForeground = new(typeof(ResourceKeys), "NavigatorWindowForeground");

        // Background of selected text in Navigator Window (AccentColor)
        public static readonly ComponentResourceKey NavigatorWindowSelectedBackground = new(typeof(ResourceKeys), "NavigatorWindowSelectedBackground");
        public static readonly ComponentResourceKey NavigatorWindowSelectedText = new(typeof(ResourceKeys), "NavigatorWindowSelectedText");
        public static readonly ComponentResourceKey NavigatorWindowUnSelectedBackground = new(typeof(ResourceKeys), "NavigatorWindowUnSelectedBackground");

        #region DockingBrushKeys
        // Defines the height and width of the docking indicator buttons that are shown when
        // documents or tool windows are dragged
        public static readonly ComponentResourceKey DockingButtonWidthKey = new(typeof(ResourceKeys), "DockingButtonWidthKey");
        public static readonly ComponentResourceKey DockingButtonHeightKey = new(typeof(ResourceKeys), "DockingButtonHeightKey");

        public static readonly ComponentResourceKey DockingButtonBackgroundBrushKey = new(typeof(ResourceKeys), "DockingButtonBackgroundBrushKey");
        public static readonly ComponentResourceKey DockingButtonForegroundBrushKey = new(typeof(ResourceKeys), "DockingButtonForegroundBrushKey");
        public static readonly ComponentResourceKey DockingButtonForegroundArrowBrushKey = new(typeof(ResourceKeys), "DockingButtonForegroundArrowBrushKey");

        public static readonly ComponentResourceKey DockingButtonStarBorderBrushKey = new(typeof(ResourceKeys), "DockingButtonStarBorderBrushKey");
        public static readonly ComponentResourceKey DockingButtonStarBackgroundBrushKey = new(typeof(ResourceKeys), "DockingButtonStarBackgroundBrushKey");

        // Preview Box is the highlighted rectangle that shows when a drop area in a window is indicated
        public static readonly ComponentResourceKey PreviewBoxBorderBrushKey = new(typeof(ResourceKeys), "PreviewBoxBorderBrushKey");
        public static readonly ComponentResourceKey PreviewBoxBackgroundBrushKey = new(typeof(ResourceKeys), "PreviewBoxBackgroundBrushKey");
        #endregion DockingBrushKeys

        #region DockTargetButton BrushKeys

        public static readonly ComponentResourceKey DockTargetButtonBackgroundBrushKey = new(typeof(ResourceKeys), "DockTargetButtonBackgroundBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonBorderBrushKey = new(typeof(ResourceKeys), "DockTargetButtonBorderBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonGlyphBackgroundBrushKey = new(typeof(ResourceKeys), "DockTargetButtonGlyphBackgroundBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonGlyphBorderBrushKey = new(typeof(ResourceKeys), "DockTargetButtonGlyphBorderBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonGlyphArrowBrushKey = new(typeof(ResourceKeys), "DockTargetButtonGlyphArrowBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonOuterBackgroundBrushKey = new(typeof(ResourceKeys), "DockTargetButtonOuterBackgroundBrushKey");
        public static readonly ComponentResourceKey DockTargetButtonOuterBorderBrushKey = new(typeof(ResourceKeys), "DockTargetButtonOuterBorderBrushKey");

        #endregion DockTargetButton BrushKeys


        #region DocumentFloatingWindow

        public static readonly ComponentResourceKey DocumentFloatingWindowTitleBarBackground = new(typeof(ResourceKeys), "DocumentFloatingWindowTitleBarBackground");
        public static readonly ComponentResourceKey DocumentFloatingWindowTitleBarText = new(typeof(ResourceKeys), "DocumentFloatingWindowTitleBarText");
        public static readonly ComponentResourceKey DocumentFloatingWindowTitleBarTextActive = new(typeof(ResourceKeys), "DocumentFloatingWindowTitleBarTextActive");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBackground = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBackground");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBackgroundHover = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBackgroundHover");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBackgroundPressed = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBackgroundPressed");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBorder = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBorder");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBorderHover = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBorderHover");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonBorderPressed = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonBorderPressed");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonGlyph = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonGlyph");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonGlyphHover = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonGlyphHover");
        public static readonly ComponentResourceKey DocumentFloatingWindowSystemButtonGlyphPressed = new(typeof(ResourceKeys), "DocumentFloatingWindowSystemButtonGlyphPressed");

        #endregion DocumentFloatingWindow

        public static readonly ComponentResourceKey DocumentTabMenuItemGlyph = new(typeof(ResourceKeys), "DocumentTabMenuItemGlyph");

        #region DocumentTab
        public static readonly ComponentResourceKey DocumentTabStateLineActive = new(typeof(ResourceKeys), "DocumentTabStateLineActive");
        public static readonly ComponentResourceKey DocumentTabStateLineInactive = new(typeof(ResourceKeys), "DocumentTabStateLineInactive");
        #endregion DocumentTab

        #region DocumentTabItem
        public static readonly ComponentResourceKey DocumentTabItemBorder = new(typeof(ResourceKeys), "DocumentTabItemBorder");
        public static readonly ComponentResourceKey DocumentTabItemBorderActive = new(typeof(ResourceKeys), "DocumentTabItemBorderActive");
        public static readonly ComponentResourceKey DocumentTabItemBorderActiveHover = new(typeof(ResourceKeys), "DocumentTabItemBorderActiveHover");
        public static readonly ComponentResourceKey DocumentTabItemBorderInactive = new(typeof(ResourceKeys), "DocumentTabItemBorderInactive");
        public static readonly ComponentResourceKey DocumentTabItemBorderUnselectedHover = new(typeof(ResourceKeys), "DocumentTabItemBorderUnselectedHover");
        public static readonly ComponentResourceKey DocumentTabItemInBorder = new(typeof(ResourceKeys), "DocumentTabItemInBorder");
        public static readonly ComponentResourceKey DocumentTabItemInBorderActive = new(typeof(ResourceKeys), "DocumentTabItemInBorderActive");
        public static readonly ComponentResourceKey DocumentTabItemInBorderActiveHover = new(typeof(ResourceKeys), "DocumentTabItemInBorderActiveHover");
        public static readonly ComponentResourceKey DocumentTabItemInBorderInactive = new(typeof(ResourceKeys), "DocumentTabItemInBorderInactive");
        public static readonly ComponentResourceKey DocumentTabItemInBorderUnselectedHover = new(typeof(ResourceKeys), "DocumentTabItemInBorderUnselectedHover");
        #endregion DocumentTabItem

        #region ToolWell
        public static readonly ComponentResourceKey ToolWellInnerBorder = new(typeof(ResourceKeys), "ToolWellInnerBorder");
        public static readonly ComponentResourceKey ToolWellInnerBorderInactive = new(typeof(ResourceKeys), "ToolWellInnerBorderInactive");
        #endregion ToolWellInnerBorder

        #region ContextMenu

        public static readonly ComponentResourceKey ContextMenuBackground = new(typeof(ResourceKeys), "ContextMenuBackground");
        public static readonly ComponentResourceKey ContextMenuBorderBrush = new(typeof(ResourceKeys), "ContextMenuBorderBrush");
        public static readonly ComponentResourceKey ContextMenuRectangleFill = new(typeof(ResourceKeys), "ContextMenuRectangleFill");

        #endregion ContextMenu

        #region TabItem

        public static readonly ComponentResourceKey TabItemForeground = new(typeof(ResourceKeys), "TabItemForeground");
        public static readonly ComponentResourceKey TabItemStaticBackground = new(typeof(ResourceKeys), "TabItemStaticBackground");
        public static readonly ComponentResourceKey TabItemStaticBorder = new(typeof(ResourceKeys), "TabItemStaticBorder");
        public static readonly ComponentResourceKey TabItemStaticInBorder = new(typeof(ResourceKeys), "TabItemStaticInBorder");
        public static readonly ComponentResourceKey TabItemStaticForeground = new(typeof(ResourceKeys), "TabItemStaticForeground");
        public static readonly ComponentResourceKey TabItemMouseOverBackground = new(typeof(ResourceKeys), "TabItemMouseOverBackground");
        public static readonly ComponentResourceKey TabItemMouseOverBorder = new(typeof(ResourceKeys), "TabItemMouseOverBorder");
        public static readonly ComponentResourceKey TabItemMouseOverInBorder = new(typeof(ResourceKeys), "TabItemMouseOverInBorder");
        public static readonly ComponentResourceKey TabItemMouseOverForeground = new(typeof(ResourceKeys), "TabItemMouseOverForeground");
        public static readonly ComponentResourceKey TabItemDisabledBackground = new(typeof(ResourceKeys), "TabItemDisabledBackground");
        public static readonly ComponentResourceKey TabItemDisabledBorder = new(typeof(ResourceKeys), "TabItemDisabledBorder");
        public static readonly ComponentResourceKey TabItemDisabledInBorder = new(typeof(ResourceKeys), "TabItemDisabledInBorder");
        public static readonly ComponentResourceKey TabItemDisabledForeground = new(typeof(ResourceKeys), "TabItemDisabledForeground");
        public static readonly ComponentResourceKey TabItemSelectedBackground = new(typeof(ResourceKeys), "TabItemSelectedBackground");
        public static readonly ComponentResourceKey TabItemSelectedBorder = new(typeof(ResourceKeys), "TabItemSelectedBorder");
        public static readonly ComponentResourceKey TabItemSelectedInBorder = new(typeof(ResourceKeys), "TabItemSelectedInBorder");
        public static readonly ComponentResourceKey TabItemSelectedForeground = new(typeof(ResourceKeys), "TabItemSelectedForeground");

        #endregion TabItem

        #region MenuItem

        public static readonly ComponentResourceKey MenuItemBackground = new(typeof(ResourceKeys), "MenuItemBackground");
        public static readonly ComponentResourceKey MenuItemBackgroundHover = new(typeof(ResourceKeys), "MenuItemBackgroundHover");
        public static readonly ComponentResourceKey MenuItemBackgroundDisabled = new(typeof(ResourceKeys), "MenuItemBackgroundDisabled");
        public static readonly ComponentResourceKey MenuItemBorder = new(typeof(ResourceKeys), "MenuItemBorder");
        public static readonly ComponentResourceKey MenuItemBorderHover = new(typeof(ResourceKeys), "MenuItemBorderHover");
        public static readonly ComponentResourceKey MenuItemBorderDisabled = new(typeof(ResourceKeys), "MenuItemBorderDisabled");
        public static readonly ComponentResourceKey MenuItemForeground = new(typeof(ResourceKeys), "MenuItemForeground");
        public static readonly ComponentResourceKey MenuItemForegroundHover = new(typeof(ResourceKeys), "MenuItemForegroundHover");
        public static readonly ComponentResourceKey MenuItemForegroundDisabled = new(typeof(ResourceKeys), "MenuItemForegroundDisabled");
        public static readonly ComponentResourceKey MenuItemGlyph = new(typeof(ResourceKeys), "MenuItemGlyph");
        public static readonly ComponentResourceKey MenuItemGlyphHover = new(typeof(ResourceKeys), "MenuItemGlyphHover");
        public static readonly ComponentResourceKey MenuItemGlyphDisabled = new(typeof(ResourceKeys), "MenuItemGlyphDisabled");
        public static readonly ComponentResourceKey MenuItemGlyphPanel = new(typeof(ResourceKeys), "MenuItemGlyphPanel");
        public static readonly ComponentResourceKey MenuItemGlyphPanelHover = new(typeof(ResourceKeys), "MenuItemGlyphPanelHover");
        public static readonly ComponentResourceKey MenuItemGlyphPanelDisabled = new(typeof(ResourceKeys), "MenuItemGlyphPanelDisabled");
        public static readonly ComponentResourceKey MenuItemGlyphPanelBorder = new(typeof(ResourceKeys), "MenuItemGlyphPanelBorder");
        public static readonly ComponentResourceKey MenuItemGlyphPanelBorderHover = new(typeof(ResourceKeys), "MenuItemGlyphPanelBorderHover");
        public static readonly ComponentResourceKey MenuItemGlyphPanelBorderDisabled = new(typeof(ResourceKeys), "MenuItemGlyphPanelBorderDisabled");

        #endregion MenuItem

        public static readonly ComponentResourceKey SystemColorsMenuText = new(typeof(ResourceKeys), "SystemColorsMenuText");

        #endregion Brush Keys
    }
}