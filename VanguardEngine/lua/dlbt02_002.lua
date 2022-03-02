-- 新世代の美 エルミニア

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.ACT)
	ability1.SetLocation(l.VC)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.Powerful)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetActivation("OnBattleEnds")
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACT()
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function OnBattleEndsTrigger()
	return obj.IsApplicable()
end

function OnBattleEndsCondition()
	return obj.SoulCount() == 0 and not obj.Exists({q.Location, l.Damage, q.Other, o.FaceUp})
end

function OnBattleEndsCost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function OnBattleEnds()
	obj.ChooseStand({q.Location, l.PlayerRC, q.Property, p.Powerful, q.Count, 1})
	obj.IncrementUntilEndOfTurnPlayerValue(ps.FreeCB, 1)
end