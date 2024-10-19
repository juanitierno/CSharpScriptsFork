# Combination-Based Auto-Looter by Smaptastic
# Feel free to use code from this for your own projects, as long as you distribute them freely.

from System.Collections.Generic import List
from System import Int32
'''
************ INFORMATION ************
This script is not meant to be a complete auto-looter. Use it alongside something like the RE autoloot agent.

The purpose of this script is flexibility, allowing you to plug in lists of desirable item stats and a number of stats from the list
which you want to loot.

For example, you might want some combination of +skills, faster casting, and faster cast recovery, but you don't need EVERY one of those things.
With this, you could have it look for any combination of 4 of the following:
Magery, Veterinary, Animal Taming, Animal Lore, Musicianship, Provocation, Peacemaking, Faster Casting, and Faster Cast Recovery.
If it finds any 4 of those (say, +Magery, +Veterinary, Faster Casting, and Faster Cast Recovery), it will grab the item.
This does now allow you to select for the magnitude of each stat, just its presence on the item.

The goal, again, is flexibility. Rather than building massive lists containing every possible combination of stats you'd accept,
you can just put in a bunch of complimentary stats and say "Grab items that have enough of these to be worth my time."

Use something else for looting common items like gold, reagents, etc. Again, this is not a complete auto-looter.


************ SETTINGS ************
Each list starts with the number of required properties, followed by the properties to filter for as a string.
So if you wanted any 2 of LMC, LRC, Faster Casting, the list would be: list1 = [2, "Lower Mana Cost", "Lower Reagent Cost", "Faster Casting"]
This cannot currently sort by the magnitude of each value (for example, you can search for Faster Casting, but not Faster Casting > 1)

You also need to include each list in the master list. So to include that list and one named list2, you'll have
masterList = [list1, list2]

You can create as many lists as you want and add them to the master list.
'''

# Bag to put autolooted stuff into. This doesn't support any sorting of loot; it all goes into one bag.
# You can put multiple serials here to allow this to be used on multiple characters, but only ONE PER CHARACTER.
# Format = [serial1, serial2]. Example: lootBagList = [0x4038063E, 0x40109720]
# Allows multiple characters to use the same script without having to create a per-character copy.
lootBagList = [0x4038063E, 0x40109720]

# Beginning of lists. You can make as many as you want, just make sure to add them to masterList. See above.
# You can name the lists whatever you want as long as you add the name to masterList properly.
# testList = [2, "Damage Eater", "Hit Chance Increase", "Faster Casting"]
smaptastic = [5, "Defense Chance Increase", "Faster Casting", "Faster Cast Recovery", "Luck", "Damage Eater", "Veterinary", "Animal Lore", "Animal Taming", "Discordance", "Musicianship", "Magery"]
casterJewelry = [5, "Spell Damage Increase", "Faster Casting", "Faster Cast Recovery", "Luck", "Lower Resource Cost", "Lower Mana Cost", "Defense Chance Increase"]
meleeJewelry = [4, "Swing Speed Increase", "Damage Increase", "Hit Chance Increase", "Defense Chance Increase", "Tactics", "Healing"]
standardWeapon = [7, "Damage Increase", "Defense Chance Increase", "Swing Speed Increase", "Hit Chance Increase", "Spell Channeling", "Hit Mana Leech", "Hit Life Leech", "Hit Stamina Leech", "Hit Fireball", "Hit Harm", "Hit Lightning", "Hit Lower Attack", "Hit Lower Defense", "Hit Cold Area", "Hit Energy Area", "Hit Fire Area", "Hit Physical Area", "Hit Poison Area"]


# Add all the lists you want to use here.
# Removing a list you haven't deleted is fine, it just won't use that list as a basis for looting.
masterList = [smaptastic, casterJewelry, meleeJewelry, standardWeapon]

# Delay between moving items. Setting this too low will cause items to fail to move. 
# There is no danger to setting it too high except it will loot more slowly.
dragDelay = 800

# Loot containers? This will loot corpses no matter what. Setting this as true will also have it loot containers on the ground.
lootContainers = False

# Gather loot for unraveling? Picks up Greater Magic Items or better and puts them in a container for an imbuer to unravel.
# This is secondary to the normal test, so it should not grab items that are covered by the normal lists.
imbueLoot = False

# The serial number of your imbue loot container. Can ignore this if imbueLoot is set to False.
# You can put multiple serials here to allow this to be used on multiple characters, but only ONE PER CHARACTER.
# Format = [serial1, serial2]. Example: imbueBagList = [0x4038063E, 0x40109720]. For just one, imbueBagList = [0x4026C97F]
# Allows multiple characters to use the same script without having to create a per-character copy.
imbueBagList = [0x4026C97F]

# The rarities you want to snag for the imbuer.
imbueRarities = ["Greater Magic Item", "Major Magic Item", "Lesser Artifact", "Greater Artifact", "Major Artifact", "Legendary Artifact"]

'''
END OF SETTINGS
'''

# This gets a list of potentially lootable items on the ground within 2 blocks of you.
def getLootList():
    global lootList
    itemFilter = Items.Filter()
    itemFilter.Enabled = True
    itemFilter.OnGround = True
    itemFilter.RangeMax = 2
    lootList = Items.ApplyFilter(itemFilter)
    if lootList:
        return True
    return False

# This actually does the item looting. Not the testing.
def lootItem(itemToLoot):
    lootable = Items.FindBySerial(itemToLoot.Container)
    if Player.DistanceTo(lootable) <= 2 and (Player.Weight + itemToLoot.Weight < Player.MaxWeight):
        Items.Move(itemToLoot, lootBag, -1)
        Misc.Pause(dragDelay)
    elif Player.Weight + itemToLoot.Weight < Player.MaxWeight:
        Player.HeadMessage(1100, "Too heavy to loot.")
        Misc.Pause(5000)

def lootImbue(itemToLoot):
    lootable = Items.FindBySerial(itemToLoot.Container)
    if Player.DistanceTo(lootable) <= 2:
        Items.Move(itemToLoot, imbueBag, -1)
        Misc.Pause(dragDelay)

def lootableTest(objectToCheck):
    if Player.DistanceTo(objectToCheck) > 2 or not objectToCheck.Contains:
        return False
    if objectToCheck.IsCorpse:
        return True
    if lootContainers and objectToCheck.IsContainer:
        return True
    return False

# This tests each item contained by each nearby potentially lootable item nearby.
# If the item meets the criteria of any of your lists in masterList, it is looted (sent to lootItem above).
def lootLootables():
    global lootList
    for lootable in lootList:
        if not lootableTest(lootable):
            continue
        for testItem in lootable.Contains:
            for propList in masterList:
                propCount = 0
                for prop in propList:
                    if propList.index(prop) > 0:
                        if any(prop.lower() in x.lower() for x in Items.GetPropStringList(testItem)):
                            propCount = propCount + 1
                if propCount >= propList[0]:
                    Player.HeadMessage(65, "Found Lootable!")
                    lootItem(testItem)
                    continue
            if imbueLoot:
                for prop in imbueRarities:
                    if any(prop.lower() in x.lower() for x in Items.GetPropStringList(testItem)):   
                        lootImbue(testItem)
            Misc.Pause(10)

lootList = []
for testSerial in lootBagList:
    lootBag = None
    lootBag = Items.FindBySerial(testSerial)
    if lootBag:
        break
for testSerial in imbueBagList:
    imbueBag = None
    imbueBag = Items.FindBySerial(testSerial)
    if imbueBag:
        break

while True:
    if getLootList():
        lootLootables()
    Misc.Pause(250)