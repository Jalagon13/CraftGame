using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSignals
{
	public static readonly Signal ON_SLOT_LEFT_CLICKED = new("OnSlotLeftClicked");
	public static readonly Signal ON_SLOT_RIGHT_CLICKED = new("OnSlotRightClicked");
	public static readonly Signal CLICKABLE_DESTROYED = new("ClickableDestroyed");
	public static readonly Signal HOTBAT_SLOT_UPDATED = new("HotbarSlotUpdated");
	public static readonly Signal FOCUS_ITEM_UPDATED = new("FocusItemUpdated");
	public static readonly Signal MOUSE_GOT_ITEM = new("MouseGotItem");
	public static readonly Signal MOUSE_GAVE_ITEM = new("MouseGotItem");
	public static readonly Signal ON_CONSUME = new("OnConsume");
	public static readonly Signal ON_UI_ACTIVATED = new("OnUiActived");
	public static readonly Signal ON_UI_UNACTIVED = new("OnUiUnActived");
	public static readonly Signal ON_CRAFT_NODE_CLICKED = new("CraftNodeClicked");
	public static readonly Signal ON_RECIPE_RESEARCHED = new("OnRecipeResearched");
	public static readonly Signal ON_EXPAND = new("OnExpand");
	public static readonly Signal ON_QUEST_COMPLETE = new("OnQuestComplete");
	public static readonly Signal ON_DAY_START = new("OnDayStart");
	public static readonly Signal ON_DAY_END = new("OnDayEnd");
}
