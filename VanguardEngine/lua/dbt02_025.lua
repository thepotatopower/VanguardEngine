-- 野生の知恵

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function GetCosts()
	return p.CB, 1
end

function Activation()
	obj.Mill(3)
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd04_001")}) then
		obj.SuperiorCall({q.Location, l.Drop, q.Grade, obj.GetGrade({q.Location, l.PlayerVC}), q.Other, o.GradeOrLess, q.Count, 2, q.Min, 0})
	else
		obj.SuperiorCall({q.Location, l.Drop, q.Grade, obj.GetGrade({q.Location, l.PlayerVC}), q.Other, o.GradeOrLess, q.Count, 1, q.Min, 0})
	end
end