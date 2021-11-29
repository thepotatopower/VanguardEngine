-- 警邏ロボ デカルコップ

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.This, q.Other, o.Boosting})
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.AddToSoul({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 2000, p.UntilEndOfTurn)
end