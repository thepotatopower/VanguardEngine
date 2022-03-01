-- ゴースト・ビルク

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.RearrangeOnTop({q.Location, l.Drop, q.Grade, 2, q.UnitType, u.Normal, q.Other, o.GradeOrHigher, q.Count, 2, q.Min, 0})
	obj.AddToSoul({q.Other, o.This})
end