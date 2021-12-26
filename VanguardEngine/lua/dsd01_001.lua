-- 天輪聖竜 ニルヴァーナ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetCanFullyResolve("ACTCanFullyResolve")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
end

function ACTCost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function ACTCanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Drop, q.Grade, 0})
end

function ACT()
	obj.SuperiorCall({q.Location, l.Drop, q.Grade, 0, q.Count, 1})
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.IsSameZone() or obj.Exists({q.Location, l.PlayerUnits, q.UnitType, u.overDress})
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	obj.AddCardValue({q.Location, l.PlayerUnits, q.UnitType, u.overDress}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end