-- デザイアデビル ガーメッツ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_003")})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanDiscard(1) end
	obj.CounterBlast(1)
	obj.Discard(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Soul, q.Grade, 3, q.Other, o.GradeOrLess, q.Count, 1})
end

function Activation()
	obj.SuperiorCall({q.Location, l.Soul, q.Grade, 3, q.Other, o.GradeOrLess, q.Count, 1})
end