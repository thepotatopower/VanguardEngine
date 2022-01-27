-- 頂の天帝 バスティオン

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetActivation("OnBattleEnds")
	ability2.SetProperty(p.OncePerTurn)
end

function Cont()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Grade, 3}, cs.BonusPower, 2000, p.Continuous)
	end
end

function OnBattleEndsTrigger()
	return obj.Exists({q.Location, l.RevealedDriveChecks, q.Grade, 3})
end

function OnBattleEndsCost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function OnBattleEnds()
	obj.Select({q.Location, l.PlayerRC, q.Count, 1})
	obj.Stand({q.Location, l.Selected})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end