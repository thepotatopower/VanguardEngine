-- ディアブロスチャージャー ネイト

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("OnAttackTrigger")
	ability1.SetCondition("OnAttackCondition")
	ability1.SetActivation("OnAttack")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.RC)
	ability2.SetCost("ACTCost")
	ability2.SetCondition("ACTCondition")
	ability2.SetActivation("ACT")
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCondition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function ACTCondition()
	return obj.InFinalRush()
end

function ACTCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function ACT()
	obj.AddCardState({q.Location, l.PlayerRC, q.Other, o.This}, cs.CanAttackAllFrontRow, p.UntilEndOfTurn)
end