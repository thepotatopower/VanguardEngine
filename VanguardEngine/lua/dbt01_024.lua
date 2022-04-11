-- 鞭撻の乙女 イレーニア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Drop, q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 1})
end

function Activation()
	obj.Store(obj.SuperiorCall({q.Location, l.Drop, q.Grade, 2, q.Other, o.GradeOrLess, q.Count, 1}))
	obj.AddCardValue({q.Location, l.Stored}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end