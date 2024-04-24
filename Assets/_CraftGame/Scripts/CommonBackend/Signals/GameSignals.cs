using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSignals
{
	public static readonly Signal ON_SLOT_LEFT_CLICKED = new("OnSlotLeftClicked");
	public static readonly Signal ON_SLOT_RIGHT_CLICKED = new("OnSlotRightClicked");
	public static readonly Signal CLICKABLE_CLICKED = new("ClickableClicked");
	public static readonly Signal CLICKABLE_DESTROYED = new("ClickableDestroyed");
	public static readonly Signal FOCUS_INVENTORY_ITEM_UPDATED = new("FocusInventoryItemUpdated");
	public static readonly Signal ON_CONSUME = new("OnConsume");
	public static readonly Signal ON_UI_ACTIVATED = new("OnUiActived");
	public static readonly Signal ON_UI_UNACTIVED = new("OnUiUnActived");
	public static readonly Signal ON_CRAFT_NODE_CLICKED = new("CraftNodeClicked");
	public static readonly Signal ON_RECIPE_RESEARCHED = new("OnRecipeResearched");
	public static readonly Signal ON_EXPAND = new("OnExpand");
	public static readonly Signal ON_QUEST_COMPLETE = new("OnQuestComplete");
}
