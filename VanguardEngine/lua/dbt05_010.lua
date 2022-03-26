-- 光の道を往く者 テグリア

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Cont")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.VC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetCost("ACTCost")
	ability2.SetActivation("ACT")
	-- on battle ends
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetTiming(a.OnBattleEnds)
	ability3.SetLocation(l.VC)
	ability3.SetTrigger("OnBattleEndsTrigger")
	ability3.SetCondition("OnBattleEndsCondition")
	ability3.SetCost("OnBattleEndsCost")
	ability3.SetCanFullyResolve("OnBattleEndsCanFullyResolve")
	ability3.SetActivation("OnBattleEnds")
	-- glitter
	local ability4 = NewAbility(GetID())
	ability4.SetDescription(4)
	ability4.SetProperty(p.IsGlitter)
end

function Cont()
	return obj.AddCardValue({q.Other, o.This}, cs.PersonaRideIfNameContains, obj.LoadName("Thegrea"), p.Continuous)
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACT()
	obj.LookAtTopOfDeck(5)
	obj.DisplayCards({q.Location, l.Looking})
	local count = 1
	if obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt05_011")}) then
		count = 2
	end
	obj.SuperiorCall({q.Location, l.Looking, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.Unit, q.Count, count, q.Min, 0})
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.RiddenFromNameIs(obj.GetNameFromCardID("dbt05_011"))
end

function OnBattleEndsCost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Resting})
end

function OnBattleEnds()
	obj.ChooseStand({q.Location, l.PlayerRC, q.Count, 1})
end