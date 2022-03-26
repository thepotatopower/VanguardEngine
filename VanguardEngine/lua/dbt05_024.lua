-- スチームメイデン バーニ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.GetNumberOf({q.Location, l.Soul, q.Other, o.DifferentNames}) >= 8
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	if obj.GetNumberOf({q.Location, l.Soul, q.Other, o.DifferentNames}) >= 10 then
		obj.SuperiorCall({q.Location, l.Soul, q.Grade, 4, q.Other, o.GradeOrLess, q.Count, 1})
	else
		obj.SuperiorCall({q.Location, l.Soul, q.Grade, 1, q.Other, o.GradeOrLess, q.Count, 1})
	end
end