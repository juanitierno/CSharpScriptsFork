import Items
import Gumps
import Misc
import Journal

fishingresources = [0x4307, 0x09CE, 0x09CC, 0x4306, 0x44C3, 0x44C4, 0x4303, 0x44C6, 0x44C5, 0x099F, 0x0DCA]
shipkey = 0x4038B587  # Replace this with your own ship key
homebook = 0x40391CB9  # Replace this with your own runebook with dropoff location
tool = 0x40887D5C

def Defend():
    while Player.Poisoned or Player.Hits < Player.HitsMax * 0.8 or Target.GetTargetFromList( 'enemy' ) != None:
        if Player.Poisoned:
            Spells.Cast("Arch Cure")
            Target.WaitForTarget(3000)
            Target.Self()
            Misc.Pause(2000)
            
        if Player.Hits < Player.HitsMax * 0.8:
            Spells.Cast("Greater Heal")
            Target.WaitForTarget(3000)
            Target.Self()
            Misc.Pause(2000)
        
        enemy = Target.GetTargetFromList( 'enemy' ) 
        if enemy != None:
            Spells.Cast("Energy Bolt")
            Target.WaitForTarget(3000)
            Target.PerformTargetFromList( 'enemy' )
            Misc.Pause(2000)
        
            
    
def Fish(tool):
    Items.UseItem(tool)
    Target.WaitForTarget(2000)
    
    statics = Statics.GetStaticsTileInfo( Player.Position.X-3, Player.Position.Y, 0 )
    if len( statics ) > 0:
        water = statics[ 0 ]
        Target.TargetExecute( Player.Position.X-3, Player.Position.Y, water.StaticZ, water.StaticID )
    else:
        Target.TargetExecute( Player.Position.X-3, Player.Position.Y, -5, 0x0000 )
    
    #Target.TargetExecute( Player.Position.X+3, Player.Position.Y, -5, 0x0000 )
    #Target.TargetExecuteRelative(Player.Serial,3)
    Misc.Pause(8000)

def DropResources(homebook):
    Items.UseItem(homebook)
    Misc.Pause(750)
    Gumps.WaitForGump(89, 10000)
    Misc.Pause(750)
    Gumps.SendAction(89, 50)
    Misc.Pause(4000)
    chest = 0x4008B2F2
    
    moveItemList = [
    0x4307, 
    0x09CE, 
    0x09CC, 
    0x4306, 
    0x44C3, 
    0x44C4, 
    0x4303, 
    0x44C6, 
    0x44C5, 
    0x099F, 
    0x0DCA,
    0x3196,
    0x573A,
    0x09CD,
    0x09CE,
    0x4307]    
    for item in Player.Backpack.Contains:
       if item.ItemID in moveItemList:
        Items.Move(item, chest, 0)
        Misc.Pause(1000)
   
            
def RecallToShipKey(shipkey):
    Spells.CastMagery('Recall')
    Misc.Pause(2000)
    Target.TargetExecute(shipkey)
    Misc.Pause(1000)
    Player.Walk("West")
    Misc.Pause(1000)
    Player.Walk("West")
    Misc.Pause(1000)
    Player.Walk("West")
    Misc.Pause(1000)

        
    
# Main Loop
while True:
    Journal.Clear()
    # Fish until the specified journal message is received
    while not Journal.GetLineText('seem to be biting', False):
        #Defend()
        Fish(tool)
        #if Player.Weight >= Player.MaxWeight * 0.7:  # Check if weight is 70% of max
        #    DropResources(homebook)
         #   RecallToShipKey(shipkey)
            
    for _ in range(4):
        Player.ChatSay(690, "forward one")
        Misc.Pause(300)




