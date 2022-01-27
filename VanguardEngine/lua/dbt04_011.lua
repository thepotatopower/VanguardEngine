-- 招来の天鉾 コルンヴォーク

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanRest({q.Other, o.This, q.Other, o.Standing}) end
	obj.SoulBlast(1)
	obj.Rest({q.Other, o.This, q.Other, o.Standing})
end

function Activation()
	obj.LookAtTopOfDeck(3)
	obj.SuperiorCall({q.Location, l.Looking, q.Grade, 3, q.NameIsNot, obj.GetName({q.Other, o.This}), q.Other, o.Unit, q.Count, 1, q.Min, 0})
	obj.RearrangeOnBottom({q.Location, l.Looking})
end