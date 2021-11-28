-- 砂塵の双撃 オーランド

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.Soul)
	ability1.SetCondition("ACTCondition")
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetActivation("OnAttack")
end

function ACTCondition()
	return obj.EnemyRetiredThisTurn()
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACT()
	obj.SuperiorCall({q.Location, l.Soul, q.Other, o.This})
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.Attacking})
end

function OnAttackCost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.AddToSoul({q.Location, l.PlayerUnits, q.Other, o.This})
end

function OnAttack()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end