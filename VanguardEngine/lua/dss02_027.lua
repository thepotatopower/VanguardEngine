-- 脈動の天鎌 レポードロン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.PlayerUnits, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.NotThisFieldID})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Grade, 3, q.Other, o.GradeOrHigher, q.Other, o.NotThisFieldID, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end