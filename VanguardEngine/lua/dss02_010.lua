-- 樹角獣 アルマディ

function RegisterAbilities()
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
	return obj.Exists({q.Location, l.BackRow, q.Other, o.Attacking, q.Other, o.NotThisFieldID})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanAddToSoul({q.Other, o.ThisFieldID}) end
	obj.CounterBlast(1)
	obj.AddToSoul({q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.BackRow, q.Other, o.Attacking, q.Other, o.NotThisFieldID, q.Other, o.SameZone})
end

function Activation()
	obj.AddCardValue({q.Location, l.BackRow, q.Other, o.Attacking, q.Other, o.NotThisFieldID, q.Other, o.SameZone}, cs.BonusPower, 10000, p.UntilEndOfBattle)
end