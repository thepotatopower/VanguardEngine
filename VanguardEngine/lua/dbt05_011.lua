-- 暗黒に咲き誇る者 テグリア

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
	ability3.SetProperty(p.OncePerTurn)
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
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 2}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 2})
end

function ACT()
	obj.Select({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	if obj.Exists({q.Location, l.Soul, q.Name, obj.GetNameFromCardID("dbt05_010")}) then
		local column = obj.GetColumn({q.Location, l.Selected})
		obj.Retire({q.Location, l.EnemyRC, q.Column, column})
	else
		obj.Retire({q.Location, l.Selected})
	end
end

function OnBattleEndsTrigger()
	return obj.IsAttackingUnit()
end

function OnBattleEndsCondition()
	return obj.RiddenFromNameIs(obj.GetNameFromCardID("dbt05_010"))
end

function OnBattleEndsCost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1)
	obj.Discard(1)
end

function OnBattleEndsCanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Resting, q.Other, o.ThisFieldID})
end

function OnBattleEnds()
	obj.Stand({q.Other, o.ThisFieldID})
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, -1, p.UntilEndOfTurn)
end