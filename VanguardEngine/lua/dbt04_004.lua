-- ディアブロス ”絶勝” ブルース

function RegisterAbilities()
	-- on ride phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRidePhase)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("OnRidePhase")
	-- on battle ends
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnBattleEnds)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("OnBattleEndsTrigger")
	ability2.SetCondition("OnBattleEndsCondition")
	ability2.SetCost("OnBattleEndsCost")
	ability2.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability2.SetActivation("OnBattleEnds")
end

function OnRidePhase()
	obj.FinalRush()
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.InFinalRush()
end

function OnBattleEndsCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(5) end
	obj.CounterBlast(1)
	obj.SoulBlast(5)
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Location, l.FrontRow, q.Other, o.Resting})
end

function OnBattleEnds()
	obj.Stand({q.Location, l.FrontRow, q.Other, o.Resting})
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusDrive, -2, p.UntilEndOfTurn)
end