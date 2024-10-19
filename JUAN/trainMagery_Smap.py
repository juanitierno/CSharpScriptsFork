# Magery trainer by Smaptastic.
# Assumes you start with ~30 magery (buy it from a trainer)

# Plug in your target skill level. Real skill, not buffed with +Magery items.
targetSkill = 120


def fcDelay(spellLevel):
    fc = int((((3 + spellLevel) / 4) * 1000) - (((Player.FasterCasting) * .25) * 1000) + 1000)
    if fc < 1000:
        fc = 1000
    return fc
    
def fcrDelay():
    fcr = int(((6 - Player.FasterCastRecovery) / 4) * 1000)
    if fcr < 1:
        fcr = 1
    return fcr

def castSelf(spellToCast, spellLevel):
    Spells.CastMagery(spellToCast)
    Target.WaitForTarget(fcDelay(spellLevel))
    Target.TargetExecute(Player.Serial)
    Misc.Pause(fcrDelay())
    
lowMana = False

while True:
    if targetSkill:
        if Player.GetRealSkillValue("Magery") >= targetSkill:
            break
    if lowMana and Player.Mana != Player.ManaMax:
        if not Player.BuffsExist("Meditation") and not Player.BuffsExist("Actively Meditating"):
            Player.UseSkill("Meditation")
        Misc.Pause(2000)
        continue
    else:
        lowMana = False
    if Player.Mana < 30:
        lowMana = True
        Player.UseSkill("Meditation")
        Misc.Pause(2000)
        continue
    if Player.GetSkillValue("Magery") < 35:
        castSelf("Bless", 3)
        continue
    if Player.GetSkillValue("Magery") < 55:
        castSelf("Mana Drain", 3)
        continue
    if Player.GetSkillValue("Magery") < 70:
        castSelf("Paralyze", 5)
        continue
    if Player.GetSkillValue("Magery") < 90:
        castSelf("Mana Vampire", 7)
        continue
    castSelf("Earthquake", 8)