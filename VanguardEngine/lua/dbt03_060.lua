-- 焔の棍僧 シャクネ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerRC, q.Grade, 0, q.Other, o.Attacking})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanRetire({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.CounterBlast(1)
	obj.Retire({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking})
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.Attacking}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end